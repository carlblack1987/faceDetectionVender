using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.IO.Pipes;

namespace AutoSellGoodsMachine.Message
{
    //自定义的消息类型，用于自动更新客户端与iVend程序之间的交互
    public static class iVend_Message
    {
        public const int USER = 0x0400;
        //关闭窗口消息
        public const int WM_CLOSE = USER + 101;
        //有更新
        public const int WM_UPDATE_EXISTS = USER + 102;
        //无更新
        public const int WM_UPDATE_NOTEXISTS = USER + 103;
        //更新内容下载完成
        public const int WM_UPDATE_DOWNLOADCOMPLETED = USER + 104;
        //可以开始更新
        public const int WM_UPDATE_INITIATEGRANTED = USER + 105;
        /// <summary>
        /// 通信命名管道名称
        /// </summary>
        public const string pipeName = "iVendPipe";

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        //启动自动更新客户端监控线程
        public static void startProcessByName(Object processName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            Process pro = new Process();
            startInfo.FileName = processName + ".exe";
            pro.StartInfo = startInfo;

            try
            {
                while (true)
                {
                    Process[] p = Process.GetProcessesByName(processName.ToString());
                    if (p.Length == 1) {
                        break;
                    }
                    else
                    {
                        //干掉多余的进程
                        foreach (Process cur in p)
                        {
                            cur.Kill();
                        }
                    }
                    pro.Start();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception)
            {
                //return false;
            }

            //return true;
        }

        //根据名称关闭进程
        public static bool stopProcessByName(string processName)
        {
            try
            {
                while (true)
                {
                    Process[] p = Process.GetProcessesByName(processName.ToString());
                    if (p.Length < 1)
                    {
                        break;
                    }
                    else
                    {
                        //干掉多余的进程
                        foreach (Process cur in p)
                        {
                            cur.Kill();
                        }
                    }
                    Thread.Sleep(3000);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //发送数据到命名管道
        public static void SendData(object msg)
        {
            try
            {
                NamedPipeClientStream _pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None, System.Security.Principal.TokenImpersonationLevel.Impersonation);
                _pipeClient.Connect();
                StreamWriter sw = new StreamWriter(_pipeClient);
                sw.WriteLine(msg);
                sw.Flush();
                Thread.Sleep(1000);
                sw.Close();
            }
            catch (Exception ex)
            {
                //Log.WriteLog(ex.Message);
                return;
            }
        }

    }
}
