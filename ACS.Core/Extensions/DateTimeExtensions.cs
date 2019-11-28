using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixDate(this DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }
}