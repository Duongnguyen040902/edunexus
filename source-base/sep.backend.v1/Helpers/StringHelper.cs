namespace sep.backend.v1.Helpers
{
    public static class StringHelper
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
                return str;

            char[] chars = str.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }

        public static string FormatMessage(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }
            return template;
        }

        public static string FormatMaxLengthMessage(string template, string attribute, int maxLength)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", attribute },
                { "maxLength", maxLength.ToString() }
            };
            return FormatMessage(template, placeholders);
        }

        public static string FormatMinLengthMessage(string template, string attribute, int minLength)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", attribute },
                { "minLength", minLength.ToString() }
            };
            return FormatMessage(template, placeholders);
        }

        public static string FormatMessage(string template, string attribute)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", attribute }
            };
            return FormatMessage(template, placeholders);
        }
        
        public static string FormatDateComparisonMessage(string template, string attribute, string comparisonDate)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", attribute },
                { "comparisonDate", comparisonDate }
            };
            return FormatMessage(template, placeholders);
        }

        public static string FormatValueComparisonMessage(string template, string attribute, string comparisonValue)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", attribute },
                { "comparisonValue", comparisonValue }
            };
            return FormatMessage(template, placeholders);
        }
    }
}