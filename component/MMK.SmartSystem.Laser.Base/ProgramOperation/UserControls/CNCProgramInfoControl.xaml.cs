using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.Laser.Base.CustomControl;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls
{
    /// <summary>
    /// CNCProgramInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class CNCProgramInfoControl : UserControl
    {
        const string ElectronWindowName = "AngualrElectron-Progress-bmp";
        CancellationTokenSource cts = new CancellationTokenSource();

        public CNCProgramInfoViewModel cncProgramVm { get; set; }
        public CNCProgramInfoControl()
        {
            InitializeComponent();
            this.DataContext = cncProgramVm = new CNCProgramInfoViewModel();
            //  RegisterDrawProgram();
            Loaded += CNCProgramInfoControl_Loaded;
            Unloaded += CNCProgramInfoControl_Unloaded;
        }

        private void CNCProgramInfoControl_Unloaded(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            cncApp.CloseWindows();

        }

        private void CNCProgramInfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            //获取cncApp控件基于屏幕的绝对位置，长宽
            var absolutePos = cncApp.PointToScreen(new Point(0, 0));
            var width = cncApp.ActualWidth;
            var height = cncApp.ActualHeight;
            Messenger.Default.Send(new PageChangeModel()
            {
                Page = PageEnum.WebComponet,
                ComponentDto = new Common.WebRouteComponentDto()
                {
                    ComponentUrl = "/home/zrender",
                    Height = height,
                    Width = width,
                    PositionX = absolutePos.X,
                    PositionY = absolutePos.Y,
                    WindowName = ElectronWindowName
                }

            });

            Task.Factory.StartNew(new Action(() =>
            {

                while (true)
                {
                    Thread.Sleep(50);
                    var result = cncApp.StartAndEmbedWindowsName(ElectronWindowName, Dispatcher);
                    if (result || cts.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }


            }), cts.Token);



        }

        /// <summary>
        /// 解析路径下的文件并进行绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterDrawProgram()
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("pack://application:,,,/MMK.SmartSystem.Laser.Base;component/Resources/Images/缩略图.bmp"));
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = image.Source;
            MyCanvas.Background = ib;


            Messenger.Default.Register<ProgramViewModel>(this, (pInfo) =>
            {

                cncProgramVm.SelectedProgram = pInfo;
                if (MyCanvas.Children.Count != 0)
                    MyCanvas.Children.Clear();

                if (pInfo.Name.Split('.').Count() > 1)
                {
                    var dg = new DrawGraphics(ref this.MyCanvas, ref this.Benchmark);
                    if (pInfo.Name.Split('.')[1] == "dxf")
                    {
                        dg.Draw(cncProgramVm.Path + @"\" + pInfo.Name);
                    }
                    else if (pInfo.Name.Split('.')[1] == "csv")
                    {
                        StreamReader reader = new StreamReader(cncProgramVm.Path + @"\" + pInfo.Name);
                        string line = "";
                        List<System.Windows.Point> pointList = new List<System.Windows.Point>();
                        //List<string[]> pointList = new List<string[]>();
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            pointList.Add(new System.Windows.Point(Convert.ToDouble(line.Split(',')[0]), Convert.ToDouble(line.Split(',')[1])));
                            //pointList.Add(line.Split(','));
                            line = reader.ReadLine();
                        }
                        dg.Draw(pointList, pInfo.Name.Split('.')[0]);
                    }
                }
            });
        }
        System.Windows.Point LastMousePosition;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var x = Math.Pow(2, e.Delta / 3.0 / Mouse.MouseWheelDeltaForOneLine);
            MyCanvas.Scale *= x;
            foreach (var p in MyCanvas.Children)
            {
                if (p is System.Windows.Shapes.Path)
                {
                    System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;
                    path.StrokeThickness /= x;
                }
            }
            var position = (Vector)e.GetPosition(Benchmark);
            MyCanvas.Offset = (System.Windows.Point)((Vector)
                (MyCanvas.Offset + position) * x - position);

            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var position = e.GetPosition(Benchmark);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MyCanvas.Offset -= position - LastMousePosition;
            }
            LastMousePosition = position;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //测试图片输出
            RenderTargetBitmap bmp = new RenderTargetBitmap(500, 500, 0, 0, PixelFormats.Pbgra32);
            bmp.Render(Benchmark);
            string file = @"C:\Users\wjj-yl\Desktop\测试用DXF\test.jpg";
            string Extension = System.IO.Path.GetExtension(file).ToLower();
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (Stream stm = File.Create(file))
            {
                encoder.Save(stm);
            }
        }
    }
}
