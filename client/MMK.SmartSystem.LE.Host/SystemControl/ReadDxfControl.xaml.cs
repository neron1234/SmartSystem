using Microsoft.Win32;
using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// ReadDxfControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReadDxfControl : UserControl
    {
        public ReadDxfControl()
        {
            InitializeComponent();
        }
        System.Windows.Point LastMousePosition;
        private DxfDocument dxf;

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "DXF 文件|*.dxf";

            if ((bool)openDlg.ShowDialog())
            {
                if (MyCanvas.Children.Count != 0)
                    MyCanvas.Children.Clear();

                var filename = openDlg.FileName;
                dxf = DxfDocument.Load(filename);

                AddLayers();
                AddGraph();
                AdjustGraph();
            }
        }

        private void BeginTrajectory_Click(object sender, RoutedEventArgs e)
        {
            StartTrajectory(0);
        }

        private void BeginReverseTrajectory_Click(object sender, RoutedEventArgs e)
        {
            StartTrajectory(1);
        }


        private async void StartTrajectory(int orientation)
        {
            var count = 0;
            foreach (var item in MyCanvas.Children)
            {
                if (item is Path)
                {
                    Path path = (Path)item;
                    if (!path.Data.Bounds.IsEmpty)
                        count++;
                }
            }
            var time = Convert.ToInt32(this.DurationTime.Text.Trim()) / count;
            if (orientation == 0)
            {
                for (int i = 0; i < MyCanvas.Children.Count; i++)
                {
                    if (MyCanvas.Children[i] is Path)
                    {
                        Path path = (Path)MyCanvas.Children[i];
                        if (!path.Data.Bounds.IsEmpty)
                        {
                            await Task.Factory.StartNew(() =>
                            {
                                this.Dispatcher.InvokeAsync(() =>
                                {
                                    foreach (var dataItem in ((GeometryGroup)path.Data).Children)
                                    {
                                        MatrixStory(orientation, dataItem.GetFlattenedPathGeometry().Figures.ToString(), time);
                                    }
                                });
                                Thread.Sleep(time * 1000);
                            });
                        }
                    }
                }
            }
            else
            {
                for (int i = MyCanvas.Children.Count - 1; i > 0; i--)
                {
                    if (MyCanvas.Children[i] is Path)
                    {
                        Path path = (Path)MyCanvas.Children[i];
                        if (!path.Data.Bounds.IsEmpty)
                        {
                            await Task.Factory.StartNew(() =>
                            {
                                this.Dispatcher.InvokeAsync(() =>
                                {
                                    foreach (var dataItem in ((GeometryGroup)path.Data).Children)
                                    {
                                        MatrixStory(orientation, dataItem.GetFlattenedPathGeometry().Figures.ToString(), time);
                                    }
                                });
                                Thread.Sleep(time * 1000);
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 路径走向
        /// </summary>
        /// <param name="orientation">0正向 1反向</param>
        /// <param name="data">路径数据</param>
        private void MatrixStory(int orientation, string data, int durationTime)
        {
            Border border = new Border();
            border.Width = 10;
            border.Height = 10;
            border.Visibility = Visibility.Collapsed;
            if (orientation == 0)
            {
                border.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                border.Background = new SolidColorBrush(Colors.Green);
                data = ConvertReverseData(data);
            }

            this.MyCanvas.Children.Add(border);
            Canvas.SetLeft(border, -border.Width / 2);
            Canvas.SetTop(border, -border.Height / 2);
            border.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

            MatrixTransform matrix = new MatrixTransform();
            TransformGroup groups = new TransformGroup();
            groups.Children.Add(matrix);
            border.RenderTransform = groups;
            //NameScope.SetNameScope(this, new NameScope());
            string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
            this.RegisterName(registname, matrix);
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(data));
            matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
            matrixAnimation.DoesRotateWithTangent = true;//旋转
            //matrixAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard story = new Storyboard();
            story.Children.Add(matrixAnimation);
            Storyboard.SetTargetName(matrixAnimation, registname);
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            #region 控制显示与隐藏
            ObjectAnimationUsingKeyFrames ObjectAnimation = new ObjectAnimationUsingKeyFrames();
            ObjectAnimation.Duration = matrixAnimation.Duration;
            DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromMilliseconds(1));
            ObjectAnimation.KeyFrames.Add(kf1);
            story.Children.Add(ObjectAnimation);
            //Storyboard.SetTargetName(border, border.Name);
            Storyboard.SetTargetProperty(ObjectAnimation, new PropertyPath(UIElement.VisibilityProperty));
            #endregion
            story.FillBehavior = FillBehavior.Stop;
            story.Begin(border, true);
        }

        /// <summary>
        /// 反向Data数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string ConvertReverseData(string data)
        {
            data = data.Replace("M", "").Replace(" ", "/");
            Regex regex = new Regex("[a-z]", RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(data);
            //item1 从上一个位置到当前位置开始的字符 (match.Index=原始字符串中发现捕获的子字符串的第一个字符的位置。)
            //item2 当前发现的匹配符号(L C Z M)
            List<Tuple<string, string>> tmpList = new List<Tuple<string, string>>();
            int curPostion = 0;
            for (int i = 0; i < mc.Count; i++)
            {
                Match match = mc[i];
                if (match.Index != curPostion)
                {
                    string str = data.Substring(curPostion, match.Index - curPostion);
                    tmpList.Add(new Tuple<string, string>(str, match.Value));
                }
                curPostion = match.Index + match.Length;
                if (i + 1 == mc.Count)//last 
                {
                    tmpList.Add(new Tuple<string, string>(data.Substring(curPostion), match.Value));
                }
            }
            //char[] spChar = new char[2] { 'C', 'L' };
            //var tmpList = data.Split(spChar);
            List<string[]> spList = new List<string[]>();
            for (int i = 0; i < tmpList.Count; i++)
            {
                var cList = tmpList[i].Item1.Split('/');
                spList.Add(cList);
            }
            List<string> strList = new List<string>();
            for (int i = spList.Count - 1; i >= 0; i--)
            {
                string[] clist = spList[i];
                for (int j = clist.Length - 1; j >= 0; j--)
                {
                    if (j == clist.Length - 2)//对于第二个元素增加 L或者C的标识
                    {
                        var pointWord = tmpList[i - 1].Item2;//获取标识
                        strList.Add(pointWord + clist[j]);
                    }
                    else
                    {
                        strList.Add(clist[j]);
                    }
                }
            }
            string reverseData = "M" + string.Join(" ", strList);
            return reverseData;

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var x = Math.Pow(2, e.Delta / 3.0 / Mouse.MouseWheelDeltaForOneLine);
            MyCanvas.Scale *= x;

            foreach (var p in MyCanvas.Children)
            {
                if (p is Path)
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
                    netDxf.Entities.Polyline polygon =(netDxf.Entities.Polyline)p;
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
                //if (p.Flags == PolylineTypeFlags.OpenPolyline || p.Flags == PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM)
                //{
                //    LightWeightPolyline polygon = (LightWeightPolyline)p;
                //    PathFigure path = new PathFigure();
                //    float bulge = 0;
                //    System.Windows.Point prePoint = new System.Windows.Point();
                //    System.Windows.Point point = new System.Windows.Point();

                //    path.IsClosed = polygon.IsClosed;

                //    for (int i = 0; i < polygon.Vertexes.Count(); ++i)
                //    {
                //        var seg = polygon.Vertexes[i];
                //        point = new System.Windows.Point(seg.Location.X, -seg.Location.Y);

                //        if (i == 0)
                //        {
                //            path.StartPoint = point;
                //            prePoint = point;
                //            bulge = seg.Bulge;
                //            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                //        }
                //        else
                //        {
                //            ArcSegment arc = new ArcSegment();
                //            arc.Point = point;

                //            //if (angle != 0)
                //            if (bulge != 0)
                //            {
                //                double angle = 4 * Math.Atan(Math.Abs(bulge)) / Math.PI * 180;
                //                double length = Math.Sqrt((point.X - prePoint.X) * (point.X - prePoint.X) + (point.Y - prePoint.Y) * (point.Y - prePoint.Y));
                //                //double radius = length / (Math.Sqrt(2 * (1 - Math.Cos(angle / 180 * Math.PI))));

                //                double radius = Math.Abs(length / (2 * Math.Sin(angle / 360 * Math.PI)));

                //                arc.Size = new System.Windows.Size(radius, radius);
                //                arc.RotationAngle = angle;

                //                arc.SweepDirection = bulge < 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                //                arc.IsLargeArc = Math.Abs(bulge) > 1 ? true : false;
                //            }

                //            prePoint = point;
                //            bulge = seg.Bulge;
                //            //angle = 4 * System.Math.Atan(seg.Bulge) / Math.PI * 180;
                //            path.Segments.Add(arc);

                //        }
                //    }
                //    PathGeometry pathgeo = new PathGeometry();
                //    pathgeo.Figures.Add(path);

                //    //foreach (var _p in MyCanvas.Children)
                //    //{
                //    //    System.Windows.Shapes.Path _path = (System.Windows.Shapes.Path)_p;

                //    //    if ((string)_path.Tag == p.Layer.Name)
                //    //    {

                //    //        ((GeometryGroup)_path.Data).Children.Add(pathgeo);
                //    //    }
                //    //}
                //    System.Windows.Shapes.Path _path = new System.Windows.Shapes.Path();

                //    GeometryGroup GeoGroup = new GeometryGroup();
                //    GeoGroup.Children.Add(pathgeo);
                //    _path.Stroke = new SolidColorBrush(System.Windows.Media.Colors.White);

                //    _path.Data = GeoGroup;
                //    _path.Tag = "polylines";
                //    _path.StrokeThickness = 2;
                //    _path.MouseLeftButtonDown += (o, s) =>
                //    {
                //        CalculateIntersection(_path);
                //    };


                //    MyCanvas.Children.Add(_path);
                //}
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
                    if (p is Path)
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
                    if (p is Path)
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

            //double maxLeft=0 ;
            //double maxRight=0;
            //double maxTop=0; 
            //double maxBottom=0 ;

            var bound = ((GeometryGroup)((System.Windows.Shapes.Path)(MyCanvas.Children[0])).Data).Bounds;

            double maxLeft = bound.Left;
            double maxRight = bound.Right;
            double maxTop = bound.Top;
            double maxBottom = bound.Bottom;

            for (int i = 0; i < MyCanvas.Children.Count; i++)
            {
                if (MyCanvas.Children[i] is Path)
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
                if (p is Path)
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
    }
}
