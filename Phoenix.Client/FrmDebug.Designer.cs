
namespace Phoenix.Client
{
    partial class FrmDebug
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
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbDebug
            // 
            this.rtbDebug.BackColor = System.Drawing.Color.Black;
            this.rtbDebug.ForeColor = System.Drawing.Color.MistyRose;
            this.rtbDebug.Location = new System.Drawing.Point(12, 12);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbDebug.Size = new System.Drawing.Size(719, 357);
            this.rtbDebug.TabIndex = 0;
            this.rtbDebug.Text = "";
            // 
            // FrmDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 381);
            this.Controls.Add(this.rtbDebug);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDebug";
            this.Text = "FrmDebug";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbDebug;
    }
}