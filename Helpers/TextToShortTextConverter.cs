using System;
using System.Globalization;
using System.Windows.Data;

// label - 32
// login - 32

namespace AmnesiaManager.Helpers
{
    internal class TextToShortTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (
                value is not string text ||
                parameter is not int maxQuantityLetters
            ) return string.Empty;

            if (text.Length > maxQuantityLetters) text = text[..(maxQuantityLetters - 2)] + "..";
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
