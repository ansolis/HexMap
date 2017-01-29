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

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Height = 10;

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
            int oldBufferSize = buffer.Height;
            
            if (buffer == null)
            {
                buffer = new Bitmap(pictureBox1.Width, 30);
            }
            else
            {
                Bitmap newBuffer = new Bitmap(buffer.Width, buffer.Height + 10);
                using (Graphics bufferGrph = Graphics.FromImage(newBuffer))
                    bufferGrph.DrawImageUnscaled(buffer, Point.Empty);
                buffer = newBuffer;
            }

            pictureBox1.Height = buffer.Height;

            Graphics g = System.Drawing.Graphics.FromImage(buffer);
            lastColorUsed += 20;
            Brush brush = new SolidBrush(Color.FromArgb(lastColorUsed));
            g.FillRectangle(brush, new Rectangle(0, oldBufferSize, buffer.Width, 10));

            pictureBox1.Image = buffer;
            pictureBox1.Refresh();
        }
    }
}
