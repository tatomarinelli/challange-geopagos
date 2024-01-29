using System.Text.RegularExpressions;

namespace ClientAuthorization.Helpers
{
    public static class StringHelper
    {
        public static string GetJSON(this object value)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                MaxDepth = 0,
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = true,
                WriteIndented = true
            };

            return System.Text.Json.JsonSerializer.Serialize(value, options);
        }

        public static string ValidateURL(string url)
        {
            string pattern = @"^(http|https)://[^\s/$.?#].[^\s]*$";
            Regex regex = new(pattern);
            if (!regex.IsMatch(url)) throw new Exception("Invalid URL");
            return url;
        }
    }
}
