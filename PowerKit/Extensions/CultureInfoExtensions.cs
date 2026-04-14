#nullable enable
using System.Collections.Generic;
using System.Globalization;

namespace PowerKit.Extensions;

internal static class CultureInfoExtensions
{
    extension(CultureInfo culture)
    {
        /// <summary>
        /// Enumerates the parent cultures of the current culture, from the immediate parent up to
        /// <see cref="CultureInfo.InvariantCulture" />.
        /// </summary>
        public IEnumerable<CultureInfo> GetParents()
        {
            var current = culture;
            while (!current.Equals(current.Parent))
            {
                current = current.Parent;
                yield return current;
            }
        }

        /// <summary>
        /// Enumerates the current culture and its parent cultures, from the current culture up to
        /// <see cref="CultureInfo.InvariantCulture" />.
        /// </summary>
        public IEnumerable<CultureInfo> GetSelfAndParents()
        {
            yield return culture;
            foreach (var parent in culture.GetParents())
            {
                yield return parent;
            }
        }
    }
}
