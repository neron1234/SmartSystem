using Abp.Dependency;
using MMK.SmartSystem.Laser.Base.CustomControl;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation
{
    /// <summary>
    /// ProgramListPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProgramListPage : Page, ITransientDependency
    {
        private ProgramListViewModel programListViewModel { get; set; }
        public ProgramListPage()
        {
            InitializeComponent();
            this.Loaded += ProgramListPage_Loaded;
        }

        private void ProgramListPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = programListViewModel = new ProgramListViewModel();
        }

        /// <summary>
        /// 解析路径下的文件并进行绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarDataViewGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((DataGrid)sender).SelectedValue;
            if (selected != null && selected is ProgramInfo)
            {
                var programInfo = (ProgramInfo)selected;
                //程序名称:FJFDJS-001
                programListViewModel.SelectedName = "程序名称:" + programInfo.Name;
                if (MyCanvas.Children.Count != 0)
                    MyCanvas.Children.Clear();

                var dg = new DrawGraphics(ref this.MyCanvas, ref this.Benchmark);
                if (programInfo.Name.Split('.')[1] == "dxf")
                {
                    dg.Draw(programListViewModel.Path + @"\" + programInfo.Name);
                }
                else
                {
                    StreamReader reader = new StreamReader(programListViewModel.Path + @"\" + programInfo.Name);
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
                    dg.Draw(pointList,programInfo.Name.Split('.')[0]);
                }
            }
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
