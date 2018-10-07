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
using System.Linq;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class SettingsGroupTest
    {
        private class TestSettingGroup : SettingsGroup
        {
            private MemoryValue<int>      _intValue  = new MemoryValue<int>();
            private MemoryValue<bool>     _boolValue = new MemoryValue<bool>();
            private MemoryValue<DateTime> _dateValue = new MemoryValue<DateTime>();

            public MemoryValue<string> _stringValue = new MemoryValue<string>();

            public new void InitializeValues()
            {
                base.InitializeValues();
            }

            public void ResetValues()
            {
                _intValue   .Value = 0;
                _boolValue  .Value = false;
                _dateValue  .Value = new DateTime();
                _stringValue.Value = string.Empty;

                _intValue   .Commit();
                _boolValue  .Commit();
                _dateValue  .Commit();
                _stringValue.Commit();
            }

            #region Example implementation

            // The following properties serve as an example for how to implement the
            // the interface for the above values.

            public MemoryValue<int> Int32Value
            {
                get => _intValue;
                set => _intValue.Value = value;
            }

            public MemoryValue<bool> BooleanValue
            {
                get => _boolValue;
                set => _boolValue.Value = value;
            }

            public MemoryValue<DateTime> DateTimeValue
            {
                get => _dateValue;
                set => _dateValue.Value = value;
            }

            public MemoryValue<object> NullValue { get; } = null;

            public MemoryValue<string> StringValue
            {
                get => _stringValue;
                set => _stringValue.Value = value;
            }

            #endregion Example implementation
        }

        private static readonly TestSettingGroup _settingGroup = new TestSettingGroup();

        private static readonly IValue[] _expectedCollection =
        {
            _settingGroup.Int32Value,
            _settingGroup.BooleanValue,
            _settingGroup.DateTimeValue,
            _settingGroup.StringValue,
            _settingGroup.NullValue,
        };
        private static MemoryValue<object> _excludedValue = new MemoryValue<object>();

        private PropertyChangedEventHandler _propchHandler;
        private EventHandler                _committingHandler;
        private EventHandler                _committedHandler;
        private EventHandler                _aboutToRollBack;
        private EventHandler                _rolledBackHandler;

        private EventHandler _committedInt32    = Substitute.For<EventHandler>();
        private EventHandler _rolldbackInt32    = Substitute.For<EventHandler>();
        private EventHandler _committedBoolean  = Substitute.For<EventHandler>();
        private EventHandler _rolldbackBoolean  = Substitute.For<EventHandler>();
        private EventHandler _committedDateTime = Substitute.For<EventHandler>();
        private EventHandler _rolldbackDateTime = Substitute.For<EventHandler>();
        private EventHandler _committedString   = Substitute.For<EventHandler>();
        private EventHandler _rolldbackString   = Substitute.For<EventHandler>();

        public static object[][] ValueOrder => new[] {
            new object[] { 0, _settingGroup.Int32Value },
            new object[] { 1, _settingGroup.BooleanValue },
            new object[] { 2, _settingGroup.DateTimeValue },
            new object[] { 3, _settingGroup.StringValue },
            new object[] { 4, _settingGroup.NullValue },
        };

        public SettingsGroupTest()
        {
            _propchHandler     = Substitute.For<PropertyChangedEventHandler>();
            _committingHandler = Substitute.For<EventHandler>();
            _committedHandler  = Substitute.For<EventHandler>();
            _aboutToRollBack   = Substitute.For<EventHandler>();
            _rolledBackHandler = Substitute.For<EventHandler>();

            _settingGroup.PropertyChanged += _propchHandler;
            _settingGroup.Committing      += _committingHandler;
            _settingGroup.Committed       += _committedHandler;
            _settingGroup.AboutToRollback += _aboutToRollBack;
            _settingGroup.RolledBack      += _rolledBackHandler;

            _settingGroup.Int32Value   .Committed  += _committedInt32;
            _settingGroup.Int32Value   .RolledBack += _rolldbackInt32;
            _settingGroup.BooleanValue .Committed  += _committedBoolean;
            _settingGroup.BooleanValue .RolledBack += _rolldbackBoolean;
            _settingGroup.DateTimeValue.Committed  += _committedDateTime;
            _settingGroup.DateTimeValue.RolledBack += _rolldbackDateTime;
            _settingGroup.StringValue  .Committed  += _committedString;
            _settingGroup.StringValue  .RolledBack += _rolldbackString;
        }

        [SetUp]
        public void Setup()
        {
            _settingGroup.Clear();
            _settingGroup.ResetValues();

            _propchHandler    .ClearReceivedCalls();
            _committingHandler.ClearReceivedCalls();
            _committedHandler .ClearReceivedCalls();
            _aboutToRollBack  .ClearReceivedCalls();
            _rolledBackHandler.ClearReceivedCalls();
            _committedInt32   .ClearReceivedCalls();
            _rolldbackInt32   .ClearReceivedCalls();
            _committedBoolean .ClearReceivedCalls();
            _rolldbackBoolean .ClearReceivedCalls();
            _committedDateTime.ClearReceivedCalls();
            _rolldbackDateTime.ClearReceivedCalls();
            _committedString  .ClearReceivedCalls();
            _rolldbackString  .ClearReceivedCalls();
        }

        [TestCaseSource(nameof(ValueOrder))]
        [TestOf(typeof(SettingsGroup))]
        public void IndexerPropertyGetter(int index, IValue value)
        {
            _settingGroup.InitializeValues();
            Assert.That(_settingGroup[index], Is.SameAs(value));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void IndexerPropertySetter()
        {
            _settingGroup.InitializeValues();

            _settingGroup[0] = new MemoryValue<Guid>();

            Assert.That(_settingGroup[0], Is.Not.SameAs(_settingGroup.Int32Value));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void CountProperty()
        {
            _settingGroup.InitializeValues();
            Assert.That(_settingGroup.Count, Is.EqualTo(5));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void ReadOnlyProperty()
        {
            Assert.That(_settingGroup.IsReadOnly, Is.False);
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void IsModifiedTest()
        {
            _settingGroup.InitializeValues();

            Assert.That(_settingGroup.IsModified, Is.False);

            _settingGroup.StringValue = "Enhancer.NET";

            Assert.That(_settingGroup.IsModified, Is.True);

            _settingGroup.StringValue.Rollback();

            Assert.That(_settingGroup.IsModified, Is.False);
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void InitializeValuesTest()
        {
            _settingGroup.InitializeValues();

            Assert.That(_settingGroup, Is.EquivalentTo(_expectedCollection));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void ClearValues()
        {
            _settingGroup.Add(new MemoryValue<int>());
            _settingGroup.Clear();

            Assert.That(_settingGroup, Is.EquivalentTo(new IValue[0]));
        }

        [TestCaseSource(nameof(ValueOrder))]
        [TestOf(typeof(SettingsGroup))]
        public void ContainsTest(object ignored, IValue value)
        {
            _settingGroup.InitializeValues();
            Assert.That(_settingGroup.Contains(value));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void DoesNotContainsTest()
        {
            _settingGroup.InitializeValues();
            Assert.That(_settingGroup.Contains(new MemoryValue<int>(28)), Is.False);
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void CopyToTest()
        {
            _settingGroup.InitializeValues();
            IValue[] values = new IValue[12];

            _settingGroup.CopyTo(values, 4);

            Assert.That(values, Is.EquivalentTo(new IValue[4].Concat(ValueOrder.Select(args => args[1])).Concat(new IValue[3])));
        }

        [TestCaseSource(nameof(ValueOrder))]
        [TestOf(typeof(SettingsGroup))]
        public void IndexOfTest(int index, IValue value)
        {
            _settingGroup.InitializeValues();

            Assert.That(_settingGroup.IndexOf(value), Is.EqualTo(index));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void InsertTest()
        {
            MemoryValue<int> value = new MemoryValue<int>(48);

            _settingGroup.InitializeValues();

            _settingGroup.Insert(3, value);

            Assert.That(_settingGroup[3], Is.SameAs(value));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void RemoveTest()
        {
            _settingGroup.InitializeValues();

            _settingGroup.Remove(_settingGroup.DateTimeValue);

            Assert.That(_settingGroup, Is.EquivalentTo(ValueOrder.Take(2).Select(args => args[1]).Concat(ValueOrder.Skip(3).Select(args => args[1]))));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void RemoveAt()
        {
            _settingGroup.InitializeValues();

            _settingGroup.RemoveAt(2);

            Assert.That(_settingGroup, Is.EquivalentTo(ValueOrder.Take(2).Select(args => args[1]).Concat(ValueOrder.Skip(3).Select(args => args[1]))));
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void Commit()
        {
            _settingGroup.InitializeValues();
            _settingGroup.Add(new SimpleValue<int>(42));

            _settingGroup.Int32Value    = 28;
            _settingGroup.BooleanValue  = !_settingGroup.BooleanValue;
            _settingGroup.DateTimeValue = new DateTime(2018, 1, 23, 12, 34, 56, 789);
            _settingGroup.StringValue   = "Enhancer.NET";

            _settingGroup.Commit();

            _committingHandler.Received(1).Invoke(Arg.Is(_settingGroup),               Arg.Any<EventArgs>());
            _committedHandler .Received(1).Invoke(Arg.Is(_settingGroup),               Arg.Any<EventArgs>());
            _committedInt32   .Received(1).Invoke(Arg.Is(_settingGroup.Int32Value),    Arg.Any<EventArgs>());
            _committedBoolean .Received(1).Invoke(Arg.Is(_settingGroup.BooleanValue),  Arg.Any<EventArgs>());
            _committedDateTime.Received(1).Invoke(Arg.Is(_settingGroup.DateTimeValue), Arg.Any<EventArgs>());
            _committedString  .Received(1).Invoke(Arg.Is(_settingGroup.StringValue),   Arg.Any<EventArgs>());
        }

        [Test(TestOf = typeof(SettingsGroup))]
        public void Rollback()
        {
            _settingGroup.InitializeValues();
            _settingGroup.Add(new SimpleValue<int>(42));

            _settingGroup.Int32Value    = 28;
            _settingGroup.BooleanValue  = !_settingGroup.BooleanValue;
            _settingGroup.DateTimeValue = new DateTime(2018, 1, 23, 12, 34, 56, 789);
            _settingGroup.StringValue   = "Enhancer.NET";

            _settingGroup.Rollback();

            _aboutToRollBack  .Received(1).Invoke(Arg.Is(_settingGroup),               Arg.Any<EventArgs>());
            _rolledBackHandler.Received(1).Invoke(Arg.Is(_settingGroup),               Arg.Any<EventArgs>());
            _rolldbackInt32   .Received(1).Invoke(Arg.Is(_settingGroup.Int32Value),    Arg.Any<EventArgs>());
            _rolldbackBoolean .Received(1).Invoke(Arg.Is(_settingGroup.BooleanValue),  Arg.Any<EventArgs>());
            _rolldbackDateTime.Received(1).Invoke(Arg.Is(_settingGroup.DateTimeValue), Arg.Any<EventArgs>());
            _rolldbackString  .Received(1).Invoke(Arg.Is(_settingGroup.StringValue),   Arg.Any<EventArgs>());
        }

        [Test(TestOf = typeof(SettingsGroup))]
        [Ignore("This feature is not completed yet.")]
        public void IsModifiedChanged()
        {
            _settingGroup.InitializeValues();
            _settingGroup.StringValue = "Enhancer.NET";

            _propchHandler.Received(1).Invoke(Arg.Is(_settingGroup), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_settingGroup.IsModified)));

            _settingGroup.Int32Value = 42;

            _propchHandler.Received(1).Invoke(Arg.Is(_settingGroup), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_settingGroup.IsModified)));

            _settingGroup.Rollback();

            _propchHandler.Received(2).Invoke(Arg.Is(_settingGroup), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == nameof(_settingGroup.IsModified)));
        }
    }
}
