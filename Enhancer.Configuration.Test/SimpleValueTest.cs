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
using static NUnit.Framework.Assert;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class SimpleValueTest
    {
        private const int _defaultValue  = 28;
        private const int _modifiedValue = 42;

        private static readonly object[][] _toStringCases =
        {
            new object[] { (SimpleValue<int>)18, "X4" },
            new object[] { (SimpleValue<DateTime>)new DateTime(2018, 12, 23, 12, 34, 56, 789), "yyyy-MM-dd HH:mm:ss.fff" },
            new object[] { (SimpleValue<bool>)true, "?" },
            new object[] { (SimpleValue<FormattableString>)(FormattableString)null, "?" },
            new object[] { (SimpleValue<Exception>)(Exception)null, "?" },
        };

        private static readonly object[][] _equalityCases =
        {
            new object[] { (SimpleValue<int>)18,                                    18,         true  },
            new object[] { (SimpleValue<int>)18,                                    09,         false },
            new object[] { (SimpleValue<int>)18,                                    Guid.Empty, false },
            new object[] { (SimpleValue<Exception>)new InvalidOperationException(), null,       false },
            new object[] { (SimpleValue<Exception>)(Exception)null,                 null,       true  },
        };

        private static readonly object[] _hashbrownCases =
        {
            (SimpleValue<int>)18,
            (SimpleValue<int>)int.MaxValue,
            (SimpleValue<string>)"Enhancer.NET",
            (SimpleValue<string>)string.Empty,
            (SimpleValue<string>)(string)null,
        };

        private SimpleValue<int>            _value;
        private EventHandler                _valueChangedHandler;
        private PropertyChangedEventHandler _propChHandler;

        public SimpleValueTest()
        {
            _value               = Substitute.ForPartsOf<SimpleValue<int>>();
            _valueChangedHandler = Substitute.For<EventHandler>();
            _propChHandler       = Substitute.For<PropertyChangedEventHandler>();

            _value.ValueChanged    += _valueChangedHandler;
            _value.PropertyChanged += _propChHandler;
        }

        [SetUp]
        public void Setup()
        {
            _value.Value = _defaultValue;

            _value              .ClearReceivedCalls();
            _valueChangedHandler.ClearReceivedCalls();
            _propChHandler      .ClearReceivedCalls();
        }

        [Test(TestOf = typeof(SimpleValue<>))]
        public void InitialValue()
        {
            AreEqual(26, new SimpleValue<int>(26).Value);
        }

        [Test(TestOf = typeof(SimpleValue<>))]
        public void ValueChanged()
        {
            _value.Value = _modifiedValue;

            AreEqual(_modifiedValue, _value.Value);
            _valueChangedHandler.Received().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propChHandler      .Received().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_value.Value)));
        }

        [Test(TestOf = typeof(SimpleValue<>))]
        public void ValueUnchanged()
        {
            _value.Value = _defaultValue;

            AreEqual(_defaultValue, _value.Value);
            _valueChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propChHandler      .DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_value.Value)));
        }

        [Test(TestOf = typeof(SimpleValue<>))]
        public void GeneralValueChanged()
        {
            ((IValue)_value).Value = _modifiedValue;

            AreEqual(_modifiedValue, ((IValue)_value).Value);
            _valueChangedHandler.Received().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propChHandler      .Received().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_value.Value)));
        }

        [Test(TestOf = typeof(SimpleValue<>))]
        public void GeneralValueUnchanged()
        {
            ((IValue)_value).Value = _defaultValue;

            AreEqual(_defaultValue, ((IValue)_value).Value);
            _valueChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
            _propChHandler      .DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_value.Value)));
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
            AreEqual((value.Value as IFormattable)?.ToString(format, null) ?? value.Value?.ToString() ?? string.Empty, value.ToString(format));
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

        [Test(TestOf = typeof(SimpleValue<>))]
        public void CastRoundTrip()
        {
            SimpleValue<int> value = _defaultValue;

            AreEqual(_defaultValue, (int)value);
        }
    }
}
