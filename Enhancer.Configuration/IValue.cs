/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Configuration.
 *
 * Enhancer.Configuration is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Configuration is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Configuration.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a setting object that holds a value.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public interface IValue
    {
        /// <summary>
        /// Gets or sets the value of the setting object.
        /// </summary>
        object Value { get; set; }
    }

    /// <summary>
    /// Represents a setting object that holds a value.
    /// </summary>
    /// <typeparam name="T">The base type of the value.</typeparam>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public interface IValue<T>
    {
        /// <summary>
        /// Gets or sets the value of the setting object.
        /// </summary>
        T Value { get; set; }
    }
}
