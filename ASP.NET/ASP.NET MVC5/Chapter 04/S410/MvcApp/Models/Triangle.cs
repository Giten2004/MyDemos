using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Triangle
    {
        [DataType("PointInfo")]
        public Point A { get; set; }

        [DataType("PointInfo")]
        public Point B { get; set; }

        [DataType("PointInfo")]
        public Point C { get; set; }
    }

    [TypeConverter(typeof(PointTypeConverter))]
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point Parse(string point)
        {
            string[] split = point.Split(',');
            if (split.Length != 2)
            {
                throw new FormatException("Invalid point expression.");
            }
            double x;
            double y;
            if (!double.TryParse(split[0], out x) ||!double.TryParse(split[1], out y))
            {
                throw new FormatException("Invalid point expression.");
            }
            return new Point(x, y);
        }
    }

    public class PointTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Point.Parse(value as string);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}