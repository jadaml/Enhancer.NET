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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using static NUnit.Framework.Assert;
using SemVer = Enhancer.Assemblies.SemanticVersion;

namespace Enhancer.Test.SemanticVersion
{
    [TestFixture]
    public class SemVerTest
    {
        private const string _creation   = "Constructor";
        private const string _parsing    = "Equality and parsing";
        private const string _comparison = "Ordering and Equality";
        private const string _formatting = "Formatting";

        private static (string, SemVer)[] _versionParsing = new(string, SemVer)[]
        {
            ("1.2.3", new SemVer(1, 2, 3)),
            ("1.2.3-alpha", new SemVer(1, 2, 3, "alpha")),
            ("1.2.3-alpha.1", new SemVer(1, 2, 3, "alpha", 1)),
            ("1.2.3+20130313144700", new SemVer(1, 2, 3, new object[0], new object[] { 20130313144700 })),
            ("1.2.3-alpha.1+20130313144700.11", new SemVer(1, 2, 3, new object[] { "alpha", 1 }, new object[] { 20130313144700, 11 })),
        };

        private static (bool, SemVer, SemVer, OperatorExecution<SemVer>)[] _comparisons = new(bool, SemVer, SemVer, OperatorExecution<SemVer>)[]
        {
            (true, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (true, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a != b)),
            (false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b <= a)),
            (false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a == b)),

