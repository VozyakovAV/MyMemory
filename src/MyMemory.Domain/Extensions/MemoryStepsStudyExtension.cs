using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public static class MemoryStepsStudyExtension
    {
        public static DateTime NextDateTime(this MemoryStepsStudy step)
        {
            var date = CustomDateTime.Now;

            switch (step.Format)
            {
                case PeriodFormat.Sec:      return date.AddSeconds(step.Period);
                case PeriodFormat.Min:      return date.AddMinutes(step.Period);
                case PeriodFormat.Hour:     return date.AddHours(step.Period);
                case PeriodFormat.Day:      return date.AddDays(step.Period);
                case PeriodFormat.Month:    return date.AddMonths(step.Period);
                case PeriodFormat.Year:     return date.AddYears(step.Period);
            }

            return date;
        }
    }
}
