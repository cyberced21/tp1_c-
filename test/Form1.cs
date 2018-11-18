using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.controleTimer1.TimerAZero += new ControleTimer.ControleTimer.ControleTimerElapsedEventHandler(triger_disable);
            this.controleTimer1.Demarrer();
        }
        

        private void disable() { txt_box_answer.Enabled = false; }
        public delegate void disableTxtBox(object source, EventArgs args);
        private void triger_disable(object source, EventArgs args)
        {
            if (this.txt_box_answer.InvokeRequired)
            {
                disableTxtBox d = new disableTxtBox(triger_disable);
                this.Invoke(d, new object[2] { System.EventArgs.Empty,System.EventArgs.Empty });
            }

            disable();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (txt_box_answer.Text == "oui" || txt_box_answer.Text == "OUI")
            {
                MessageBox.Show("Tu est le ou la meilleur(e)");
            }
        }
    }
}
