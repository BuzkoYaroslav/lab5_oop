using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library
{
    public partial class Table : Form
    {
        const string lPolyString = "L(x)";
        const string pPolyString = "P(x)";
        const string qPolyString = "Q(x)";
        const string xString = "X";

        const int digitsAfterComma = 3;

        public Table()
        {
            InitializeComponent();
        }
        public Table(Polynomial LPoly, Polynomial PPoly, Polynomial QPoly, double[] x, double[] f): this()
        {
            InitializeDataGridView(LPoly, PPoly, QPoly, x, f);
        }

        private void InitializeDataGridView(Polynomial LPoly, Polynomial PPoly, Polynomial QPoly, double[] x, double[] f)
        {
            DataGridViewColumn xCol = new DataGridViewColumn(),
                               lCol = new DataGridViewColumn(),
                               pCol = new DataGridViewColumn(),
                               qCol = new DataGridViewColumn();

            ConfigureColumn(ref xCol, xString);
            ConfigureColumn(ref lCol, lPolyString);
            ConfigureColumn(ref pCol, pPolyString);
            ConfigureColumn(ref qCol, qPolyString);

            dataGridView1.Columns.Add(xCol);
            dataGridView1.Columns.Add(lCol);
            dataGridView1.Columns.Add(pCol);
            dataGridView1.Columns.Add(qCol);

            int n = LPoly.Power;

            for (int i = -1; i <= n; i++)
            {
                double xNew = 0;
                if (i == -1)
                    xNew = x[0] - (x[1] - x[0]) / 2;
                else if (i == n)
                    xNew = x[n] + (x[n] - x[n - 1]) / 2;
                else
                    xNew = x[i] + (x[i + 1] - x[i]) / 2;

                var newRow = new DataGridViewRow();

                newRow.HeaderCell.Value = i.ToString();
                for (int j = 0; j < 4; j++)
                    newRow.Cells.Add(new DataGridViewTextBoxCell());

                newRow.Cells[0].Value = Math.Round(xNew, digitsAfterComma);
                newRow.Cells[1].Value = Math.Round(LPoly.GetValue(xNew), digitsAfterComma);
                newRow.Cells[2].Value = Math.Round(PPoly.GetValue(xNew), digitsAfterComma);
                newRow.Cells[3].Value = Math.Round(QPoly.GetValue(xNew), digitsAfterComma);

                dataGridView1.Rows.Add(newRow);
            }
        } 

        private void ConfigureColumn(ref DataGridViewColumn col, string headerText)
        {
            col.HeaderCell.Value = headerText;

            col.CellTemplate = new DataGridViewTextBoxCell();
        }
    }
}
