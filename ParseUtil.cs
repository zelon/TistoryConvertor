using System;

namespace TistoryConvertor
{
    class ParseUtil
    {
        public static DateTime ParseDateTime(string s)
        {
            long numeric = long.Parse(s);
            var offset = DateTimeOffset.FromUnixTimeSeconds(numeric).AddHours(9);
            DateTime output = offset.DateTime;
            return output;
        }
    }
}
