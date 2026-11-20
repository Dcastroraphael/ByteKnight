using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ByteKnight
{
    public class Bullet : PictureBox
    {
        private const int Velocidade = 10;
        private static readonly Image ImagemProjetil = Image.FromFile(
            Path.Combine(Application.StartupPath, "assets", "bala.png"));
        public int Direcao { get; }

        public Bullet(int x, int y, int direcao = 1)
        {
            Direcao = direcao < 0 ? -1 : 1;
            Location = new Point(x, y);
            Size = new Size(36, 24);
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
            Image = ImagemProjetil; // Recurso compartilhado; não pertence a cada bala.
        }

        public void Mover() => Left += Velocidade * Direcao;

        public bool SaiuDaTela() => Parent != null &&
            (Right < 0 || Left > Parent.ClientSize.Width);
    }
}
