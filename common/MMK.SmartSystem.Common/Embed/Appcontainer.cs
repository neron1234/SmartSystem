using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace MMK.SmartSystem.Common.Embed
{
    public class AppContainer : ContentControl
    {
        private WindowsFormsHost _winFormHost;
        private System.Windows.Forms.Panel _hostPanel;
        private readonly ManualResetEvent _eventDone = new ManualResetEvent(false);
        private Process _process;
        internal IntPtr _embededWindowHandle;
        private IntPtr windowIntptr;
        public string HostFormName { set; get; }

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }

                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }




        public AppContainer()
        {
            var res = new ResourceDictionary();
            res.Source = new Uri("pack://application:,,,/MMK.SmartSystem.Common;component/Embed/AppContainer.xaml");
            Application.Current.Resources.MergedDictionaries.Add(res);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // _winFormHost = GetTemplateChild(HostFormName) as WindowsFormsHost;
            //if (_winFormHost != null)
            //{
            //    _hostPanel = new System.Windows.Forms.Panel();
            //    _winFormHost.Child = _hostPanel;
            //}
        }
        public bool StartAndEmbedWindowsName(string windowName, WindowsFormsHost windowsFormsHost, System.Windows.Threading.Dispatcher dispatcher = null)
        {
            _winFormHost = windowsFormsHost;
            if (_winFormHost != null && dispatcher != null)
            {
                dispatcher.Invoke(new Action(() =>
                {
                    _hostPanel = new System.Windows.Forms.Panel();
                    _winFormHost.Child = _hostPanel;

                }));

            }
            else if (_winFormHost != null)
            {
                _hostPanel = new System.Windows.Forms.Panel();
                _winFormHost.Child = _hostPanel;

            }
            var isStartAndEmbedSuccess = false;
            var process = Win32Api.FindWindow(null, windowName);
            windowIntptr = process;
            isStartAndEmbedSuccess = EmbedApp(process, dispatcher);
            if (!isStartAndEmbedSuccess)
            {
                CloseApp(_process);
            }
            return isStartAndEmbedSuccess;
        }
        public bool StartAndEmbedProcess(string processPath, WindowsFormsHost windowsFormsHost, System.Windows.Threading.Dispatcher dispatcher = null)
        {
            _winFormHost = windowsFormsHost;
            if (_winFormHost != null && dispatcher != null)
            {
                dispatcher.Invoke(new Action(() =>
                {
                    _hostPanel = new System.Windows.Forms.Panel();
                    _winFormHost.Child = _hostPanel;

                }));

            }
            var isStartAndEmbedSuccess = false;
            _eventDone.Reset();
            _process = new Process();
            // _process.StartInfo.FileName = @"E:\CODE\electron\angular-electron\release\win-unpacked\angular-electron.exe";

            _process.StartInfo.FileName = processPath;
            //_process.StartInfo.UseShellExecute = false;
            //_process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;//加上这句效果更好 
            // Start the process 
            _process.Start();
            System.Threading.Thread.Sleep(200);
            if (_process == null)
            {
                return false;
            }


            // Wait for process to be created and enter idle condition 
            _process.WaitForInputIdle();

            //  Get the main handle
            var thread = new Thread(() =>
            {
                while (true)
                {
                    if (_process.MainWindowHandle != (IntPtr)0)
                    {
                        _eventDone.Set();
                        break;
                    }
                    Thread.Sleep(10);
                }
            });
            thread.Start();
            //嵌入进程
            if (_eventDone.WaitOne(10000))
            {
                isStartAndEmbedSuccess = EmbedApp(_process, dispatcher);
                if (!isStartAndEmbedSuccess)
                {
                    CloseApp(_process);
                }
            }
            return isStartAndEmbedSuccess;
        }

        public bool EmbedExistProcess(Process process)
        {
            _process = process;
            return EmbedApp(process);
        }

        /// <summary>
        /// 将外进程嵌入到当前程序
        /// </summary>
        /// <param name="process"></param>
        private bool EmbedApp(Process process, System.Windows.Threading.Dispatcher dispatcher = null)
        {
            //是否嵌入成功标志，用作返回值
            var isEmbedSuccess = false;
            //外进程句柄
            var processHwnd = process.MainWindowHandle;
            //容器句柄
            IntPtr panelHwnd = (IntPtr)0;
            if (dispatcher != null)
            {
                dispatcher.Invoke(new Action(() =>
                {
                    panelHwnd = _hostPanel.Handle;

                }));
            }
            else
            {
                panelHwnd = _hostPanel.Handle;

            }

            if (processHwnd != (IntPtr)0 && panelHwnd != (IntPtr)0)
            {
                //把本窗口句柄与目标窗口句柄关联起来
                var setTime = 0;
                while (!isEmbedSuccess && setTime < 50)
                {
                    // Put it into this form
                    isEmbedSuccess = Win32Api.SetParent(processHwnd, panelHwnd) != 0;
                    Thread.Sleep(10);
                    setTime++;
                }

                //  Win32Api.SetParent(_process.MainWindowHandle, _hostPanel.Handle);
                // Remove border and whatnot
                Win32Api.SetWindowLong(new HandleRef(this, _process.MainWindowHandle), Win32Api.GWL_STYLE, Win32Api.WS_VISIBLE);

                ////   Win32Api.SetWindowLong(processHwnd, Win32Api.GWL_STYLE, Win32Api.WS_CHILDWINDOW | Win32Api.WS_CLIPSIBLINGS | Win32Api.WS_CLIPCHILDREN | Win32Api.WS_VISIBLE);

                //// Move the window to overlay it on this window
                Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
                //Win32Api.ShowWindow(_process.MainWindowHandle, 3);

            }

            if (isEmbedSuccess)
            {
                _embededWindowHandle = _process.MainWindowHandle;
            }

            return isEmbedSuccess;
        }
        public void CloseWindows()
        {
            if (windowIntptr != null)
            {
                Win32Api.DestroyWindow(windowIntptr);
            }
        }
        private bool EmbedApp(IntPtr processHwnd, System.Windows.Threading.Dispatcher dispatcher = null)
        {
            //是否嵌入成功标志，用作返回值
            var isEmbedSuccess = false;
            IntPtr panelHwnd = (IntPtr)0;
            //外进程句柄
            //容器句柄
            if (dispatcher != null)
            {
                dispatcher.Invoke(new Action(() =>
                {
                    panelHwnd = _hostPanel.Handle;

                }));
            }
            else
            {
                panelHwnd = _hostPanel.Handle;

            }
            //var panelHwnd = _hostPanel.Handle;

            if (processHwnd != (IntPtr)0 && panelHwnd != (IntPtr)0)
            {
                //把本窗口句柄与目标窗口句柄关联起来
                var setTime = 0;
                while (!isEmbedSuccess && setTime < 50)
                {
                    // Put it into this form
                    isEmbedSuccess = Win32Api.SetParent(processHwnd, panelHwnd) != 0;
                    Thread.Sleep(10);
                    setTime++;
                }
                //  Win32Api.SetParent(_process.MainWindowHandle, _hostPanel.Handle);
                // Win32Api.ShowWindow(_process.MainWindowHandle, 3);
                // Remove border and whatnot
                Win32Api.SetWindowLong(processHwnd, Win32Api.GWL_STYLE, Win32Api.WS_CHILDWINDOW | Win32Api.WS_CLIPSIBLINGS | Win32Api.WS_CLIPCHILDREN | Win32Api.WS_VISIBLE);
                Win32Api.MoveWindow(processHwnd, 0, 0, (int)ActualWidth, (int)ActualHeight, false);
                // Move the window to overlay it on this window
                // Win32Api.ShowWindow(processHwnd, 3);
            }

            if (isEmbedSuccess)
            {
                _embededWindowHandle = processHwnd;
            }

            return isEmbedSuccess;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_process != null)
            {
                //Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            }

            base.OnRender(drawingContext);
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    InvalidateVisual();
        //    base.OnRenderSizeChanged(sizeInfo);
        //}

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="process"></param>
        private void CloseApp(Process process)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

        public void CloseProcess()
        {
            CloseApp(_process);
        }
    }
}
