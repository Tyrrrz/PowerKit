#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class VersionExtensions
{
    extension(Version version)
    {
        /// <summary>
        /// Formats the version as a semantic version string, omitting the revision component if it is not set or is zero.
        /// </summary>
        public string ToSemanticString() =>
            version.Build < 0 ? version.ToString(2)
            : version.Revision <= 0 ? version.ToString(3)
            : version.ToString();
    }
}
