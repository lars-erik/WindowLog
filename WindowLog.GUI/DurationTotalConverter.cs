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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<object> coll)
            {
                if (coll.FirstOrDefault() is EntryModel)
                {
                    return coll.Cast<EntryModel>().Aggregate(TimeSpan.Zero, (c, e) => c + (e.Duration ?? TimeSpan.Zero));
                }

                if (coll.FirstOrDefault() is CollectionViewGroup)
                {
                    return coll.Cast<CollectionViewGroup>().Aggregate(TimeSpan.Zero, (span, item) => span + (TimeSpan)Convert(item.Items, targetType, parameter, culture));
                }
            }

            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