            (true, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (true, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a != b)),
            (false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b <= a)),
            (false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a == b)),

            (true, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (true, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a != b)),
            (false, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => b <= a)),
            (false, new SemVer(1,0,0), new SemVer(1,0,2), new OperatorExecution<SemVer>((a, b) => a == b)),

            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b <= a)),

            (true, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b <= a)),

            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b <= a)),

            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a < b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b > a)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a <= b)),
            (true, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b >= a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a > b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b < a)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a >= b)),
            (false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b <= a)),

            (true, new SemVer(1,2,0), new SemVer(1,2,0), new OperatorExecution<SemVer>((a, b) => a == b)),
            (true, new SemVer(1,2,0), new SemVer(1,2,0), new OperatorExecution<SemVer>((a, b) => a.CompareTo(b) == 0)),
            (true, new SemVer(1,2,0,"10"), new SemVer(1,2,0,"10"), new OperatorExecution<SemVer>((a, b) => a == b)),
            (true, new SemVer(1,2,0,"10"), new SemVer(1,2,0,"10"), new OperatorExecution<SemVer>((a, b) => a.CompareTo(b) == 0)),
            (true, new SemVer(1,2,0,"alpha"), new SemVer(1,2,0,"alpha"), new OperatorExecution<SemVer>((a, b) => a == b)),
            (true, new SemVer(1,2,0,"alpha"), new SemVer(1,2,0, new object[] { "alpha" }, new object[] { "ignored" }), new OperatorExecution<SemVer>((a, b) => a == b)),
        };

        private static string[] _faultyVersions = new string[]
        {
            "?",
            "-",
            "+",
            "0",
            "0-",
            "0+",
            "0.0",
            "0.0-",
            "0.0+",
            "01.0.0",
            "0.01.0",
            "0.0.01",
            "0.0.0.",
            "0.0.0-",
            "0.0.0+",
            "0.0.0-?",
            "0.0.0+?",
            "0.0.0+0+0",
        };

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorNullExceptionString()
        {
            Throws<ArgumentNullException>(() => new SemVer(null));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorExceptionEmptyString()
        {
            Throws<ArgumentException>(() => new SemVer(""));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorNullExceptionPreRelease()
        {
            Throws<ArgumentNullException>(() => new SemVer(0, 0, 0, null as IEnumerable<object>));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorNullExceptionMetaData()
        {
            Throws<ArgumentNullException>(() => new SemVer(0, 0, 0, new object[0], null as IEnumerable<object>));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorExceptionPreRelease()
        {
            Throws<ArgumentException>(() => new SemVer(0, 0, 0, new object[] { "?" }));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_creation)]
        public void ConstructorExceptionMetaData()
        {
            Throws<ArgumentException>(() => new SemVer(0, 0, 0, new object[0], new object[] { "?" }));
        }

        [TestCaseSource(nameof(_versionParsing), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void FromString((string, SemVer) input)
        {
            AreEqual(input.Item2, SemVer.Parse(input.Item1));
        }

        [TestCaseSource(nameof(_versionParsing), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void TryParse((string, SemVer) input)
        {
            IsTrue(SemVer.TryParse(input.Item1, out _));
        }

        [TestCaseSource(nameof(_versionParsing), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void AsString((string, SemVer) input)
        {
            AreEqual(input.Item1, input.Item2.ToString());
        }

        [TestCaseSource(nameof(_faultyVersions), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void ParsingError(string input)
        {
            Throws<FormatException>(() => SemVer.Parse(input));
        }

        [TestCaseSource(nameof(_faultyVersions), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void TryParsingFaulty(string input)
        {
            IsFalse(SemVer.TryParse(input, out _));
        }

        [TestCaseSource(nameof(_comparisons), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void Comparisons((bool, SemVer, SemVer, OperatorExecution<SemVer>) input)
        {
            if (input.Item1)
            {
                IsTrue(input.Item4.Invoke(input.Item2, input.Item3));
            }
            else
            {
                IsFalse(input.Item4.Invoke(input.Item2, input.Item3));
            }
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void LTNANull()
        {
            Throws<ArgumentNullException>(() => _ = (null as SemVer) < SemVer.Empty);
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void LTNBNull()
        {
            Throws<ArgumentNullException>(() => _ = SemVer.Empty < (null as SemVer));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void GTRANull()
        {
            Throws<ArgumentNullException>(() => _ = (null as SemVer) > SemVer.Empty);
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void BTRBNull()
        {
            Throws<ArgumentNullException>(() => _ = SemVer.Empty > (null as SemVer));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void LEQANull()
        {
            Throws<ArgumentNullException>(() => _ = (null as SemVer) <= SemVer.Empty);
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void LEQBNull()
        {
            Throws<ArgumentNullException>(() => _ = SemVer.Empty <= (null as SemVer));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void GEQANull()
        {
            Throws<ArgumentNullException>(() => _ = (null as SemVer) >= SemVer.Empty);
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_comparison)]
        public void BEQBNull()
        {
            Throws<ArgumentNullException>(() => _ = SemVer.Empty >= (null as SemVer));
        }

        [Test(TestOf = typeof(SemVer))]
        [Category(_formatting)]
        public void ToStringFormatException()
        {
            Throws<FormatException>(() => SemVer.Empty.ToString("8", null));
        }

        [Test(TestOf = typeof(SemVer))]
        public void InitialVersion()
        {
            IsTrue(SemVer.Empty.IsInitial);
            IsFalse(new SemVer(1, 0, 0).IsInitial);
        }

        [Test(TestOf = typeof(SemVer))]
        public void PreVersion()
        {
            IsFalse(SemVer.Empty.IsPreRelease);
            IsTrue(new SemVer(1, 0, 0, 1).IsPreRelease);
        }

        [Test(TestOf = typeof(SemVer))]
        public void BugFix()
        {
            IsTrue(new SemVer(1, 0, 0).IsPatch(new SemVer(1, 0, 1)));
            IsFalse(new SemVer(1, 0, 0).IsPatch(new SemVer(1, 1, 0)));
        }

        [Test(TestOf = typeof(SemVer))]
        public void BackwardCompatible()
        {
            IsTrue(new SemVer(1, 0, 0).IsBackwardCompatible(new SemVer(1, 1, 0)));
            IsFalse(new SemVer(1, 0, 0).IsBackwardCompatible(new SemVer(2, 0, 0)));
        }

        [Test(TestOf = typeof(SemVer))]
        public void Breaking()
        {
            IsTrue(new SemVer(1, 0, 0).IsBreaking(new SemVer(2, 0, 0)));
            IsFalse(new SemVer(1, 0, 0).IsBreaking(new SemVer(1, 1, 0)));
        }

        // TODO: Clone
    }
}
