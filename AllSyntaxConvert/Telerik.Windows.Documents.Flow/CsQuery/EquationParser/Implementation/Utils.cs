using System;
using System.Collections.Generic;
using CsQuery.EquationParser.Implementation.Functions;

namespace CsQuery.EquationParser.Implementation
{
	static class Utils
	{
		public static bool IsIntegralType<T>()
		{
			return Utils.IsIntegralType(typeof(T));
		}

		public static bool IsIntegralType(IConvertible value)
		{
			return Utils.IsIntegralType(value.GetType());
		}

		public static bool IsIntegralType(Type type)
		{
			return type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) || type == typeof(char) || type == typeof(byte) || type == typeof(bool);
		}

		public static bool IsIntegralValue(IConvertible value)
		{
			bool result = false;
			if (!Utils.IsIntegralType(value))
			{
				try
				{
					double num = (double)Convert.ChangeType(value, typeof(double));
					double num2 = Math.Floor(num);
					return num2 == num;
				}
				catch
				{
					result = false;
				}
				return result;
			}
			result = true;
			return result;
		}

		public static bool IsNumericType<T>()
		{
			return Utils.IsNumericType(typeof(T));
		}

		public static bool IsNumericType(object obj)
		{
			Type underlyingType = Utils.GetUnderlyingType(obj.GetType());
			return Utils.IsNumericType(underlyingType);
		}

		public static bool IsNumericType(Type type)
		{
			return type.IsPrimitive && (!(type == typeof(string)) && !(type == typeof(char))) && !(type == typeof(bool));
		}

		public static bool IsNumericConvertible(Type type)
		{
			return type.IsPrimitive && !(type == typeof(string));
		}

		public static bool IsText(object value)
		{
			Type type = value.GetType();
			return type == typeof(string) || type == typeof(char);
		}

		public static IFunction GetFunction<T>(string functionName)
		{
			if (functionName != null && functionName == "abs")
			{
				return new Abs();
			}
			throw new ArgumentException("Undefined function '" + functionName + "'");
		}

		public static IOperand EnsureOperand(IConvertible value)
		{
			if (value is IOperand)
			{
				return (IOperand)value;
			}
			if (value is string)
			{
				return Equations.CreateVariable((string)value);
			}
			return Equations.CreateLiteral(value);
		}

		public static Type GetUnderlyingType(Type type)
		{
			if (type != typeof(string) && Utils.IsNullableType(type))
			{
				return Nullable.GetUnderlyingType(type);
			}
			return type;
		}

		public static bool IsNullableType(Type type)
		{
			return type == typeof(string) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		public static IEnumerable<T> EmptyEnumerable<T>()
		{
			yield break;
		}
	}
}
