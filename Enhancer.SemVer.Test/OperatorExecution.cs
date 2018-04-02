/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.SemVer.Test.
 *
 * Enhancer.SemVer.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.SemVer.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.SemVer.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Linq.Expressions;

namespace Enhancer.Test.SemanticVersion
{
    public class OperatorExecution<T>
    {
        private Func<T, T, bool> operation;

        public string Display { get; }

        public OperatorExecution(Func<T, T, bool> operation, string display)
        {
            this.operation = operation;
            Display = display;
        }

        public OperatorExecution(Expression<Func<T, T, bool>> operationExpression)
        {
            operation = operationExpression.Compile();
            Display = operationExpression.Body.ToString();
        }

        public bool Invoke(T a, T b)
        {
            return operation(a, b);
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
