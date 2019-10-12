using Abp.Dependency;
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
            programListViewModel = new ProgramListViewModel();
            if (Directory.Exists(@"C:\Users\wjj-yl\Desktop\测试用DXF"))
            {
                programListViewModel.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
                GetFileName(programListViewModel.Path);
            }
            this.DataContext = programListViewModel;
        }

        public void GetFileName(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);
                programListViewModel.ProgramList = new System.Collections.ObjectModel.ObservableCollection<ProgramInfo>();
                foreach (FileInfo f in root.GetFiles())
                {
                    programListViewModel.ProgramList.Add(new ProgramInfo
                    {
                        Name = f.Name,
                        CreateTime = f.CreationTime.ToString(),
                        Size = (f.Length / 1024).ToString() + "KB"
                    });
                }
            }
        }

        private DxfDocument dxf;
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
                if (programInfo.Name.Split('.')[1] == "dxf")
                {
                    dxf = DxfDocument.Load(programListViewModel.Path + @"\" + programInfo.Name);
                    if (dxf != null)
                    {
                        AddLayers();
                        AddGraph();
                        AdjustGraph();
                    }
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
                    //AddLayers();
                    AddLineTest(pointList,programInfo.Name.Split('.')[0]);
                    //AddCirclesTest(pointList);
                    AdjustGraph();
                }
            }
        }

        private void AddLineTest(List<System.Windows.Point> points,string fileName)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            GeometryGroup GeoGroup = new GeometryGroup();
            var brush = new SolidColorBrush(Colors.White);
            for (int i = 0; i < points.Count -2; i++)
            {
                GeoGroup.Children.Add(new LineGeometry(points[i], points[i + 1]));
            }
            path.Stroke = brush;
            path.Data = GeoGroup;
            path.Tag = "line";
            path.StrokeThickness = 2;
            //path.MouseLeftButtonDown += (o, s) =>
            //{
            //    CalculateIntersection(path);
            //};
            MyCanvas.Children.Add(path);
        }

        private void AddCirclesTest(List<System.Windows.Point> points)
        {
            
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            GeometryGroup GeoGroup = new GeometryGroup();

            var c = FittingCircle(points);
            EllipseGeometry circle = new EllipseGeometry(new System.Windows.Point(c.X, c.Y), c.R, c.R);
            GeoGroup.Children.Add(circle);

            path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

            path.Data = GeoGroup;
            path.Tag = "tu";
            path.StrokeThickness = 2;
            path.MouseLeftButtonDown += (o, s) =>
            {
                CalculateIntersection(path);
            };

            MyCanvas.Children.Add(path);
        }

        #region DXF解析
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
                    //if (path.Name != "label")
                    //{
                    path.StrokeThickness /= x;
                    //}
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

        void AddGraph()
        {
            AddCircles();

            AddEllipses();

            AddArcs();

            AddLines();

            AddPolylines();

            AddText();

            AddMText();

            AddLwPolylines();
        }

        private void AddLwPolylines()
        {
            foreach (var lp in dxf.LwPolylines)
            {
                if (lp.Type == EntityType.LwPolyline)
                {
                    LwPolyline polygon = (LwPolyline)lp;

                    PathFigure path = new PathFigure();
                    float bulge = 0;
                    System.Windows.Point prePoint = new System.Windows.Point();
                    System.Windows.Point point = new System.Windows.Point();

                    path.IsClosed = polygon.IsClosed;

                    for (int i = 0; i < polygon.Vertexes.Count(); ++i)
                    {
                        var seg = polygon.Vertexes[i];
                        //point = new System.Windows.Point(seg.Location.X, -seg.Location.Y);
                        point = new System.Windows.Point(seg.Position.X, -seg.Position.Y);

                        if (i == 0)
                        {
                            path.StartPoint = point;
                            prePoint = point;
                            //bulge = seg.Bulge;
                            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                        }
                        else
                        {
                            ArcSegment arc = new ArcSegment();
                            arc.Point = point;

                            //if (angle != 0)
                            if (bulge != 0)
                            {
                                double angle = 4 * Math.Atan(Math.Abs(bulge)) / Math.PI * 180;
                                double length = Math.Sqrt((point.X - prePoint.X) * (point.X - prePoint.X) + (point.Y - prePoint.Y) * (point.Y - prePoint.Y));
                                //double radius = length / (Math.Sqrt(2 * (1 - Math.Cos(angle / 180 * Math.PI))));

                                double radius = Math.Abs(length / (2 * Math.Sin(angle / 360 * Math.PI)));

                                arc.Size = new System.Windows.Size(radius, radius);
                                arc.RotationAngle = angle;

                                arc.SweepDirection = bulge < 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                                arc.IsLargeArc = Math.Abs(bulge) > 1 ? true : false;
                            }

                            prePoint = point;
                            //bulge = seg.Bulge;
                            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                            path.Segments.Add(arc);

                        }
                    }
                    PathGeometry pathgeo = new PathGeometry();
                    pathgeo.Figures.Add(path);

                    //foreach (var _p in MyCanvas.Children)
                    //{
                    //    System.Windows.Shapes.Path _path = (System.Windows.Shapes.Path)_p;

                    //    if ((string)_path.Tag == p.Layer.Name)
                    //    {

                    //        ((GeometryGroup)_path.Data).Children.Add(pathgeo);
                    //    }
                    //}
                    System.Windows.Shapes.Path _path = new System.Windows.Shapes.Path();

                    GeometryGroup GeoGroup = new GeometryGroup();
                    GeoGroup.Children.Add(pathgeo);
                    _path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

                    _path.Data = GeoGroup;
                    _path.Tag = "polylines";
                    _path.StrokeThickness = 2;
                    _path.MouseLeftButtonDown += (o, s) =>
                    {
                        CalculateIntersection(_path);
                    };


                    MyCanvas.Children.Add(_path);
                }
            }
        }

        private void AddCircles()
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            GeometryGroup GeoGroup = new GeometryGroup();
            foreach (var c in dxf.Circles)
            {
                EllipseGeometry circle = new EllipseGeometry(new System.Windows.Point(c.Center.X, -c.Center.Y), c.Radius, c.Radius);
                GeoGroup.Children.Add(circle);
            }
            path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

            path.Data = GeoGroup;
            path.Tag = "tu";
            path.StrokeThickness = 2;

            MyCanvas.Children.Add(path);
        }

        private void CalculateIntersection(System.Windows.Shapes.Path path)
        {
            path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
            if (path.Tag.ToString() == "line")
            {
                LineGeometry line = (LineGeometry)(((GeometryGroup)path.Data).Children[0]);

                double dx = line.StartPoint.X - line.EndPoint.X;
                double k = 0;
                double b = 0;

                if (dx != 0)
                {
                    k = (line.StartPoint.Y - line.EndPoint.Y) / (line.StartPoint.X - line.EndPoint.X);
                    b = line.StartPoint.Y - line.StartPoint.X;

                }
                else
                {
                    k = 1000;
                    b = 1000;
                }
                //line1.Text = string.Format("{0},{1}", k.ToString(), b.ToString());
            }
        }

        private void AddLines()
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();

            GeometryGroup GeoGroup = new GeometryGroup();
            foreach (var l in dxf.Lines)
            {
                LineGeometry line = new LineGeometry(new System.Windows.Point(l.StartPoint.X, -l.StartPoint.Y), new System.Windows.Point(l.EndPoint.X, -l.EndPoint.Y));
                GeoGroup.Children.Add(line);
            }
            
            path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

            path.Data = GeoGroup;
            path.Tag = "line";
            path.StrokeThickness = 2;
            path.MouseLeftButtonDown += (o, s) =>
            {
                CalculateIntersection(path);
            };


            MyCanvas.Children.Add(path);
        }

        private void AddEllipses()
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            GeometryGroup GeoGroup = new GeometryGroup();
            foreach (var e in dxf.Ellipses)
            {
                RotateTransform trans = new RotateTransform(-e.Rotation, e.Center.X, -e.Center.Y);
                EllipseGeometry ellipse = new EllipseGeometry(new System.Windows.Point(e.Center.X, -e.Center.Y), e.MajorAxis / 2, e.MinorAxis / 2, trans);

                GeoGroup.Children.Add(ellipse);
            }
            path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

            path.Data = GeoGroup;
            path.Tag = "ellipse";
            path.StrokeThickness = 2;
            path.MouseLeftButtonDown += (o, s) =>
            {
                CalculateIntersection(path);
            };


            MyCanvas.Children.Add(path);
        }

        private void AddArcs()
        {
            foreach (var a in dxf.Arcs)
            {
                double Sx = a.Center.X + a.Radius * System.Math.Cos(a.StartAngle / 180 * Math.PI);
                double Sy = -a.Center.Y - a.Radius * System.Math.Sin(a.StartAngle / 180 * Math.PI);
                System.Windows.Point Start = new System.Windows.Point(Sx, Sy);

                double Ex = a.Center.X + a.Radius * System.Math.Cos(a.EndAngle / 180 * Math.PI);
                double Ey = -a.Center.Y - a.Radius * System.Math.Sin(a.EndAngle / 180 * Math.PI);
                System.Windows.Point End = new System.Windows.Point(Ex, Ey);



                ArcSegment arc = new ArcSegment();
                arc.Point = End;

                arc.RotationAngle = a.EndAngle - a.StartAngle;
                arc.RotationAngle = arc.RotationAngle >= 0 ? arc.RotationAngle : arc.RotationAngle + 360;


                arc.IsLargeArc = arc.RotationAngle > 180 ? true : false;
                arc.SweepDirection = SweepDirection.Counterclockwise;

                arc.Size = new System.Windows.Size(a.Radius, a.Radius);

                PathFigure path = new PathFigure();
                path.StartPoint = Start;
                path.Segments.Add(arc);

                PathGeometry pathgeo = new PathGeometry();
                pathgeo.Figures.Add(path);

                //foreach (var p in MyCanvas.Children)
                //{
                //    System.Windows.Shapes.Path _path = (System.Windows.Shapes.Path)p;

                //    if ((string)_path.Tag == a.Layer.Name)
                //    {

                //        ((GeometryGroup)_path.Data).Children.Add(pathgeo);
                //    }
                //}
                System.Windows.Shapes.Path _path = new System.Windows.Shapes.Path();

                GeometryGroup GeoGroup = new GeometryGroup();
                GeoGroup.Children.Add(pathgeo);
                _path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

                _path.Data = GeoGroup;
                _path.Tag = "arc";
                _path.StrokeThickness = 2;
                _path.MouseLeftButtonDown += (o, s) =>
                {
                    CalculateIntersection(_path);
                };


                MyCanvas.Children.Add(_path);
            }
        }

        private void AddPolylines()
        {
            foreach (var p in dxf.Polylines)
            {
                if (p.Type == netDxf.Entities.EntityType.Polyline)
                {
                    netDxf.Entities.Polyline polygon = (netDxf.Entities.Polyline)p;
                    PathFigure path = new PathFigure();
                    float bulge = 0;
                    System.Windows.Point prePoint = new System.Windows.Point();
                    System.Windows.Point point = new System.Windows.Point();

                    path.IsClosed = polygon.IsClosed;

                    for (int i = 0; i < polygon.Vertexes.Count(); ++i)
                    {
                        var seg = polygon.Vertexes[i];
                        point = new System.Windows.Point(seg.Position.X, -seg.Position.Y);

                        if (i == 0)
                        {
                            path.StartPoint = point;
                            prePoint = point;
                            //bulge = seg.Bulge;
                            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                        }
                        else
                        {
                            ArcSegment arc = new ArcSegment();
                            arc.Point = point;

                            //if (angle != 0)
                            if (bulge != 0)
                            {
                                double angle = 4 * Math.Atan(Math.Abs(bulge)) / Math.PI * 180;
                                double length = Math.Sqrt((point.X - prePoint.X) * (point.X - prePoint.X) + (point.Y - prePoint.Y) * (point.Y - prePoint.Y));
                                //double radius = length / (Math.Sqrt(2 * (1 - Math.Cos(angle / 180 * Math.PI))));

                                double radius = Math.Abs(length / (2 * Math.Sin(angle / 360 * Math.PI)));

                                arc.Size = new System.Windows.Size(radius, radius);
                                arc.RotationAngle = angle;

                                arc.SweepDirection = bulge < 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                                arc.IsLargeArc = Math.Abs(bulge) > 1 ? true : false;
                            }

                            prePoint = point;
                            //bulge = seg.Bulge;
                            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                            path.Segments.Add(arc);

                        }
                    }
                    PathGeometry pathgeo = new PathGeometry();
                    pathgeo.Figures.Add(path);

                    System.Windows.Shapes.Path _path = new System.Windows.Shapes.Path();

                    GeometryGroup GeoGroup = new GeometryGroup();
                    GeoGroup.Children.Add(pathgeo);
                    _path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

                    _path.Data = GeoGroup;
                    _path.Tag = "polylines";
                    _path.StrokeThickness = 2;
                    _path.MouseLeftButtonDown += (o, s) =>
                    {
                        CalculateIntersection(_path);
                    };


                    MyCanvas.Children.Add(_path);
                }
            }
        }

        private void AddText()
        {

            foreach (var t in dxf.Texts)
            {
                FormattedText formattedText = new FormattedText(t.Value, CultureInfo.GetCultureInfo("zh-CN"), FlowDirection.LeftToRight, new Typeface("仿宋体"), t.Height * (96.0 / 72.0), System.Windows.Media.Brushes.White);


                //SolidColorBrush brush = new SolidColorBrush(Colors.Orange);
                LinearGradientBrush brush = new LinearGradientBrush(
                            Colors.Orange,
                            Colors.Teal,
                            90.0);
                formattedText.SetForegroundBrush(brush);

                //Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(t.BasePoint.X, -t.BasePoint.Y - t.Height * (96.0 / 72.0)));
                Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(t.Position.X, -t.Position.Y - t.Height * (96.0 / 72.0)));
                PathGeometry pathGeometry = textGeometry.GetFlattenedPathGeometry();

                foreach (var p in MyCanvas.Children)
                {
                    if (p is System.Windows.Shapes.Path)
                    {
                        System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;

                        if ((string)path.Tag == "Character")
                        {

                            ((GeometryGroup)path.Data).Children.Add(pathGeometry);
                        }
                    }
                }
                //Characters.Children.Add(pathGeometry);
            }
        }

        private void AddMText()
        {
            foreach (var t in dxf.MTexts)
            {
                FormattedText formattedText = new FormattedText(t.Value, CultureInfo.GetCultureInfo("zh-CN"), FlowDirection.LeftToRight, new Typeface("仿宋体"), t.Height * (96.0 / 72.0), System.Windows.Media.Brushes.White);

                double angle = 0;
                if (t.Normal.X != 0){
                    angle = Math.Atan(t.Normal.Y / t.Normal.X) / Math.PI * 180;
                    angle = angle > 0 ? angle : angle + 360;
                }
                else{
                    if (t.Normal.Y > 0)
                    {
                        angle = 90;
                    }
                    else if (t.Normal.Y < 0)
                    {
                        angle = 270;
                    }
                    else
                    {
                        angle = 0;
                    }

                }

                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                //LinearGradientBrush brush = new LinearGradientBrush(
                //            Colors.Orange,
                //            Colors.Teal,
                //            90.0);

                formattedText.SetForegroundBrush(brush);

                //Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(t.BasePoint.X, -t.BasePoint.Y - t.Height * (96.0 / 72.0)));
                //Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(t.BasePoint.X, -t.BasePoint.Y));
                Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(t.Position.X, -t.Position.Y));

                //RotateTransform rt = new RotateTransform(-angle, t.BasePoint.X, -t.BasePoint.Y);
                RotateTransform rt = new RotateTransform(-angle, t.Position.X, -t.Position.Y);

                textGeometry.Transform = rt;

                PathGeometry pathGeometry = textGeometry.GetFlattenedPathGeometry();

                foreach (var p in MyCanvas.Children)
                {
                    if (p is System.Windows.Shapes.Path)
                    {
                        System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;

                        if ((string)path.Tag == "Character")
                        {

                            ((GeometryGroup)path.Data).Children.Add(pathGeometry);
                        }
                    }
                }
                //Characters.Children.Add(pathGeometry);
            }
        }

        private void AddLayers()
        {
            foreach (var lay in dxf.Layers)
            {
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                path.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(lay.Color.ToColor().A, lay.Color.ToColor().R, lay.Color.ToColor().G, lay.Color.ToColor().B));
                path.Tag = lay.Name;

                GeometryGroup GeoGroup = new GeometryGroup();
                path.Data = GeoGroup;

                MyCanvas.Children.Add(path);

                //ComboBoxItem comItem = new ComboBoxItem();
                //comItem.Content = lay.Name;
                //comBox.Items.Add(comItem);
            }

            System.Windows.Shapes.Path label = new System.Windows.Shapes.Path();

            GeometryGroup Character = new GeometryGroup();
            label.Data = Character;
            label.Tag = "Character";

            MyCanvas.Children.Add(label);
        }

        private void AdjustGraph()
        {

            MyCanvas.Offset = new System.Windows.Point(0, 0);
            MyCanvas.Scale = 1;

            var bound = ((GeometryGroup)((System.Windows.Shapes.Path)(MyCanvas.Children[0])).Data).Bounds;

            double maxLeft = bound.Left;
            double maxRight = bound.Right;
            double maxTop = bound.Top;
            double maxBottom = bound.Bottom;

            for (int i = 0; i < MyCanvas.Children.Count; i++)
            {
                if (MyCanvas.Children[i] is System.Windows.Shapes.Path)
                {
                    System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)MyCanvas.Children[i];
                    if ((string)path.Tag == "Character")
                    {
                        path.StrokeThickness = 0.05;
                        path.Stroke = System.Windows.Media.Brushes.White;
                        path.Fill = System.Windows.Media.Brushes.White;
                    }
                    else
                    {

                        path.StrokeThickness = 1;
                    }
                    CalBounds(ref maxLeft, ref maxRight, ref maxTop, ref maxBottom, path);
                }
            }

            double Ex = Benchmark.ActualWidth / 2;
            double Ey = Benchmark.ActualHeight / 2;

            double Sx = maxLeft + (maxRight - maxLeft) / 2;
            double Sy = maxTop + (maxBottom - maxTop) / 2;

            double scaleX = Benchmark.ActualWidth / (maxRight - maxLeft);
            double scaleY = Benchmark.ActualHeight / (maxBottom - maxTop);

            var scale = (scaleX < scaleY ? scaleX : scaleY) * 0.8;

            MyCanvas.Scale *= scale;

            foreach (var p in MyCanvas.Children)
            {
                if (p is System.Windows.Shapes.Path)
                {
                    ((System.Windows.Shapes.Path)p).StrokeThickness /= scale;
                }
            }

            System.Windows.Point size = new System.Windows.Point(Ex - scale * Sx, Ey - scale * Sy);
            MyCanvas.Offset -= (Vector)size;
        }

        private static void CalBounds(ref double maxLeft, ref double maxRight, ref double maxTop, ref double maxBottom, System.Windows.Shapes.Path path)
        {
            maxLeft = maxLeft < ((GeometryGroup)path.Data).Bounds.Left ? maxLeft : ((GeometryGroup)path.Data).Bounds.Left;
            maxRight = maxRight > ((GeometryGroup)path.Data).Bounds.Right ? maxRight : ((GeometryGroup)path.Data).Bounds.Right;
            maxTop = maxTop < ((GeometryGroup)path.Data).Bounds.Top ? maxTop : ((GeometryGroup)path.Data).Bounds.Top;
            maxBottom = maxBottom > ((GeometryGroup)path.Data).Bounds.Bottom ? maxBottom : ((GeometryGroup)path.Data).Bounds.Bottom;
        }


        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetFileName(programListViewModel.Path);
            return;
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


        /// <summary>
        /// 拟合圆
        /// </summary>
        private struct Circle
        {
            public double X;//圆心X
            public double Y;//圆心Y
            public double R;//半径R
        }

        /// <summary>
        /// 拟合圆程序
        /// </summary>
        /// <param name="pPointList">要拟合点集</param>
        /// <returns>返回圆对象</returns>
        private Circle FittingCircle(List<System.Windows.Point> pPointList)
        {
            Circle pCircle = new Circle();
            if (pPointList.Count < 3){
                return pCircle;
            }
            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = 0;
            double X3 = 0;
            double Y3 = 0;
            double X1Y1 = 0;
            double X1Y2 = 0;
            double X2Y1 = 0;
            for (int i = 0; i < pPointList.Count; i++){
                X1 = X1 + pPointList[i].X;
                Y1 = Y1 + pPointList[i].Y;
                X2 = X2 + pPointList[i].X * pPointList[i].X;
                Y2 = Y2 + pPointList[i].Y * pPointList[i].Y;
                X3 = X3 + pPointList[i].X * pPointList[i].X * pPointList[i].X;
                Y3 = Y3 + pPointList[i].Y * pPointList[i].Y * pPointList[i].Y;
                X1Y1 = X1Y1 + pPointList[i].X * pPointList[i].Y;
                X1Y2 = X1Y2 + pPointList[i].X * pPointList[i].Y * pPointList[i].Y;
                X2Y1 = X2Y1 + pPointList[i].X * pPointList[i].X * pPointList[i].Y;
            }
            double C, D, E, G, H, N;
            double a, b, c;
            N = pPointList.Count;
            C = N * X2 - X1 * X1;
            D = N * X1Y1 - X1 * Y1;
            E = N * X3 + N * X1Y2 - (X2 + Y2) * X1;
            G = N * Y2 - Y1 * Y1;
            H = N * X2Y1 + N * Y3 - (X2 + Y2) * Y1;
            a = (H * D - E * G) / (C * G - D * D);
            b = (H * C - E * D) / (D * D - G * C);
            c = -(a * X1 + b * Y1 + X2 + Y2) / N;
            pCircle.X = a / (-2);
            pCircle.Y = b / (-2);
            pCircle.R = Math.Sqrt(a * a + b * b - 4 * c) / 2;
            return pCircle;
        }
    }
}
