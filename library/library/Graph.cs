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
    public partial class Graph : Form
    {

        const int maxX = 20;
        const int maxY = 20;

        const int axesWidth = 2;
        const int graphWidth = 2;
        const int pointSize = 10;

        double step = 0.01;

        Polynomial lPoly, pPoly, qPoly;

        Color axesColor = Color.Black;

        Color lCol = Color.Red;
        Color pCol = Color.Blue;
        Color qCol = Color.Green;
        Color pointCol = Color.Violet;

        double[] x, f;

        public Graph()
        {
            InitializeComponent();
            InitLegendaColors();
        }
        public Graph(Polynomial LPoly, Polynomial PPoly, Polynomial QPoly, double[] x, double[] f): this()
        {
            lPoly = LPoly;
            pPoly = PPoly;
            qPoly = QPoly;
            this.x = x;
            this.f = f;
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            Draw();
        }

        private void InitLegendaColors()
        {
            lColor.BackColor = lCol;
            pColor.BackColor = pCol;
            qColor.BackColor = qCol;
            pointColor.BackColor = pointCol;
        }
        private void Draw()
        {
            pictureBox1.Invalidate();

            Bitmap newBit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(newBit);

            DrawAxes(ref g);
            DrawGraphics(ref g);
            DrawPoints(ref g);

            pictureBox1.Image = newBit;
        }
        private void DrawAxes(ref Graphics g)
        {
            g.DrawLine(new Pen(axesColor, axesWidth), (float)pictureBox1.Width / 2, 0, (float)pictureBox1.Width / 2, pictureBox1.Height);
            g.DrawLine(new Pen(axesColor, axesWidth), 0, (float)pictureBox1.Height / 2, pictureBox1.Width, (float)pictureBox1.Height / 2);

            g.DrawString("Y", new Font("Arial", 15), new SolidBrush(axesColor), new PointF((float)pictureBox1.Width / 2 + 10, 10));
            g.DrawString("X", new Font("Arial", 15), new SolidBrush(axesColor), new PointF(pictureBox1.Width - 30, (float)pictureBox1.Height / 2 + 10));

            for (int i = 0; i < maxX * 2; i++)
                g.DrawLine(new Pen(axesColor, axesWidth), (float)pictureBox1.Width / (maxX * 2) * i, (float)pictureBox1.Height / 2 - axesWidth * 2,
                    (float)pictureBox1.Width / (maxX * 2) * i, (float)pictureBox1.Height / 2 + axesWidth * 2);
            for (int j = 0; j < maxY * 2; j++)
                g.DrawLine(new Pen(axesColor, axesWidth), (float)pictureBox1.Width / 2 - axesWidth * 2, (float)pictureBox1.Height / (maxY * 2) * j,
                        (float)pictureBox1.Width / 2 + axesWidth * 2, (float)pictureBox1.Height / (maxY * 2) * j);
        }
        private void DrawPoints(ref Graphics g)
        {
            for (int i = 0; i < x.Length; i++)
                g.FillEllipse(new SolidBrush(pointCol), XToPixels(x[i]) - pointSize / 2, YToPixels(f[i]) - pointSize / 2,
                    pointSize, pointSize);
        }
        private void DrawGraphics(ref Graphics g)
        {
            PointF[] lPoints = new PointF[(int)(maxX / step * 2)],
                     pPoints = new PointF[(int)(maxX / step * 2)],
                     qPoints = new PointF[(int)(maxX / step * 2)];

            int index = 0;

            for (double x = -maxX; x < maxX; x += step)
            {
                lPoints[index] = new PointF(XToPixels(x), YToPixels(lPoly.GetValue(x)));
                pPoints[index] = new PointF(XToPixels(x), YToPixels(pPoly.GetValue(x)));
                qPoints[index] = new PointF(XToPixels(x), YToPixels(qPoly.GetValue(x)));

                index++;
            }

            g.DrawLines(new Pen(lCol, graphWidth), lPoints);
            g.DrawLines(new Pen(pCol, graphWidth), pPoints);
            g.DrawLines(new Pen(qCol, graphWidth), qPoints);
        }

        private int XToPixels(double x)
        {
            int xPixel = pictureBox1.Width / 2;

            xPixel += (int)(x * pictureBox1.Width / (maxX * 2));

            return xPixel;
        }
        private int YToPixels(double y)
        {
            int yPixel = pictureBox1.Height / 2;

            yPixel += (int)(y * pictureBox1.Height / (maxY * 2));

            return yPixel;
        }
    }
}
