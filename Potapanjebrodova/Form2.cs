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
        private bool[] potopljen = new bool[5];
        
        
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
            for (int i = 0; i < 10; i++)
            {
                int k = i;
                pics[k] = Controls.Find($"pictureBox{i + 2}", true)[0] as PictureBox;

                if(k < 5)
                {
                    img = Properties.Resources.ResourceManager.GetObject($"boat{k}H") as Bitmap;

                    pics[k].Image = img;
                    pics[k].BackColor = Color.Transparent;
                    pics[k].SizeMode = PictureBoxSizeMode.StretchImage;
                }  
            }

            //Postavljanje slika tezine i protivnika
            img = Properties.Resources.ResourceManager.GetObject(Program.tezina + "Difficulty") as Bitmap;
            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            label101.Text = "Protivnik: " + Program.tezina;
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

        //tu postavimo brodove od protivnika
        private void InitializeMatrix(int lvl)
        {
            if (lvl > 0)
            {
                ai.setBattleshipsMediumHard(ref protivnik_matrix, Boats.boats);
            }
            else
            {
                ai.setBattleshipsEasy(ref protivnik_matrix, Boats.boats);
            }
        }

        //punjenje igraceve matrice da se vidi koji brodovi su pogodzeni
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
            Tuple<int, int> tuple = new Tuple<int, int> (0,0);
            if(s == "easy")
            {
                tuple = ai.nextMoveEasy(Program.stanje);
            }

            else if (s == "medium")
            {
                tuple = ai.nextMoveIntermediate(Program.stanje);
            }

            else
            {
                List<int> lst = new List<int>();
                for(int i = 0; i < 5; i++)
                {
                    if (!Boats.sinked[i])
                    {
                        lst.Add(Boats.boats[i]);
                    }
                }
                tuple = ai.nextMoveHard(Program.stanje, lst);
            }

            //ai tu puca torpedo
            int x = tuple.Item1;
            int y = tuple.Item2;
            
            bool hit = false;

            Program.stanje[x,y] = State.MISSED;

            if (Boats.igrac_matrix[x, y] != "")
            {
                hit = true;
                Program.stanje[x, y] = State.HIT;
                for(int i = 0; i < 5; i++)
                {
                    if (!Boats.sinked[i])
                    {
                        Boats.sinked[i] = Boats.checkIfSinked(i);
                        if (Boats.sinked[i]) FileOperation.UpdateBrodovi(true);
                    }
                }
            }

            Boats.AddExplosionImage(panel1, x, y, hit);
        }

        //funckije za plocu gjde igrac gađa
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

        private bool shipSinked(int index)
        {
            int cnt = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (kliknuto[i, j] && protivnik_matrix[i, j] - 1 == index)
                    {
                        cnt++;
                    }
                }
            }

            return (cnt == Boats.boats[index]);
        }

        private void MakeMove(int x, int y)
        {
            if (kliknuto[x, y]) return;

            kliknuto[x, y] = true;
            if (protivnik_matrix[x, y] != 0)
            {
                ZapisiPogodak(x, y);
                if (shipSinked(protivnik_matrix[x, y] - 1))
                {
                    PotopioBrod(protivnik_matrix[x, y] - 1);
                    potopljen[protivnik_matrix[x, y] - 1] = true;
                    FileOperation.UpdateBrodovi(false);
                    //ispis na ekran ako je brod potopljen, u protivnik_matrix[x, y] - 1 je 
                    //index broda
                }
            }

            else
            {
                ZapisiPromasaj(x, y);
            }

            //nakon svakog poteza treba vidjeti jel igra zavrsila
            bool ret = false;
            if (checkIfGameEnd())
            {
                FileOperation.UpdatePobjede(true);
                MakeEndScreen(true);
                ret = true;
            }

            OpponentMakesMove(Program.tezina);

            if (checkIfGameEnd() && !ret)
            {
                FileOperation.UpdatePobjede(false);
                MakeEndScreen(false);
            }
        }

        //označava se koji su protivnikovi brodovi potopljeni
        private void PotopioBrod(int brod_index)
        {
            Bitmap img = Properties.Resources.ResourceManager.GetObject("explosion") as Bitmap;

            int index = 5 + brod_index;

            pics[index].Image = img;
            pics[index].SizeMode = PictureBoxSizeMode.StretchImage;
            pics[index].BackColor = Color.FromArgb(128, Color.Red);
        }

        private bool checkIfGameEnd()
        {
            bool ret = true;
            for(int  i = 0; i < 5; i++)
            {
                if (!Boats.sinked[i]) ret = false;
            }

            if (ret) return true;

            ret = true;
            for(int i = 0; i < 5; i++)
            {
                if (!potopljen[i]) ret = false;
            }

            return ret;
        }

        //ovo napravi zalon kada igra zavrsi
        private void MakeEndScreen(bool igracJePobjedio)
        {
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }

            Label label = new Label();
            string text = "Čestitam, pobjedili ste :)";

            if (!igracJePobjedio)
            {
                text = "Nažalost, protivnik je pobjedio :(";
            }
            
            label.Text = text;
            label.Font = new Font("Arial", 30, FontStyle.Bold);

            label.Size = new Size(300, 180);
            label.TextAlign = ContentAlignment.MiddleCenter;

            int x = (this.Width - label.Width) / 2;
            int y = this.Height / 2 - label.Height;

            label.Location = new Point(x, y);

            this.Controls.Add(label);

            Button but = new Button();

            but.Text = "Početni meni";
            but.Font = new Font("Arial", 12, FontStyle.Bold);
            but.Size = new Size(100, 60);
            but.TextAlign = ContentAlignment.MiddleCenter;

            x = (this.Width - but.Width) / 2;
            y = this.Height / 2 + but.Height;

            but.Location = new Point(x, y);
            but.Click += ButtonOpenPocetniScreen_Click;

            this.Controls.Add(but);
        }

        public Form2()
        {
            InitializeComponent();
            InitializeLables();

            if(Program.tezina == "easy")
            {
                InitializeMatrix(0);
            }
            else
            {
                InitializeMatrix(1);
            }
            
            RightMatrixFill();
            MakeAllLabelsClickable();
        }

        //funckije za zatvaranje forme i otvaranje nove
        private void ButtonOpenPocetniScreen_Click(object sender, EventArgs e)
        {
            Thread th;
            this.Close();
            th = new Thread(OpenForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void OpenForm()
        {
            Application.Run(new pocetniScreen());
        }
    }
}
