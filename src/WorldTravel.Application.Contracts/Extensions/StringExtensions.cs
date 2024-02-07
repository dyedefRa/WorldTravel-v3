using System;
using System.Globalization;
using System.Linq;

namespace WorldTravel.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// wwwroot alanını kaldırır.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFileShownPath(this string value)
        {
            return value.Replace("wwwroot", "");
        }

        public static bool IsValidTcNumber(this string value)
        {
            bool returnvalue = false;

            if (value.Length == 11 && value.All(char.IsDigit))
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(value);

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

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string ApplyShortening(this string value, int maxLength = 1000)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            else if (value.Length < maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength) + " ...";
        }

        public static string ToAllDateString(this DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm");
        }

        public static string ToLocalDateString(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        public static string ToTurkishDateString(this DateTime value)
        {
            return value.ToString("dd MMMM yyyy", new CultureInfo("tr-TR"));
        }

        public static string ToZodiacSign(this DateTime value)
        {
            if (value == null)
            {
                return "";
            }
            int month = value.Month;
            int day = value.Day;
            string astro_sign = "";
            if (month == 12)
            {
                if (day < 22)
                    astro_sign = "Yay";
                else
                    astro_sign = "Oğlak";
            }
            else if (month == 1)
            {
                if (day < 20)
                    astro_sign = "Oğlak";
                else
                    astro_sign = "Kova";
            }
            else if (month == 2)
            {
                if (day < 19)
                    astro_sign = "Kova";
                else
                    astro_sign = "Balık";
            }
            else if (month == 3)
            {
                if (day < 21)
                    astro_sign = "Balık";
                else
                    astro_sign = "Koç";
            }
            else if (month == 4)
            {
                if (day < 20)
                    astro_sign = "Koç";
                else
                    astro_sign = "Boğa";
            }
            else if (month == 5)
            {
                if (day < 21)
                    astro_sign = "Boğa";
                else
                    astro_sign = "İkizler";
            }
            else if (month == 6)
            {
                if (day < 21)
                    astro_sign = "İkizler";
                else
                    astro_sign = "Yengeç";
            }
            else if (month == 7)
            {
                if (day < 23)
                    astro_sign = "Yengeç";
                else
                    astro_sign = "Aslan";
            }
            else if (month == 8)
            {
                if (day < 23)
                    astro_sign = "Aslan";
                else
                    astro_sign = "Başak";
            }
            else if (month == 9)
            {
                if (day < 23)
                    astro_sign = "Başak";
                else
                    astro_sign = "Terazi";
            }
            else if (month == 10)
            {
                if (day < 23)
                    astro_sign = "Terazi";
                else
                    astro_sign = "Akrep";
            }
            else if (month == 11)
            {
                if (day < 22)
                    astro_sign = "Akrep";
                else
                    astro_sign = "Yay";
            }

            return astro_sign + " Burcu";
        }

        public static string ToMessageSendDateString(this DateTime value)
        {
            var now = DateTime.Now;

            var totalMinutes = (now - value).TotalMinutes;

            if (totalMinutes <= 3)
            {
                return "Şimdi";
            }
            if (totalMinutes <= 5)
            {
                return "Az önce";
            }
            else if (totalMinutes <= 60)
            {
                return Math.Ceiling(totalMinutes).ToString() + " dakika önce";
            }

            var totalHours = (now - value).TotalHours;
            if (totalHours <= 23)
            {
                return Math.Ceiling(totalHours).ToString() + " saat önce";
            }

            var totalDays = (now - value).TotalDays;
            if (totalDays <= 29)
            {
                return Math.Ceiling(totalDays).ToString() + " gün önce";
            }

            var totalMonth = GetMonthDifference(now, value);

            if (totalMonth <= 11)
            {
                return Math.Ceiling(totalDays).ToString() + " ay önce";
            }

            return "Yıllar önce";

        }

        private static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static string GenerateUserName(string email)
        {
            Random rand = new Random();
            string number = rand.Next(1000).ToString("D3");

            return email.Split('@')[0] + number;
        }
    }
}
