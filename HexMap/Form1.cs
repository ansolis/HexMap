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
        public Form1()
        {
            InitializeComponent();
            //SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ////base.OnPaint(e);
            //System.Drawing.Pen myPen = new System.Drawing.Pen(Color.Aqua);
            //e.Graphics.DrawRectangle(myPen, new Rectangle(this.Location.X + 5,
            //                                              this.Location.Y + 5,
            //                                              this.Location.X + 10,
            //                                              this.Location.Y + 10));

            var p = sender as Panel;
            var g = e.Graphics;

            Brush brush = new SolidBrush(Color.DarkGreen);
            g.FillRectangle(new SolidBrush(Color.Black), p.DisplayRectangle);
            g.FillRectangle(brush, new Rectangle(5, 5, 15, 15));
        }
    }
}
