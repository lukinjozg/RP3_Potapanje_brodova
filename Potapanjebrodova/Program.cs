using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Potapanjebrodova
{
    internal static class Program
    {
        
        public static string tezina = "";
        public static State[,] stanje = new State[10, 10];
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Boats.igrac_matrix = new string[10, 10];
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
            //Application.Run(new pocetniScreen());
        }
    }
}
