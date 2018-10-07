/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Configuration.Test.
 *
 * Enhancer.Configuration.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Configuration.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Configuration.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using NSubstitute;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static NUnit.Framework.Assert;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class ValueBaseTest
    {
        private class TestValue : ValueBase<int?>
        {
            internal int? _reference;

            internal int? LastReportedValue { get; private set; }

            public override int? Reference
            {
                get => _reference;
                protected set => _reference = value;
            }

            public TestValue(int? value)
            {
                _reference        = value;
                LastReportedValue = value;
            }

            internal void ResetValue() => LastReportedValue = _reference;

            protected override void OnValueChanged(EventArgs e)
            {
                AreNotEqual(LastReportedValue, Value);
                base.OnValueChanged(e);
                LastReportedValue = Value;
            }
        }

        private const int    _initialValue  = 28;
        private const int    _modifiedValue = 42;
        private const string _name          = "Test";

        private static readonly PropertyInfo _debugDisplayProp = typeof(ValueBase<int?>).GetProperty("DebuggerDisplay", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly object[][] _toStringCases =
        {
            new object[] { (ValueBase<int>)18, "X4" },
            new object[] { (ValueBase<DateTime>)new DateTime(2018, 12, 23, 12, 34, 56, 789), "yyyy-MM-dd HH:mm:ss.fff" },
            new object[] { (ValueBase<bool>)true, "?" },
            new object[] { (ValueBase<FormattableString>)(FormattableString)null, "?" },
            new object[] { (ValueBase<Exception>)(Exception)null, "?" },
        };

        private static readonly object[][] _equalityCases =
        {
            new object[] { (ValueBase<int>)18,                                    18,         true  },
            new object[] { (ValueBase<int>)18,                                    09,         false },
            new object[] { (ValueBase<int>)18,                                    Guid.Empty, false },
            new object[] { (ValueBase<Exception>)new InvalidOperationException(), null,       false },
            new object[] { (ValueBase<Exception>)(Exception)null,                 null,       true  },
        };

        private static readonly object[] _hashbrownCases =
        {
            (ValueBase<int>)18,
            (ValueBase<int>)int.MaxValue,
            (ValueBase<string>)"Enhancer.NET",
            (ValueBase<string>)string.Empty,
            (ValueBase<string>)(string)null,
        };

        private TestValue                   _testInstance;
        private EventHandler                _valChHandler;
        private EventHandler                _commiHandler;
        private EventHandler                _rollbHandler;
        private PropertyChangedEventHandler _propCHanlder;

        private ValueBase<int?> TestInstance
        {
            get => _testInstance;
            set => _testInstance.Value = value;
        }

        public ValueBaseTest()
        {
            _testInstance = new TestValue(_initialValue);
            _valChHandler = Substitute.For<EventHandler>();
            _commiHandler = Substitute.For<EventHandler>();
            _rollbHandler = Substitute.For<EventHandler>();
            _propCHanlder = Substitute.For<PropertyChangedEventHandler>();

            _testInstance.ValueChanged    += _valChHandler;
            _testInstance.Committed       += _commiHandler;
            _testInstance.RolledBack      += _rollbHandler;
            _testInstance.PropertyChanged += _propCHanlder;
        }

        [SetUp]
        public void Setup()
        {
            _testInstance.AutoCommit = false;
            _testInstance._reference = _initialValue;
            _testInstance.Rollback();

            ClearAllReceivedCalls();
            _testInstance.ResetValue();
        }

        private void ClearAllReceivedCalls()
        {
            _valChHandler.ClearReceivedCalls();
            _commiHandler.ClearReceivedCalls();
            _rollbHandler.ClearReceivedCalls();
            _propCHanlder.ClearReceivedCalls();
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void ValuePropertyUnchanged()
        {
            AreEqual(_initialValue, _testInstance.Value);

            TestInstance = _initialValue;

            AreEqual(_initialValue, TestInstance);
            AreEqual(_initialValue, TestInstance.Value);
            AreEqual(_initialValue, ((IValue)TestInstance).Value);
            AreEqual(_testInstance.LastReportedValue, _initialValue);
            IsFalse(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void ValuePropertyChanged()
        {
            AreEqual(_initialValue, _testInstance.Value);

            TestInstance = _modifiedValue;

            AreEqual(_modifiedValue, TestInstance);
            AreEqual(_modifiedValue, TestInstance.Value);
            AreEqual(_modifiedValue, ((IValue)TestInstance).Value);
            AreEqual(_modifiedValue, _testInstance.LastReportedValue);
            IsTrue(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void AutoCommitModified()
        {
            TestInstance.AutoCommit = true;

            TestInstance = _modifiedValue;

            AreEqual(_modifiedValue, TestInstance);
            AreEqual(_modifiedValue, TestInstance.Value);
            AreEqual(_modifiedValue, ((IValue)TestInstance).Value);
            AreEqual(_modifiedValue, TestInstance.Reference);
            AreEqual(_modifiedValue, ((ITransactionedValue)TestInstance).Reference);
            IsFalse(_testInstance.IsModified);

            _commiHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(2).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void AutoCommitRevert()
        {
            TestInstance = _modifiedValue;
            TestInstance.AutoCommit = true;

            ClearAllReceivedCalls();

            TestInstance = _initialValue;

            AreEqual(_initialValue, TestInstance);
            AreEqual(_initialValue, TestInstance.Value);
            AreEqual(_initialValue, ((IValue)TestInstance).Value);
            AreEqual(_initialValue, TestInstance.Reference);
            AreEqual(_initialValue, ((ITransactionedValue)TestInstance).Reference);
            IsFalse(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void GeneralValuePropertyUnchanged()
        {
            AreEqual(_initialValue, _testInstance.Value);

            ((IValue)TestInstance).Value = _initialValue;

            AreEqual(_initialValue, TestInstance);
            AreEqual(_initialValue, TestInstance.Value);
            AreEqual(_initialValue, ((IValue)TestInstance).Value);
            AreEqual(_testInstance.LastReportedValue, _initialValue);
            IsFalse(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void GeneralValuePropertyChanged()
        {
            AreEqual(_initialValue, _testInstance.Value);

            ((IValue)TestInstance).Value = _modifiedValue;

            AreEqual(_modifiedValue, TestInstance);
            AreEqual(_modifiedValue, TestInstance.Value);
            AreEqual(_modifiedValue, ((IValue)TestInstance).Value);
            AreEqual(_modifiedValue, _testInstance.LastReportedValue);
            IsTrue(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void GeneralAutoCommitModified()
        {
            TestInstance.AutoCommit = true;

            ((IValue)TestInstance).Value = _modifiedValue;

            AreEqual(_modifiedValue, TestInstance);
            AreEqual(_modifiedValue, TestInstance.Value);
            AreEqual(_modifiedValue, ((IValue)TestInstance).Value);
            AreEqual(_modifiedValue, TestInstance.Reference);
            AreEqual(_modifiedValue, ((ITransactionedValue)TestInstance).Reference);
            IsFalse(_testInstance.IsModified);

            _commiHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(2).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void GeneralAutoCommitRevert()
        {
            TestInstance = _modifiedValue;
            TestInstance.AutoCommit = true;

            ClearAllReceivedCalls();

            ((IValue)TestInstance).Value = _initialValue;

            AreEqual(_initialValue, TestInstance);
            AreEqual(_initialValue, TestInstance.Value);
            AreEqual(_initialValue, ((IValue)TestInstance).Value);
            AreEqual(_initialValue, TestInstance.Reference);
            AreEqual(_initialValue, ((ITransactionedValue)TestInstance).Reference);
            IsFalse(_testInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void DebuggerDisplayUnchanged()
        {
            AreEqual(_initialValue.ToString(), _debugDisplayProp.GetValue(TestInstance));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void DebuggerDisplayChanged()
        {
            TestInstance = _modifiedValue;
            AreEqual($"{_modifiedValue} (Modified)", _debugDisplayProp.GetValue(TestInstance));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        [Ignore("ValueBase<T> don't have Name property.")]
        public void DebuggerDisplayName()
        {
            //_testInstance.Name = _name;
            AreEqual($"{_name}: {_initialValue}", _debugDisplayProp.GetValue(TestInstance));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        [Ignore("ValueBase<T> don't have Name property.")]
        public void DebuggerDisplayNameChanged()
        {
            //_testInstance.Name = _name;
            TestInstance       = _modifiedValue;
            AreEqual($"{_name}: {_modifiedValue} (Modified)", _debugDisplayProp.GetValue(TestInstance));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void CommitUnchanged()
        {
            TestInstance.Commit();

            _commiHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void RollbackUnchanged()
        {
            TestInstance.Rollback();

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void RollbackChanges()
        {
            TestInstance = _modifiedValue;

            ClearAllReceivedCalls();

            _testInstance.Rollback();

            AreEqual(_initialValue, TestInstance.Value);
            AreEqual(_initialValue, TestInstance.Reference);
            AreEqual(_initialValue, _testInstance.LastReportedValue);
            IsFalse(TestInstance.IsModified);

            _commiHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _rollbHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _valChHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propCHanlder.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_testInstance.IsModified)));
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void CastRoundtrip()
        {
            AreEqual(_initialValue, (ValueBase<int>)_initialValue);
        }

        [Test(TestOf = typeof(ValueBase<>))]
        public void CastReferenceException()
        {
            Throws<InvalidOperationException>(() => ((ValueBase<int>)_initialValue).Commit());
            Throws<InvalidOperationException>(() => ((ValueBase<int>)_initialValue).Rollback());
        }

        [TestCaseSource(nameof(_toStringCases))]
        [TestOf(typeof(ValueBase<>))]
        public void ToStringTest(dynamic value, object ignored)
        {
            AreEqual(value.Value?.ToString() ?? string.Empty, value.ToString());
        }

        [TestCaseSource(nameof(_toStringCases))]
        [TestOf(typeof(ValueBase<>))]
        public void ToStringFormatTest(dynamic value, string format)
        {
            AreEqual((value.Value as IFormattable)?.ToString(format, null) ?? value.Value?.ToString() ?? "", value.ToString(format));
        }

        [TestCaseSource(nameof(_toStringCases))]
        [TestOf(typeof(ValueBase<>))]
        public void ToStringCulture(dynamic value, object ignored)
        {
            AreEqual((value.Value as IFormattable)?.ToString(null, CultureInfo.GetCultureInfo("en-US")) ?? value.Value?.ToString() ?? string.Empty, value.ToString(null, CultureInfo.GetCultureInfo("en-US")));
        }

        [TestCaseSource(nameof(_equalityCases))]
        [TestOf(typeof(ValueBase<>))]
        public void EqualsTest(IValue @this, object that, bool areEqual)
        {
            if (areEqual)
            {
                IsTrue(@this.Equals(that));
            }
            else
            {
                IsFalse(@this.Equals(that));
            }
        }

        [TestCaseSource(nameof(_hashbrownCases))]
        [TestOf(typeof(ValueBase<>))]
        public void HashCode(IValue value)
        {
            AreEqual(value.Value?.GetHashCode() ?? 0, value.GetHashCode());
        }
    }
}
