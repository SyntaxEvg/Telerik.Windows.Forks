using System;
using System.Collections.Generic;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Range
	{
		public static Range<byte> Create(byte start, byte end)
		{
			if (start > end)
			{
				return new Range<byte>(start, end, new Func<byte, byte>(Range.GetNextDesc));
			}
			return new Range<byte>(start, end, new Func<byte, byte>(Range.GetNext));
		}

		public static Range<short> Create(short start, short end)
		{
			if (start > end)
			{
				return new Range<short>(start, end, new Func<short, short>(Range.GetNextDesc));
			}
			return new Range<short>(start, end, new Func<short, short>(Range.GetNext));
		}

		public static Range<int> Create(int start, int end)
		{
			if (start > end)
			{
				return new Range<int>(start, end, new Func<int, int>(Range.GetNextDesc));
			}
			return new Range<int>(start, end, new Func<int, int>(Range.GetNext));
		}

		public static Range<int> Create(int start, int end, int step)
		{
			if (step == 0)
			{
				throw new Exception("The range leads to an infinite series.");
			}
			if (start < end && step < 0)
			{
				throw new Exception("The range leads to an infinite series.");
			}
			if (start > end && step > 0)
			{
				throw new Exception("The range leads to an infinite series.");
			}
			return new Range<int>(start, end, (int d) => d + step);
		}

		public static Range<long> Create(long start, long end)
		{
			if (start > end)
			{
				return new Range<long>(start, end, new Func<long, long>(Range.GetNextDesc));
			}
			return new Range<long>(start, end, new Func<long, long>(Range.GetNext));
		}

		public static T[] ToArray<T>(this Range<T> range)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			List<T> list = new List<T>();
			range.ForEach(new Action<T>(list.Add));
			return list.ToArray();
		}

		public static Range<double> Create(double start, double end)
		{
			if (start > end)
			{
				return new Range<double>(start, end, new Func<double, double>(Range.GetNextDesc));
			}
			return new Range<double>(start, end, new Func<double, double>(Range.GetNext));
		}

		public static Range<double> Create(double start, double step, int amount = 10)
		{
			if (amount < 1)
			{
				throw new ArgumentException("The amount should be greater than one.", "amount");
			}
			double end = start + (double)(amount - 1) * step;
			return Range.Create<double>(start, end, (double d) => d + step);
		}

		public static Range<float> Create(float start, float end)
		{
			if (start > end)
			{
				return new Range<float>(start, end, new Func<float, float>(Range.GetNextDesc));
			}
			return new Range<float>(start, end, new Func<float, float>(Range.GetNext));
		}

		public static Range<decimal> Create(decimal start, decimal end)
		{
			if (!(start <= end))
			{
				return new Range<decimal>(start, end, new Func<decimal, decimal>(Range.GetNextDesc));
			}
			return new Range<decimal>(start, end, new Func<decimal, decimal>(Range.GetNext));
		}

		public static Range<char> Create(char start, char end)
		{
			if (start > end)
			{
				return new Range<char>(start, end, new Func<char, char>(Range.GetNextDesc));
			}
			return new Range<char>(start, end, new Func<char, char>(Range.GetNext));
		}

		public static Range<DateTime> Create(DateTime start, DateTime end)
		{
			if (!(start <= end))
			{
				return new Range<DateTime>(start, end, new Func<DateTime, DateTime>(Range.GetNextDesc));
			}
			return new Range<DateTime>(start, end, new Func<DateTime, DateTime>(Range.GetNext));
		}

		public static Range<T> Create<T>(T start, T end, Func<T, T> getNext, Comparison<T> compare)
		{
			return new Range<T>(start, end, getNext, compare);
		}

		public static Range<T> Create<T>(T start, T end, Func<T, T> getNext)
		{
			return new Range<T>(start, end, getNext);
		}

		static byte GetNext(byte val)
		{
			return val + 1;
		}

		static short GetNext(short val)
		{
			return val + 1;
		}

		static int GetNext(int val)
		{
			return val + 1;
		}

		static long GetNext(long val)
		{
			return val + 1L;
		}

		static double GetNext(double val)
		{
			return val + 1.0;
		}

		static float GetNext(float val)
		{
			return val + 1f;
		}

		static decimal GetNext(decimal val)
		{
			return ++val;
		}

		static DateTime GetNext(DateTime val)
		{
			return val.AddDays(1.0);
		}

		static char GetNext(char val)
		{
			return val + '\u0001';
		}

		static byte GetNextDesc(byte val)
		{
			return val - 1;
		}

		static short GetNextDesc(short val)
		{
			return val - 1;
		}

		static int GetNextDesc(int val)
		{
			return val - 1;
		}

		static long GetNextDesc(long val)
		{
			return val - 1L;
		}

		static double GetNextDesc(double val)
		{
			return val - 1.0;
		}

		static float GetNextDesc(float val)
		{
			return val - 1f;
		}

		static decimal GetNextDesc(decimal val)
		{
			return --val;
		}

		static DateTime GetNextDesc(DateTime val)
		{
			return val.AddDays(-1.0);
		}

		static char GetNextDesc(char val)
		{
			return val - '\u0001';
		}

		public static double EnsureRange(double value, double? min, double? max)
		{
			if (min != null && value < min.Value)
			{
				return min.Value;
			}
			if (max != null && value > max.Value)
			{
				return max.Value;
			}
			return value;
		}

		public static Rect CreateRectangle(Point startPoint, Point endPoint)
		{
			return new Rect(startPoint, new Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
		}
	}
}
