namespace Enhancer.Extensions
{
    static partial class NumberExtensions
    {
        /// <summary>
        /// Restricts an <see cref="sbyte"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static sbyte Clamp(this sbyte value, sbyte min, sbyte max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts a <see cref="byte"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static byte Clamp(this byte value, byte min, byte max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="short"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static short Clamp(this short value, short min, short max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="ushort"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static ushort Clamp(this ushort value, ushort min, ushort max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="int"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="uint"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static uint Clamp(this uint value, uint min, uint max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="long"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static long Clamp(this long value, long min, long max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts an <see cref="ulong"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static ulong Clamp(this ulong value, ulong min, ulong max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts a <see cref="float"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Restricts a <see cref="double"/> value in between the
        /// specified values.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        /// <param name="min">
        /// The lower-bound below which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <param name="max">
        /// The upper-bound above which <paramref name="value"/> isn't allowed
        /// to go.
        /// </param>
        /// <returns>
        /// If the <paramref name="value"/> goes below <paramref name="min"/>
        /// it will return <paramref name="min"/>;
        /// if the <paramref name="value"/> goes above <paramref name="max"/>
        /// it will return <paramref name="max"/>;
        /// otherwise it returns <paramref name="value"/>.
        /// </returns>
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }
    }
}