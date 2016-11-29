using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

using System.Timers;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables
        //Image from camera
        Mat frame;
        //Object of camera
        Capture capture;
        //Flag of find face
        int findFlag = 0;
        //To be loaded classifier for face detection
        private string haarXmlPath1 = "classifier/haarcascade_frontalface_alt2.xml";
        private string haarXmlPath2 = "classifier/haarcascade_eye.xml";
        //Interval of face detection
        int detectInterval = 5;
        //Current second;
        int sec;
        //Number of face
        int faceNum = 0;
        //Result of detection rate
        double detectRate = 0;
        //Threshold value for judging success of detection
        double successThres = 0.45;
        //Flag of detection result
        public bool detectResult = false;

        private bool capture_flag = true;
        private Image<Bgr, Byte> frame2 = null;
        public Image<Gray, Byte> gray = null;
        Image<Bgr, Byte> smallframe = null;
        Image<Bgr, Byte> faceArea = null;
        double scale = 1.5;
        //Timer for load image
        private System.Timers.Timer capture_tick;

        #endregion

        #region Private Functions

        private int startGetVideo()
        {
            capture = new Capture();
            this.sec = DateTime.Now.Second;

            capture_tick = new System.Timers.Timer();
            capture_tick.Interval = 100;
            capture_tick.Enabled = true;
            capture_tick.Start();
            capture_tick.Elapsed += new ElapsedEventHandler(CaptureProcess);

            return 1;
        }

        private void CaptureProcess(object sender, EventArgs arg)
        {
            int curSec = DateTime.Now.Second;
            if ((curSec - this.sec > detectInterval) || (curSec - this.sec < 0 && curSec - this.sec > 60 - detectInterval))
            {
                capture_tick.Stop();
                frame = capture.QueryFrame();
                capture.Dispose();
                capture = null;
                detectRate = (double)this.faceNum / (detectInterval * 10);

                if (detectRate >= successThres)
                    this.detectResult = true;

                this.Dispatcher.Invoke(new Action(delegate
                {
                    this.Close();
                }));
                //Trace.WriteLine("FaceNum:" + detectRate);
                //if (detectRate >= successThres)
                //    MessageBox.Show("Detection succeeded");
                //else
                //    MessageBox.Show("Detection failed");
                //System.Environment.Exit(0);
                return;
            }
            else
            {
                frame = capture.QueryFrame();
                getFaceInPic(frame, ref frame2, ref findFlag);
                if (frame2 != null)
                    //imageBox_Input.Image = frame2;
                    //_image.Source = ImageConverter.ToBitmapSource(frame2.Bitmap);
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        _image.Source = ImageConverter.ToBitmapSource(frame2.Bitmap);
                    }));
            }
        }

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
                CascadeClassifier ccr = new CascadeClassifier(haarXmlPath1);
                Rectangle[] rects = ccr.DetectMultiScale(gray, 1.3, 3, new System.Drawing.Size(20, 20), System.Drawing.Size.Empty);
                if (rects.Length == 1) flag = 1;
                else flag = 0;
                foreach (Rectangle r in rects)
                {
                    //Get the face part in the image and resize it
                    //faceArea = img.Copy(r).Resize(imageBox_Face.Width, imageBox_Face.Height, Emgu.CV.CvEnum.Inter.Cubic);
                    //imageBox_Face.Image = faceArea;
                    img.Draw(r, new Bgr(System.Drawing.Color.GreenYellow), 2);//绘制检测框
                    this.faceNum++;
                }
            }
        }

        #endregion

        private int i = 0;

        public MainWindow()
        {
            InitializeComponent();
            startGetVideo();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    _camera = new Capture();
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show(ee.Message);
            //}

            //_face = new HaarCascade(haarXmlPath1);
            //_eye = new HaarCascade(haarXmlPath2);
            //_timer.Interval = TimeSpan.FromMilliseconds(2);
            ////_timer.IsEnabled = true;
            //_timer.Tick += TakingVideo;
            //_timer.Start();
        }

        public double getDetectionRate()
        {
            return this.detectRate;
        }

        public bool getDetectionResult()
        {
            return this.detectResult;
        }
    }

    class ImageConverter
    {
        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(Bitmap image)
        {
            IntPtr ptr = image.GetHbitmap();
            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero,
                                                                                           Int32Rect.Empty,
                                                                                           System.Windows.Media.Imaging.
                                                                                                  BitmapSizeOptions.
                                                                                                  FromEmptyOptions());
            DeleteObject(ptr);
            return bs;
        }
    }
}
