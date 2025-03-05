using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        // field Image
        private Image myImage;

        public Image Image
        {
            get
            {
                return myImage;
            }
            set
            {
                myImage = value;
                this.AutoScrollMinSize = myImage.Size;
            }
        }

        // <summary>
        /// Paint event handler for child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(myImage, this.AutoScrollPosition.X,
                this.AutoScrollPosition.Y, myImage.Width, myImage.Height);
        }
    }
}
