using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompteARebours
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_confirmation_Click(object sender, EventArgs e)
        {
            if (txt_box_reponse.Text == "salut")
            {
                MessageBox.Show("You win!!!!");
            } 
        }

        private void controleTimer1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controleTimer1.TInitial = new TimeSpan(0, 0, 10);
            controleTimer1.Demarrer();
        }
    }
}
