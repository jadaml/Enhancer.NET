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

using System;

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a setting value object which can report any changes made to
    /// it's value.
    /// </summary>
    public interface IValueChanged : IValue
    {
        /// <summary>
        /// Raised when the <see cref="IValue.Value"/> changed.
        /// </summary>
        event EventHandler ValueChanged;
    }
}
