using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexMap
{
    public partial class Form1 : Form
    {
        private Bitmap buffer;
        private int lastColorUsed = 0;
        //private bool oldScrollbarVisibleValue = false;
        private SortedDictionary<uint, Byte> map = new SortedDictionary<uint, Byte>();
        private const int SQUARE_SIZE = 8;
        private const int SQUARE_MARGIN = 2;
        private const uint INVALID_INDEX = 0xFFFFFFFF;
        private uint indexSelected = INVALID_INDEX;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Height = 10;

            panel1.AutoScroll = false;
            panel1.HorizontalScroll.Visible = false;
            panel1.HorizontalScroll.Enabled = false;
            panel1.HorizontalScroll.Maximum = 0;
            panel1.AutoScroll = true;

            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = System.Drawing.Graphics.FromImage(buffer);
            Brush blackBrush = new SolidBrush(Color.Black);
            lastColorUsed = Color.Black.ToArgb();
            g.FillRectangle(blackBrush, new Rectangle(0, 0, buffer.Width, buffer.Height));
            pictureBox1.Image = buffer;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadLine();

            DrawHexImage();



            //int oldBufferSize = buffer.Height;

            //if (buffer == null)
            //{
            //    buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //}
            //else
            //{
            //    Bitmap newBuffer = new Bitmap(pictureBox1.Width, buffer.Height + 10);

            //    using (Graphics bufferGrph = Graphics.FromImage(newBuffer))
            //        bufferGrph.DrawImageUnscaled(buffer, Point.Empty);
            //    buffer = newBuffer;
            //}

            //pictureBox1.Height = buffer.Height;

            //if (panel1.VerticalScroll.Visible != oldScrollbarVisibleValue)
            //{
            //    oldScrollbarVisibleValue = panel1.VerticalScroll.Visible;
            //    // TODO:  Update only
            //}
            //else
            //{
            //    // TODO:  Redraw all
            //}

            //Graphics g = System.Drawing.Graphics.FromImage(buffer);
            //lastColorUsed += 20;
            //Brush brush = new SolidBrush(Color.FromArgb(lastColorUsed));
            //g.FillRectangle(brush, new Rectangle(0, oldBufferSize, buffer.Width, 10));

            //Brush yellowBrush = new SolidBrush(Color.Yellow);
            //g.FillRectangle(yellowBrush, new Rectangle(buffer.Width - 30, oldBufferSize, 1, 10));
            //g.FillRectangle(yellowBrush, new Rectangle(buffer.Width - 20, oldBufferSize, 1, 10));
            //g.FillRectangle(yellowBrush, new Rectangle(buffer.Width - 10, oldBufferSize, 1, 10));
            //g.FillRectangle(yellowBrush, new Rectangle(buffer.Width - 1, oldBufferSize, 1, 10));

            //pictureBox1.Image = buffer;
            ////pictureBox1.Refresh();
        }

        private void LoadLine()
        {
            string line;
            uint addressOffset = 0;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader("C:\\Users\\ansol\\OneDrive\\Documents\\file.hex");

            while ((line = file.ReadLine()) != null)
            {
                if (line[0] == ':')
                {
                    line = line.TrimStart(':');
                    int count = Convert.ToInt32(line.Substring(0, 2), 16);
                    uint address = Convert.ToUInt32(line.Substring(2, 4), 16);
                    int recType = Convert.ToInt32(line.Substring(6, 2), 16);
                    if (recType == 4)
                        addressOffset = Convert.ToUInt32(line.Substring(8, 4), 16) << 16;
                    if (recType == 0)
                    {
                        for (uint i = 0; i < count; i++)
                        {
                            map.Add(addressOffset + address + i, Convert.ToByte(line.Substring((int)(8 + i * 2), 2), 16));
                        }
                    }
                }
            }

            file.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = ((e.X) / SQUARE_SIZE).ToString();
            textBox2.Text = ((e.Y) / SQUARE_SIZE).ToString();

            uint index = (uint)(((e.Y/SQUARE_SIZE) * buffer.Width) / SQUARE_SIZE + e.X / SQUARE_SIZE);
            textBox3.Text = index.ToString();

            if (buffer != null)
            {
                if (map.Keys.Contains(index))
                {
                    textBox4.Text = map[index].ToString();
                }
                else
                {
                    textBox4.Text = string.Empty;
                }


                //Bitmap newBuffer = new Bitmap(buffer);
                //using (Graphics bufferGrph = Graphics.FromImage(newBuffer))
                //    bufferGrph.DrawImageUnscaled(buffer, Point.Empty);
                //buffer = newBuffer;
                //pictureBox1.Image = buffer;

                Graphics g = Graphics.FromImage(buffer);
                Brush blueBrush = new SolidBrush(Color.Blue);
                int widthSquares = buffer.Width / SQUARE_SIZE;

                if (indexSelected != INVALID_INDEX)
                {
                    Brush yellowBrush = new SolidBrush(Color.Yellow);
                    Brush blackBrush = new SolidBrush(Color.Black);
                    g.FillRectangle(blackBrush, new Rectangle((int)(indexSelected % widthSquares) * SQUARE_SIZE,
                                                              (int)(indexSelected / widthSquares) * SQUARE_SIZE,
                                                              SQUARE_SIZE,
                                                              SQUARE_SIZE));
                    g.FillRectangle(yellowBrush, new Rectangle((int)(indexSelected % widthSquares) * SQUARE_SIZE,
                                                               (int)(indexSelected / widthSquares) * SQUARE_SIZE,
                                                               SQUARE_SIZE - SQUARE_MARGIN,
                                                               SQUARE_SIZE - SQUARE_MARGIN));
                }
                indexSelected = index;
                g.FillRectangle(blueBrush, new Rectangle((int)(index % widthSquares) * SQUARE_SIZE,
                                                         (int)(index / widthSquares) * SQUARE_SIZE,
                                                         SQUARE_SIZE,
                                                         SQUARE_SIZE));
                pictureBox1.Image = buffer;
            }
        }

        private void DrawHexImage()
        {
            int widthSquares = (pictureBox1.Width - 15) / SQUARE_SIZE;
            int width = widthSquares * SQUARE_SIZE;
            int heightSquares = ((int)map.Keys.Last() / widthSquares) + 1;
            int height = heightSquares * SQUARE_SIZE;
            buffer = new Bitmap(width, height);
            pictureBox1.Height = height;
            Graphics g = Graphics.FromImage(buffer);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Brush blackBrush = new SolidBrush(Color.Black);

            foreach (uint key in map.Keys)
            {
                g.FillRectangle(blackBrush, new Rectangle((int)(key % widthSquares) * SQUARE_SIZE,
                                                          (int)(key / widthSquares) * SQUARE_SIZE,
                                                          SQUARE_SIZE,
                                                          SQUARE_SIZE));
                g.FillRectangle(yellowBrush, new Rectangle((int)(key % widthSquares) * SQUARE_SIZE,
                                                           (int)(key / widthSquares) * SQUARE_SIZE,
                                                           SQUARE_SIZE - SQUARE_MARGIN,
                                                           SQUARE_SIZE - SQUARE_MARGIN));
            }

            pictureBox1.Image = buffer;
        }
    }
}
