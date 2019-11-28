using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_SquareTileV2._0
{
    public partial class Form1 : Form
    {
        private bool fourCorners = false;
        private List<Point> cornerPoints = new List<Point>(4);
        private Point northwest, northeast, southwest, southeast;
        private Point[,] pointMatrix;
        private SquareCalculations sqCalc = new SquareCalculations();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //////////////////////////////////////////////////////////////////////
        //upload button click functions

        private void btnUpload_Click(object sender, EventArgs e)
        {
            fourCorners = false;
            cornerPoints.Clear();
            //tileMap.Clear();
            pnlMap.Controls.Clear();

            string filepath = getMapImageFilepath();

            txtFilePath.Text = filepath;

            if (filepath != "")
            {
                changeMapPaneImage(filepath);
            }
        }

        private string getMapImageFilepath()
        {
            string filepath = "";
            //allow user to browse to file
            OpenFileDialog filePicker = new OpenFileDialog();
            filePicker.Filter = "All Files (*.*)|*.*";
            filePicker.FilterIndex = 1;
            filePicker.Multiselect = false;

            if (filePicker.ShowDialog() == DialogResult.OK)
            {
                filepath = filePicker.FileName;
            }

            return filepath;
        }//getMapImageFilepath()

        private void changeMapPaneImage(string filepath)
        {
            Image img = Image.FromFile(filepath);
            pnlMap.BackgroundImage = img;
        }//changeMapPaneImage()

        //////////////////////////////////////////////////////////////////////
        //Map Panel Click Functions

        private void pnlMapPaneImage_Click(object sender, EventArgs e)
        {
            if (!fourCorners)
            {
                captureMouseClick();
            }
        }//pnlMapPaneImage_Click()

        private void captureMouseClick()
        {
            Point pnt = pnlMap.PointToClient(Cursor.Position);
            paint(pnt);

            if (!fourCorners)
            {
                cornerPoints.Add(pnt);
                if (cornerPoints.Count == 4)
                {
                    fourCorners = true;
                    clacCorners();
                }
            }
        }//captureMouseClick()

        private void paint(Point p)
        {
            Graphics g = pnlMap.CreateGraphics();
            g.DrawEllipse(Pens.Black, p.X - 5, p.Y - 5, 10, 10);
        }//paint()

        private void paintR(Point p)
        {
            //Console.WriteLine(p.ToString());
            Graphics g = pnlMap.CreateGraphics();
            g.DrawEllipse(Pens.Red, p.X - 10, p.Y - 10, 20, 20);

            /*
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(
               fontFamily,
               8,
               FontStyle.Regular,
               GraphicsUnit.Point);
            RectangleF rectF = new RectangleF(10, 10, 500, 500);
            SolidBrush solidBrush = new SolidBrush(Color.Red);

            Point temp = new Point(p.X - 25, p.Y - 20);

            g.DrawString("(" + p.X + " " + p.Y + ")", font, solidBrush, temp);
            */
        }//paintR()

        private void clacCorners()
        {
            int x = 0, y = 0;
            foreach (Point p in cornerPoints)
            {
                x += p.X;
                y += p.Y;
            }

            x /= 4;
            y /= 4;

            foreach (Point p in cornerPoints)
            {
                if (p.X < x)
                {
                    if (p.Y < y)
                    {
                        northwest = p; //NW
                        Console.WriteLine("NW - " + northwest.ToString());
                    }
                    else
                    {
                        southwest = p; //SW
                        Console.WriteLine("SW - " + southwest.ToString());
                    }
                }
                else
                {
                    if (p.Y < y)
                    {
                        northeast = p;//NE
                        Console.WriteLine("NE - " + northeast.ToString());
                    }
                    else
                    {
                        southeast = p;//SE
                        Console.WriteLine("SE - " + southeast.ToString());
                    }
                }
            }

            cornerPoints[0] = northwest;
            cornerPoints[1] = northeast;
            cornerPoints[2] = southwest;
            cornerPoints[3] = southeast;

        }//clacCorners()

        ///////////////////////////////////////////////////////////////////
        //Calculate button click functions

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (txtHeight.Text != "" && txtWidth.Text != "")
            {
                int numTileWide = Convert.ToInt32(txtWidth.Text);
                int numTileHigh = Convert.ToInt32(txtHeight.Text);

                pointMatrix = sqCalc.calculateSquPoints(cornerPoints.ToArray(), numTileHigh, numTileWide);

                Console.WriteLine("return checkpoint");
                printPoints(pointMatrix, numTileHigh, numTileWide);
                Console.WriteLine("print checkpoint");

            }
        }//btnCalculate_Click()

        private void printPoints(Point[,] points, int height, int width)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.WriteLine("\t\t(" + x + "," + y + ") - " + points[x,y].X + " : " + points[x,y].Y);
                    if (points[x, y].X != 0 && points[x, y].Y != 0)
                    {
                        paintR(points[x, y]);
                    }

                }
            }
        }

    }
}
