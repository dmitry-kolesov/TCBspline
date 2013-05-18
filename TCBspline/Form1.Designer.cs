namespace TCBspline
{
    partial class Form1
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
            this.tensionBar = new System.Windows.Forms.TrackBar();
            this.biasBar = new System.Windows.Forms.TrackBar();
            this.continuityBar = new System.Windows.Forms.TrackBar();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.isCloseRB = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.tensionBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.biasBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.continuityBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tensionBar
            // 
            this.tensionBar.Location = new System.Drawing.Point(343, 611);
            this.tensionBar.Minimum = -10;
            this.tensionBar.Name = "tensionBar";
            this.tensionBar.Size = new System.Drawing.Size(104, 45);
            this.tensionBar.TabIndex = 7;
            // 
            // biasBar
            // 
            this.biasBar.Location = new System.Drawing.Point(563, 611);
            this.biasBar.Minimum = -10;
            this.biasBar.Name = "biasBar";
            this.biasBar.Size = new System.Drawing.Size(104, 45);
            this.biasBar.TabIndex = 6;
            // 
            // continuityBar
            // 
            this.continuityBar.Location = new System.Drawing.Point(453, 611);
            this.continuityBar.Minimum = -10;
            this.continuityBar.Name = "continuityBar";
            this.continuityBar.Size = new System.Drawing.Size(104, 45);
            this.continuityBar.TabIndex = 5;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1055, 571);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(340, 595);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tension";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 595);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Continuity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(560, 595);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Bias";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(992, 595);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 11;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // isCloseRB
            // 
            this.isCloseRB.AutoSize = true;
            this.isCloseRB.Location = new System.Drawing.Point(12, 593);
            this.isCloseRB.Name = "isCloseRB";
            this.isCloseRB.Size = new System.Drawing.Size(57, 17);
            this.isCloseRB.TabIndex = 12;
            this.isCloseRB.TabStop = true;
            this.isCloseRB.Text = "Closed";
            this.isCloseRB.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 616);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(57, 17);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Closed";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 668);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.isCloseRB);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tensionBar);
            this.Controls.Add(this.biasBar);
            this.Controls.Add(this.continuityBar);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.tensionBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.biasBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.continuityBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TrackBar tensionBar;
        internal System.Windows.Forms.TrackBar biasBar;
        internal System.Windows.Forms.TrackBar continuityBar;
        internal System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.RadioButton isCloseRB;
        private System.Windows.Forms.RadioButton radioButton1;

    }
}

