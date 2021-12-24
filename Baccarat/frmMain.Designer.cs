namespace Baccarat
{
    partial class frmMain
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
            this.btnCombination = new System.Windows.Forms.Button();
            this.btnBankerOnly2 = new System.Windows.Forms.Button();
            this.btnQuadruple = new System.Windows.Forms.Button();
            this.btnSimilator = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCombination
            // 
            this.btnCombination.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombination.Location = new System.Drawing.Point(115, 32);
            this.btnCombination.Name = "btnCombination";
            this.btnCombination.Size = new System.Drawing.Size(434, 64);
            this.btnCombination.TabIndex = 0;
            this.btnCombination.Text = "Thuật toán tổ hợp";
            this.btnCombination.UseVisualStyleBackColor = true;
            this.btnCombination.Click += new System.EventHandler(this.btnCombination_Click);
            // 
            // btnBankerOnly2
            // 
            this.btnBankerOnly2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBankerOnly2.Location = new System.Drawing.Point(115, 125);
            this.btnBankerOnly2.Name = "btnBankerOnly2";
            this.btnBankerOnly2.Size = new System.Drawing.Size(434, 64);
            this.btnBankerOnly2.TabIndex = 1;
            this.btnBankerOnly2.Text = "Thuật toán Banker Only 2";
            this.btnBankerOnly2.UseVisualStyleBackColor = true;
            this.btnBankerOnly2.Click += new System.EventHandler(this.btnBankerOnly2_Click);
            // 
            // btnQuadruple
            // 
            this.btnQuadruple.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuadruple.Location = new System.Drawing.Point(115, 215);
            this.btnQuadruple.Name = "btnQuadruple";
            this.btnQuadruple.Size = new System.Drawing.Size(434, 64);
            this.btnQuadruple.TabIndex = 2;
            this.btnQuadruple.Text = "Thuật toán Quadruple";
            this.btnQuadruple.UseVisualStyleBackColor = true;
            this.btnQuadruple.Click += new System.EventHandler(this.btnQuadruple_Click);
            // 
            // btnSimilator
            // 
            this.btnSimilator.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimilator.Location = new System.Drawing.Point(115, 299);
            this.btnSimilator.Name = "btnSimilator";
            this.btnSimilator.Size = new System.Drawing.Size(434, 64);
            this.btnSimilator.TabIndex = 3;
            this.btnSimilator.Text = "Giả lập";
            this.btnSimilator.UseVisualStyleBackColor = true;
            this.btnSimilator.Visible = false;
            this.btnSimilator.Click += new System.EventHandler(this.btnSimilator_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 387);
            this.Controls.Add(this.btnSimilator);
            this.Controls.Add(this.btnQuadruple);
            this.Controls.Add(this.btnBankerOnly2);
            this.Controls.Add(this.btnCombination);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chọn thuật toán ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCombination;
        private System.Windows.Forms.Button btnBankerOnly2;
        private System.Windows.Forms.Button btnQuadruple;
        private System.Windows.Forms.Button btnSimilator;
    }
}

