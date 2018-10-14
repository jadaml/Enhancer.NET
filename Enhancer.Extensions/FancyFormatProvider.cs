/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Extensions.
 *
 * Enhancer.Extensions is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Extensions is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Extensions.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class provides additional formatting options for various base types.
    /// </summary>
    public class FancyFormatProvider : IFormatProvider, ICustomFormatter
    {
        private static readonly IReadOnlyDictionary<Tuple<Type, string>, int> _typeBaseDigitsMapping = new Dictionary<Tuple<Type, string>, int>()
        {
            [Tuple.Create(typeof(byte),   "X")] = 2,
            [Tuple.Create(typeof(sbyte),  "X")] = 2,
            [Tuple.Create(typeof(ushort), "X")] = 4,
            [Tuple.Create(typeof(short),  "X")] = 4,
            [Tuple.Create(typeof(uint),   "X")] = 8,
            [Tuple.Create(typeof(int),    "X")] = 8,
            [Tuple.Create(typeof(ulong),  "X")] = 16,
            [Tuple.Create(typeof(long),   "X")] = 16,
            [Tuple.Create(typeof(byte),   "D")] = 3,
            [Tuple.Create(typeof(sbyte),  "D")] = 3,
            [Tuple.Create(typeof(ushort), "D")] = 5,
            [Tuple.Create(typeof(short),  "D")] = 5,
            [Tuple.Create(typeof(uint),   "D")] = 10,
            [Tuple.Create(typeof(int),    "D")] = 10,
            [Tuple.Create(typeof(ulong),  "D")] = 20,
            [Tuple.Create(typeof(long),   "D")] = 20,
        };

        private static readonly string[] _decDataUnits = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static readonly string[] _iecUnits = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };

        private static FancyFormatProvider _instance;

        /// <summary>
        /// Gets a <see cref="FancyFormatProvider"/> instance.
        /// </summary>
        public static FancyFormatProvider Provider => _instance ?? (_instance = new FancyFormatProvider());

        /// <summary>
        /// Retrieves the string resource for “On” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        public static string On(CultureInfo culture = null) => StringLoader.GetString("On", culture);

        /// <summary>
        /// Retrieves the string resource for “Off” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        public static string Off(CultureInfo culture = null) => StringLoader.GetString("Off", culture);

        /// <summary>
        /// Retrieves the string resource for “Yes” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        public static string Yes(CultureInfo culture = null) => StringLoader.GetString("Yes", culture);

        /// <summary>
        /// Retrieves the string resource for “No” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        public static string No(CultureInfo culture = null) => StringLoader.GetString("No", culture);

        /// <summary>
        /// Retrieves the string resource for “True” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        /// <remarks>
        /// The returned value is not dependent on <see cref="bool.TrueString"/>.
        /// </remarks>
        public static string True(CultureInfo culture = null) => StringLoader.GetString("True", culture);

        /// <summary>
        /// Retrieves the string resource for “False” used when formatting boolean values.
        /// </summary>
        /// <param name="culture">
        /// The culture to be searched for the string resource, or
        /// <see langword="null"/> to search the current culture.
        /// </param>
        /// <returns>
        /// The string resource for the specified culture.
        /// </returns>
        /// <remarks>
        /// The returned value is not dependent on <see cref="bool.FalseString"/>.
        /// </remarks>
        public static string False(CultureInfo culture = null) => StringLoader.GetString("False", culture);

        /// <summary>
        /// Returns itself as a format providing service.
        /// </summary>
        /// <param name="formatType">
        /// The type of requested format providing service.
        /// </param>
        /// <returns>
        /// Itself, if <paramref name="formatType"/> is <see cref="ICustomFormatter"/>,
        /// otherwise <see langword="null"/>.
        /// </returns>
        public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

        /// <inheritdoc/>
        /// <param name="format">
        /// A format string containing formatting specifications.
        /// </param>
        /// <param name="arg">An object to format.</param>
        /// <param name="provider">
        /// An optional format provider to use when formatting the object.
        /// </param>
        public string Format(string format, object arg, IFormatProvider provider = null)
        {
            switch (arg)
            {
                case sbyte si08:
                    return FormatInt(format, (ulong)si08.Clamp(0, sbyte.MaxValue), provider);
                case byte ui08:
                    return FormatInt(format, ui08, provider);
                case short si16:
                    return FormatInt(format, (ulong)si16.Clamp(0, short.MaxValue), provider);
                case ushort ui16:
                    return FormatInt(format, ui16, provider);
                case int si32:
                    return FormatInt(format, (ulong)si32.Clamp(0, int.MaxValue), provider);
                case uint ui32:
                    return FormatInt(format, ui32, provider);
                case long si64:
                    return FormatInt(format, (ulong)si64.Clamp(0, long.MaxValue), provider);
                case ulong ui64:
                    return FormatInt(format, ui64, provider);
                case Enum @enum:
                    return FormatEnum(format, @enum, provider);
                case bool l:
                    return FormatBool(format, l, provider);
                default:
                    return (arg as IFormattable)?.ToString(format, provider) ?? arg?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Formats an integral value.
        /// </summary>
        /// <param name="format">The format string for the value.</param>
        /// <param name="value">The integral value.</param>
        /// <param name="provider">The format provider to use.</param>
        /// <returns>The string representation of <paramref name="value"/>.</returns>
        /// <remarks>
        /// <para>
        /// The following format specifiers are supported:
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Format specifier</term>
        /// <term>Description</term>
        /// </listheader>
        /// <item>
        /// <description>B</description>
        /// <description>
        /// <para>
        /// Formats the integral value as if it specifies the number of bytes
        /// of any arbitrary object, and automatically converts them the next
        /// highest unit of bytes, up to YiB. It will calculate the next highest
        /// value if the value to be written reached 1,000.
        /// </para>
        /// <para>
        /// This format uses the unit symbols specified by the IEC standard.
        /// </para>
        /// <para>Examples:</para>
        /// <para>15 -> 15&#xA0;B</para>
        /// <para>1234567 -> 1.18&#xA0;MiB</para>
        /// </description>
        /// </item>
        /// <item>
        /// <description>B#</description>
        /// <description>
        /// <para>
        /// Same as B, but you can specify a value at when conversion should
        /// occur.
        /// </para>
        /// <para>Examples for "B1500":</para>
        /// <para>15 -> 15&#xA0;B</para>
        /// <para>1234567 -> 1,205.63&#xA0;KiB</para>
        /// </description>
        /// </item>
        /// <item>
        /// <description>b</description>
        /// <description>
        /// <para>
        /// This is similar to the B format specified, but it will rounds the
        /// number by 1000 instead of 1024, and uses the DEC standard.
        /// </para>
        /// <para>Examples:</para>
        /// <para>15 -> 15&#xA0;B</para>
        /// <para>1234567 -> 1.23&#xA0;MB</para>
        /// </description>
        /// </item>
        /// <item>
        /// <description>B#</description>
        /// <description>
        /// <para>
        /// Same as b, but you can specify a value at when conversion should
        /// occur.
        /// </para>
        /// <para>Examples for "b1500":</para>
        /// <para>15 -> 15&#xA0;B</para>
        /// <para>1234567 -> 1,234.57&#xA0;kB</para>
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        private static string FormatInt(string format, ulong value, IFormatProvider provider)
        {
            // Note: So far, the following implementation does not inherently throws exceptions.
            if (format?.ToUpper().StartsWith("B") ?? false)
            {
                if (!uint.TryParse(format.Substring(1), out uint rounding))
                {
                    rounding = 1000;
                }

                return FormatBytes(provider, value, format[0] == 'B', rounding);
            }

            try
            {
                return value.ToString(format, provider);
            }
            catch (FormatException fex)
            {
                throw new FormatException(fex.Message, fex);
            }
        }

        /// <summary>
        /// Provides additional formatting string support for enumerable values.
        /// </summary>
        /// <param name="format">The format specifier for the value.</param>
        /// <param name="arg">The value to format.</param>
        /// <param name="provider">The format provider to use during conversion.</param>
        /// <returns>The string representation of the value.</returns>
        /// <remarks>
        /// <para>
        /// The following additional format specifiers are supported:
        /// </para>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Format specifier</term>
        ///         <term>Description</term>
        ///     </listheader>
        ///     <item>
        ///         <description>N</description>
        ///         <description>
        ///             <para>
        ///             Converts the enumerable value name to a human friendly representation.
        ///             </para>
        ///             <para>Example:</para>
        ///             <para>ConsoleColor.DarkBlue -> "Dark Blue"</para>
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>X#</description>
        ///         <description>
        ///             <para>
        ///             Converts the enumerable to it's hexadecimal representation to the
        ///             specified number of digits.
        ///             </para>
        ///             <para>Example:</para>
        ///             <para>X3: AttributeTargets.GenericParameter -> 4000</para>
        ///             <para>X6: AttributeTargets.GenericParameter -> 004000</para>
        ///             <para>X0: AttributeTargets.GenericParameter -> 00004000</para>
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>x#</description>
        ///         <description>Same as X but with lowercase letters.</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <para>D#</para>
        ///             <para>d#</para>
        ///         </description>
        ///         <description>
        ///             <para>
        ///             Converts the enumerable to it's decimal representation to the
        ///             specified number of digits.
        ///             </para>
        ///             <para>Example:</para>
        ///             <para>D3: AttributeTargets.GenericParameter -> 16384</para>
        ///             <para>D6: AttributeTargets.GenericParameter -> 016384</para>
        ///             <para>D0: AttributeTargets.GenericParameter -> 0000016384</para>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        private static string FormatEnum(string format, Enum arg, IFormatProvider provider)
        {
            if (format?.ToUpper() == "N")
            {
                return arg.ToString().ToUserFriendlyString();
            }

            if (format is null || format.Length <= 1)
            {
                try
                {
                    return arg.ToString(format);
                }
                catch (FormatException fex)
                {
                    throw new FormatException(fex.Message, fex);
                }
            }

            string formatCode = format.Substring(0, 1);
            int    digits;

            try
            {
                digits = int.Parse(format.Substring(1));
            }
            catch (FormatException fex)
            {
                throw new FormatException("Invalid format string.", fex);
            }

            Type baseType = Enum.GetUnderlyingType(arg.GetType());

            if (digits == 0
             && !_typeBaseDigitsMapping.TryGetValue(Tuple.Create(baseType, formatCode.ToUpper()), out digits))
            { // Failsafe: We didn't cover all base types an enumeration can have.
                digits = formatCode.ToUpper() == "X" ? 8 : 10;
            }

            return ((IFormattable)Convert.ChangeType(arg, baseType)).ToString(formatCode + digits, provider);
        }

        private static string FormatBool(string format, bool value, IFormatProvider provider)
        {
            if (format?.StartsWith("[") ?? false)
            {
                int candidate = -1;
                int splitIndex = -1;
                string trueString;
                string falseString;

                for (int i = 1; i < format.Length; ++i)
                {
                    switch (format[i])
                    {
                        case ']':
                            candidate = candidate == -1 ? i : -1;
                            break;
                        default:
                            splitIndex = candidate == -1 ? -1 : i;
                            break;
                    }

                    if (splitIndex != -1)
                    {
                        break;
                    }
                }

                if (splitIndex == -1)
                {
                    trueString  = format;
                    falseString = "";
                }
                else
                {
                    trueString  = format.Substring(0, splitIndex);
                    falseString = format.Substring(splitIndex);
                }

                return FilterBoolCaseFormat(value ? trueString : falseString);

                string FilterBoolCaseFormat(string subfmt)
                {
                    if (subfmt.Length == 0)
                    {
                        return subfmt;
                    }

                    StringBuilder result = new StringBuilder(subfmt.Length, subfmt.Length);
                    bool openBracket = false;
                    bool closeBracket = false;

                    foreach (char ch in subfmt.Substring(1))
                    {
                        if (!openBracket && ch == '[' || !closeBracket && ch == ']')
                        {
                            openBracket  = ch == '[';
                            closeBracket = ch == ']';
                        }
                        else if (openBracket && ch == '[')
                        {
                            result.Append(ch);
                            openBracket = false;
                        }
                        else if (closeBracket && ch == ']')
                        {
                            result.Append(ch);
                            closeBracket = false;
                        }
                        else
                        {
                            result.Append(ch);
                        }
                    }

                    return result.ToString();
                }
            }

            switch (format)
            {
                case "ud":
                case "UD":
                    return value ? "↑" : "↓";
                case "10":
                    return value ? "1" : "0";
                case "of":
                    return (value ? On(provider as CultureInfo) : Off(provider as CultureInfo)).ToLower();
                case "OF":
                    return value ? On(provider as CultureInfo) : Off(provider as CultureInfo);
                case "yn":
                    return (value ? Yes(provider as CultureInfo) : No(provider as CultureInfo)).ToLower();
                case "YN":
                    return value ? Yes(provider as CultureInfo) : No(provider as CultureInfo);
                case "tf":
                    return (value ? True(provider as CultureInfo) : False(provider as CultureInfo)).ToLower();
                case "TF":
                    return value ? True(provider as CultureInfo) : False(provider as CultureInfo);
                case "g":
                    return value.ToString().ToLower();
                case "G":
                case null:
                    return value.ToString();
                default:
                    throw new FormatException("Invalid format string. Only supported g, G, tf, TF, 10, yn, YN and ?<yes>:<no>");
            }
        }

        private static string FormatBytes(IFormatProvider provider, ulong value, bool twoPower, uint rounding)
        {
            int divisor;
            int power;
            string[] workingUnits;
            double result;

            workingUnits = twoPower ? _iecUnits : _decDataUnits;

            for (result = value, power = 0, divisor = twoPower ? 1024 : 1000;
                power + 1 < workingUnits.Length && result > rounding;
                result /= (uint)divisor, ++power) { }

            return string.Format(provider,
                "{0:N2}\x00A0{1}",
                //$"{{0:N2}}\x00A0{{1,-{workingUnits.Max(s => s.Length)}}}",
                result, workingUnits[power]);
        }
    }
}
