using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Potapanjebrodova
{
    internal class FileOperation
    {
        public static string filePath = Environment.CurrentDirectory + "\\userStats.txt";
        public static void UpdatePogodci(bool pogodak)
        {
            int hits, misses;

            using (StreamReader reader = new StreamReader(filePath))
            {
                for(int i = 0; i < 7; i++)
                {
                    reader.ReadLine();
                }

                string line = reader.ReadLine();
                string[] parts = line.Split(':');

                hits = int.Parse(parts[1]);

                line = reader.ReadLine();
                misses = int.Parse(line);

                if (pogodak) hits++;
                else misses++;
            }

            double percentage = (double)hits / (hits + misses);
            double roundedPercentage = (int)Math.Ceiling(percentage * 100);

            UpdateLine(6, $"Postotak Pogodaka: {roundedPercentage}");
            UpdateLine(7, $"Pogoci: {hits}");
            UpdateLine(8, misses.ToString());

        }

        public static void UpdatePobjede(bool win)
        {
            int wins, loses;

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                reader.ReadLine();

                string line = reader.ReadLine();
                string[] parts = line.Split(':');

                wins = int.Parse(parts[1]);

                line = reader.ReadLine();
                parts = line.Split(':');

                loses = int.Parse(parts[1]);

                if (win) wins++;
                else loses++;
            }

            double percentage = (double)wins / (wins + loses);
            double roundedPercentage = (int)Math.Ceiling(percentage * 100);

            UpdateLine(0, $"Odigrano igara: {wins + loses}");
            UpdateLine(1, $"Postotak Pobjede: {roundedPercentage}%");
            UpdateLine(2, $"Pobjede: {wins}");
            UpdateLine(3, $"Porazi: {loses}");
        }

        public static void UpdateBrodovi(bool moj_brod_potonuo)
        {
            int potopljeno, potonulo;

            using (StreamReader reader = new StreamReader(filePath))
            {
                for(int i = 0;i  < 4; i++)
                {
                    reader.ReadLine();
                }

                string line = reader.ReadLine();
                string[] parts = line.Split(':');

                potopljeno = int.Parse(parts[1]);

                line = reader.ReadLine();
                parts = line.Split(':');

                potonulo = int.Parse(parts[1]);

                if (moj_brod_potonuo) potonulo++;
                else potopljeno++;  
            }

            UpdateLine(4, $"Potopljeno Protivnika: {potopljeno}");
            UpdateLine(5, $"Potonulo Brodova: {potonulo}");
        }

        public static void UpdateLine(int lineNumber, string newText)
        {
            string[] lines = File.ReadAllLines(filePath);
            lines[lineNumber] = newText;
            File.WriteAllLines(filePath, lines);
        }
    }
}
