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

        private static object[][] _versionParsing = new object[][]
        {
            new object[] { "1.2.3", new SemVer(1, 2, 3) },
            new object[] { "1.2.3-alpha", new SemVer(1, 2, 3, "alpha") },
            new object[] { "1.2.3-alpha.1.-00", new SemVer(1, 2, 3, "alpha", 1, "-00") },
            new object[] { "1.2.3+20130313144700.Gorillas.-00", new SemVer(1, 2, 3, new object[0], new object[] { 20130313144700, "Gorillas", "-00" }) },
            new object[] { "1.2.3-alpha.1+20130313144700.11", new SemVer(1, 2, 3, new object[] { "alpha", 1 }, new object[] { 20130313144700, 11 }) },
        };

        private static object[][] _versionFormat = new object[][]
        {
            new object[] { "1.2.3-alpha.28+this.is.ignored", null },
            new object[] { "1.2.3", "0" },
            new object[] { "1.2.3-alpha.28", "1" },
            new object[] { "1.2.3+this.is.ignored", "2" },
            new object[] { "1.2.3-alpha.28+this.is.ignored", "3" },
        };

        private static object[][] _comparisons = new object[][]
        {
            new object[] { true,  new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a != b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(1,2,0), new SemVer(2,0,0), new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(2,0,0), new SemVer(10,0,0), new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a != b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,1,0), new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { true,  new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a != b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(1,2,0), new SemVer(1,10,0), new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { true,  new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a != b) },
            new object[] { false, new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(1,0,0), new SemVer(1,0,2),  new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { true,  new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a != b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => b <= a) },
            new object[] { false, new SemVer(1,0,2), new SemVer(1,0,10), new OperatorExecution<SemVer>((a, b) => a == b) },

            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0), new OperatorExecution<SemVer>((a, b) => b <= a) },

            new object[] { true,  new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0,"alpha",1), new SemVer(1,0,0,"alpha","beta"), new OperatorExecution<SemVer>((a, b) => b <= a) },

            new object[] { true,  new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0,"alpha",2), new SemVer(1,0,0,"alpha",10), new OperatorExecution<SemVer>((a, b) => b <= a) },

            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"beta"), new OperatorExecution<SemVer>((a, b) => b <= a) },

            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a < b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b > a) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a <= b) },
            new object[] { true,  new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b >= a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a > b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b < a) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => a >= b) },
            new object[] { false, new SemVer(1,0,0,"alpha"), new SemVer(1,0,0,"alpha","1"), new OperatorExecution<SemVer>((a, b) => b <= a) },

            new object[] { true, new SemVer(1,2,0), new SemVer(1,2,0), new OperatorExecution<SemVer>((a, b) => a.CompareTo(b) == 0) },
            new object[] { true, new SemVer(1,2,0,"10"), new SemVer(1,2,0,"10"), new OperatorExecution<SemVer>((a, b) => a.CompareTo(b) == 0) },
            new object[] { true, new SemVer(1,2,0,"alpha"), new SemVer(1,2,0,"alpha"), new OperatorExecution<SemVer>((a, b) => a == b) },
            new object[] { true, new SemVer(1,2,0,"alpha"), new SemVer(1,2,0, new object[] { "alpha" }, new object[] { "ignored" }), new OperatorExecution<SemVer>((a, b) => a == b) },
        };

        private static object[][] _compareToTestCases =
        {
            new object[] { new SemVer(0,0,0), new SemVer(1,0,0), -1 },
            new object[] { new SemVer(2,0,0), new SemVer(1,0,0), +1 },
            new object[] { new SemVer(1,0,0), new SemVer(1,1,0), -1 },
            new object[] { new SemVer(1,2,0), new SemVer(1,1,0), +1 },
            new object[] { new SemVer(1,1,0), new SemVer(1,1,1), -1 },
            new object[] { new SemVer(1,1,2), new SemVer(1,1,1), +1 },

            new object[] { new SemVer(1,1,1,"alpha"), new SemVer(1,1,1),         -1 },
            new object[] { new SemVer(1,1,1),         new SemVer(1,1,1,"alpha"), +1 },
            new object[] { new SemVer(1,1,1,"a"),     new SemVer(1,1,1,"z"),     -1 },
            new object[] { new SemVer(1,1,1,"z"),     new SemVer(1,1,1,"a"),     +1 },
            new object[] { new SemVer(1,1,1,1),       new SemVer(1,1,1,"a"),     -1 },
            new object[] { new SemVer(1,1,1,"a"),     new SemVer(1,1,1,1),       +1 },
            new object[] { new SemVer(1,1,1,0),       new SemVer(1,1,1,1),       -1 },
            new object[] { new SemVer(1,1,1,2),       new SemVer(1,1,1,1),       +1 },
            new object[] { new SemVer(1,1,1,1),       new SemVer(1,1,1,1,1),     -1 },
            new object[] { new SemVer(1,1,1,1,1),     new SemVer(1,1,1,1),       +1 },

            new object[] { new SemVer(1,1,1,"alpha",1), new SemVer(1,1,1,"alpha",1), 0 },
        };

        private static object[][] _methodComparison = new object[][]
        {
            new object[] { 00, new SemVer(1,2,0), new SemVer(1,2,0) },
            new object[] { 01, new SemVer(2,0,0), new SemVer(1,2,0) },
            new object[] { -1, new SemVer(1,2,0), new SemVer(2,0,0) },

            new object[] { 00, new SemVer(1,2,3), new SemVer(1,2,3) },
            new object[] { 01, new SemVer(1,3,0), new SemVer(1,2,3) },
            new object[] { -1, new SemVer(1,2,3), new SemVer(1,3,0) },

            //new object[] { 00, new SemVer(1,2,3), new SemVer(1,2,3) },
            new object[] { 01, new SemVer(1,2,4), new SemVer(1,2,3) },
            new object[] { -1, new SemVer(1,2,3), new SemVer(1,2,4) },

            //new object[] { 00, new SemVer(1,2,3),     new SemVer(1,2,3) },
            new object[] { 01, new SemVer(1,2,3),     new SemVer(1,2,3,1,2) },
            new object[] { -1, new SemVer(1,2,3,1,2), new SemVer(1,2,3) },

            new object[] { 00, new SemVer(1,2,3,"alpha"), new SemVer(1,2,3,"alpha") },
            new object[] { 01, new SemVer(1,2,3,"beta-"), new SemVer(1,2,3,"alpha") },
            new object[] { -1, new SemVer(1,2,3,"alpha"), new SemVer(1,2,3,"beta-") },

            //new object[] { 00, new SemVer(1,2,3,"alpha"), new SemVer(1,2,3,"alpha") },
            new object[] { 01, new SemVer(1,2,3,"alpha"), new SemVer(1,2,3,1) },
            new object[] { -1, new SemVer(1,2,3,1),       new SemVer(1,2,3,"alpha") },

            //new object[] { 00, new SemVer(1,2,3,"alpha"),   new SemVer(1,2,3,"alpha") },
            new object[] { 01, new SemVer(1,2,3,"alpha",1), new SemVer(1,2,3,"alpha") },
            new object[] { -1, new SemVer(1,2,3,"alpha"),   new SemVer(1,2,3,"alpha",1) },

            new object[] { 00, new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }),
                new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { }) },
            new object[] { 00, new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { }),
                new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }) },
            new object[] { 00, new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }),
                new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }) },
            new object[] { 00, new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }),
                new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "skipped" }) },
            new object[] { 00, new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "skipped" }),
                new SemVer(1,2,3, new object[] { "alpha", 8 }, new object[] { "ignored" }) },
        };

        private static SemVer _equatalon = new SemVer(1,2,3,new object[] { "alpha", 28 }, new object[] { "this", "is", "ignored" });

        private static SemVer _equaequal = new SemVer(_equatalon.Major, _equatalon.Minor, _equatalon.Patch, _equatalon.PreRelease);

        private static SemVer[] _equalist = new SemVer[]
        {
            new SemVer(0,0,0),
            new SemVer(_equatalon.Major,0,0),
            new SemVer(_equatalon.Major,_equatalon.Minor,0),
            new SemVer(_equatalon.Major,_equatalon.Minor,_equatalon.Patch),
            new SemVer(_equatalon.Major,_equatalon.Minor,_equatalon.Patch,_equatalon.PreRelease[0]),
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
            "-1.0.0",
            "0.-1.0",
            "0.0.-1",
            "0.0.0.",
            "0.0.0-",
            "0.0.0+",
            "0.0.0-!",
            "0.0.0-?",
            "0.0.0-_",
            "0.0.0-~",
            "0.0.0-0.",
            "0.0.0+!",
            "0.0.0+?",
            "0.0.0+_",
            "0.0.0+~",
            "0.0.0+0.",
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
        public void FromString(string ver, SemVer expected)
        {
            AreEqual(expected, SemVer.Parse(ver));
        }

        [TestCaseSource(nameof(_versionParsing), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void TryParse(string ver, SemVer ignored)
        {
            IsTrue(SemVer.TryParse(ver, out _));
        }

        [TestCaseSource(nameof(_versionParsing), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void AsString(string expected, SemVer ver)
        {
            AreEqual(expected, ver.ToString());
        }

        [TestCaseSource(nameof(_versionFormat), Category = _parsing)]
        [TestOf(typeof(SemVer))]
        public void FormatString(string expected, string format)
        {
            AreEqual(expected, _equatalon.ToString(format));
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

        [TestCaseSource(nameof(_compareToTestCases), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void CompareToTest(SemVer a, SemVer b, int expect)
        {
            AreEqual(expect, a.CompareTo(b));
        }

        [TestCaseSource(nameof(_comparisons), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void Comparisons(bool expected, SemVer a, SemVer b, OperatorExecution<SemVer> op)
        {
            if (expected)
            {
                IsTrue(op.Invoke(a, b));
            }
            else
            {
                IsFalse(op.Invoke(a, b));
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
            IsTrue(SemVer.Empty.IsDevelopmentVersion);
            IsFalse(new SemVer(1, 0, 0).IsDevelopmentVersion);
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

        [Test(TestOf = typeof(SemVer))]
        public void Cloning()
        {
            SemVer a = new SemVer(1, 2, 3, new object[] { 8 }, new object[] { 16 });
            SemVer b = a.Clone() as SemVer;

            AreNotSame(a,            b);
            AreNotSame(a.PreRelease, b.PreRelease);
            AreNotSame(a.MetaData,   b.MetaData);
            AreEqual  (a,            b);
        }

        [TestCaseSource(typeof(SemVerTest), nameof(_methodComparison), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void TypedCompareToTest(int expected, SemVer a, SemVer b)
        {
            AreEqual(expected, a.CompareTo(b));
        }

        [TestCaseSource(typeof(SemVerTest), nameof(_methodComparison), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void ObjectCompareToTest(int expected, IComparable a, IComparable b)
        {
            AreEqual(expected, a.CompareTo(b));
        }

        [Test(TestOf = typeof(SemVer))]
        public void ObjectCompareToFail()
        {
            Throws<ArgumentException>(() => ((IComparable)SemVer.Empty).CompareTo(new object()));
        }

        [TestCaseSource(typeof(SemVerTest), nameof(_equalist), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void TypedInequals(SemVer version)
        {
            IsFalse(_equatalon.Equals(version));
        }

        [Test(TestOf = typeof(SemVer))]
        public void TypedEquals()
        {
            IsTrue(_equatalon.Equals(_equaequal));
        }

        [TestCaseSource(typeof(SemVerTest), nameof(_equalist), Category = _comparison)]
        [TestOf(typeof(SemVer))]
        public void ObjectInequals(SemVer version)
        {
            IsFalse(((object)_equatalon).Equals(version));
        }

        [Test(TestOf = typeof(SemVer))]
        public void ObjectEquals()
        {
            IsTrue(((object)_equatalon).Equals(_equaequal));
        }

        [Test(TestOf = typeof(SemVer))]
        public void ObjectEqualsFail()
        {
            IsFalse(SemVer.Empty.Equals(new object()));
        }

        [TestCase("0.0.0")]
        [TestCase("1.0.0")]
        [TestCase("1.4.35-Alpha.28")]
        [TestOf(typeof(SemVer))]
        public void GetHashCodeConsistency(string strver)
        {
            IsTrue(new SemVer(strver).GetHashCode() == new SemVer(strver).GetHashCode(), "The hash-code does not yields the same result for same versions.");
        }
    }
}
