using NpgsqlTypes;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp2
{
    public class EnumConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = value == null ? null : (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }

    }

    public enum criticimport
    {
        [Description("I")]
        I = 0,
        [Description("II")]
        II = 1,
        [Description("III")]
        III = 2
    }
    public enum dangerclass
    {
        [Description("I")]
        I = 0,
        [Description("II")]
        II = 1,
        [Description("II")]
        III = 2,
        [Description("IV")]
        IV = 3
    }
    public enum mechtype
    {
        [Description("ИТСО")]
        I = 0,
        [Description("Служба безопасности")]
        II = 1
    }
    public enum wartype
    {
        [Description("Чрезвычайная ситуация")]
        I = 0,
        [Description("Противоправные действия")]
        II = 1
    }

    class Utils
    {

    }

}
