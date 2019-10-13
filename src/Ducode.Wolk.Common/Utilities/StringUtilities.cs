namespace Ducode.Wolk.Common.Utilities
{
    public static class StringUtilities
    {
        public static string Shorten(
            this string input,
            int maxLength = 100,
            string textAfterShortened = "...",
            bool stripNewlines = false)
        {
            var result = input;
            if (stripNewlines)
            {
                result = result
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty);
            }

            if (input.Length > maxLength)
            {
                result = result.Substring(0, maxLength) + textAfterShortened;
            }

            return result;
        }
    }
}
