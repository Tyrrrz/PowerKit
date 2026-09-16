using System;

namespace PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Random" />.
/// </summary>
public static class RandomExtensions
{
    extension(Random random)
    {
        /// <summary>
        /// Generates a random boolean value, with the specified probability of being <see langword="true" />.
        /// </summary>
        /// <param name="trueProbability">
        /// The probability of the result being <see langword="true" />, expressed as a value between
        /// 0 (never) and 1 (always) inclusive.
        /// </param>
        public bool NextBoolean(double trueProbability = 0.5)
        {
            if (double.IsNaN(trueProbability) || trueProbability is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(trueProbability),
                    "The probability must be between 0 and 1."
                );
            }

            return random.NextDouble() < trueProbability;
        }
    }
}
