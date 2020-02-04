using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using System.IO;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Globalization;
namespace Arch.Utilities.Manager
{
    public static class UtilityManager
    {
        public static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; 
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }
        public static bool IsIdentityNo(string identityNo)
        {
            bool returnvalue = false;
            if (identityNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(identityNo);

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return returnvalue;
        }

        public static string CreateCode()
        {
            Random rand = new Random();
            var result = "";
            for (int i = 0; i < 6; i++)
            {
                result += rand.Next(0, 9).ToString();
            }
            return result;
        }

        public static string GetInitials(string text)
        {
            var result = text.Split(" ".ToCharArray());
            text = "";
            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item))
                    text += item.Substring(0, 1);
            }
            return text;
        }

        public static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(long value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            long i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }
        static string[] iconArray = { "fa fa-file-excel-o", "fa fa-file-excel-o", "fa fa-file-pdf-o", "fa fa-file-word-o", "fa fa-file-powerpoint-o", "fa fa-file-zip-o", "fa fa-file-image-o", "fa fa-file-text", "fa fa-file-audio-o", "fa fa-file-video-o", "fa fa-file-o" };
        static string[] fileContentTypeArray = { "excel", "sheet", "pdf", "word", "presentation", "octet-stream", "image", "text", "audio", "video", "access" };
        public static string GetIcon(string dosyaUzantisi)
        {
            for (int i = 0; i < iconArray.Length; i++)
                if (dosyaUzantisi.Contains(fileContentTypeArray[i]))
                    return iconArray[i];
            return "";
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        public static string ToTitleCase(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        public static string GetNameableDate(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return (ts.Seconds == 1 || ts.Seconds == 0) ? "şimdi" : ts.Seconds + " saniye önce";

            if (delta < 2 * MINUTE)
                return "bir dakika önce";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " dakika önce";

            if (delta < 90 * MINUTE)
                return "bir saat önce";

            if (delta < 24 * HOUR)
                return ts.Hours + " saat önce";

            if (delta < 48 * HOUR)
                return "dün";

            if (delta < 30 * DAY)
                return ts.Days + " gün önce";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "bir ay önce" : months + " ay önce";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "bir yıl önce" : years + " yıl önce";
            }
        }
    }
}
