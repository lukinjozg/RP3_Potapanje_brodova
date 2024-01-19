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
        private readonly Label[] labels = new Label[100];
        private readonly Button[] buttons = new Button[5];
        private readonly EventHandler[] ButtonHandler = new EventHandler[5];
        private readonly EventHandler[] LabelHandler = new EventHandler[100];

        private bool[] existing_buttons = new bool[] { true, true, true, true, true };
        
        private void InitializeMatrix()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Boats.igrac_matrix[i, j] = "";
                }
            }
        }

        private void InitializeLablesAndButtons()
        {
            //gumb za krenuti sa igrom, prvo ga sakrimo
            PlayButton.Click += ButtonOpenForm2_Click;
            PlayButton.Visible = false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;
                    Label label = new Label
                    {
                        BackColor = Color.Transparent,
                        Size = new Size(44, 44),
                        Location = new Point(44 * y, 44 * x)
                    };

                    panel1.Controls.Add(label);

                    labels[10 * x + y] = label;
                }
            }

            Bitmap img;

            //tu brodove postavimo kao gumbe
            for (int i = 0; i < 5; i++)
            {
                int k = i;
                buttons[i] = Controls.Find($"button{i + 1}", true)[0] as Button;
                ButtonHandler[k] = (sender, e) => SelectBoat(k);

                string imageName = $"boat{k}H";
                img = Properties.Resources.ResourceManager.GetObject(imageName) as Bitmap;

                buttons[i].Click += ButtonHandler[k];
                buttons[i].Image = img;
            }

            //postavljanje pozadine
            img = Properties.Resources.ResourceManager.GetObject("stormysea") as Bitmap;

            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public Form1()
        {
            InitializeMatrix();
            InitializeComponent();
            InitializeLablesAndButtons();
        }

       
        private void PlaceBoat(int i1, int j1, int i2, int j2, int boat_index)
        {
            int boat_size = Boats.boats[boat_index] - 1;

            int[] smx = new int[] { -boat_size, boat_size, 0, 0 };
            int[] smy = new int[] { 0, 0, -boat_size, boat_size };

            labels[10 * i1 + j1].BackColor = Color.Transparent;

            for (int i = 0; i < 4; i++)
            {
                int k = i;
                if (Boats.CheckIfBoatCanBePlaced(i1, j1, i1 + smx[k], j1 + smy[k]))
                {
                    labels[(i1 + smx[k]) * 10 + j1 + smy[k]].BackColor = Color.Transparent;
                    labels[(i1 + smx[k]) * 10 + j1 + smy[k]].Click -= LabelHandler[(i1 + smx[k]) * 10 + j1 + smy[k]];
                }
            }

            //mijenjamo redosljed koordinata da možemo ispisati normalno pozicije brodova u igrac_matrix
            Boats.PoredajMinPaMax(ref i1, ref i2);
            Boats.PoredajMinPaMax(ref j1, ref j2);

            Boats.ZapisiPozicijuBroda(boat_index, i1, j1, i2, j2);

            for (int i = i1; i <= i2; i++)
            {
                int k = i;
                Boats.igrac_matrix[k, j1] = Boats.boat_names[boat_index];
            }

            for (int j = j1; j <= j2; j++)
            {
                int k = j;
                Boats.igrac_matrix[i1, k] = Boats.boat_names[boat_index];
            }

            Boats.AddBoatImageToPanel(panel1,$"boat{boat_index}", i1, j1, i2, j2);

            //onemogućili smo klikanje na brodove, i sad ga omogućujemo opet za brodove koje jos treba postaviti
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

            //tu gledamo ako su svi brodovi postavljeni onda je vrijeme za pokrenuti igru
            if (!flag)
            {
                PlayButton.Visible = true;
            }
        }

        //tu biramo od kuda ćemo krenuti s postavljanjem broda, gdje ćemo mu neki kraj postaviti
        private void SelectBoat(int boat_index)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int x = i, y = j;

                    if (Boats.igrac_matrix[x, y] == "")
                    {
                        //provjeravamo da ako je tu prvi kraj broda, ima li igdje mjesta za drugi kraj
                        if (Boats.CheckIfBoatCanStartHere(x, y, Boats.boats[boat_index]))
                        {
                            LabelHandler[x * 10 + y] = (sender, e) => WhereToPlaceBoat(x, y, boat_index);
                            labels[x * 10 + y].Click += LabelHandler[x * 10 + y];
                        }
                    }
                }
            }

            //drugi gumbi se nemogu kliknuti dok s ebrod postavlja
            for (int i = 0; i < 5; i++)
            {
                int k = i;
                buttons[k].Click -= ButtonHandler[k];
            }
        }

        //postavljanje drugog ruba broda broda
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

            int boat_size = Boats.boats[boat_index] - 1;

            labels[10 * x + y].BackColor = Color.FromArgb(128, Color.Orange);

            int[] smx = new int[] { -boat_size, boat_size, 0, 0 };
            int[] smy = new int[] { 0, 0, -boat_size, boat_size };

            for (int i = 0; i < 4; i++)
            {
                int k = i;
                if (Boats.CheckIfBoatCanBePlaced(x, y, x + smx[k], y + smy[k]))
                {
                    labels[(x + smx[k]) * 10 + y + smy[k]].BackColor = Color.FromArgb(64, Color.Green);

                    LabelHandler[(x + smx[k]) * 10 + y + smy[k]] = (sender, e) => PlaceBoat(x, y, x + smx[k], y + smy[k], boat_index);
                    labels[(x + smx[k]) * 10 + y + smy[k]].Click += LabelHandler[(x + smx[k]) * 10 + y + smy[k]];
                }
            }
        }

        //otvaranje nove forme
        private void ButtonOpenForm2_Click(object sender, EventArgs e)
        {
            Thread th;
            this.Close();
            th = new Thread(OpenForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void OpenForm()
        {
            Application.Run(new Form2());
        }
    }
}
