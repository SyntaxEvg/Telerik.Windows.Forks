using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Utilities
{
	public static class Guard
	{
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				throw new InvalidOperationException(message);
			}
		}

		public static void ThrowPositionOutOfRangeException()
		{
			throw new InvalidOperationException("Position is out of range.");
		}

		public static void ThrowArgumentException(string argument)
		{
			throw new ArgumentException("Argument not in expected format.", argument);
		}

		public static void ThrowNotSupportedException()
		{
			throw new NotSupportedException("Operation is not supported.");
		}

		public static void ThrowExceptionIfLessThan<T>(T lowerBound, T param, string paramName) where T : IComparable<T>
		{
			if (lowerBound.CompareTo(param) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be greater or equal than {1}.", paramName, lowerBound));
			}
		}

		public static void ThrowExceptionIfLessThanOrEqual<T>(T lowerBound, T param, string paramName) where T : IComparable<T>
		{
			if (lowerBound.CompareTo(param) >= 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be greater than {1}.", paramName, lowerBound));
			}
		}

		public static void ThrowExceptionIfGreaterThan<T>(T upperBound, T param, string paramName) where T : IComparable<T>
		{
			if (upperBound.CompareTo(param) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be less or equal than {1}.", paramName, upperBound));
			}
		}

		public static void ThrowExceptionIfOutOfRange<T>(T lowerBound, T upperBound, T param, string paramName) where T : IComparable<T>
		{
			if (param.CompareTo(lowerBound) < 0 || param.CompareTo(upperBound) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be greater or equal than {1} and less or equal than {2}.", paramName, lowerBound, upperBound));
			}
		}

		public static void ThrowExceptionIfNotEqual<T>(T expected, T param, string paramName)
		{
			if (!ObjectExtensions.EqualsOfT<T>(expected, param))
			{
				throw new ArgumentException(paramName, string.Format("{0} should be equal to {1}.", paramName, param));
			}
		}

		public static void ThrowExceptionIfNull<T>(T param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		public static void ThrowExceptionIfNotNull<T>(T param, string paramName)
		{
			if (param != null)
			{
				throw new ArgumentException(paramName + " should be null");
			}
		}

		public static void ThrowExceptionIfContainsNullOrEmpty(IEnumerable<string> param, string paramName)
		{
			foreach (string value in param)
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException(paramName);
				}
			}
		}

		public static void ThrowExceptionIfNullOrEmpty<T>(IEnumerable<T> param, string paramName) where T : class
		{
			if (param == null || param.Count<T>() == 0)
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ThrowExceptionIfContainsWhitespace(string param, string paramName)
		{
			if (param.Contains(' '))
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ThrowExceptionIfContainsNull<T>(IEnumerable<T> param, string paramName) where T : class
		{
			foreach (T t in param)
			{
				if (t == null)
				{
					throw new ArgumentException(paramName);
				}
			}
		}

		public static void ThrowExceptionIfNullOrEmpty(string param, string paramName)
		{
			Guard.ThrowExceptionIfNull<string>(param, paramName);
			if (string.IsNullOrEmpty(param))
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ThrowExceptionIfTrue(bool condition, string message)
		{
			if (condition)
			{
				throw new InvalidOperationException(message);
			}
		}

		public static void ThrowExceptionIfFalse(bool condition, string message)
		{
			if (!condition)
			{
				throw new InvalidOperationException(message);
			}
		}

		public static void ThrowExceptionIfInvalidDouble(double argument, string argumentName)
		{
			if (double.IsInfinity(argument) || double.IsNaN(argument))
			{
				throw new ArgumentException(string.Format("{0} must be a valid double.", argumentName), argumentName);
			}
		}

		public static void ThrowExceptionIfPositiveInfinity(double argument, string argumentName)
		{
			if (double.IsPositiveInfinity(argument))
			{
				throw new ArgumentException(string.Format("{0} is positive infinity.", argumentName), argumentName);
			}
		}

		public static void ThrowExceptionIfNegativeInfinity(double argument, string argumentName)
		{
			if (double.IsNegativeInfinity(argument))
			{
				throw new ArgumentException(string.Format("{0} is negative infinity.", argumentName), argumentName);
			}
		}

		public static void ThrowExceptionIfNaN(double argument, string argumentName)
		{
			if (double.IsNaN(argument))
			{
				throw new ArgumentException(string.Format("{0} is not a number.", argumentName), argumentName);
			}
		}

		public static void ThrowExceptionIfConditionNotSatisfied(bool actual, bool expected, string message)
		{
			if (actual != expected)
			{
				throw new ArgumentException(message);
			}
		}
	}
}
