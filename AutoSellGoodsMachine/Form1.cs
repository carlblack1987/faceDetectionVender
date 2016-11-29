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

using System.Timers;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace faceDetectionAutoClose
{
    public partial class faceDetectWin : Form
    {
        #region Status Code

        int STAT_SUC = 0; //Sucess
        int FIND_SUC = 1; //Find face
        int FIND_FAIL = 2; //Not find face
        int SAVE_FAIL = 3; //Saving file failed
        int CAPTURE_FAIL = 4; //Capture video failed

        #endregion

        #region Private Variables
        //Image from camera
        Mat frame;
        //Object of camera
        Capture capture;
        //Flag of find face
        int findFlag = 0;
        //To be loaded classifier for face detection
        //private string haarXmlPath = "classifier/haarcascade_frontalface_alt_tree.xml";
        private string haarXmlPath = "classifier/haarcascade_frontalface_alt2.xml";
        //Interval of face detection, unit: second
        private int detectInterval = 5;
        //Interval of out of time, unit: second
        private int successInterval = 5;
        //Interval of capturing face, unit: millisecond
        private int captureInterval = 200;
        //Current Time;
        DateTime sec;
        //Number of face
        int faceNum = 0;
        //Result of detection rate
        double detectRate = 0;
        //Threshold value for judging success of detection
        double successThres = 0.50;
        //Flag of detection result
        public bool detectResult = false;
        //Flag of save face image
        public bool isSaved = false;
        //Relative path of image file
        public String filePath = "faceImage\\";
        //Tail of image file
        public String fileTail = ".jpg";

        private Image<Bgr, Byte> frame2 = null;
        public Image<Gray, Byte> gray = null;
        Image<Bgr, Byte> faceArea = null;
        double scale = 1.5;
        //Timer for load image
        private System.Timers.Timer capture_tick;

        #endregion

        #region Private Functions

        private int startGetVideo()
        {
            try
            {
                capture = new Capture();
                frame = capture.QueryFrame();
                //根据视频窗口的大小调整窗口大小
                this.Width = frame.Width + 28;
                this.Height = frame.Height + 14;
                this.sec = DateTime.Now;

                capture_tick = new System.Timers.Timer();
                capture_tick.Interval = captureInterval;
                capture_tick.Enabled = Enabled;
                capture_tick.Start();
                capture_tick.Elapsed += new ElapsedEventHandler(CaptureProcess);
            }
            catch (Exception ex)
            {
                return CAPTURE_FAIL;
            }

            return STAT_SUC;
        }

        private void CaptureProcess(object sender, EventArgs arg)  
        {
            //frame = capture.QueryFrame();
            //getFaceInPic(frame, ref frame2, ref findFlag);
            //if(frame2 != null)
            //    imageBox_Input.Image = frame2;

            DateTime curTime = DateTime.Now;
            if ((curTime - this.sec).TotalSeconds > detectInterval)
            {
                capture_tick.Stop();
                frame = capture.QueryFrame();
                capture.Dispose();
                capture = null;
                detectRate = (double)this.faceNum / (successInterval * 1000 / captureInterval);
                //Judge the detection result
                if (detectRate >= successThres)
                    this.detectResult = true;

                this.Invoke(new Action(delegate
                {
                    this.Close();
                }));
                return;
            }
            else
            {
                frame = capture.QueryFrame();
                getFaceInPic(frame, ref frame2, ref findFlag);
                //Display image from camera
                if(frame2 != null)
                    imageBox_Input.Image = frame2;
                if (!isSaved && 1 == findFlag)
                {
                    savePicture(frame2);
                    isSaved = true;
                }

                //detectRate = (double)this.faceNum / (successInterval * 1000 / captureInterval);
                ////Judge the detection result
                //if (detectRate >= successThres)
                //{
                //    capture_tick.Stop();
                //    capture.Dispose();
                //    capture = null;
                //    this.detectResult = true;

                //    this.Invoke(new Action(delegate
                //    {
                //        this.Close();
                //    }));
                //    return;
                //}
            }
        }  

        #endregion

        public faceDetectWin()
        {
            //Make the window can not be resized
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            //Set the location of the dialog to display
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int x = (ScreenWidth - this.Width) / 2;
            int y = 10;
            this.Location = new Point(x, y);

            if (CAPTURE_FAIL == startGetVideo())
                throw new Exception("CAPTURE_FAIL");
        }

        public void destroy()
        {
            capture_tick.Stop();
            capture = null;
            frame = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        //Open the dialog of saving picture
        //private void btn_SavePicture_Click(object sender, EventArgs e)
        //{
        //    showImage win_Result = new showImage();
        //    getFaceInPic(capture.QueryFrame(), ref frame2, ref findFlag);
        //    win_Result.setPicture(frame2);
        //    win_Result.ShowDialog();

        //    win_Result.Dispose();
        //}

        private void getFaceInPic(Mat frame, ref Image<Bgr, byte> img, ref int flag)
        {
            if (frame != null)
            {

                //face detection
                img = frame.ToImage<Bgr, byte>();
                //frame = frame.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                //smallframe = frame2.Resize(1, Emgu.CV.CvEnum.Inter.Linear);//缩放摄像头拍到的大尺寸照片  
                gray = img.Convert<Gray, Byte>(); //Convert it to Grayscale
                CvInvoke.CvtColor(img, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                gray._EqualizeHist();//均衡化
                //Load the classifier
                CascadeClassifier ccr = new CascadeClassifier(haarXmlPath);
                Rectangle[] rects = ccr.DetectMultiScale(gray, 1.3, 3, new Size(20, 20), Size.Empty);
                if (rects.Length >= 1) flag = 1;
                else flag = 0;
                foreach (Rectangle r in rects)
                {
                    //Get the face part in the image and resize it
                    //faceArea = img.Copy(r).Resize(imageBox_Face.Width, imageBox_Face.Height, Emgu.CV.CvEnum.Inter.Cubic);
                    //imageBox_Face.Image = faceArea;

                    img.Draw(r, new Bgr(Color.GreenYellow), 2);//绘制检测框
                    this.faceNum++;
                }
            }
        }

        public double getDetectionRate()
        {
            return this.detectRate;
        }

        public bool getDetectionResult()
        {
            return this.detectResult;
        }

        public void setDetectInterval(int inter)
        {
            this.detectInterval = inter;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private int savePicture(Image<Bgr, byte>img)
        {
            //Save file
            String filename = filePath + DateTime.Now.ToString("yyyyMMddHHmmss") + fileTail;
            imageBox_Input.Image.Save(filename);
            //Get file length to judge success
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Length > 0) {
                //MessageBox.Show("保存文件成功");
                return STAT_SUC;
            }
            //MessageBox.Show("保存文件失败");
            return SAVE_FAIL;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageBox_Input_Click(object sender, EventArgs e)
        {

        }
    }
}