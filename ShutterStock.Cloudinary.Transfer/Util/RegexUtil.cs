using System.Text.RegularExpressions;

namespace ShutterStock.Cloudinary.Transfer.Util
{
    public static class RegexUtil
    {
        public static int? ParseId(string input)
        {
            var regex = new Regex(@"\-([0-9])\w+\.([a-z]{2,4})\w+");
            var match = regex.Match(input.ToLower());
            var result = match.Success ? match.Groups[0].Value : null;

            if (string.IsNullOrEmpty(result))
                return null;

            var idAsString = Regex.Match(result, @"\d+").Value;

            if (string.IsNullOrEmpty(idAsString))
                return null;

            return int.Parse(idAsString);
        }

        public static string GetStringBetweenCharacters(string input, char charFrom, char charTo)
        {
            var posFrom = input.IndexOf(charFrom);
            if (posFrom != -1) //if found char
            {
                var posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1) //if found char
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }
    }
}

