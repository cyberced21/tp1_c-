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

        public ControleTimer()
        {
            InitializeComponent();
        }

        private void ControleTimer_Load(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            lblTimer.Text = TInitial.ToString();
        }

        public void Demarrer()
        {
            afficher = new AfficherDelegue(AfficherTimerTxt);
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

        protected virtual void OnTimerAZero()
        {
                TimerAZero?.Invoke(this, EventArgs.Empty);
        }
    }
}
