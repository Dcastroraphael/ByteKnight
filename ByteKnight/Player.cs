using System;
using System.Collections.Generic;
using System.Text;

namespace ByteKnight
{
    public class Player : PictureBox
    {
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
