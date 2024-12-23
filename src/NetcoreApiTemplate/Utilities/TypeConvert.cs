using System.Globalization;
using System.Text.Json;

namespace NetcoreApiTemplate.Utilities
{
    public static class TypeConvert
    {

        public static bool IsDateString(this string dateStr)
        {
            DateTime dateTime;
            bool result = false;
            try
            {
                result = DateTime.TryParse(dateStr, out dateTime);
            }
            catch
            {
                result = false;
            }

            return result;
        }
        public static DateTime ToDate(this string dateStr)
        {
            return DateTime.Parse(dateStr);
        }
        public static DateTime? ToDateNull(this string dateStr)
        {
            DateTime? dateTime;
            try
            {
                dateTime = DateTime.Parse(dateStr);
            }
            catch
            {
                dateTime = null;
            }

            return dateTime;
        }
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            var str = obj.ToString() ?? string.Empty;
            return decimal.Parse(str, NumberStyles.Float);
        }
        public static decimal? ToDecimalNull(this object obj)
        {
            try
            {
                return obj.ToDecimal();
            }
            catch
            {
                return null;
            }
        }
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj.ToString());
        }
        public static int? ToIntNull(this object obj)
        {
            try
            {
                return obj.ToInt();
            }
            catch
            {
                return null;
            }
        }
        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj.ToString());
        }
        public static double? ToToDoubleNull(this object obj)
        {
            try
            {
                return obj.ToDouble();
            }
            catch
            {
                return null;
            }
        }
        public static string ToJson(this object obj)
        {
            obj ??= "";
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var result = JsonSerializer.Serialize(obj, jsonOptions);
            return result;
        }
        public static T? JsonToObject<T>(this string str, bool caseSensitive = false)
        {
            try
            {
                var jsonOptions = caseSensitive ? null : new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<T>(str, jsonOptions);
                return result;
            }
            catch
            {
                return default;
            }
        }

        public static T? Clone<T>(this T obj)
        {
            try
            {
                var res = obj!.ToJson().JsonToObject<T>();
                return res;
            }
            catch
            {
                return default;
            }
        }
        public static bool IsOddNumber(this int value)
        {
            return value % 2 != 0;
        }
    }
}
