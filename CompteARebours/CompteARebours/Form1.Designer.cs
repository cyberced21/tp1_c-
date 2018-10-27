namespace CompteARebours
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_confirmation = new System.Windows.Forms.Button();
            this.controleTimer1 = new ControleTimer.ControleTimer();
            this.txt_box_reponse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_confirmation
            // 
            this.btn_confirmation.Location = new System.Drawing.Point(133, 237);
            this.btn_confirmation.Name = "btn_confirmation";
            this.btn_confirmation.Size = new System.Drawing.Size(75, 23);
            this.btn_confirmation.TabIndex = 0;
            this.btn_confirmation.Text = "Confirmation";
            this.btn_confirmation.UseVisualStyleBackColor = true;
            this.btn_confirmation.Click += new System.EventHandler(this.btn_confirmation_Click);
            // 
            // controleTimer1
            // 
            this.controleTimer1.Location = new System.Drawing.Point(12, 12);
            this.controleTimer1.Name = "controleTimer1";
            this.controleTimer1.Size = new System.Drawing.Size(196, 131);
            this.controleTimer1.TabIndex = 1;
            this.controleTimer1.TInitial = System.TimeSpan.Parse("00:10:00");
            this.controleTimer1.Load += new System.EventHandler(this.controleTimer1_Load);
            // 
            // txt_box_reponse
            // 
            this.txt_box_reponse.Location = new System.Drawing.Point(121, 211);
            this.txt_box_reponse.Name = "txt_box_reponse";
            this.txt_box_reponse.Size = new System.Drawing.Size(100, 20);
            this.txt_box_reponse.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "C\'est quoi qu\'on dit a quelqu\'un qu\'on vient de rencontrer?";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_box_reponse);
            this.Controls.Add(this.controleTimer1);
            this.Controls.Add(this.btn_confirmation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_confirmation;
        private ControleTimer.ControleTimer controleTimer1;
        private System.Windows.Forms.TextBox txt_box_reponse;
        private System.Windows.Forms.Label label1;
    }
}

