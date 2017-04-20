using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryStepsStudy : BaseObject
    {
        public int Number { get; set; }
        public PeriodFormat Format { get; set; }
        public int Period { get; set; }

        public MemoryStepsStudy()
        { }

        public MemoryStepsStudy(int number, PeriodFormat format, int period)
        {
            this.Number = number;
            this.Format = format;
            this.Period = period;
        }
    }

    public enum PeriodFormat
    {
        Sec = 0,
        Min = 1,
        Hour = 2,
        Day = 3,
        Month = 4,
        Year = 5
    }
}
