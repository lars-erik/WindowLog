using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WindowLog.GUI
{
    public class DurationTotalConverter : IValueConverter
    {
        private const string FormatString = @"d\.hh\:mm\:ss";
        private static readonly string Blank = TimeSpan.Zero.ToString(FormatString);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is ICollection<object> coll)
                {
                    if (coll.FirstOrDefault() is EntryModel)
                    {
                        var retVal = coll.Cast<EntryModel>()
                            .Aggregate(TimeSpan.Zero, (c, e) => c + (e.Duration ?? TimeSpan.Zero));
                        return retVal;
                    }

                    if (coll.FirstOrDefault() is CollectionViewGroup)
                    {
                        var retVal = coll.Cast<CollectionViewGroup>().Aggregate(TimeSpan.Zero,
                            (span, item) => span + item.Items.Cast<EntryModel>().Aggregate(TimeSpan.Zero, (c, e) => c + (e.Duration ?? TimeSpan.Zero)));
                        return retVal;
                    }
                }
            }
            catch
            {
                throw;
            }
            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new []{ value };
        }
    }
    public class DurationMultiConverter : IMultiValueConverter
    {
        private const string FormatString = @"d\.hh\:mm\:ss";
        private static readonly string Blank = TimeSpan.Zero.ToString(FormatString);

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value[1] is ICollection<object> coll)
                {
                    if (coll.FirstOrDefault() is EntryModel)
                    {
                        var retVal = coll.Cast<EntryModel>()
                            .Aggregate(TimeSpan.Zero, (c, e) => c + (e.Duration ?? TimeSpan.Zero));
                        return retVal.ToString(FormatString);
                    }

                    if (coll.FirstOrDefault() is CollectionViewGroup)
                    {
                        var retVal = coll.Cast<CollectionViewGroup>().Aggregate(TimeSpan.Zero,
                            (span, item) => span + item.Items.Cast<EntryModel>().Aggregate(TimeSpan.Zero, (c, e) => c + (e.Duration ?? TimeSpan.Zero)));
                        return retVal.ToString(FormatString);
                    }
                }
            }
            catch
            {
                throw;
            }
            return Blank;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new []{ value };
        }
    }
}
