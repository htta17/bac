namespace Baccarat
{
    partial class UtilCal
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
            this.btnGetResult = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetResult
            // 
            this.btnGetResult.Location = new System.Drawing.Point(12, 30);
            this.btnGetResult.Name = "btnGetResult";
            this.btnGetResult.Size = new System.Drawing.Size(141, 61);
            this.btnGetResult.TabIndex = 0;
            this.btnGetResult.Text = "Get Baccarat results";
            this.btnGetResult.UseVisualStyleBackColor = true;
            this.btnGetResult.Click += new System.EventHandler(this.btnGetResult_Click);
            // 
            // UtilCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 246);
            this.Controls.Add(this.btnGetResult);
            this.Name = "UtilCal";
            this.Text = "Utilities functions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetResult;
    }
}