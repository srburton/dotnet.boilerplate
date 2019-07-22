using System.Text.RegularExpressions;

namespace App.Infra.Integration.Redis.Extensions
{
    internal static class StringExtension
    {
        public static string Clean(this string str)
            => Regex.Replace(str, @"\W", string.Empty);
    }
}
