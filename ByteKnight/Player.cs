using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ByteKnight
{
    public class Player : PictureBox
    {
        public List<Bullet> balas { get; } = new List<Bullet>();
        public int Direcao { get; private set; } = 1;

        public void OlharPara(int direcao)
        {
            Direcao = direcao < 0 ? -1 : 1;
        }

        public Bullet Atirar()
        {
            int x = Direcao > 0 ? Right - 12 : Left - 24;
            int y = Bottom - 50; // Alinha o projétil aos inimigos que caminham no chão.
            Bullet bala = new Bullet(x, y, Direcao);
            balas.Add(bala);
            return bala;
        }

        public Player()
        {
            Size = new Size(160, 160);
            SizeMode = PictureBoxSizeMode.StretchImage;
            BackColor = Color.Transparent;
            string caminho = Path.Combine(Application.StartupPath, "assets", "cavaleiro.png");
            Image = Image.FromFile(caminho);
        }
    }
}
