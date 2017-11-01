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
    public partial class MultiGraph : Form
    {
        const int maxX = 20;
        const int maxY = 20;

        const int axesWidth = 2;
        const int graphWidth = 2;
        const int pointSize = 10;

        double step = 0.01;

        Color axesColor = Color.Black;

        Color graphColor = Color.Red;

        MathFunction[] funcs;
        Random rnd = new Random();

        public MultiGraph()
        {
            InitializeComponent();
        }
        public MultiGraph(params MathFunction[] funcs): this()
        {
            this.funcs = funcs;
        }

        private void SingleGraph_Load(object sender, EventArgs e)
        {
            Draw();
            
            foreach (MathFunction func in funcs)
                richTextBox1.Text += "F(x) = " + func.ToString();
        }

        private void Draw()
        {
            pictureBox1.Invalidate();

            Bitmap newBit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(newBit);

            DrawAxes(ref g);
            foreach (MathFunction func in funcs)
                DrawGraphic(ref g, func);

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
        private void DrawGraphic(ref Graphics g, MathFunction func)
        {
            PointF[] points = new PointF[(int)(maxX / step * 2)];

            int index = 0;

            for (double x = -maxX; x < maxX; x += step)
            {
                points[index] = new PointF(XToPixels(x), YToPixels(func.Calculate(x)));

                index++;
            }

            g.DrawLines(new Pen(GetRandColor(), graphWidth), points);
        }
        private Color GetRandColor()
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
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

            yPixel -= (int)(y * pictureBox1.Height / (maxY * 2));

            return yPixel;
        }
    }
}
