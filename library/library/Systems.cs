using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using library.Systems_methods;

namespace library
{
    public partial class Systems : Form
    {
        private static double[,] defaultMatrix = new double[,] { {5, 1, 1},
                                                      {-3, 8, 1},
                                                      {-2, 1, 4} };
        private static double[] defaultValues = new double[] { 13, 4, 5 };

        private static double defaultValue = 0;
        const string fColumnHeaderText = "f";

        public Systems()
        {
            InitializeComponent();
        }

        private void Systems_Load(object sender, EventArgs e)
        {
            InitializeDataGridViewForDefaults();
        }


        private void InitializeDataGridViewForDefaults()
        {
            for (int i = 0; i < defaultMatrix.GetLength(1) + 1; i++)
            {
                var column = new DataGridViewColumn();
                if (i == defaultMatrix.GetLength(1))
                    column.HeaderCell.Value = fColumnHeaderText;
                else
                    column.HeaderCell.Value = i.ToString();


                column.CellTemplate = new DataGridViewTextBoxCell();
                dataGridView1.Columns.Add(column);
            }

            for (int i = 0; i < defaultMatrix.GetLength(0); i++)
            {
                DataGridViewRow row = new DataGridViewRow();

                for (int j = 0; j < defaultMatrix.GetLength(1) + 1; j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell());

                    if (j == defaultMatrix.GetLength(1))
                        row.Cells[j].Value = defaultValues[i].ToString();
                    else
                        row.Cells[j].Value = defaultMatrix[i, j].ToString();
                }

                dataGridView1.Rows.Add(row);
            }
        }
        private void InitializeDataGridView(int count)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            for (int i = 0; i < count + 1; i++)
            {
                var column = new DataGridViewColumn();
                if (i == count)
                    column.HeaderCell.Value = fColumnHeaderText;
                else
                    column.HeaderCell.Value = i.ToString();

                column.CellTemplate = new DataGridViewTextBoxCell();
                dataGridView1.Columns.Add(column);
            }

            for (int i = 0; i < count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();

                for (int j = 0; j < count + 1; j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell());

                    row.Cells[j].Value = defaultValue;
                }

                dataGridView1.Rows.Add(row);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitializeDataGridView(Convert.ToInt32(textBox1.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;

            Matrix matrix = ParseCoeficientsNearVariables();
            Vector vect = ParseRightPart();

            SLAE system = new SLAE(matrix, vect);

            switch(index)
            {
                case 0:
                    system.Solve(new GaussMethod().Solve);
                    break;
                case 1:
                    system.Solve(new HoleckiiMethod().Solve);
                    break;
                case 2:
                    break;
                case 3:
                    system.Solve(new SimpleIterationSystem().Solve);
                    break;
                case 4:
                    system.Solve(new ZeidelMethod().Solve);
                    break;
                default:
                    break;
            }

            new Solution(system).Show();
        }

        private double[,] ParseCoeficientsNearVariables()
        {
            double[,] result = new double[dataGridView1.RowCount, dataGridView1.ColumnCount - 1];

            for (int i = 0; i < dataGridView1.RowCount; i++)
                for (int j = 0; j < dataGridView1.ColumnCount - 1; j++)
                    result[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);

            return result;
        }
        private double[] ParseRightPart()
        {
            double[] result = new double[dataGridView1.RowCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
                 result[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value);

            return result;
        }
    }
}
