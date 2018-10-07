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
    public partial class AttachedValue<T>
    {
        private class TransitiveValue : AttachedValue<T>
        {
            private T _value;

            public override T Value
            {
                get => base.Value;
                set => throw new InvalidOperationException();
            }

            public TransitiveValue(T value) : base()
            {
                _value = value;
                _refObj = () => throw new InvalidOperationException();
                _getter = () => _value;
                //_setter = va => throw new InvalidOperationException(); // Never going to be called.
            }
        }
    }
}
