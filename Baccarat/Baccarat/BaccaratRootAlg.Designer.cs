
namespace Midas.Baccarat
{
    partial class BaccaratRootAlg
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
            this.components = new System.ComponentModel.Container();
            this.lbl_ClickedReport = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.txt_1 = new System.Windows.Forms.TextBox();
            this.btn11 = new System.Windows.Forms.Button();
            this.btn10 = new System.Windows.Forms.Button();
            this.lbUnit = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.btnBackward = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSeeLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_ClickedReport
            // 
            this.lbl_ClickedReport.AutoSize = true;
            this.lbl_ClickedReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ClickedReport.ForeColor = System.Drawing.Color.Black;
            this.lbl_ClickedReport.Location = new System.Drawing.Point(125, 91);
            this.lbl_ClickedReport.Name = "lbl_ClickedReport";
            this.lbl_ClickedReport.Size = new System.Drawing.Size(184, 29);
            this.lbl_ClickedReport.TabIndex = 113;
            this.lbl_ClickedReport.Text = "Bắt đầu phiên....";
            // 
            // btnCalculate
            // 
            this.btnCalculate.BackColor = System.Drawing.Color.Silver;
            this.btnCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculate.Location = new System.Drawing.Point(22, 338);
            this.btnCalculate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(439, 102);
            this.btnCalculate.TabIndex = 112;
            this.btnCalculate.Text = "XÁC NHẬN\r\n(Nhấn Space)";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // txt_1
            // 
            this.txt_1.BackColor = System.Drawing.Color.White;
            this.txt_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_1.Location = new System.Drawing.Point(22, 20);
            this.txt_1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_1.Name = "txt_1";
            this.txt_1.ReadOnly = true;
            this.txt_1.Size = new System.Drawing.Size(439, 56);
            this.txt_1.TabIndex = 111;
            this.txt_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_1.TextChanged += new System.EventHandler(this.txt_1_TextChanged);
            this.txt_1.DoubleClick += new System.EventHandler(this.txt_8_DoubleClick);
            // 
            // btn11
            // 
            this.btn11.BackColor = System.Drawing.Color.Red;
            this.btn11.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn11.ForeColor = System.Drawing.Color.White;
            this.btn11.Location = new System.Drawing.Point(261, 147);
            this.btn11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn11.Name = "btn11";
            this.btn11.Size = new System.Drawing.Size(200, 146);
            this.btn11.TabIndex = 110;
            this.btn11.Text = "BANKER";
            this.btn11.UseVisualStyleBackColor = false;
            this.btn11.Click += new System.EventHandler(this.btn51_Click);
            this.btn11.MouseEnter += new System.EventHandler(this.btn10_MouseHover);
            this.btn11.MouseLeave += new System.EventHandler(this.btn120_MouseLeave);
            // 
            // btn10
            // 
            this.btn10.BackColor = System.Drawing.Color.Blue;
            this.btn10.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn10.ForeColor = System.Drawing.Color.White;
            this.btn10.Location = new System.Drawing.Point(18, 147);
            this.btn10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn10.Name = "btn10";
            this.btn10.Size = new System.Drawing.Size(200, 146);
            this.btn10.TabIndex = 109;
            this.btn10.Text = "PLAYER";
            this.btn10.UseVisualStyleBackColor = false;
            this.btn10.Click += new System.EventHandler(this.btn51_Click);
            this.btn10.MouseEnter += new System.EventHandler(this.btn10_MouseHover);
            this.btn10.MouseLeave += new System.EventHandler(this.btn120_MouseLeave);
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUnit.Location = new System.Drawing.Point(390, 524);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(31, 32);
            this.lbUnit.TabIndex = 119;
            this.lbUnit.Text = "u";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(173, 521);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 32);
            this.label17.TabIndex = 118;
            this.label17.Text = "==";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(35, 468);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 32);
            this.label12.TabIndex = 117;
            this.label12.Text = "Giá trị";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(250, 468);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(151, 32);
            this.label11.TabIndex = 116;
            this.label11.Text = "Khối lượng";
            // 
            // txtValue
            // 
            this.txtValue.BackColor = System.Drawing.Color.White;
            this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(22, 521);
            this.txtValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Size = new System.Drawing.Size(145, 38);
            this.txtValue.TabIndex = 115;
            this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtVolume
            // 
            this.txtVolume.BackColor = System.Drawing.Color.White;
            this.txtVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVolume.Location = new System.Drawing.Point(239, 521);
            this.txtVolume.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.ReadOnly = true;
            this.txtVolume.Size = new System.Drawing.Size(145, 38);
            this.txtVolume.TabIndex = 114;
            this.txtVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBackward
            // 
            this.btnBackward.BackgroundImage = global::Midas.Properties.Resources._160041651;
            this.btnBackward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBackward.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackward.Location = new System.Drawing.Point(22, 583);
            this.btnBackward.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(45, 44);
            this.btnBackward.TabIndex = 120;
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);
            this.btnBackward.MouseEnter += new System.EventHandler(this.btnBackward_MouseEnter);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.Control;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.Red;
            this.btnReset.Location = new System.Drawing.Point(81, 584);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(119, 44);
            this.btnReset.TabIndex = 121;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSeeLog
            // 
            this.btnSeeLog.BackColor = System.Drawing.SystemColors.Control;
            this.btnSeeLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSeeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeeLog.ForeColor = System.Drawing.Color.Black;
            this.btnSeeLog.Location = new System.Drawing.Point(217, 583);
            this.btnSeeLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSeeLog.Name = "btnSeeLog";
            this.btnSeeLog.Size = new System.Drawing.Size(119, 44);
            this.btnSeeLog.TabIndex = 122;
            this.btnSeeLog.Text = "LOG";
            this.btnSeeLog.UseVisualStyleBackColor = false;
            this.btnSeeLog.Click += new System.EventHandler(this.btnSeeLog_Click);
            // 
            // BaccaratRootAlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 660);
            this.Controls.Add(this.btnSeeLog);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnBackward);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.txtVolume);
            this.Controls.Add(this.lbl_ClickedReport);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.txt_1);
            this.Controls.Add(this.btn11);
            this.Controls.Add(this.btn10);
            this.Name = "BaccaratRootAlg";
            this.Text = "Giải thuật gốc cây.....rau muống";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaccaratQuad_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_ClickedReport;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.TextBox txt_1;
        private System.Windows.Forms.Button btn11;
        private System.Windows.Forms.Button btn10;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSeeLog;
    }
}