using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Timers;

namespace ControleTimer
{
    public partial class ControleTimer: UserControl
    {
        private System.Timers.Timer timer = new System.Timers.Timer();
        public TimeSpan TInitial { get; set; }
        private Thread thread;
        private delegate void AfficherDelegue();
        AfficherDelegue afficher;
        public delegate void ControleTimerElapsedEventHandler(object source, EventArgs args);
        public event ControleTimerElapsedEventHandler TimerAZero;
        private int largeurHorloge;
        private int hauteurHorloge;
        private int aiguilleSec = 100, aiguilleMin = 75, aiguilleHeure = 50;
        private int centreHorlogeX, centreHorlogeY;

        Bitmap bmp;
        Graphics g;

        public ControleTimer()
        {
            InitializeComponent();
        }

        private void ControleTimer_Load(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            lblTimer.Text = TInitial.ToString();

            // Affectation des variables qui concernent l'horloge
            largeurHorloge = this.pictureBox1.Width - 5;
            hauteurHorloge = this.pictureBox1.Height - 5;
            bmp = new Bitmap(largeurHorloge + 1, hauteurHorloge + 1);
            centreHorlogeX = largeurHorloge / 2;
            centreHorlogeY = hauteurHorloge / 2;
        }

        public void Demarrer()
        {
            afficher = new AfficherDelegue(AfficherTimerHorloge);
            thread = new Thread(new ThreadStart(CompteARebours));
            thread.Start();
        }

        private void OnTimer_Tick(object sender, EventArgs e)
        {
            TInitial = TInitial.Subtract(new TimeSpan(0, 0, 1));
            Invoke(afficher);
        }

        private void CompteARebours()
        {
            timer.Elapsed += new ElapsedEventHandler(OnTimer_Tick);
            timer.Start();
            Invoke(afficher);
            while (Thread.CurrentThread.IsAlive)
            {
                if (TInitial.Equals(new TimeSpan(0, 0, 0)))
                {
                    timer.Stop();
                    OnTimerAZero();
                    thread.Abort();
                }
            }
        }

        private void AfficherTimerTxt()
        {            
            lblTimer.Text = TInitial.ToString();                
        }

        private void AfficherTimerHorloge()
        {        
            // Cree lobjet Graphics
            g = Graphics.FromImage(bmp);

            // On retrouve le temps
            int ss = TInitial.Seconds;
            int mm = TInitial.Minutes;
            int hh = TInitial.Hours;

            int[] aiguilleCoord = new int[2];

            // Clear a blanc
            g.Clear(Color.White);

            // Dessine le cercle
            g.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, largeurHorloge, hauteurHorloge);

            // Dessine les chiffres
            g.DrawString("12", new Font("Arial", 12), Brushes.Black, new PointF(140, 2));
            g.DrawString("3", new Font("Arial", 12), Brushes.Black, new PointF(295, 100));
            g.DrawString("6", new Font("Arial", 12), Brushes.Black, new PointF(142, 200));
            g.DrawString("9", new Font("Arial", 12), Brushes.Black, new PointF(0, 100));

            // Aiguille secondes
            aiguilleCoord = minsecCoord(ss, aiguilleSec);
            g.DrawLine(new Pen(Color.Red), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            // Aiguille minutes
            aiguilleCoord = minsecCoord(mm, aiguilleMin);
            g.DrawLine(new Pen(Color.Red), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            // Aiguille Heures
            aiguilleCoord = minsecCoord(hh%12, aiguilleHeure);
            g.DrawLine(new Pen(Color.Black), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            /*
            // On redimensionne le label
            this.lblTimer.Size = bmp.Size;
            */
            // Charge le bmp dans le pictureBox
            this.pictureBox1.Image = bmp;
            this.pictureBox1.Visible = true;

            // Dispose
            g.Dispose();
        }

        // Coordonnees pour les aiguilles minutes et secondes
        private int[] minsecCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6; // A chaque min et sec on fait 6 degrees

            if(val >= 0 && val <= 180)
            {
                coord[0] = centreHorlogeX + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = centreHorlogeX - (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        // Coordonnees pour l'aiguille des heures
        private int[] hrCoord(int hVal, int mVal, int hlen)
        {
            int[] coord = new int[2];
            int val = (int)((hVal * 30)); // On fait 30 degrees chaque heure

            if (val >= 0 && val <= 180)
            {
                coord[0] = centreHorlogeX + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Sin(Math.PI * val / 180));
            }
            else
            {
                coord[0] = centreHorlogeX - (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Sin(Math.PI * val / 180));
            }
            return coord;
        }        

        protected virtual void OnTimerAZero()
        {
                TimerAZero?.Invoke(this, EventArgs.Empty);
        }
    }
}
