using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MMK.SmartSystem.LE.Host.Converters
{
    public class ImageCoverter : IValueConverter
    {

        #region Converter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {

                string temp = $"pack://application:,,,/MMK.SmartSystem.LE.Host;component/Resources/Images/menu-{path}.png";
                return new BitmapImage(new Uri(temp));
            }
            else
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion

    }

}
