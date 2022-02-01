
namespace Midas
{
    partial class AutoLogin
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnTakePhoto = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnCamera = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(549, 575);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnCamera);
            this.tabPage1.Controls.Add(this.btnTakePhoto);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(541, 546);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MANUAL";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnTakePhoto
            // 
            this.btnTakePhoto.Location = new System.Drawing.Point(31, 150);
            this.btnTakePhoto.Name = "btnTakePhoto";
            this.btnTakePhoto.Size = new System.Drawing.Size(166, 53);
            this.btnTakePhoto.TabIndex = 1;
            this.btnTakePhoto.Text = "Start ";
            this.btnTakePhoto.UseVisualStyleBackColor = true;
            this.btnTakePhoto.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(31, 63);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(166, 53);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Login";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(541, 546);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnCamera
            // 
            this.btnCamera.BackgroundImage = global::Midas.Properties.Resources.camera_icon_png;
            this.btnCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.Location = new System.Drawing.Point(31, 218);
            this.btnCamera.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(45, 44);
            this.btnCamera.TabIndex = 115;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // AutoLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 575);
            this.Controls.Add(this.tabControl1);
            this.Name = "AutoLogin";
            this.Text = "AutoLogin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoLogin_FormClosing);
            this.Load += new System.EventHandler(this.AutoLogin_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnTakePhoto;
        private System.Windows.Forms.Button btnCamera;
    }
}