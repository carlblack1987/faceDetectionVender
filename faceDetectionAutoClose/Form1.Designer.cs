namespace faceDetectionAutoClose
{
    partial class faceDetectWin
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
            this.imageBox_Input = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Input)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox_Input
            // 
            this.imageBox_Input.BackColor = System.Drawing.Color.Transparent;
            this.imageBox_Input.Location = new System.Drawing.Point(12, 15);
            this.imageBox_Input.Name = "imageBox_Input";
            this.imageBox_Input.Size = new System.Drawing.Size(686, 452);
            this.imageBox_Input.TabIndex = 2;
            this.imageBox_Input.TabStop = false;
            this.imageBox_Input.Click += new System.EventHandler(this.imageBox_Input_Click);
            // 
            // faceDetectWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(710, 492);
            this.Controls.Add(this.imageBox_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "faceDetectWin";
            this.ShowIcon = false;
            this.Text = "Face Detection";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Input)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox_Input;
    }
}

