namespace ByteKnight
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnJogar = new Button();
            btnSair = new Button();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            SuspendLayout();
            // 
            // btnJogar
            // 
            btnJogar.Location = new Point(265, 138);
            btnJogar.Name = "btnJogar";
            btnJogar.Size = new Size(119, 25);
            btnJogar.TabIndex = 0;
            btnJogar.Text = "JOGAR";
            btnJogar.UseVisualStyleBackColor = true;
            btnJogar.Click += btnJogar_Click;
            // 
            // btnSair
            // 
            btnSair.Location = new Point(265, 239);
            btnSair.Name = "btnSair";
            btnSair.Size = new Size(119, 23);
            btnSair.TabIndex = 1;
            btnSair.Text = "SAIR";
            btnSair.UseVisualStyleBackColor = true;
            btnSair.Click += btnSair_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Location = new Point(579, 149);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(78, 15);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "BYTE KNIGHT";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Location = new Point(672, 149);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(109, 15);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "A Lenda do Código";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblTitulo);
            Controls.Add(btnSair);
            Controls.Add(btnJogar);
            Name = "Form1";
            Text = "Byte Knight";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnJogar;
        private Button btnSair;
        private Label lblTitulo;
        private Label lblSubtitulo;
    }
}
