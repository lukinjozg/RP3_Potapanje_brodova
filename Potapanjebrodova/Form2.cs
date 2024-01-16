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
        private Label[] labels = new Label[200];
        private string[,] protivnik_matrix = new string[10, 10];
        private EventHandler[] LabelHandler = new EventHandler[100];
        private void InitializeLables()
        {
            tableLayoutPanel2.SendToBack();
            for (int i = 0; i < 200; i++)
            {
                labels[i] = Controls.Find($"label{i + 1}", true)[0] as Label;

                int k = i;

                if(k < 100)
                {
                    labels[k].Text = "";
                    labels[k].BackColor = Color.Transparent;
                    labels[k].Font = new Font("Microsoft YaHei", 26, FontStyle.Bold);
                }

                else
                {
                    labels[k].Text = "";
                    labels[k].BackColor = Color.Transparent;
                    labels[k].Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
                }

                labels[k].TextAlign = ContentAlignment.MiddleCenter;
            }  
        }

        private void MakeAllLabelsClickable()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    LabelHandler[x*10 + y] = (sender, e) => MakeMove(x,y);
                    labels[x * 10 + y].Click += LabelHandler[x*10 + y];
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
        private void InitializeMatrix()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    protivnik_matrix[i, j] = Program.igrac_matrix[i,j];
                }
            }
        }

        private void RightMatrixFill(string[,] matrix)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    labels[100 + 10 * x + y].Text = matrix[x,y];
                }
            }

            for(int i = 0;i < 5; i++)
            {
                int k = i;

                AddImageToTableLayoutPanel($"boat{k}", tableLayoutPanel2, Program.boat_pos[k, 0], Program.boat_pos[k, 1], Program.boat_pos[k, 2], Program.boat_pos[k, 3]);
            }

            AddExplosionImage(1, 1);
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
        }

        private void ZapisiPromasaj(int x, int y)
        {
            labels[10 * x + y].Text = "O";
            labels[10 * x + y].ForeColor = Color.White;
        }

        private void MakeMove(int x, int y)
        {
            if(protivnik_matrix[x,y] != "")
            {
                ZapisiPogodak(x,y);
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
            InitializeMatrix();
            RightMatrixFill(Program.igrac_matrix);
            MakeAllLabelsClickable();
        }

        private void AddImageToTableLayoutPanel(string picture_name, TableLayoutPanel table,int x1, int y1, int x2, int y2)
        {
            int dodaj_labels_ovisno_o_panelu = 0;

            if(table == tableLayoutPanel2)
            {
                dodaj_labels_ovisno_o_panelu = 100;
            }

            string smjer = "V";
            PictureBox picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            for (int i = x1; i <= x2; i++)
            {
                int k = i;
                table.Controls.Remove(labels[dodaj_labels_ovisno_o_panelu + k * 10 + y1]);
            }

            if (x1 == x2)
            {
                for (int j = y1 + 1; j <= y2; j++)
                {
                    int k = j;
                    table.Controls.Remove(labels[dodaj_labels_ovisno_o_panelu + x1 * 10 + k]);
                }

                smjer = "H";
            }

            if (picture_name[0] != 'b')
            {
                smjer = "";
            }

            string imageName = picture_name + smjer;
            Bitmap img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;
            picture.Image = img;
            picture.Dock = DockStyle.Fill;
            picture.BackColor = Color.Transparent;

            table.SetRowSpan(picture, x2 - x1 + 1);
            table.SetColumnSpan(picture, y2 - y1 + 1);

            table.Controls.Add(picture, y1, x1);
        }

        private void AddExplosionImage(int x, int y)
        {
            PictureBox picture = new PictureBox();
            Bitmap img = Properties.Resources.ResourceManager.GetObject("explosion") as Bitmap;
            
            picture.Image = img;
            picture.Location = new Point(478 + 32* y, 146 + 30 * x);
            picture.Size = new Size(32, 30);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            Controls.Add(picture);
        }
    }
}
