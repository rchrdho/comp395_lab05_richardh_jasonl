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
    /// <summary>
    /// Represents a dialog form displayed when the user attempts to close
    /// the main form, providing options to save or cancel the closure.
    /// </summary>
    public partial class FormMainCloseDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormMainCloseDialog"/> class
        /// and sets up its components.
        /// </summary>
        public FormMainCloseDialog()
        {
            InitializeComponent();
        }
    }
}
