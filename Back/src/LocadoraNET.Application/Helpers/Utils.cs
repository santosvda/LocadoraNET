using System;

namespace LocadoraNET.Application.Helpers
{
    public static class Utils
    {
        public static DateTime StringToDate(string dateString, string format = "dd/MM/yyyy")
        {
            return DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
