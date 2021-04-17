using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.IO;

namespace QuickZip.Tools
{
    [ValueConversion(typeof(string), typeof(string))]
    public class PathToNameConverter : IValueConverter
    {
        public static PathToNameConverter Instance = new PathToNameConverter();

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string fileName = (string)value;
            return Path.GetFileName(fileName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class WidthSansMarginConverterNegative50 : IValueConverter
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
            if (targetType != typeof(double)) { return null; }

            double dParentWidth = Double.Parse(value.ToString()) - 250;
           // double dAdjustedWidth = dParentWidth - m_Margin.Left - m_Margin.Right;
            return (dParentWidth < 0 ? 0 : dParentWidth);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class WidthSansMarginConverterNegative100 : IValueConverter
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
            if (targetType != typeof(double)) { return null; }

            double dParentWidth = Double.Parse(value.ToString()) - 270;
            // double dAdjustedWidth = dParentWidth - m_Margin.Left - m_Margin.Right;


            return (dParentWidth < 0 ? 0 : dParentWidth);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class WidthSansMarginConverterNegative180 : IValueConverter
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
            if (targetType != typeof(double)) { return null; }

            double dParentWidth = Double.Parse(value.ToString()) - 180;
            // double dAdjustedWidth = dParentWidth - m_Margin.Left - m_Margin.Right;
            return (dParentWidth < 0 ? 0 : dParentWidth);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class WidthSansMarginConverterNegative190 : IValueConverter
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
            if (targetType != typeof(double)) { return null; }

            double dParentWidth = Double.Parse(value.ToString()) - 195;
            // double dAdjustedWidth = dParentWidth - m_Margin.Left - m_Margin.Right;
            return (dParentWidth < 0 ? 0 : dParentWidth);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
