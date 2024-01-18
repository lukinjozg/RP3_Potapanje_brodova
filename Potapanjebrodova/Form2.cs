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
        private string[,] protivnik_matrix = new string[10, 10];
        private EventHandler[] LabelHandler = new EventHandler[100];
        private AI ai = new AI();
        private void InitializeLables()
        {
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

            Bitmap img = Properties.Resources.ResourceManager.GetObject("moregrid") as Bitmap;

            panel1.BackgroundImage = img;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            img = Properties.Resources.ResourceManager.GetObject("radar") as Bitmap;

            tableLayoutPanel1.BackgroundImage = img;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Stretch;

            img = Properties.Resources.ResourceManager.GetObject("stormysea") as Bitmap;

            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            for (int i = 0; i < 5; i++)
            {
                int k = i;
                pics[k] = Controls.Find($"pictureBox{i + 2}", true)[0] as PictureBox;

                img = Properties.Resources.ResourceManager.GetObject($"boat{k}H") as Bitmap;

                pics[k].Image = img;
                pics[k].BackColor = Color.Transparent;
                pics[k].SizeMode = PictureBoxSizeMode.StretchImage;
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

        private void RemoveClicksFromAllLabels()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    labels[x * 10 + y].Click -= LabelHandler[x * 10 + y];
                }
            }
        }
        //tu valjda postavimo brodove od protivnika
        private void InitializeMatrix(int lvl)
        {
            int[,] shipPositions = new int[10, 10];
            if (lvl > 0)
            {
                ai.setBattleshipsMediumHard(ref shipPositions);
            }
            else
            {
                ai.setBattleshipsEasy(ref shipPositions);
            }
        }

        private void RightMatrixFill()
        {
            for (int i = 0; i < 5; i++)
            {
                int k = i;

                Boats.AddBoatImageToPanel(panel1,$"boat{k}", Program.boat_pos[k, 0], Program.boat_pos[k, 1], Program.boat_pos[k, 2], Program.boat_pos[k, 3]);
            }

            Boats.AddExplosionImage(panel1,0, 0, false);
        }

        async Task OpponentMakesMove(string s)
        {
            //tu ide ai

            await Task.Delay(5000);
            MakeAllLabelsClickable();
        }

        private void ZapisiPogodak(int x, int y)
        {
            labels[10 * x + y].Text = "X";
            labels[10 * x + y].ForeColor = Color.Red;
            labels[10 * x + y].BackColor = Color.FromArgb(128, Color.Red);

        }

        private void ZapisiPromasaj(int x, int y)
        {
            labels[10 * x + y].Text = "O";
            labels[10 * x + y].ForeColor = Color.White;
            labels[10 * x + y].BackColor = Color.FromArgb(128, Color.White);
        }

        private void MakeMove(int x, int y)
        {
            if (protivnik_matrix[x, y] != "")
            {
                ZapisiPogodak(x, y);
            }

            else
            {
                ZapisiPromasaj(x, y);
            }

            RemoveClicksFromAllLabels();

            OpponentMakesMove(/* Tu kasnije ide tezina koja je zapisana u Program.tezina*/"tezina");
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
