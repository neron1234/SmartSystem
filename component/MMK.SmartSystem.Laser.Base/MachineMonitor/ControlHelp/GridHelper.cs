using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor
{
    public class GridHelper
    {
        public static bool GetShowBorder(DependencyObject obj){
            return (bool)obj.GetValue(ShowBorderProperty);
        }

        public static void SetShowBorder(DependencyObject obj, bool value){
            obj.SetValue(ShowBorderProperty, value);
        }

        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(GridHelper), new PropertyMetadata(OnShowBorderChanged));


        public static string GetBorderColor(DependencyObject obj)
        {
            return (string)obj.GetValue(BorderColorProperty);
        }

        public static void SetBorderColor(DependencyObject obj, string value)
        {
            obj.SetValue(BorderColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.RegisterAttached("BorderColor", typeof(string), typeof(GridHelper), new PropertyMetadata(OnBorderColorChanged));

        private static void OnBorderColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
        }


        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e){
            var grid = d as Grid;
            if((bool)e.OldValue)
            {
                grid.Loaded -= (s, arg) => { };
            }
            if((bool)e.NewValue)
            {
                grid.Loaded += (s, arg) =>
                {
                    var controls = grid.Children;
                    var count = controls.Count;

                    for(int i = 0; i < count; i++)
                    {
                        var item = controls[i] as FrameworkElement;
                        var border = new Border()
                        {
                            BorderBrush = (Brush)new BrushConverter().ConvertFromString("#222529"),
                            BorderThickness = new Thickness(1),
                            Padding = new Thickness(20)
                        };

                        var row = Grid.GetRow(item);
                        var column = Grid.GetColumn(item);
                        var rowspan = Grid.GetRowSpan(item);
                        var columnspan = Grid.GetColumnSpan(item);

                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);


                        grid.Children.RemoveAt(i);
                        border.Child = item;
                        grid.Children.Insert(i, border);
                    }
                };
            }
        }
    }
}
