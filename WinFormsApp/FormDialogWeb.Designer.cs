namespace WinFormsApp
{
    partial class FormDialogWeb
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtURL = new TextBox();
            WebDialogOk_Button = new Button();
            WebDialogCancel_Button = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(112, 89);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "Location:";
            // 
            // txtURL
            // 
            txtURL.Location = new Point(192, 134);
            txtURL.Name = "txtURL";
            txtURL.Text = "https://www.w3schools.com/css/img_5terre.jpg";
            txtURL.Size = new Size(371, 23);
            txtURL.TabIndex = 1;
            // 
            // WebDialogOk_Button
            // 
            WebDialogOk_Button.AutoSize = true;
            WebDialogOk_Button.Location = new Point(260, 218);
            WebDialogOk_Button.Name = "WebDialogOk_Button";
            WebDialogOk_Button.Size = new Size(75, 25);
            WebDialogOk_Button.TabIndex = 2;
            WebDialogOk_Button.Tag = "";
            WebDialogOk_Button.Text = "OK";
            WebDialogOk_Button.UseVisualStyleBackColor = true;
            WebDialogOk_Button.Click += WebDialogOk_Button_Click;
            // 
            // WebDialogCancel_Button
            // 
            WebDialogCancel_Button.Location = new Point(423, 218);
            WebDialogCancel_Button.Name = "WebDialogCancel_Button";
            WebDialogCancel_Button.Size = new Size(75, 23);
            WebDialogCancel_Button.TabIndex = 3;
            WebDialogCancel_Button.Text = "Cancel";
            WebDialogCancel_Button.UseVisualStyleBackColor = true;
            WebDialogCancel_Button.Click += WebDialogCancel_Button_Click;
            // 
            // FormDialogWeb
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(WebDialogCancel_Button);
            Controls.Add(WebDialogOk_Button);
            Controls.Add(txtURL);
            Controls.Add(label1);
            Name = "FormDialogWeb";
            Text = "FormWeb";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtURL;
        private Button WebDialogOk_Button;
        private Button WebDialogCancel_Button;
    }
}