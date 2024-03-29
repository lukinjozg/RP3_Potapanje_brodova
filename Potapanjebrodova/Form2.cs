﻿using System;
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
        private int[,] protivnik_matrix = new int[10, 10];
        private bool[,] kliknuto = new bool[10, 10];
        private EventHandler[] LabelHandler = new EventHandler[100];
        private AI ai = new AI();
        private void InitializeLables()
        {
            for (int i = 0; i < 100; i++)
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

            Bitmap img = Properties.Resources.ResourceManager.GetObject("moregrid") as Bitmap;

            panel1.BackgroundImage = img;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void MakeAllLabelsClickable()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (kliknuto[i, j]) continue;

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
                    if (kliknuto[i, j]) continue;

                    int x = i, y = j;
                    labels[x * 10 + y].Click -= LabelHandler[x * 10 + y];
                }
            }
        }
        //tu valjda postavimo brodove od protivnika
        private void InitializeMatrix(int level)
        {
            if(level > 0)
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
            for(int i = 0; i < 5; i++)
            {
                int k = i;

                AddBoatImageToPanel($"boat{k}",Program.boat_pos[k, 0], Program.boat_pos[k, 1], Program.boat_pos[k, 2], Program.boat_pos[k, 3]);
            }

            AddExplosionImage(0, 0, false);
        }

        async Task OpponentMakesMove(string s)
        {
            //tu ide ai
            Tuple<int, int> tuple = ai.nextMoveEasy(Program.stanje);

            await Task.Delay(500);
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

        private bool shipSinked(int vel)
        {
            int cnt = 0;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
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
            kliknuto[x, y] = true;

            if (protivnik_matrix[x,y] != 0)
            {
                ZapisiPogodak(x,y);
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

        private void AddBoatImageToPanel(string picture_name,int x1, int y1, int x2, int y2)
        {
            int ly = x2 - x1;
            int lx = y2 - y1;
            string smjer = "V";
            PictureBox picture = new PictureBox();

            int add = 0;

            if (picture_name[4] == '3')
            {
                add = 3;
            }

            if (x1 == x2)
            {
                smjer = "H";
            }

            string imageName = picture_name + smjer;
            
            Bitmap img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;

            picture.Image = img;
            picture.Size = new Size(32 * (lx + 1), 31 * (ly + 1));
            picture.BackColor = Color.Transparent;

            picture.Location = new Point(32*y1 - add, 31*x1);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            panel1.Controls.Add(picture);
        }

        private void AddExplosionImage(int x, int y, bool hit)
        {
            PictureBox picture = new PictureBox();
            Bitmap img = Properties.Resources.ResourceManager.GetObject("explosion") as Bitmap;
            
            picture.Image = img;
       
            
            picture.Size = new Size(32, 31);
            picture.Location = new Point(32*x, 31*y);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            if (hit)
            {
                picture.BackColor = Color.FromArgb(128, Color.Red);
            }

            else
            {
                picture.BackColor = Color.FromArgb(128, Color.Green);
            }
            

            panel1.Controls.Add(picture);
            panel1.Controls.SetChildIndex(picture, 0);
        }
    }
}
