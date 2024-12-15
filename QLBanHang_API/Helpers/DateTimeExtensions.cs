namespace PBL6_BackEnd.Helpers
{
    public static class DateTimeExtensions
    {
        public static long GetTimeStamp(this DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }
    }
}
