using System.Text.RegularExpressions;

namespace SFA.DAS.FAA.Web.Extensions;

public static class AccessibilityExtensions
{
    private static readonly TimeSpan RegexMaxTimeOut = TimeSpan.FromSeconds(3);
    
    public static string? MakeSingleItemListsAccessible(this string? source)
    {
        if (source is null)
        {
            return null;
        }

        var regex = new Regex(@"(?<List><ul>[\n|\r| ]*<li>(?<Content>.*?)<\/li>[\n|\r| ]*<\/ul>)", RegexOptions.None, RegexMaxTimeOut);
        var matches = regex.Matches(source);
        var lookup = matches.Select(x => Tuple.Create(x.Groups["List"].Value, $"<p>{x.Groups["Content"].Value}</p>")).Distinct().ToDictionary(x => x.Item1, x => x.Item2);
        return regex.Replace(source, m => lookup[m.Value]);
    }
}