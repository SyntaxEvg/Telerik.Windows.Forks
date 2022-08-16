using System;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class Guard
	{
		public static void ThrowExceptionIfInfinity(double value, string paramName)
		{
			if (double.IsInfinity(value))
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ThrowExceptionIfNaN(double param, string paramName)
		{
			if (double.IsNaN(param))
			{
				throw new ArgumentException(paramName);
			}
		}

		public static void ThrowExceptionIfNull<T>(T param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		public static void ThrowExceptionIfOutOfRange<T>(T lowerBound, T upperBound, T param, string paramName) where T : IComparable<T>
		{
			if (param.CompareTo(lowerBound) < 0 || param.CompareTo(upperBound) > 0)
			{
				throw new ArgumentOutOfRangeException(paramName, string.Format("{0} should be greater or equal than {1} and less or equal than {2}.", paramName, lowerBound, upperBound));
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

		public static void ThrowExceptionIfInvalidRowIndex(int rowIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.RowCount - 1, rowIndex, "rowIndex");
		}

		public static void ThrowExceptionIfInvalidColumnIndex(int columnIndex)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, DefaultValues.ColumnCount - 1, columnIndex, "columnIndex");
		}
	}
}
