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
using System.IO;

namespace Potapanjebrodova
{
    public partial class Form1 : Form
    {
        private Thread th;
        private Label[] labels = new Label[100];
        private Button[] buttons = new Button[5];
        private EventHandler[] ButtonHandler = new EventHandler[5];
        private EventHandler[] LabelHandler = new EventHandler[100];

        private bool[] existing_buttons = new bool[] {true,true,true,true,true};
        private int[] boats = new int[] {2,3,3,4,5};
        private string[] boat_names = new string[] { "A", "B", "C", "D","E"};
        private void InitializeMatrix()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Program.igrac_matrix[i, j] = "";
                }
            }
        }

        private void ZapisiPozicijuBroda(int boat_index,int x1, int y1, int x2, int y2)
        {
            Program.boat_pos[boat_index,0] = x1;
            Program.boat_pos[boat_index,1] = y1;
            Program.boat_pos[boat_index,2] = x2;
            Program.boat_pos[boat_index,3] = y2;
        }

        private void InitializeLablesAndButtons()
        {
            PlayButton.Click += buttonOpenForm2_Click;
            //ayButton.Visible = false;
            for (int i = 0; i < 100; i++)
            {
                int k = i;
                labels[i] = Controls.Find($"label{i + 1}", true)[0] as Label;
                tableLayoutPanel1.Controls.Add(labels[k], k%10, k/10);
                labels[k].BackColor = Color.Transparent;
            }

            for (int i = 0; i < 5; i++)
            {
                int k = i;
                buttons[i] = Controls.Find($"button{i + 1}", true)[0] as Button;
                ButtonHandler[k] = (sender, e) => SelectBoat(k);

                string imageName = $"boat{k}H";
                Bitmap img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;

                buttons[i].Click += ButtonHandler[k];
                buttons[i].Image = img;
            }
        }

        private void ChangeLabelsToMatchMatix(string[,] matrix)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    labels[x * 10 + y].Text = matrix[x, y];
                }
            }
        }

        public Form1()
        {
            InitializeMatrix();
            InitializeComponent();

            InitializeLablesAndButtons();
            ChangeLabelsToMatchMatix(Program.igrac_matrix);
        }

        private bool InBoard(int i, int j)
        {
            return (i >= 0 && i < 10 && j >= 0 && j < 10);
        }

        private void PoredajMinPaMax(ref int i1, ref int i2)
        {
            if (i1 > i2)
            {
                int c = i1;
                i1 = i2;
                i2 = c;
            }
        }

        private bool CheckIfBoatCanStartHere(int x,int y, int boat_size)
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
        private bool CheckIfBoatCanBePlaced(int i1,int j1,int i2,int j2)
        {
            PoredajMinPaMax(ref i1,ref i2);
            PoredajMinPaMax(ref j1, ref j2);
           
            if (!(InBoard(i1,j1) && InBoard(i2, j2)))
            {
                return false;
            }

            for(int i = i1; i <= i2; i++)
            {
                int k = i;
                if (Program.igrac_matrix[k,j1] != "")
                {
                    return false;
                }
            }

            for (int j = j1; j <= j2; j++)
            {
                int k = j;
                if (Program.igrac_matrix[i1, k] != "")
                {
                    return false;
                }
            }

            return true;
        }

        private void PlaceBoat(int i1, int j1, int i2, int j2, int boat_index)
        {
            int boat_size = boats[boat_index] - 1;

            int[] smx = new int[] { -boat_size, boat_size, 0, 0 };
            int[] smy = new int[] { 0, 0, -boat_size, boat_size };

            for (int i = 0; i < 4; i++)
            {
                int k = i;
                if (CheckIfBoatCanBePlaced(i1, j1, i1 + smx[k], j1 + smy[k]))
                {
                    labels[(i1 + smx[k]) * 10 + j1 + smy[k]].Text = "";
                    labels[(i1 + smx[k]) * 10 + j1 + smy[k]].Click -= LabelHandler[(i1 + smx[k]) * 10 + j1 + smy[k]];
                }
            }

            PoredajMinPaMax(ref i1, ref i2);
            PoredajMinPaMax(ref j1, ref j2);

            ZapisiPozicijuBroda(boat_index, i1, j1, i2, j2);

            for (int i = i1; i <= i2; i++)
            {
                int k = i;
                Program.igrac_matrix[k, j1] = boat_names[boat_index];
            }

            for (int j = j1; j <= j2; j++)
            {
                int k = j;
                Program.igrac_matrix[i1, k] = boat_names[boat_index];
            }

            AddImageToTableLayoutPanel(boat_index,i1,j1,i2,j2);

            ChangeLabelsToMatchMatix(Program.igrac_matrix);
            
            bool flag = false;
            for (int i = 0; i < 5; i++)
            {
                int k = i;
                
                if (existing_buttons[k])
                {
                    ButtonHandler[k] = (sender, e) => SelectBoat(k);
                    buttons[i].Click += ButtonHandler[k];

                    flag = true;
                }  
            }

            if (!flag)
            {
                PlayButton.Visible = true;
                ChangeLabelsToMatchMatix(Program.igrac_matrix);
            }
        }

        private void SelectBoat(int boat_index)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;

                    if (Program.igrac_matrix[x,y] == "")
                    {
                        if (CheckIfBoatCanStartHere(x, y, boats[boat_index]))
                        {
                            LabelHandler[x * 10 + y] = (sender, e) => WhereToPlaceBoat(x, y, boat_index);
                            labels[x * 10 + y].Click += LabelHandler[x * 10 + y];
                        }
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                int k = i;
                buttons[k].Click -= ButtonHandler[k];
            }
        }

        private void WhereToPlaceBoat(int x, int y, int boat_index)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x1 = i, y1 = j;
                    labels[x1 * 10 + y1].Click -= LabelHandler[x1 * 10 + y1];
                }
            }

            buttons[boat_index].Visible = false;
            existing_buttons[boat_index] = false;
            int boat_size = boats[boat_index] - 1;

            labels[10*x + y].Text = boat_names[boat_index];

            int[] smx = new int[] { -boat_size, boat_size, 0, 0 };
            int[] smy = new int[] { 0, 0, - boat_size, boat_size};

            for(int i = 0;i < 4; i++)
            {
                int k = i;
                if(CheckIfBoatCanBePlaced(x,y,x + smx[k],y + smy[k]))
                {
                    labels[(x + smx[k]) * 10 + y + smy[k]].Text = "X";
                    LabelHandler[(x + smx[k]) * 10 + y + smy[k]] = (sender, e) => PlaceBoat(x, y, x + smx[k], y + smy[k], boat_index);
                    labels[(x + smx[k]) * 10 + y + smy[k]].Click += LabelHandler[(x + smx[k]) * 10 + y + smy[k]];
                }
            }
        }

        private void AddImageToTableLayoutPanel(int picture_index, int x1, int y1, int x2, int y2)
        {
            PoredajMinPaMax(ref x1, ref x2);
            PoredajMinPaMax(ref y1, ref y2);

            string smjer = "V";
            PictureBox picture = new PictureBox();

            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            for(int i = x1; i <= x2; i++)
            {
                int k = i;
                tableLayoutPanel1.Controls.Remove(labels[k * 10 + y1]);
            }

            if (x1 == x2)
            {
                for (int j = y1 + 1; j <= y2; j++)
                {
                    int k = j;
                    tableLayoutPanel1.Controls.Remove(labels[x1 * 10 + k]);
                }

                smjer = "H";
            }

            string imageName = $"boat{picture_index}" + smjer;
            Bitmap img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;
            picture.Image = img;
            picture.Dock = DockStyle.Fill;
            picture.BackColor = Color.Transparent;

            tableLayoutPanel1.SetRowSpan(picture, x2 - x1 + 1);
            tableLayoutPanel1.SetColumnSpan(picture, y2 - y1 + 1);

            tableLayoutPanel1.Controls.Add(picture, y1, x1);
        }
        private void buttonOpenForm2_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(OpenForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void OpenForm()
        {
            Application.Run(new Form2());
        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void PlayButton_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label59_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
