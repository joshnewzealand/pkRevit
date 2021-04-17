using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace pkRevitDatasheets.Converters

{
    [ValueConversion(typeof(string), typeof(string))]
    public class PutExtensionFirst : IValueConverter
    {
        public static PutExtensionFirst Instance = new PutExtensionFirst();

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string fileName = value.ToString();

            string str_Last4Characters = fileName.Substring(Math.Max(0, fileName.Length - 3)).ToUpper();

            fileName = "(" + str_Last4Characters + ") " + fileName;

            return fileName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



    [ValueConversion(typeof(Bitmap), typeof(BitmapImage))]
    public class BitMapToBitmapImage : IValueConverter
        {
            private Thickness m_Margin = new Thickness(0.0);

            public Thickness Margin
            {
                get { return m_Margin; }
                set { m_Margin = value; }
            }
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            MemoryStream ms = new MemoryStream();
            ((Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }



}