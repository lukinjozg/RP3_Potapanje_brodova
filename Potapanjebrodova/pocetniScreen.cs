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
    public partial class pocetniScreen : Form
    {
        public pocetniScreen()
        {
            InitializeComponent();
            /*
             * Odigrano igara
             * Postotak Pobjede
             * Pobjede
             * Porazi
             * Potopljeno protivnika
             * Potonulo brodova
             * Pogoci
             * Postotak Pogodaka
             */
            string[] lines = File.ReadAllLines("userStats.txt");
            lines = lines.Take(lines.Length - 1).ToArray();
            string currUserStats = "\r\n" + string.Join("\r\n\r\n", lines);
            userStats.Text = currUserStats;
            
            Program.tezina = "";
        }

        private void easyAI_Click(object sender, EventArgs e)
        {
            Program.tezina = "easy";
            closeForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.tezina = "medium";
            closeForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.tezina = "hard";
            closeForm();
        }

        private void closeForm()
        {
            Thread th;
            this.Close();
            th = new Thread(OpenForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void OpenForm()
        {
            Application.Run(new Form1());
        }
    }
}
