using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ByteKnight
{
    public partial class Game : Form
    {
        private Player jogador = null!;
        private int chaoY;
        private bool esquerda, direita;
        private int velocidadeY = 0;
        private readonly System.Windows.Forms.Timer relogio = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer timerDisparos = new System.Windows.Forms.Timer();

        private readonly List<PictureBox> moedas = new List<PictureBox>();
        private readonly Random sorteio = new Random();
        private Image imagemMoeda = null!;
        private Label lblPontos = null!;
        private int pontos = 0;
        private int inimigosDerrotados = 0;
        private Label lblCombate = null!;
        private long ultimoDisparoMs = -10000;
        private readonly System.Diagnostics.Stopwatch tempoJogo = System.Diagnostics.Stopwatch.StartNew();
        private const int IntervaloDisparoMs = 280;

        // NOVO: inimigos, vidas e tela de Game Over
        private readonly List<PictureBox> inimigos = new List<PictureBox>();
        private Image imagemInimigo = null!;
        private Label lblVidas = null!;
        private Label lblGameOver = null!;
        private Label lblResultado = null!;
        private Button btnReiniciar = null!;
        private Button btnSairJogo = null!;
        private int vidas = VidasIniciais;
        private int invencivel = 0;          // quadros restantes de invencibilidade
        private int proximoInimigo = 120;    // quadros até o próximo bug aparecer
        private bool fimDeJogo = false;

        private const int Velocidade = 12;
        private const int Gravidade = 2;
        private const int ForcaPulo = -32;
        private const int QuantidadeMoedas = 5;
        private const int VidasIniciais = 3;       // NOVO
        private const int QuadrosInvencivel = 90;  // NOVO (90 quadros ≈ 1,5 segundo)

        public Game()
        {
            InitializeComponent();
            FormClosed += (s, e) => Application.Exit();
            KeyPreview = true;
            DoubleBuffered = true;
            KeyDown += Game_KeyDown;
            KeyUp += Game_KeyUp;
            timerDisparos.Interval = 100;
            timerDisparos.Enabled = true;
            timerDisparos.Tick += Timer_Tick;
        }

        private void Game_Load(object sender, EventArgs e)
        {
            string caminho = Path.Combine(Application.StartupPath, "assets", "fundo.png");
            BackgroundImage = Image.FromFile(caminho);
            BackgroundImageLayout = ImageLayout.Stretch;

            chaoY = (int)(ClientSize.Height * 0.9);

            jogador = new Player();
            jogador.Location = new Point(100, chaoY - jogador.Height);
            Controls.Add(jogador);

            imagemMoeda = Image.FromFile(Path.Combine(Application.StartupPath, "assets", "moeda.png"));
            // NOVO
            imagemInimigo = Image.FromFile(Path.Combine(Application.StartupPath, "assets", "inimigo.png"));

            lblPontos = new Label();
            lblPontos.AutoSize = true;
            lblPontos.BackColor = Color.Transparent;
            lblPontos.ForeColor = Color.Gold;
            lblPontos.Font = new Font("Impact", 32F);
            lblPontos.Location = new Point(30, 20);
            lblPontos.Text = "BITS: 0";
            Controls.Add(lblPontos);

            // NOVO: placar de vidas
            lblVidas = new Label();
            lblVidas.AutoSize = true;
            lblVidas.BackColor = Color.Transparent;
            lblVidas.ForeColor = Color.Tomato;
            lblVidas.Font = new Font("Impact", 32F);
            lblVidas.Location = new Point(30, 80);
            lblVidas.Text = "VIDAS: " + vidas;
            Controls.Add(lblVidas);

            lblCombate = new Label();
            lblCombate.AutoSize = true;
            lblCombate.BackColor = Color.Transparent;
            lblCombate.ForeColor = Color.White;
            lblCombate.Font = new Font("Impact", 24F);
            lblCombate.Location = new Point(30, 145);
            lblCombate.Text = "INIMIGOS: 0";
            Controls.Add(lblCombate);

            for (int i = 0; i < QuantidadeMoedas; i++)
            {
                CriarMoeda();
            }

            CriarTelaGameOver(); // NOVO

            relogio.Interval = 16;
            relogio.Tick += Atualizar;
            relogio.Start();
        }

        // NOVO: monta (escondida) a tela de Game Over
        private void CriarTelaGameOver()
        {
            lblGameOver = new Label();
            lblGameOver.AutoSize = true;
            lblGameOver.BackColor = Color.Transparent;
            lblGameOver.ForeColor = Color.Tomato;
            lblGameOver.Font = new Font("Impact", 96F);
            lblGameOver.Text = "GAME OVER";
            lblGameOver.Visible = false;
            Controls.Add(lblGameOver);

            lblResultado = new Label();
            lblResultado.AutoSize = true;
            lblResultado.BackColor = Color.Transparent;
            lblResultado.ForeColor = Color.White;
            lblResultado.Font = new Font("Segoe UI", 28F, FontStyle.Italic);
            lblResultado.Visible = false;
            Controls.Add(lblResultado);

            btnReiniciar = CriarBotaoJogo("JOGAR DE NOVO", Color.SeaGreen);
            btnReiniciar.Click += (s, e) => ReiniciarJogo();

            btnSairJogo = CriarBotaoJogo("SAIR", Color.Firebrick);
            btnSairJogo.Click += (s, e) => Application.Exit();
        }

        private Button CriarBotaoJogo(string texto, Color cor)
        {
            Button botao = new Button();
            botao.Text = texto;
            botao.Size = new Size(400, 80);
            botao.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            botao.ForeColor = Color.White;
            botao.BackColor = cor;
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.Cursor = Cursors.Hand;
            botao.TabStop = false;   // evita que Espaço/Enter "cliquem" no botão
            botao.Visible = false;
            botao.MouseEnter += (s, e) => botao.BackColor = ControlPaint.Light(cor);
            botao.MouseLeave += (s, e) => botao.BackColor = cor;
            Controls.Add(botao);
            return botao;
        }

        private void CriarMoeda()
        {
            PictureBox moeda = new PictureBox();
            moeda.Size = new Size(48, 48);
            moeda.SizeMode = PictureBoxSizeMode.StretchImage;
            moeda.BackColor = Color.Transparent;
            moeda.Image = imagemMoeda;

            int x = sorteio.Next(Math.Min(300, Math.Max(0, ClientSize.Width - 100)), Math.Max(301, ClientSize.Width - 100));
            int y = chaoY - sorteio.Next(60, 330);
            moeda.Location = new Point(x, y);

            moedas.Add(moeda);
            Controls.Add(moeda);
        }

        // NOVO: cria um bug na borda direita, em cima do chão
        private void CriarInimigo()
        {
            PictureBox inimigo = new PictureBox();
            inimigo.Size = new Size(72, 60);
            inimigo.SizeMode = PictureBoxSizeMode.StretchImage;
            inimigo.BackColor = Color.Transparent;
            inimigo.Image = imagemInimigo;
            inimigo.Location = new Point(ClientSize.Width, chaoY - inimigo.Height);

            inimigos.Add(inimigo);
            Controls.Add(inimigo);
        }

        private void Game_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Application.Exit();

            // NOVO: no Game Over, só o Enter faz algo (reinicia)
            if (fimDeJogo)
            {
                if (e.KeyCode == Keys.Enter) ReiniciarJogo();
                return;
            }

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) { esquerda = true; jogador.OlharPara(-1); }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) { direita = true; jogador.OlharPara(1); }

            if (e.KeyCode == Keys.Space)
            {
                if (tempoJogo.ElapsedMilliseconds - ultimoDisparoMs >= IntervaloDisparoMs)
                {
                    ultimoDisparoMs = tempoJogo.ElapsedMilliseconds;
                    Bullet bala = jogador.Atirar();
                    Controls.Add(bala);
                    bala.BringToFront();
                }
                e.SuppressKeyPress = true;
            }

            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && NoChao())
                velocidadeY = ForcaPulo;
        }

        private void Game_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) esquerda = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) direita = false;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (fimDeJogo || jogador == null) return;

            for (int i = jogador.balas.Count - 1; i >= 0; i--)
            {
                Bullet bala = jogador.balas[i];
                bala.Mover();

                bool atingiuInimigo = false;
                for (int j = inimigos.Count - 1; j >= 0; j--)
                {
                    Rectangle hitInimigo = Rectangle.Inflate(inimigos[j].Bounds, -4, -4);
                    if (bala.Bounds.IntersectsWith(hitInimigo))
                    {
                        RemoverInimigo(j);
                        inimigosDerrotados++;
                        lblCombate.Text = "INIMIGOS: " + inimigosDerrotados;
                        atingiuInimigo = true;
                        break;
                    }
                }

                if (atingiuInimigo || bala.SaiuDaTela())
                {
                    Controls.Remove(bala);
                    jogador.balas.RemoveAt(i);
                    bala.Dispose();
                }
            }
        }

        private bool NoChao()
        {
            return jogador.Bottom >= chaoY;
        }

        private void Atualizar(object? sender, EventArgs e)
        {
            if (esquerda) jogador.Left -= Velocidade;
            if (direita) jogador.Left += Velocidade;

            velocidadeY += Gravidade;
            jogador.Top += velocidadeY;

            if (jogador.Bottom > chaoY)
            {
                jogador.Top = chaoY - jogador.Height;
                velocidadeY = 0;
            }

            int limiteDireito = ClientSize.Width - jogador.Width;
            if (jogador.Left < 0) jogador.Left = 0;
            if (jogador.Left > limiteDireito) jogador.Left = limiteDireito;

            // NOVO: pisca enquanto está invencível
            if (invencivel > 0)
            {
                invencivel--;
                jogador.Visible = (invencivel / 6) % 2 == 0;
            }

            VerificarMoedas();
            AtualizarInimigos(); // NOVO
        }

        private void VerificarMoedas()
        {
            for (int i = moedas.Count - 1; i >= 0; i--)
            {
                PictureBox moeda = moedas[i];

                if (jogador.Bounds.IntersectsWith(moeda.Bounds))
                {
                    Controls.Remove(moeda);
                    moeda.Dispose();
                    moedas.RemoveAt(i);

                    pontos++;
                    lblPontos.Text = "BITS: " + pontos;

                    CriarMoeda();
                }
            }
        }

        // NOVO: cria, move e testa colisão dos bugs
        private void AtualizarInimigos()
        {
            // Cria um bug novo de tempos em tempos
            proximoInimigo--;
            if (proximoInimigo <= 0)
            {
                CriarInimigo();
                proximoInimigo = sorteio.Next(80, 170);
            }

            // Dificuldade: cada 5 bits deixam os bugs um pouco mais rápidos (máximo 18)
            int velocidadeInimigo = Math.Min(7 + pontos / 5, 18);

            // Retângulos de colisão menores que as imagens (mais justo)
            Rectangle hitJogador = Rectangle.Inflate(jogador.Bounds, -40, -20);

            for (int i = inimigos.Count - 1; i >= 0; i--)
            {
                PictureBox inimigo = inimigos[i];
                inimigo.Left -= velocidadeInimigo;

                // Saiu pela esquerda: remove
                if (inimigo.Right < 0)
                {
                    RemoverInimigo(i);
                    continue;
                }

                Rectangle hitInimigo = Rectangle.Inflate(inimigo.Bounds, -10, -10);

                if (invencivel == 0 && hitJogador.IntersectsWith(hitInimigo))
                {
                    RemoverInimigo(i);
                    PerderVida();
                    if (fimDeJogo) return;
                }
            }
        }

        private void RemoverInimigo(int indice)
        {
            PictureBox inimigo = inimigos[indice];
            Controls.Remove(inimigo);
            inimigo.Dispose();
            inimigos.RemoveAt(indice);
        }

        private void PerderVida()
        {
            vidas--;
            lblVidas.Text = "VIDAS: " + vidas;
            invencivel = QuadrosInvencivel;

            if (vidas <= 0)
            {
                MostrarGameOver();
            }
        }

        private void MostrarGameOver()
        {
            fimDeJogo = true;
            relogio.Stop();
            timerDisparos.Stop();
            esquerda = false;
            direita = false;
            jogador.Visible = true;

            lblResultado.Text = "BITS: " + pontos + "  |  INIMIGOS: " + inimigosDerrotados;

            lblGameOver.Left = (ClientSize.Width - lblGameOver.Width) / 2;
            lblGameOver.Top = ClientSize.Height / 5;

            lblResultado.Left = (ClientSize.Width - lblResultado.Width) / 2;
            lblResultado.Top = lblGameOver.Bottom;

            btnReiniciar.Left = (ClientSize.Width - btnReiniciar.Width) / 2;
            btnReiniciar.Top = ClientSize.Height / 2 + 20;

            btnSairJogo.Left = btnReiniciar.Left;
            btnSairJogo.Top = btnReiniciar.Bottom + 30;

            lblGameOver.Visible = true;
            lblResultado.Visible = true;
            btnReiniciar.Visible = true;
            btnSairJogo.Visible = true;

            // Garante que a tela de Game Over fique na frente de tudo
            lblGameOver.BringToFront();
            lblResultado.BringToFront();
            btnReiniciar.BringToFront();
            btnSairJogo.BringToFront();
        }

        private void ReiniciarJogo()
        {
            // Remove os disparos antigos antes de reiniciar
            foreach (Bullet bala in jogador.balas)
            {
                Controls.Remove(bala);
                bala.Dispose();
            }
            jogador.balas.Clear();

            // Remove todos os bugs da tela
            foreach (PictureBox inimigo in inimigos)
            {
                Controls.Remove(inimigo);
                inimigo.Dispose();
            }
            inimigos.Clear();

            // Zera o estado
            pontos = 0;
            inimigosDerrotados = 0;
            ultimoDisparoMs = -10000;
            esquerda = false;
            direita = false;
            vidas = VidasIniciais;
            invencivel = 0;
            velocidadeY = 0;
            proximoInimigo = 120;
            fimDeJogo = false;

            lblPontos.Text = "BITS: 0";
            lblVidas.Text = "VIDAS: " + vidas;
            lblCombate.Text = "INIMIGOS: 0";

            jogador.Visible = true;
            jogador.Location = new Point(100, chaoY - jogador.Height);

            // Esconde a tela de Game Over
            lblGameOver.Visible = false;
            lblResultado.Visible = false;
            btnReiniciar.Visible = false;
            btnSairJogo.Visible = false;
            ActiveControl = null;

            relogio.Start();
            timerDisparos.Start();
        }
    }
}