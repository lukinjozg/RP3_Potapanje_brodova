using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Potapanjebrodova
{
    public partial class Form2 : Form
    {
        private Label[] labels = new Label[100];
        private PictureBox[] pics = new PictureBox[10];
        private EventHandler[] LabelHandler = new EventHandler[100];

        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private int[,] protivnik_matrix = new int[10, 10];
        private bool[,] kliknuto = new bool[10, 10];
        
        
        private AI ai = new AI();
        private void InitializeLables()
        {
            //Postavljanje labela na tablelayoutpanel
            for (int i = 0; i < 100; i++)
            {
                labels[i] = Controls.Find($"label{i + 1}", true)[0] as Label;

                int k = i;

                if (k < 100)
                {
                    labels[k].Text = "";
                    labels[k].BackColor = Color.Transparent;
                    labels[k].Font = new Font("Microsoft YaHei", 22, FontStyle.Bold);
                    labels[k].Margin = Padding.Empty;
                    labels[k].Dock = DockStyle.Fill;
                }

                labels[k].TextAlign = ContentAlignment.MiddleCenter;
            }

            //uljepsavanje prozora
            Bitmap img = Properties.Resources.ResourceManager.GetObject("moregrid") as Bitmap;

            panel1.BackgroundImage = img;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            img = Properties.Resources.ResourceManager.GetObject("radar") as Bitmap;

            tableLayoutPanel1.BackgroundImage = img;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Stretch;

            img = Properties.Resources.ResourceManager.GetObject("stormysea") as Bitmap;

            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //postavljanje slika brodova
            for (int i = 0; i < 5; i++)
            {
                int k = i;
                pics[k] = Controls.Find($"pictureBox{i + 2}", true)[0] as PictureBox;

                img = Properties.Resources.ResourceManager.GetObject($"boat{k}H") as Bitmap;

                pics[k].Image = img;
                pics[k].BackColor = Color.Transparent;
                pics[k].SizeMode = PictureBoxSizeMode.StretchImage;
            }

            //Postavljanje slika tezine i protivnika
            img = Properties.Resources.ResourceManager.GetObject(Program.tezina + "Difficulty") as Bitmap;
            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            label101.Text = "Protivnik: " + Program.tezina;

            for (int i = 0;i < 10;i++)
            {

            }
        }

        private void MakeAllLabelsClickable()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    LabelHandler[x * 10 + y] = (sender, e) => MakeMove(x, y);
                    labels[x * 10 + y].Click += LabelHandler[x * 10 + y];
                }
            }
        }

        //tu valjda postavimo brodove od protivnika
        private void InitializeMatrix(int lvl)
        {
            if (lvl > 0)
            {
                ai.setBattleshipsMediumHard(ref protivnik_matrix);
            }
            else
            {
                ai.setBattleshipsEasy(ref protivnik_matrix);
            }
        }

        private void RightMatrixFill()
        {
            for (int i = 0; i < 5; i++)
            {
                int k = i;

                Boats.AddBoatImageToPanel(panel1,$"boat{k}", Boats.boat_pos[k, 0], Boats.boat_pos[k, 1], Boats.boat_pos[k, 2], Boats.boat_pos[k, 3]);
            }
        }

        private void OpponentMakesMove(string s)
        {
            //tu ide ai
            int[] polje = { 1, 2, 3 };

            Tuple<int, int> tuple = new Tuple<int, int> (0,0);
            if(s == "easy")
            {
                tuple = ai.nextMoveEasy(Program.stanje);
            }

            else if (s == "medium")
            {
                tuple = ai.nextMoveIntermediate(Program.stanje);
            }

            /*
            else
            {
                tuple = ai.nextMoveHard(Program.stanje, polje);
            }*/

            int x = tuple.Item1;
            int y = tuple.Item2;
            
            bool hit = false;

            Program.stanje[x,y] = State.MISSED;

            if (Boats.igrac_matrix[x, y] != "")
            {
                hit = true;
                Program.stanje[x, y] = State.HIT;
            }

            Boats.AddExplosionImage(panel1, x, y, hit);
        }

        private void ZapisiPogodak(int x, int y)
        {
            labels[10 * x + y].Text = "X";
            labels[10 * x + y].ForeColor = Color.Red;
            labels[10 * x + y].BackColor = Color.FromArgb(128, Color.Red);
            FileOperation.UpdatePogodci(true);

        }

        private void ZapisiPromasaj(int x, int y)
        {
            labels[10 * x + y].Text = "O";
            labels[10 * x + y].ForeColor = Color.White;
            labels[10 * x + y].BackColor = Color.FromArgb(128, Color.White);
            FileOperation.UpdatePogodci(false);
        }

        private bool shipSinked(int vel)
        {
            int cnt = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (kliknuto[i, j] && protivnik_matrix[i, j] == vel)
                    {
                        cnt++;
                    }
                }
            }

            return (cnt == vel);
        }

        private void MakeMove(int x, int y)
        {
            if (kliknuto[x, y]) return;

            kliknuto[x, y] = true;
            if (protivnik_matrix[x, y] != 0)
            {
                ZapisiPogodak(x, y);
                if (shipSinked(protivnik_matrix[x, y]))
                {
                    //ispis na ekran ako je brod potopljen, u protivnik_matrix[x, y] je 
                    //velicina broda
                }
            }

            else
            {
                ZapisiPromasaj(x, y);
            }

            OpponentMakesMove(Program.tezina);
        }

        private void PotopiBrod(int brod_index)
        {

        }

        public Form2()
        {
            InitializeComponent();
            InitializeLables();
            InitializeMatrix(0);
            RightMatrixFill();
            MakeAllLabelsClickable();
        }
    }
}
