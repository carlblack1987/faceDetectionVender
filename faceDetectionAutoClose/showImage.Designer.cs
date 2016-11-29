namespace faceDetectionAutoClose
{
    partial class showImage
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
            this.imageBox_Main = new Emgu.CV.UI.ImageBox();
            this.btn_SavePicture = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox_Main
            // 
            this.imageBox_Main.Location = new System.Drawing.Point(12, 12);
            this.imageBox_Main.Name = "imageBox_Main";
            this.imageBox_Main.Size = new System.Drawing.Size(786, 473);
            this.imageBox_Main.TabIndex = 3;
            this.imageBox_Main.TabStop = false;
            // 
            // btn_SavePicture
            // 
            this.btn_SavePicture.Location = new System.Drawing.Point(509, 491);
            this.btn_SavePicture.Name = "btn_SavePicture";
            this.btn_SavePicture.Size = new System.Drawing.Size(137, 31);
            this.btn_SavePicture.TabIndex = 4;
            this.btn_SavePicture.Text = "Save Picture";
            this.btn_SavePicture.UseVisualStyleBackColor = true;
            this.btn_SavePicture.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(661, 491);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(137, 31);
            this.btn_Close.TabIndex = 5;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // showImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 525);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_SavePicture);
            this.Controls.Add(this.imageBox_Main);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "showImage";
            this.Text = "showImage";
            this.Load += new System.EventHandler(this.showImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox_Main;
        private System.Windows.Forms.Button btn_SavePicture;
        private System.Windows.Forms.Button btn_Close;
    }
}