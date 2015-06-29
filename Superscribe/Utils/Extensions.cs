namespace Superscribe.Utils
{
    public static class Extensions
    {
        public static void SplitPathAndQuery(this string route, out string path, out string querystring)
        {
            path = string.Empty;
            querystring = string.Empty;

            var parts = route.Split('?');
            if (parts.Length > 0)
            {
                path = parts[0];
            }

            if (parts.Length > 1)
            {
                querystring = parts[1];
            }
        }
    }
}