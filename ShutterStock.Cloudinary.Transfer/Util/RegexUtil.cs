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
    }
}

