using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Potapanjebrodova
{
    public partial class Boats
    {
        public static string[,] igrac_matrix = new string[10, 10];

        public static void AddBoatImageToPanel(Panel panel, string picture_name, int x1, int y1, int x2, int y2)
        {
            int ly = x2 - x1;
            int lx = y2 - y1;

            int w = panel.Width / 10;
            int h = panel.Height / 10;

            string smjer = "V";
            PictureBox picture = new PictureBox();

            if (x1 == x2)
            {
                smjer = "H";
            }

            string imageName = picture_name + smjer;

            Bitmap img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;

            picture.Image = img;
            picture.Size = new Size(w * (lx + 1), h * (ly + 1));
            picture.BackColor = Color.Transparent;

            picture.Location = new Point(w * y1, h * x1);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            panel.Controls.Add(picture);
            panel.Controls.SetChildIndex(picture, 0);
        }

        public static void AddExplosionImage(Panel panel,int x, int y, bool hit)
        {
            int w = panel.Width / 10;
            int h = panel.Height / 10;

            PictureBox picture = new PictureBox();
            Bitmap img = Properties.Resources.ResourceManager.GetObject("explosion") as Bitmap;

            picture.Image = img;


            picture.Size = new Size(w, h);
            picture.Location = new Point(w * x, h * y);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            if (hit)
            {
                picture.BackColor = Color.FromArgb(128, Color.Red);
            }

            else
            {
                picture.BackColor = Color.FromArgb(128, Color.Green);
            }

            panel.Controls.Add(picture);
            panel.Controls.SetChildIndex(picture, 0);
        }
    }
}
