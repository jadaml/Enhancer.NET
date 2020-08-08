/* Copyright (c) 2019, 2020, Ádám L. Juhász
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

using System.ComponentModel;

namespace Enhancer.Extensions
{
    /// <summary>
    /// Provides an interface for raising <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanging" 
    /// <revisionHistory>
    /// <revision date="2019-03-31" version="0.2.0" author="Ádám L. Juhász &lt;jadaml@gmail.com&gt;">
    /// Introduced.
    /// </revision>
    /// </revisionHistory>
    public interface IRaiseNotifyPropertyChanging
    {
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanging.PropertyChanging"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that is about to change.</param>
        void OnPropertyChanging(string propertyName);
    }
}
