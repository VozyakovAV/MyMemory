using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class CustomDateTime
    {
        public static DateTime? FakeDate { get; set; }

        public static bool IsFake
        {
            get 
            { 
                return FakeDate.HasValue; 
            }
        }

        public static DateTime Now
        {
            get 
            { 
                return IsFake ? FakeDate.Value : DateTime.Now; 
            }
        }
    }
}
