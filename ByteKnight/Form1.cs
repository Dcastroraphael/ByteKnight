namespace ByteKnight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string caminho = Path.Combine(Application.StartupPath, "assets", "fundo.png");
            BackgroundImage = Image.FromFile(caminho);
            BackgroundImageLayout = ImageLayout.Stretch;
            EstilizarTela();
            Resize += (s, e) => CentralizarControles();
        }
        private void EstilizarTela()
        {
            lblTitulo.Font = new Font("Impact", 96F);
            lblTitulo.ForeColor = Color.Gold;

            lblSubtitulo.Font = new Font("Segoe UI", 26F, FontStyle.Italic);
            lblSubtitulo.ForeColor = Color.White;

            EstilizarBotao(btnJogar, "JOGAR", Color.SeaGreen);
            EstilizarBotao(btnSair, "SAIR", Color.Firebrick);

            CentralizarControles();
        }

        private void EstilizarBotao(Button botao, string texto, Color cor)
        {
            botao.Text = texto;
            botao.Size = new Size(400, 80);
            botao.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            botao.ForeColor = Color.White;
            botao.BackColor = cor;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.Cursor = Cursors.Hand;

            botao.MouseEnter += (s, e) => botao.BackColor = ControlPaint.Light(cor);
            botao.MouseLeave += (s, e) => botao.BackColor = cor;
        }

        private void CentralizarControles()
        {
            lblTitulo.Left = (ClientSize.Width - lblTitulo.Width) / 2;
            lblTitulo.Top = ClientSize.Height / 6;

            lblSubtitulo.Left = (ClientSize.Width - lblSubtitulo.Width) / 2;
            lblSubtitulo.Top = lblTitulo.Bottom;

            btnJogar.Left = (ClientSize.Width - btnJogar.Width) / 2;
            btnJogar.Top = ClientSize.Height / 2 + 40;

            btnSair.Left = btnJogar.Left;
            btnSair.Top = btnJogar.Bottom + 30;
        }

        private void btnJogar_Click(object sender, EventArgs e)
        {
            Game jogo = new Game();
            jogo.Show();
            Hide();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}