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
    public partial class NewImageDialog : Form
    {
        public Size SelectedSize { get; set; }

        public NewImageDialog()
        {
            InitializeComponent();
        }

        // <summary>
        /// Returns the image height corresponding with the radio button selected.
        /// </summary>
        /// <returns>the image height selected, an int</returns>
        public int GetHeight()
        {
            if (radioButton1.Checked)
            {
                return 640;
            }
            else if (radioButton2.Checked)
            {
                return 800;
            }
            else if (radioButton3.Checked)
            {
                return 1024;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the image width corresponding with the radio button selected.
        /// </summary>
        /// <returns>the image width selected, an int</returns>
        public int GetWidth()
        {
            if (radioButton1.Checked)
            {
                return 480;
            }
            else if (radioButton2.Checked)
            {
                return 600;
            }
            else if (radioButton3.Checked)
            {
                return 768;
            }
            else
            {
                return 0;
            }
        }


    }
}
