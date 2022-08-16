using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
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

		public static void ThrowExceptionIfLessThan<T>(T lowerBound, T param, string paramName) where T : IComparable<T>
		{
			if (lowerBound.CompareTo(param) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be greater or equal to {1}.", paramName, lowerBound));
			}
		}

		public static void ThrowExceptionIfGreaterThan<T>(T upperBound, T param, string paramName) where T : IComparable<T>
		{
			if (upperBound.CompareTo(param) < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be less or equal to {1}.", paramName, upperBound));
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
			if (!TelerikHelper.EqualsOfT<T>(expected, param))
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

		internal static void ThrowExceptionIfTrue(bool param, string paramName)
		{
			if (param)
			{
				throw new InvalidOperationException(string.Format("{0} must be false.", paramName));
			}
		}

		internal static void ThrowExceptionIfFalse(bool param, string paramName)
		{
			if (!param)
			{
				throw new InvalidOperationException(string.Format("{0} must be true.", paramName));
			}
		}

		internal static void ThrowExceptionIfInvalidDouble(double argument, string argumentName)
		{
			if (double.IsInfinity(argument) || double.IsNaN(argument))
			{
				throw new ArgumentException(string.Format("{0} must be a valid double.", argumentName), argumentName);
			}
		}

		internal static void ThrowExceptionIfNotSupportedType<T>(object param, string paramName) where T : class
		{
			if (!(param is T))
			{
				throw new NotSupportedException(string.Format("{0} is not supported. The expected value type is: {1}", paramName, typeof(T)));
			}
		}

		public static void ThrowExceptionIfInvalidRowIndex(int rowIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.RowCount - 1, rowIndex, "rowIndex");
		}

		public static void ThrowExceptionIfInvalidColumnIndex(int columnIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.ColumnCount - 1, columnIndex, "columnIndex");
		}

		internal static void ThrowExceptionIfInvalidColumnWidth(double columnWidth)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(0.0, SpreadsheetDefaultValues.MaxColumnWidth, columnWidth, "columnWidth");
		}

		internal static void ThrowExceptionIfInvalidRowHeight(double rowHeight)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(0.0, SpreadsheetDefaultValues.MaxRowHeight, rowHeight, "rowHeight");
		}

		internal static void ThrowExceptionIfInvalidIndent(int indent)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, SpreadsheetDefaultValues.MaxIndent, indent, "indent");
		}

		internal static void ThrowExceptionIfInvalidScaleFactor(Size scaleFactor)
		{
			Guard.ThrowExceptionIfInvalidScaleFactor(scaleFactor.Width);
			Guard.ThrowExceptionIfInvalidScaleFactor(scaleFactor.Height);
		}

		internal static void ThrowExceptionIfInvalidScaleFactor(double scaleFactor)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(SpreadsheetDefaultValues.MinScaleFactor, SpreadsheetDefaultValues.MaxScaleFactor, scaleFactor, "scaleFactor");
		}
	}
}
