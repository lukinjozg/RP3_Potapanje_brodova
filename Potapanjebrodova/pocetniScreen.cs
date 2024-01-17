using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            string currUserStats = string.Join("\r\n\r\n", lines);
            userStats.Text = currUserStats;
        }
    }
}
