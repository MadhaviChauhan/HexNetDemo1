using System;

namespace Test_Solution1.Common.Extension
{
    public static class DateTimeExtension
    {
        public static string ToSqlDate(this DateTime? date)
        {
            return date == null ? null : date.Value.ToString("yyyy-MM-dd");
        }

        /// There is one implementation here http://stackoverflow.com/questions/4604461/c-sharp-datetime-to-add-subtract-working-days
        /// But, Going to keep this other implementation, altough a bit less easy to read than the above one
        /// it does not use loops which is better performing.
        /// http://stackoverflow.com/questions/1044688/add-business-days-and-getbusinessdays
        /// <summary>
        ///     Returns a date in the future or in the past skipping Saturdays and Sundays (we are excluding complex holidays logic for now just want to avoid weekends):
        ///     Example: So if today is thursday and we pass a +3 it should return the date for next tuesday (Friday/Monday & Tuesday are three business days from today)
        ///     If today is Tuesday and we pass a -5 then it should return the date for last Tuesday (Monday/ Friday/Thursday/Wednesday & Tuesday are five business days ago from today)
        /// </summary>
        /// <param name="date">a valid non null date from where the days are going to be added or substracted</param>
        /// <param name="numberOfDays">a positive number to get a date in the future or negative number to get a date in the past</param>
        /// <returns>Returns a datetime which is always a weekday/valid business day</returns>
        public static DateTime AddBusinessDays(this DateTime date, int numberOfDays)
        {
            if (numberOfDays == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                numberOfDays -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                numberOfDays -= 1;
            }

            date = date.AddDays(numberOfDays / 5 * 7);
            var extraDays = numberOfDays % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }
            return date.AddDays(extraDays);
        }
    }
}