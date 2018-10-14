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

using System.Text;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class provides extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Transform an identifier name to separate words.
        /// </summary>
        /// <param name="value">The input string to transform.</param>
        /// <returns>The separated words transformed from the specified string value.</returns>
        public static string ToUserFriendlyString(this string value)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < value.Length; ++i)
            {
                if (i == 0)
                {
                    result.Append(char.ToUpper(value[i]));
                    continue;
                }

                switch (value[i])
                {
                    case char ch when char.IsLower(value[i - 1]) && char.IsUpper(ch)
                                   || !char.IsNumber(value[i - 1]) && char.IsNumber(ch)
                                   || char.IsNumber(value[i - 1]) && !char.IsNumber(ch)
                                   || i < value.Length - 1 && char.IsUpper(value[i - 1]) && char.IsUpper(ch) && char.IsLower(value[i + 1]):
                        result.AppendFormat(" {0}", ch);
                        break;
                    case char ch when ch == '_':
                        result.Append(' ');
                        break;
                    case char ch when value[i - 1] == '_':
                        result.Append(char.ToUpper(ch));
                        break;
                    default:
                        result.Append(value[i]);
                        break;
                }
            }

            return result.ToString();
        }
    }
}
