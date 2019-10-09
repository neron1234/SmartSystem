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
            programListViewModel.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
            GetFileName(programListViewModel.Path);
            this.DataContext = programListViewModel;
        }

        public void GetFileName(string path)
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
                }) ;
            }
        }

        private DxfDocument dxf;
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

                dxf = DxfDocument.Load(programListViewModel.Path + @"\" + programInfo.Name);
                if (dxf != null)
                {
                    AddLayers();
                    AddGraph();
                    AdjustGraph();
                }
            }
        }

        #region DXF解析
        //System.Windows.Point LastMousePosition;
        //protected override void OnMouseWheel(MouseWheelEventArgs e)
        //{
        //    var x = Math.Pow(2, e.Delta / 3.0 / Mouse.MouseWheelDeltaForOneLine);
        //    MyCanvas.Scale *= x;

        //    foreach (var p in MyCanvas.Children)
        //    {
        //        if (p is System.Windows.Shapes.Path)
        //        {
        //            System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;
        //            //if (path.Name != "label")
        //            //{
        //            path.StrokeThickness /= x;
        //            //}
        //        }
        //    }

        //    var position = (Vector)e.GetPosition(Benchmark);

        //    MyCanvas.Offset = (System.Windows.Point)((Vector)
        //        (MyCanvas.Offset + position) * x - position);

        //    e.Handled = true;
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    var position = e.GetPosition(Benchmark);
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        MyCanvas.Offset -= position - LastMousePosition;
        //    }
        //    LastMousePosition = position;
        //}

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
            foreach (var c in dxf.Circles)
            {
                EllipseGeometry circle = new EllipseGeometry(new System.Windows.Point(c.Center.X, -c.Center.Y), c.Radius, c.Radius);

                //foreach (var p in MyCanvas.Children)
                //{
                //    System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;

                //    if ((string)path.Tag == c.Layer.Name)
                //    {

                //        ((GeometryGroup)path.Data).Children.Add(circle);
                //    }
                //}

                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();

                GeometryGroup GeoGroup = new GeometryGroup();
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
            foreach (var l in dxf.Lines)
            {
                LineGeometry line = new LineGeometry(new System.Windows.Point(l.StartPoint.X, -l.StartPoint.Y), new System.Windows.Point(l.EndPoint.X, -l.EndPoint.Y));

                //foreach (var p in MyCanvas.Children)
                //{
                //    System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;

                //    if ((string)path.Tag == l.Layer.Name)
                //    {

                //        ((GeometryGroup)path.Data).Children.Add(line);
                //    }
                //}

                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();

                GeometryGroup GeoGroup = new GeometryGroup();
                GeoGroup.Children.Add(line);
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
        }

        private void AddEllipses()
        {
            foreach (var e in dxf.Ellipses)
            {
                RotateTransform trans = new RotateTransform(-e.Rotation, e.Center.X, -e.Center.Y);
                EllipseGeometry ellipse = new EllipseGeometry(new System.Windows.Point(e.Center.X, -e.Center.Y), e.MajorAxis / 2, e.MinorAxis / 2, trans);

                //foreach (var p in MyCanvas.Children)
                //{
                //    System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)p;

                //    if ((string)path.Tag == e.Layer.Name)
                //    {

                //        ((GeometryGroup)path.Data).Children.Add(ellipse);
                //    }
                //}
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();

                GeometryGroup GeoGroup = new GeometryGroup();
                GeoGroup.Children.Add(ellipse);
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
                if (t.Normal.X != 0)
                {
                    angle = Math.Atan(t.Normal.Y / t.Normal.X) / Math.PI * 180;
                    angle = angle > 0 ? angle : angle + 360;
                }
                else
                {
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

        
    }
}
