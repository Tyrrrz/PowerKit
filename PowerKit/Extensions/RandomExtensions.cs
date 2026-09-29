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
        /// Generates a random double value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="minValue">The minimum value of the random number.</param>
        /// <param name="maxValue">The maximum value of the random number.</param>
        public double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    "The minimum value must be less than or equal to the maximum value."
                );
            }

            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Generates a random double value between 0 and the specified maximum value.
        /// </summary>
        /// <param name="maxValue">The maximum value of the random number.</param>
        public double NextDouble(double maxValue) => random.NextDouble(0, maxValue);

        /// <summary>
        /// Generates a random float value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="minValue">The minimum value of the random number.</param>
        /// <param name="maxValue">The maximum value of the random number.</param>
        public float NextSingle(float minValue, float maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    "The minimum value must be less than or equal to the maximum value."
                );
            }

            return random.NextSingle() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Generates a random float value between 0 and the specified maximum value.
        /// </summary>
        /// <param name="maxValue">The maximum value of the random number.</param>
        public float NextSingle(float maxValue) => random.NextSingle(0, maxValue);

        /// <summary>
        /// Generates a random boolean value, with the specified probability of being <see langword="true" />.
        /// </summary>
        /// <param name="trueProbability">
        /// The probability of the result being <see langword="true" />, expressed as a value between
        /// 0 (never) and 1 (always) inclusive.
        /// </param>
        public bool NextBoolean(double trueProbability)
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

        /// <summary>
        /// Generates a random boolean value, with the specified probability of being <see langword="true" />.
        /// </summary>
        /// <param name="trueProbability">
        /// The probability of the result being <see langword="true" />, expressed as a value between
        /// 0 (never) and 1 (always) inclusive.
        /// </param>
        public bool NextBoolean(float trueProbability)
        {
            if (float.IsNaN(trueProbability) || trueProbability is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(trueProbability),
                    "The probability must be between 0 and 1."
                );
            }

            return random.NextSingle() < trueProbability;
        }

        /// <summary>
        /// Generates a random boolean value with a 50% probability of being <see langword="true" />.
        /// </summary>
        public bool NextBoolean() => random.NextBoolean(0.5f);
    }
}
