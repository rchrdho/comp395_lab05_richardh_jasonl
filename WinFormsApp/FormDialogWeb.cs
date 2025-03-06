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
    /// Represents a dialog form for entering a URL to load an image from the web.
    /// </summary>
    public partial class FormDialogWeb : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormDialogWeb"/> class
        /// and sets a default URL.
        /// </summary>
        public FormDialogWeb()
        {
            InitializeComponent();
            WebImageUrl = "https://www.w3schools.com/css/img_5terre.jpg"; // Default url
        }

        /// <summary>
        /// Gets or sets the URL string entered by the user.
        /// </summary>
        public string WebImageUrl { get; set; }

        /// <summary>
        /// Handles the Click event of the OK button. Validates the URL
        /// and closes the form with a DialogResult of OK if valid.
        /// </summary>
        /// <param name="sender">The source of the event (the OK button).</param>
        /// <param name="e">Provides data for the event.</param>
        private void WebDialogOk_Button_Click(object sender, EventArgs e)
        {
            WebImageUrl = txtURL.Text.Trim();

            if (string.IsNullOrEmpty(WebImageUrl))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Cancel button. Closes the form
        /// with a DialogResult of Cancel.
        /// </summary>
        /// <param name="sender">The source of the event (the Cancel button).</param>
        /// <param name="e">Provides data for the event.</param>
        private void WebDialogCancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
