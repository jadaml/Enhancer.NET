/* Copyright (c) 2019, 2020, Ádám L. Juhász
 *
 * This file is part of Enhancer.Extensions.Test.
 *
 * Enhancer.Extensions.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Extensions.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Extensions.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class PropertyNotifierTest
    {
        private class TestObject : IRaiseNotifyPropertyChanging, IRaiseNotifyPropertyChanged
        {
            internal List<string> _callHistory = new List<string>(4);

            public object Dependency1 => throw new NotImplementedException();
            public object Dependency2 => throw new NotImplementedException();
            public object Dependency3 => throw new NotImplementedException();

            [DependsOnProperty(nameof(Dependency1))]
            [DependsOnProperty(nameof(Dependency3))]
            public object Dependent1 => throw new NotImplementedException();

            [DependsOnProperty(nameof(Dependency2))]
            [DependsOnProperty(nameof(Dependency3))]
            public object Dependent2 => throw new NotImplementedException();

            [DependsOnProperty(nameof(RecursiveDependency2))]
            public object RecursiveDependency1 => throw new NotImplementedException();

            [DependsOnProperty(nameof(RecursiveDependency1))]
            public object RecursiveDependency2 => throw new NotImplementedException();

            public void OnPropertyChanged(string propertyName)
            {
                _callHistory.Add(propertyName);
                PropertyNotifier.RaiseDependentPropertyChanged(this, propertyName);
            }

            public void OnPropertyChanging(string propertyName)
            {
                _callHistory.Add(propertyName);
                PropertyNotifier.RaiseDependentPropertyChanging(this, propertyName);
            }
        }

        private TestObject _tobj;

        [OneTimeSetUp]
        public void Initialization()
        {
            _tobj = new TestObject();
        }

        [SetUp]
        public void SetupTest()
        {
            _tobj._callHistory.Clear();
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependentChanged()
        {
            _tobj.OnPropertyChanged(nameof(TestObject.Dependent1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[] { nameof(TestObject.Dependent1) }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependentChanging()
        {
            _tobj.OnPropertyChanging(nameof(TestObject.Dependent1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[] { nameof(TestObject.Dependent1) }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChanged1()
        {
            _tobj.OnPropertyChanged(nameof(TestObject.Dependency1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[]
            {
                nameof(TestObject.Dependency1),
                nameof(TestObject.Dependent1),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChanged2()
        {
            _tobj.OnPropertyChanged(nameof(TestObject.Dependency3));

            Assert.That(_tobj._callHistory, Is.EquivalentTo(new[]
            {
                nameof(TestObject.Dependency3),
                nameof(TestObject.Dependent1),
                nameof(TestObject.Dependent2),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChangedRecursion()
        {
            _tobj.OnPropertyChanged(nameof(TestObject.RecursiveDependency1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[]
            {
                nameof(TestObject.RecursiveDependency1),
                nameof(TestObject.RecursiveDependency2),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChanging1()
        {
            _tobj.OnPropertyChanging(nameof(TestObject.Dependency1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[]
            {
                nameof(TestObject.Dependency1),
                nameof(TestObject.Dependent1),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChanging2()
        {
            _tobj.OnPropertyChanging(nameof(TestObject.Dependency3));

            Assert.That(_tobj._callHistory, Is.EquivalentTo(new[]
            {
                nameof(TestObject.Dependency3),
                nameof(TestObject.Dependent1),
                nameof(TestObject.Dependent2),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DependencyChangingRecursion()
        {
            _tobj.OnPropertyChanging(nameof(TestObject.RecursiveDependency1));

            Assert.That(_tobj._callHistory, Is.EqualTo(new[]
            {
                nameof(TestObject.RecursiveDependency1),
                nameof(TestObject.RecursiveDependency2),
            }));
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DepndencyChangingSenderNull()
        {
            Assert.That(() => PropertyNotifier.RaiseDependentPropertyChanging(null, ""),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DepndencyChangingPropNameNull()
        {
            Assert.That(() => PropertyNotifier.RaiseDependentPropertyChanging(_tobj, null),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DepndencyChangedSenderNull()
        {
            Assert.That(() => PropertyNotifier.RaiseDependentPropertyChanged(null, ""),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test(TestOf = typeof(PropertyNotifier))]
        public void DepndencyChangedPropNameNull()
        {
            Assert.That(() => PropertyNotifier.RaiseDependentPropertyChanged(_tobj, null),
                Throws.InstanceOf<ArgumentNullException>());
        }
    }
}
