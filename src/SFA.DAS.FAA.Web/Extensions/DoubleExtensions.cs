namespace SFA.DAS.FAA.Web.Extensions;

public static class DoubleExtensions
{
    public static decimal ToGeoDecimalWithMetreAccuracy(this double value)
    {
        // 5 decimal places is meter accuracy
        const int accuracy = 100000;
        var d = Convert.ToDecimal(value) * accuracy;
        return decimal.Truncate(d) / accuracy;
    }
}