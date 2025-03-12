using System.Globalization;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class StringExtensions
    {
        public static int GetWordCount(this string? text)
        {
            if (text == null) return 0;

            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }

        public static string? ToDisplayWage(this decimal? text)
        {
            return text.HasValue ? $"{text.Value.ToString("C", new CultureInfo("en-GB")).Replace(".00", "")}" : null;
        }
    }
}
