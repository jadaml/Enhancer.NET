using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Configuration.Test
{
    internal class ConversionTestCases
    {
        private const bool   _defBolVal = true;
        private const char   _defChrVal = 'J';
        private const int    _defIntVal = 28;
        private const string _defStrVal = "Enhancer.NET";

        private static readonly DateTime _defTimeVal = new DateTime(1234, 12, 31, 12, 34, 56, 789);

        private static readonly Type[] _testTypes =
        {
            typeof(bool),
            typeof(char),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(string),
            typeof(Guid),
            typeof(Stream),
        };

        private static readonly IDictionary<Type, object> _defTypeValMap = new Dictionary<Type, object>()
        {
            [typeof(bool)]     =          _defBolVal,
            [typeof(char)]     =          _defChrVal,
            [typeof(sbyte)]    = (sbyte)  _defIntVal,
            [typeof(byte)]     = (byte)   _defIntVal,
            [typeof(short)]    = (short)  _defIntVal,
            [typeof(ushort)]   = (ushort) _defIntVal,
            [typeof(int)]      =          _defIntVal,
            [typeof(uint)]     = (uint)   _defIntVal,
            [typeof(long)]     = (long)   _defIntVal,
            [typeof(ulong)]    = (ulong)  _defIntVal,
            [typeof(float)]    = (float)  _defIntVal,
            [typeof(double)]   = (double) _defIntVal,
            [typeof(decimal)]  = (decimal)_defIntVal,
            [typeof(DateTime)] =          _defTimeVal,
            [typeof(string)]   =          _defStrVal,
            [typeof(Guid)]     =          Guid.NewGuid(),
            [typeof(Stream)]   =          new MemoryStream(),
        };

        internal static IEnumerable<TestCaseData> Cases(TestCases testCase, Type baseType)
        {
            switch (testCase)
            {
                case TestCases.ChangeType:
                    {
                        foreach (var conversion in from srcType in _testTypes
                                                from dstType in _testTypes
                                                select new { Source = srcType, Target = dstType })
                        {
                            yield return new TestCaseData(
                                CreateValue(conversion.Source),
                                ConvertF(conversion.Target),
                                conversion.Target != typeof(string) && conversion.Source != conversion.Target)
                            {
                                ExpectedResult = conversion.Target != typeof(string) ? _defTypeValMap[conversion.Source] : _defTypeValMap[conversion.Source]?.ToString() ?? "",
                                TestName       = $"{_defTypeValMap[conversion.Source].GetType().Name} value to {conversion.Target.Name}",
                            };
                        }
                    }
                    break;
                case TestCases.TypeCode:
                    {
                        foreach (Type type in _testTypes)
                        {
                            TypeCode typeCode = Enum.TryParse(type.Name, out typeCode) ? typeCode : TypeCode.Object;

                            yield return new TestCaseData((IValue)Activator.CreateInstance(baseType.MakeGenericType(type), _defTypeValMap[type]))
                            {
                                ExpectedResult = typeCode,
                                TestName       = $"{type.Name}:{typeCode}",
                            };
                        }
                    }
                    break;
            }

            yield break;

            IValue CreateValue(Type type)
            {
                object value = _defTypeValMap[type];

                return (IValue)Activator.CreateInstance(
                    baseType.MakeGenericType(value.GetType()),
                    value);
            }

            Func<object, object> ConvertF(Type type)
            {
                return obj =>
                {
                    try
                    {
                        if (obj is null)
                        {
                            Assert.Inconclusive("The object meant to be converted is <null>.");
                            return null;
                        }

                        return typeof(Convert).GetMethod($"To{type.Name}", new[] { typeof(object) })?.Invoke(null, new[] { obj })
                            ?? Convert.ChangeType(obj, type);
                    }
                    catch (TargetInvocationException tiex)
                    {
                        ExceptionDispatchInfo.Capture(tiex.InnerException).Throw();
                        throw;
                    }
                };
            }
        }
    }

    internal enum TestCases
    {
        ChangeType,
        TypeCode,
    }
}
