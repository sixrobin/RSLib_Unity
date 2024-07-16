namespace RSLib.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Returns the fractional part of a value.
        /// For example, the fractional part of 5.64f is 0.64f.
        /// </summary>
        /// <param name="value">Source value to get the fractional part of.</param>
        /// <returns>Fractional part of the given value.</returns>
        public static float Frac(this float value)
        {
            return value - System.MathF.Floor(value);
        }
        
        /// <summary>
        /// Rounds a value's decimal part to its nearest fraction (half, third, quarter, etc. based on denominator).
        /// For example, rounding 1.6f with a denominator of 2 will return 1.5f. With a denominator of 3, it will return 1.666f.
        /// </summary>
        /// <param name="value">Source value to round to nearest fraction.</param>
        /// <param name="denominator">Fraction denominator.</param>
        /// <returns>Value with decimal part rounded.</returns>
        public static float RoundNearestFraction(this float value, float denominator)
        {
            return System.MathF.Round(value * denominator, System.MidpointRounding.AwayFromZero) / denominator;
        }
    }
}