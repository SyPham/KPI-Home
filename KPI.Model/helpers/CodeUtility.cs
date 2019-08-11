using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KPI.Model.helpers
{
    public static class CodeUtility
    {
        public static int ConvertStringDayOfWeekToNumber(this string dayofweek)
        {
            var value = dayofweek.ToSafetyString().ToUpper();
            int result;
            switch (value)
            {
                case "MON":
                    result = 2;
                    break;
                case "TUE":
                    result = 3;
                    break;
                case "WED":
                    result = 4;
                    break;
                case "THU":
                    result = 5;
                    break;
                case "FRI":
                    result = 6;
                    break;
                case "SAT":
                    result = 7;
                    break;
                case "SUN":
                    result = 8;
                    break;
                case "MONDay":
                    result = 2;
                    break;
                case "TUESDAY":
                    result = 3;
                    break;
                case "WEDNESDAY":
                    result = 4;
                    break;
                case "THURSDAY":
                    result = 5;
                    break;
                case "FRIDAY":
                    result = 6;
                    break;
                case "SATURDAY":
                    result = 7;
                    break;
                case "SUNDAY":
                    result = 8;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public static int GetQuarterOfYear(this DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 1;
            else if (date.Month >= 7 && date.Month <= 9)
                return 2;
            else if (date.Month >= 10 && date.Month <= 12)
                return 3;
            else
                return 4;
        }
        /// <summary>
        /// Trả về tuần trong năm.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Trả về tuần trong năm.</returns>
        public static int GetIso8601WeekOfYear(this DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo
                .InvariantCulture
                .Calendar
                .GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static int GetQuarter(this DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 1;
            else if (date.Month >= 7 && date.Month <= 9)
                return 2;
            else if (date.Month >= 10 && date.Month <= 12)
                return 3;
            else
                return 4;
        }
        /// <summary>
        /// Chuyển value về dạng chuỗi.
        /// Trả về dạng chuỗi của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng chuỗi của value.</returns>
        public static string ToSafetyString(this object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();

        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(byte).
        /// Trả về dạng số nguyên(byte) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (byte) của value.</returns>
        public static byte ToByte(this object value)
        {
            if (value == null)
                return 0;
            //Khai báo giá trị chứa kết quả mặ định, mặc định là 0
            byte result = 0;

            //Thử ép value thành kiểu byte
            byte.TryParse(value.ToString(), out result);

            //Trả về kết quả đã ép kiểu
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(SByte).
        /// Trả về dạng số nguyên(SByte) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (SByte) của value.</returns>
        public static SByte ToSByte(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            sbyte result = 0;
            sbyte.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(Short).
        /// Trả về dạng số nguyên(Short) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (Short) của value.</returns>
        public static short ToShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            short result = 0;
            short.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(ToUInt).
        /// Trả về dạng số nguyên(ToUInt) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (ToUInt) của value.</returns>
        public static uint ToUInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(Ushort).
        /// Trả về dạng số nguyên(Ushort) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (Ushort) của value.</returns>
        public static ushort ToUShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(int).
        /// Trả về dạng số nguyên(int) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (int) của value.</returns>
        public static int ToInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            int result = 0;
            int.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Float).
        /// Trả về dạng số thực(Float) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Float) của value.</returns>
        public static float ToFloat(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            float result = 0;
            float.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Double).
        /// Trả về dạng số thực (Double) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (double) của value.</returns>
        public static double ToDouble(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            double result = 0;

            double.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Long).
        /// Trả về dạng số thực (Long) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Long) của value.</returns>
        public static long ToLong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            long result = 0;

            long.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Long).
        /// Trả về dạng số thực (Long) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Long) của value.</returns>
        public static ulong ToULong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            ulong result = 0;

            ulong.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(decimal).
        /// Trả về dạng số thực (decimal) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (decimal) của value.</returns>
        public static decimal ToDecimal(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            decimal result = 0;

            decimal.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng kí tự (char).
        /// Trả về dạng kí tự (char) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng kí tự (char) của value.</returns>
        public static char ToChar(this object value)
        {
            //Tối ưu hơn phân cách khi dùng hàm ||
            if (value == null || value.ToString() == string.Empty || (value.ToString().Length > 1))
            {
                return ' ';
            }
            char result = ' ';
            char.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng luận lý (bool).
        /// Trả về dạng luận lý (bool) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng luận lý (bool) của value.</returns>
        public static bool ToBool(this object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.ToInt() == 1)
            {
                return true;
            }

            bool result = false;
            bool.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng ngày giờ (DateTime).
        /// Trả về dạng ngày giờ (DateTime) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng ngày giờ (DateTime) của value.</returns>
        public static DateTime ToDateTime(this object value)
        {
            if (value == null || value.ToString() == string.Empty || value.ToString() == " ")
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;

            string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "d/M/yyyy HH:mm:ss", "d/M/yyyy HH:mm", "dd/MM/yyyy HH:mm", "HH:mm:ss", "HH:mm",
"d-M-yyyy", "dd-MM-yyyy", "d-M-yyyy HH:mm:ss", "d-M-yyyy HH:mm", "dd-MM-yyyy HH:mm", "HH:mm:ss", "HH:mm"};//HH phủ cả từ 1-24h còn hh chỉ phủ từ 1-12h

            string[] dateStrings = {"5/1/2009 6:32 PM", "05/01/2009 6:32:05 PM",
                              "5/1/2009 6:32:00", "05/01/2009 06:32",
                              "05/01/2009 06:32:00 PM", "05/01/2009 06:32:00"};
            DateTime.TryParseExact(value.ToString(), formats,
                                    //new CultureInfo("en-US"),//Lấy văn hóa của Mỹ
                                    CultureInfo.CurrentCulture,//Lấy văn hóa của máy tính đang dùng
                                    DateTimeStyles.None,
                                    out result);
            return result;

        }

        public static bool IsNullOrEmpty(this object value)
        {
            if (value == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(value.ToString());
        }

        public static string SHA256Hash(this string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value.ToSafetyString()));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToSafetyString();
        }

        public static string ReplaceSpecial(this string value)
        {
            if (value != string.Empty)
            {
                value = Regex.Replace(value, @"(\s+|@|&|'|\(|\)|<|>|#|+|-)", "");
            }
            return value;
        }

    }
}
