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
        public TimeSpan TInitial { get; set; } // Le timer prendra son temps d'un timespan
        private Thread thread;
        private delegate void AfficherDelegue();
        AfficherDelegue afficher; // Delegue qui nous permettra de choisir le mode d'affichage
        public delegate void ControleTimerElapsedEventHandler(object source, EventArgs args); 
        public event ControleTimerElapsedEventHandler TimerAZero; // Evenement qui sera leve quand le timer sera a zero
        private int largeurHorloge;
        private int hauteurHorloge;
        private int aiguilleSec = 100, aiguilleMin = 75, aiguilleHeure = 50; // Magnitude des aiguilles de l'horloge
        private int centreHorlogeX, centreHorlogeY;

        Bitmap bmp; // Image qui sera colle sur le pictureBox
        Graphics g;

        public ControleTimer()
        {
            InitializeComponent();
        }

        private void ControleTimer_Load(object sender, EventArgs e)
        {
            // On met l'intervale du timer a 1 sec et on l'affiche dans le label
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
            afficher = new AfficherDelegue(AfficherTimerTxt); // Au depart, on affiche le timer dans le label
            thread = new Thread(new ThreadStart(CompteARebours)); // On demarre le timer dans un thread
            thread.Start();
        }

        // Lorsque le timer "tickera" on soustrait une seconde et on invoque
        // le delegue afficher qui pointe vers le bon mode d'affichage
        private void OnTimer_Tick(object sender, EventArgs e)
        {
            TInitial = TInitial.Subtract(new TimeSpan(0, 0, 1));
            Invoke(afficher);
        }

        /*
            On change le mode d'affichage quand l'utilisateur clic sur le controle
            Cette methode doit etre misent comme gestionnaire de l'evenement click a la fois
            dans le controleur de dans la pictureBox car celle-ci prend toute la place du controleur
            Si on oublie de l'jouter dans le pictureBox, on ne pourra jamais cliquer sur le controlleur
        */
        private void On_Click(object sender, EventArgs e)
        {
            if (this.lblTimer.Visible)
            {
                this.lblTimer.Visible = false;
                this.pictureBox1.Visible = true;
                afficher = new AfficherDelegue(AfficherTimerHorloge); // On fait pointer le delegue vers le nouveau mode daffichage
            }
            else
            {
                this.lblTimer.Visible = true;
                this.pictureBox1.Visible = false;
                afficher = new AfficherDelegue(AfficherTimerTxt); // On fait pointer le delegue vers le nouveau mode daffichage
            }
        }

        // Methode qui est lancer dans le thread
        private void CompteARebours()
        {
            timer.Elapsed += new ElapsedEventHandler(OnTimer_Tick); // Ajoute la methode OnTimer_Tick a l'evenement qui se produit quand le timer tick
            timer.Start();
            Invoke(afficher); // On invoque le delegue qui pointe vers le bon mode d'affichage

            // Tant que le thread est en vie on verifie si le timer est a zero...
            while (Thread.CurrentThread.IsAlive)
            {
                // ... s'il est a zero, on arrete le timer, on lance l'evenement TimerAZero et on detruit le thread
                if (TInitial.Equals(new TimeSpan(0, 0, 0)))
                {
                    timer.Stop();
                    OnTimerAZero();
                    thread.Abort();
                }
            }
        }

        // Affiche simplement le compte a rebours sous forme de texte
        private void AfficherTimerTxt()
        {            
            lblTimer.Text = TInitial.ToString();                
        }

        // Affiche le compte a rebours avec l'horloge
        private void AfficherTimerHorloge()
        {        
            // Cree lobjet Graphics
            g = Graphics.FromImage(bmp);

            // On retrouve le temps
            int ss = TInitial.Seconds;
            int mm = TInitial.Minutes;
            int hh = TInitial.Hours;

            // Represente la direction des aiguilles a partir du centre(x et y)
            int[] aiguilleCoord = new int[2];

            // Clear a blanc
            g.Clear(Color.White);

            // Dessine le cercle
            g.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, largeurHorloge, hauteurHorloge);



            // Dessine les chiffres en calculant les coordonner a pour chaque 5 minutes
            // pour qu'il soit egale sur l'horloge
            int compteur_nb = 12;
            for (int i = 0; i < 60; i+=5) 
            {
                if (i == 5) { compteur_nb = 1; };
                g.DrawString(compteur_nb.ToString(), new Font("Arial", 12),
                    Brushes.Black, 
                    new PointF(minsecCoord(i,130)[0], minsecCoord(i, 130)[1]));
                compteur_nb++;
            }

            // Aiguille secondes
            aiguilleCoord = minsecCoord(ss, aiguilleSec);
            g.DrawLine(new Pen(Color.Red), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            // Aiguille minutes
            aiguilleCoord = minsecCoord(mm, aiguilleMin);
            g.DrawLine(new Pen(Color.Red), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            // Aiguille Heures
            aiguilleCoord = minsecCoord(hh%12, aiguilleHeure);
            g.DrawLine(new Pen(Color.Black), new Point(centreHorlogeX, centreHorlogeY), new Point(aiguilleCoord[0], aiguilleCoord[1]));

            // Charge le bmp dans le pictureBox
            this.pictureBox1.Image = bmp;            

            // Dispose
            g.Dispose();
        }
        /*
            Coordonnees pour les aiguilles minutes et secondes
            val represente les min ou sec ou on est rendu
            et hlen represente la longueur de l'aiguille(min ou sec)
        */
        private int[] minsecCoord(int val, int hlen)
        {
            int[] coord = new int[2]; // Valeur a retourner
            val *= 6; // A chaque min et sec on fait 6 degrees

            // On calcule la position x et y de l'aiguille par rapport au centre
            if(val >= 0 && val <= 180)
            {
                coord[0] = centreHorlogeX + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = centreHorlogeX - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = centreHorlogeY - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord; // Retourne la direction de l'aiguille (x et y)
        }

        // Coordonnees pour l'aiguille des heures
        private int[] hrCoord(int hVal, int mVal, int hlen)
        {
            int[] coord = new int[2]; // Valeur a retourner
            int val = (int)((hVal * 30)); // On fait 30 degrees chaque heure

            // On calcule la position x et y de l'aiguille par rapport au centre
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
            return coord; // Retourne la direction de l'aiguille (x et y)
        }        

        // Evenement lance quand le timer arrive a zero
        protected virtual void OnTimerAZero()
        {
            // Si TimerAZero n'est pas null, on lance l'evenement    
            TimerAZero?.Invoke(this, EventArgs.Empty);
        }

        // Si l'application utilisant le controle se ferme, on detruit le thread
        protected override void OnHandleDestroyed(EventArgs e)
        {
            thread.Abort();
            base.OnHandleDestroyed(e);
        }
    }
}
