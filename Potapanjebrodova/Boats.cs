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
        public static int[] boats = new int[] { 2, 3, 3, 4, 5 };
        public static string[] boat_names = new string[] { "A", "B", "C", "D", "E" };
        public static int[,] boat_pos = new int[5, 4];
        public static bool[] sinked = new bool[5];

        public static void ZapisiPozicijuBroda(int boat_index, int x1, int y1, int x2, int y2)
        {
            boat_pos[boat_index, 0] = x1;
            boat_pos[boat_index, 1] = y1;
            boat_pos[boat_index, 2] = x2;
            boat_pos[boat_index, 3] = y2;
        }

        private static int determineDirection(int x1, int x2)
        {
            if (x1 > x2) return -1;
            else if (x1 == x2) return 0;
            else return 1;
        }

        public static bool checkIfSinked(int boat_index)
        {
            int sx = determineDirection(boat_pos[boat_index, 0], boat_pos[boat_index, 2]);
            int sy = determineDirection(boat_pos[boat_index, 1], boat_pos[boat_index, 3]);

            int x = boat_pos[boat_index, 0];
            int y = boat_pos[boat_index, 1];

            bool ret = true;
            for (int i = 0; i < boats[boat_index]; i++)
            {
                if (Program.stanje[x, y] != State.HIT) ret = false;
                x += sx;
                y += sy;
            }

            if (ret)
            {
                x = boat_pos[boat_index, 0];
                y = boat_pos[boat_index, 1];
                for (int i = 0; i < boats[boat_index]; i++)
                {
                    Program.stanje[x, y] = State.SINKED;
                    x += sx;
                    y += sy;
                }
            }

            return ret;
        }

        public static bool InBoard(int i, int j)
        {
            return (i >= 0 && i < 10 && j >= 0 && j < 10);
        }

        public static void PoredajMinPaMax(ref int i1, ref int i2)
        {
            if (i1 > i2)
            {
                (i2, i1) = (i1, i2);
            }
        }

        public static bool CheckIfBoatCanStartHere(int x, int y, int boat_size)
        {
            boat_size -= 1;

            int[] smx = new int[] { -boat_size, boat_size, 0, 0 };
            int[] smy = new int[] { 0, 0, -boat_size, boat_size };

            for (int i = 0; i < 4; i++)
            {
                int k = i;
                if (CheckIfBoatCanBePlaced(x, y, x + smx[k], y + smy[k]))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CheckIfBoatCanBePlaced(int i1, int j1, int i2, int j2)
        {
            PoredajMinPaMax(ref i1, ref i2);
            PoredajMinPaMax(ref j1, ref j2);

            if (!(InBoard(i1, j1) && InBoard(i2, j2)))
            {
                return false;
            }

            for (int i = i1; i <= i2; i++)
            {
                int k = i;
                if (igrac_matrix[k, j1] != "")
                {
                    return false;
                }
            }

            for (int j = j1; j <= j2; j++)
            {
                int k = j;
                if (igrac_matrix[i1, k] != "")
                {
                    return false;
                }
            }

            return true;
        }

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
            picture.Location = new Point(w * y, h * x);
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
