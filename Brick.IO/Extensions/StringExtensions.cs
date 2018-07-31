namespace Brick.IO
{
    public static class StringExtensions
    {
        public static string LastRightPart(this string text, char needle)
        {
            if (text == null)
            {
                return null;
            }

            var needlePosition = text.LastIndexOf(needle);
            
            if (needlePosition != -1)
            {
                return text.Substring(needlePosition + 1);
            }

            return text;
        }
    }
}