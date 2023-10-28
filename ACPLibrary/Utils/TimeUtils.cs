namespace ACPLibrary.Utils;

public class TimeUtils
{
    public static long GetTimeStamp()
    {
        System.DateTime time = System.DateTime.Now;
        long ts = ConvertDateTimeToInt(time);
        return ts;
    }

    private static long ConvertDateTimeToInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000;     
        return t;
    }
}