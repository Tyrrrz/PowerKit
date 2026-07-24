using System;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Version" />.
/// </summary>
public static class VersionExtensions
{
    extension(Version version)
    {
        /// <summary>
        /// Formats the version as a semantic version string, omitting the revision component if it is not set or is zero.
        /// Missing build or patch components are replaced with zero to ensure at least three components are present.
        /// </summary>
        public string ToSemanticString() =>
            version.Build < 0 ? version.ToString(2) + ".0"
            : version.Revision <= 0 ? version.ToString(3)
            : version.ToString();
    }
}
