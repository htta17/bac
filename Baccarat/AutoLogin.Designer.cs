
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
            this.label4 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnCamera = new System.Windows.Forms.Button();
            this.btnTakePhoto = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(550, 417);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnCamera);
            this.tabPage1.Controls.Add(this.btnTakePhoto);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(542, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MANUAL";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 17);
            this.label4.TabIndex = 123;
            this.label4.Text = "Resolution";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(131, 143);
            this.numWidth.Maximum = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(120, 22);
            this.numWidth.TabIndex = 122;
            this.numWidth.Value = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 121;
            this.label3.Text = "Interval (min)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 120;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 119;
            this.label1.Text = "User name";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(131, 82);
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(120, 22);
            this.numInterval.TabIndex = 118;
            this.numInterval.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(131, 52);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(141, 22);
            this.txtPassword.TabIndex = 117;
            this.txtPassword.Text = "makeitreal1";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(131, 21);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(141, 22);
            this.txtUserName.TabIndex = 116;
            this.txtUserName.Text = "anhforex85";
            // 
            // btnCamera
            // 
            this.btnCamera.BackgroundImage = global::Midas.Properties.Resources.camera_icon_png;
            this.btnCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.Location = new System.Drawing.Point(8, 263);
            this.btnCamera.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(45, 44);
            this.btnCamera.TabIndex = 115;
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnTakePhoto
            // 
            this.btnTakePhoto.Location = new System.Drawing.Point(64, 263);
            this.btnTakePhoto.Name = "btnTakePhoto";
            this.btnTakePhoto.Size = new System.Drawing.Size(86, 44);
            this.btnTakePhoto.TabIndex = 1;
            this.btnTakePhoto.Text = "Start ";
            this.btnTakePhoto.UseVisualStyleBackColor = true;
            this.btnTakePhoto.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(8, 313);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(142, 53);
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
            this.tabPage2.Size = new System.Drawing.Size(542, 324);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(86, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 124;
            this.label5.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(81, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 17);
            this.label6.TabIndex = 126;
            this.label6.Text = "Height";
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(131, 171);
            this.numHeight.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(120, 22);
            this.numHeight.TabIndex = 125;
            this.numHeight.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.numHeight);
            this.groupBox1.Controls.Add(this.numInterval);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numWidth);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 202);
            this.groupBox1.TabIndex = 127;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setting";
            // 
            // AutoLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 417);
            this.Controls.Add(this.tabControl1);
            this.Name = "AutoLogin";
            this.Text = "AutoLogin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoLogin_FormClosing);
            this.Load += new System.EventHandler(this.AutoLogin_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnTakePhoto;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}