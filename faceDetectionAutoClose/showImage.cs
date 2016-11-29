using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace faceDetectionAutoClose
{
    public partial class showImage : Form
    {
        public PictureBox picBox = null;

        public String filePath = "image\\";

        public String fileTail = ".jpg";

        public showImage()
        {
            //Make the window can not be resized
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Save file
            String filename = filePath + DateTime.Now.ToString("yyyyMMddHH24mmss") + fileTail;
            imageBox_Main.Image.Save(filename);
            //Get file length to judge success
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Length > 0)
                MessageBox.Show("保存文件成功");
            else
                MessageBox.Show("保存文件失败");
        }

        private void showImage_Load(object sender, EventArgs e)
        {

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        public void setPicture(Image<Bgr,byte> img)
        {
            if(img != null)
                imageBox_Main.Image = img;
        }
    }
}
