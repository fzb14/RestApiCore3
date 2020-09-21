using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetAge(this DateTimeOffset dateTimeOffset)
        {
            var now = DateTime.UtcNow;
            int age = now.Year - dateTimeOffset.Year;
            if(now < dateTimeOffset.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
