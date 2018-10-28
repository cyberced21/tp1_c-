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
            controle = new ControleTime();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.controleTimer1.Demarrer();
        }
        private void OnTimerDone(Object Source, EventArgs e)
        {
            MessageBox.Show("Timer fini");
        }
    }
}
