using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.IoC;
using VaBank.Common.Util;

namespace VaBank.Core.Processing
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class BankingSystemSchedule
    {
        private static readonly TimeZoneInfo TimeZoneInfo =
            TimeZoneInfo.CreateCustomTimeZone("BY", TimeSpan.FromHours(3), "BY MINSK", "BY MINSK");

        private static readonly List<DayOfWeek> WorkingDays = new List<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        }; 

        private static readonly Range<TimeSpan> WorkingHours = 
            new Range<TimeSpan>(new TimeSpan(8, 15, 0), new TimeSpan(16, 45, 0));

        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo; }
        }

        public DateTime GetPostDateUtc()
        {
            var now = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo);
            if (!WorkingDays.Contains(localTime.DayOfWeek))
            {
                var nextMonday = DateMath.GetNextWeekday(localTime, DayOfWeek.Monday);
                return TimeZoneInfo.ConvertTimeToUtc(nextMonday.Date);
            }
            var time = localTime.TimeOfDay;
            if (WorkingHours.Contains(time))
            {
                return TimeZoneInfo.ConvertTimeToUtc(localTime);
            }
            if (localTime.DayOfWeek == WorkingDays.Last())
            {
                var nextMonday = DateMath.GetNextWeekday(localTime, DayOfWeek.Monday);
                return TimeZoneInfo.ConvertTimeToUtc(nextMonday.Date);
            }
            return TimeZoneInfo.ConvertTimeToUtc(localTime.AddDays(1));
        }
    }
}
