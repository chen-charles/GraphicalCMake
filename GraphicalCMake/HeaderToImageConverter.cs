using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using System.IO;
namespace GraphicalCMake
{
    #region HeaderToImageConverter

    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //MessageBox.Show(value as string);
            foreach (string i in Directory.GetLogicalDrives())
            {
                if (i == value as string)
                {
                    Uri uri = new Uri("pack://application:,,,/Images/diskdrive.png");
                    BitmapImage source = new BitmapImage(uri);
                    return source;
                }
            }
            if (Directory.Exists(value as string))
            {
                Uri uri = new Uri("pack://application:,,,/Images/folder.png");
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }

    #endregion // DoubleToIntegerConverter
}
