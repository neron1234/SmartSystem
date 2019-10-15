using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MMK.SmartSystem.Laser.Base.CustomControl
{
    public interface ISoftKeyHelper
    {
        /// <summary>
        /// 检测软键盘是否已经打开了
        /// </summary>
        /// <returns>如果打开，返回true，否则返回false</returns>
        bool IsKeyboardOpen();


        /// <summary>
        /// 打开软键盘。调用之前应先通过IsKeyboardOpen()来检查keyboard是否已经被打开，
        /// 如果已经打开，就不要再打开了
        /// </summary>
        void OpenKeyBoard(int x, int y, int width, int height);


        /// <summary>
        /// 关闭软键盘
        /// </summary>
        void CloseKeyBoard();
    }

    public class SoftKeyBoard
    {
        /// <summary>
        /// 软件盘打开或关闭的帮助类，暴露了IsKeyboardOpen()、OpenKeyBoard()和CloseKeyboard()
        /// </summary>
        public class SoftKeyHelper : ISoftKeyHelper
        {
            private const int WM_CLOSE = 0x0010;


            /// <summary>
            /// 检测软键盘是否已经打开了
            /// </summary>
            /// <returns>如果打开，返回true，否则返回false</returns>
            public bool IsKeyboardOpen()
            {
                if (_softKeyboardProc == null)
                    return false;
                return !_softKeyboardProc.HasExited;

                //以下是采用检查是否存在主窗口来判断是否打开
                //int id = _softKeyboardProc.Id;
                //IntPtr ptr = GetMainWindowHandle(_softKeyboardProc.Id);  //通过检查其主窗口是否还在。也可以用Process.HasExited属性
                //return ptr != IntPtr.Zero;
            }

            /// <summary>
            /// 打开软键盘。调用之前应先通过IsKeyboardOpen()来检查keyboard是否已经被打开，如果已经打开，就不要再打开了
            /// </summary>
            public void OpenKeyBoard(int x = 0,int y = 0,int width = 1000,int height = 400)
            {
                string oskFileName = Path.Combine(Environment.CurrentDirectory, "osk.exe");

                if (!System.IO.File.Exists(oskFileName))
                {
                    MessageBox.Show("软键盘可执行文件osk.exe不存在！");
                    return;
                }
                try
                {
                    // Start a process and raise an event when done.
                    _softKeyboardProc = new System.Diagnostics.Process();
                    _softKeyboardProc.StartInfo.FileName = oskFileName;
                    _softKeyboardProc.EnableRaisingEvents = true;  //允许退出

                    _softKeyboardProc.Exited += SoftKeyboard_Exited;  //如果退出，则调用回调函数来关闭对象
                    _softKeyboardProc.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("打开文件  \"{0}\" 时发生错误:" + "\n" + ex.Message, oskFileName);
                    return;
                }

                System.Threading.Thread.Sleep(100);
                _keyBoardWindowHwnd = GetMainWindowHandle(_softKeyboardProc.Id);

                if (_keyBoardWindowHwnd != IntPtr.Zero)
                {

                }

                //设定键盘显示位置
                NativeMethods.MoveWindow(_keyBoardWindowHwnd, x, y, width, height, true);
                //将键盘设置于顶面
                NativeMethods.SetWindowPos(_keyBoardWindowHwnd, -1, 0, 0, 0, 0, 1 | 2);
                //设置软键盘到前端显示
                NativeMethods.SetForegroundWindow(_keyBoardWindowHwnd);

            }
            /// <summary>
            /// 关闭软键盘
            /// </summary>
            public void CloseKeyBoard()
            {
                if (_softKeyboardProc != null)
                {
                    try
                    {
                        _softKeyboardProc.Kill();//虽然_softKeyboardProc！=null存在，也不代表进程没有被关闭。如果进程已经被关闭，则调用Kill会抛出异常
                    }
                    catch
                    { }
                    _softKeyboardProc.Dispose();
                    _softKeyboardProc = null;
                    _keyBoardWindowHwnd = IntPtr.Zero;
                }
            }

            /// <summary>
            /// 当键盘被手工关闭的时候，调用该函数，关闭_softKeyboardProc对象并将_keyBoardWindowHwnd置为IntPtr.Zero
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void SoftKeyboard_Exited(object sender, EventArgs e)
            {
                if (_softKeyboardProc != null)
                {

                    _softKeyboardProc.Dispose();
                    _softKeyboardProc = null;
                    _keyBoardWindowHwnd = IntPtr.Zero;
                }
            }
            /// <summary>
            /// 根据进程ID，获取其主窗口
            /// </summary>
            /// <param name="processId"></param>
            /// <returns></returns>
            private static IntPtr GetMainWindowHandle(int processId)
            {
                IntPtr MainWindowHandle = IntPtr.Zero;

                NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc((hWnd, lParam) =>
                {
                    IntPtr PID;
                    NativeMethods.GetWindowThreadProcessId(hWnd, out PID);

                    if (PID == lParam && NativeMethods.IsWindowVisible(hWnd) && NativeMethods.GetWindow(hWnd, NativeMethods.GW_OWNER) == IntPtr.Zero)
                    {
                        MainWindowHandle = hWnd;
                        return false;
                    }

                    return true;

                }), new IntPtr(processId));

                return MainWindowHandle;
            }
            /// <summary>
            /// 进程对象
            /// </summary>
            private System.Diagnostics.Process _softKeyboardProc;


            /// <summary>
            /// 主窗口句柄
            /// </summary>
            private IntPtr _keyBoardWindowHwnd = IntPtr.Zero;

        }

        public static class NativeMethods
        {

            [DllImport("User32.dll", EntryPoint = "FindWindow")]
            public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
            public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
            [DllImport("User32.dll", EntryPoint = "SendMessage")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

            public const uint GW_OWNER = 4;

            public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern bool IsWindowVisible(IntPtr hWnd);

        }
    }
}
