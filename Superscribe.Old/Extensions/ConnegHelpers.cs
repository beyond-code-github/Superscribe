namespace Superscribe.Owin.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class ConnegHelpers
    {
        public static IEnumerable<string> GetWeightedValues(IEnumerable<string> values)
        {
            values = GetSplitValues(values);

            var parsed = values.Select(x =>
            {
                var sections = x.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var mediaRange = sections[0].Trim();
                var quality = 1m;

                for (var index = 1; index < sections.Length; index++)
                {
                    var trimmedValue = sections[index].Trim();
                    if (trimmedValue.StartsWith("q=", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal temp;
                        var stringValue = trimmedValue.Substring(2);
                        if (decimal.TryParse(stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                        {
                            quality = temp;
                            break;
                        }
                    }
                }

                return new Tuple<string, decimal>(mediaRange, quality);
            });

            return parsed.OrderByDescending(x => x.Item2).Select(o => o.Item1);
        }

        private static IEnumerable<string> GetSplitValues(IEnumerable<string> values)
        {
            return values
                .SelectMany(x => x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim())
                .ToList();
        }
    }
}
