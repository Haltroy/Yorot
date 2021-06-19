using HTAlt;
using System;

namespace Yorot
{
    /// <summary>
    /// Yorot Date and Time Formats Class
    /// </summary>
    public class YorotDateAndTime
    {
        /// <summary>
        /// Creates a new Yorot Date and Time Formattor
        /// </summary>
        /// <param name="shortFormat">Short format, used by <see cref="DateTime.ToString"/>.</param>
        /// <param name="longFormat">Long format, used by <see cref="GetLongName(DateTime, string[])"/> and <seealso cref="GetLongNameWithoutClock(DateTime, string[])"/>.</param>
        public YorotDateAndTime(string name, string shortFormat, string longFormat)
        {
            Name = name;
            ShortFormat = shortFormat;
            LongFormat = longFormat;
        }

        /// <summary>
        /// The super-short name of the format.
        /// </summary>
        public string Name { get; set; } = "DMY";

        /// <summary>
        /// Short format, used by <see cref="DateTime.ToString"/>.
        /// </summary>
        public string ShortFormat { get; set; } = "dd/MM/yyyy HH:mm:ss:FFFFFFF";

        /// <summary>
        /// Long format, used by <see cref="GetLongName(DateTime, string[])"/> and <seealso cref="GetLongNameWithoutClock(DateTime, string[])"/>.
        /// </summary>
        public string LongFormat { get; set; } = "[day] [month] [year] [clocksecond] [dayofweek]";

        /// <summary>
        /// Converts <paramref name="dateTime"/> to short format.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns><see cref="string"/></returns>
        public string GetShortName(DateTime dateTime)
        {
            return dateTime.ToString(ShortFormat);
        }

        /// <summary>
        /// Converts <paramref name="dateTime"/> to long format.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <param name="months">A <see cref="string[]"/> containing a list of month names.</param>
        /// <param name="days">A <see cref="string[]"/> containing a list of day names staring from Sunday.</param>
        /// <returns><see cref="string"/></returns>
        public string GetLongName(DateTime dateTime, string[] months, string[] days)
        {
            string[] list = LongFormat.Split(' ');
            string x = string.Empty;
            for (int i = 0; i < list.Length; i++)
            {
                switch (list[i].ToLowerEnglish())
                {
                    case "[day]":
                        x += dateTime.Day + " ";
                        break;

                    case "[month]":
                        x += months[dateTime.Month - 1] + " ";
                        break;

                    case "[year]":
                        x += dateTime.Year + " ";
                        break;

                    case "[clock]":
                    case "[24clock]":
                        x += dateTime.Hour + ":" + dateTime.Minute + " ";
                        break;

                    case "[12clock]":
                        x += (dateTime.Hour > 12 ? ((dateTime.Hour - 12) + ":" + dateTime.Minute + " PM") : (dateTime.Hour + ":" + dateTime.Minute + " AM")) + " ";
                        break;

                    case "[12clocksecond]":
                        x += (dateTime.Hour > 12 ? ((dateTime.Hour - 12) + ":" + dateTime.Minute + ":" + dateTime.Second + " PM") : (dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + " AM")) + " ";
                        break;

                    case "[clocksecond]":
                    case "[24clocksecond]":
                        x += dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + " ";
                        break;

                    case "[dayofweek]":
                        x += days[(int)dateTime.DayOfWeek] + " ";
                        break;
                }
            }
            return x;
        }

        /// <summary>
        /// Converts <paramref name="dateTime"/> to long format without clock.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <param name="months">A <see cref="string[]"/> containing a list of month names.</param>
        /// <param name="days">A <see cref="string[]"/> containing a list of day names staring from Sunday.</param>
        /// <returns><see cref="string"/></returns>
        public string GetLongNameWithoutClock(DateTime dateTime, string[] months, string[] days)
        {
            string[] list = LongFormat.Split(' ');
            string x = string.Empty;
            for (int i = 0; i < list.Length; i++)
            {
                switch (list[i].ToLowerEnglish())
                {
                    case "[day]":
                        x += dateTime.Day + " ";
                        break;

                    case "[month]":
                        x += months[dateTime.Month - 1] + " ";
                        break;

                    case "[year]":
                        x += dateTime.Year + " ";
                        break;

                    case "[clock]":
                    case "[24clock]":
                    case "[12clock]":
                    case "[12clocksecond]":
                    case "[clocksecond]":
                    case "[24clocksecond]":
                        break;

                    case "[dayofweek]":
                        x += days[(int)dateTime.DayOfWeek] + " ";
                        break;
                }
            }
            return x;
        }

        /// <summary>
        /// Day-Month-Year
        /// </summary>
        public static YorotDateAndTime DMY => new YorotDateAndTime("DMY", "dd/MM/yyyy HH:mm:ss:FFFFFFF", "[day] [month] [year] [clocksecond] [dayofweek]");

        /// <summary>
        /// Year-Month-Day
        /// </summary>
        public static YorotDateAndTime YMD => new YorotDateAndTime("YMD", "yyyy/MM/dd HH:mm:ss:FFFFFFF", "[day] [month] [year] [clocksecond] [dayofweek]");

        /// <summary>
        /// Month-Day-Year
        /// </summary>
        public static YorotDateAndTime MDY => new YorotDateAndTime("MDY", "MM/dd/yyyy HH:mm:ss:FFFFFFF", "[day] [month] [year] [12clocksecond] [dayofweek]");
    }
}