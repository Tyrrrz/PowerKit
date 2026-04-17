#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class HashAlgorithmExtensions
{
    extension(HashAlgorithm)
    {
        /// <summary>
        /// Computes the hash of the specified data using the given algorithm, then disposes the algorithm.
        /// </summary>
        public static byte[] ComputeHash(HashAlgorithm algorithm, byte[] data)
        {
            using (algorithm)
                return algorithm.ComputeHash(data);
        }
    }
}
