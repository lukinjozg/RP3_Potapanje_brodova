using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Potapanjebrodova
{
    public partial class Form2 : Form
    {
        private Label[] labels = new Label[200];
        private string[,] protivnik_matrix = new string[10, 10];

        private void InitializeLablesAndButtons()
        {
            for (int i = 0; i < 200; i++)
            {
                labels[i] = Controls.Find($"label{i + 1}", true)[0] as Label;
            }  
        }
        private void InitializeMatrix()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    protivnik_matrix[i, j] = "O";
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
                    labels[100 + 10*x + y].Text = matrix[x,y];
                }
            }
        }

        public Form2()
        {
            InitializeComponent();
            InitializeLablesAndButtons();
            InitializeMatrix();
            RightMatrixFill(Program.igrac_matrix);
        }
    }
}
