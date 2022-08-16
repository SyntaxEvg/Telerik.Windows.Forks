using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Sequence
	{
		public static bool AreSameLists<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
		{
			if (list1 == null)
			{
				throw new ArgumentNullException("list1");
			}
			if (list2 == null)
			{
				throw new ArgumentNullException("list2");
			}
			if (list1.Count<T>() != list2.Count<T>())
			{
				return false;
			}
			for (int i = 0; i < list1.Count<T>(); i++)
			{
				if (!EqualityComparer<T>.Default.Equals(list1.ElementAt(i), list2.ElementAt(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static IEnumerable<TData> Create<TData>(Func<TData, TData> getNext, TData startVal, Func<TData, bool> endReached)
		{
			return Sequence.CreateSequence<TData>(getNext, startVal, endReached);
		}

		public static IEnumerable<DateTime> CreateRandomDates(int count)
		{
			DateTime minDate = DateTime.MinValue;
			DateTime maxValue = DateTime.MaxValue;
			int delta = (int)Math.Floor((maxValue - minDate).TotalDays);
			return from m in Range.Create(1, count).ToList<int>()
				select minDate.AddDays((double)Sequence.Rand.Next(delta));
		}

		public static IEnumerable<int> CreateRandomNumbers(int count, int min = 0, int max = 1000)
		{
			return from m in Range.Create(1, count).ToList<int>()
				select Sequence.Rand.Next(min, max);
		}

		public static IEnumerable<double> CreateRandomDoubles(int count, double min = 0.0, double max = 1000.0)
		{
			if (max <= min)
			{
				throw new Exception("The maximum needs to be bigger than the minimum.");
			}
			return from m in Range.Create(1, count).ToList<int>()
				select min + Sequence.Rand.NextDouble() * max;
		}

		public static IEnumerable<Point> FindDuplicates(IEnumerable<Point> source)
		{
			HashSet<Point> seenKeys = new HashSet<Point>();
			return from element in source
				where !seenKeys.Add(element)
				select element;
		}

		internal static IEnumerable<T> CreateSequence<T>(Func<T, T> getNext, T startVal, Func<T, bool> endReached)
		{
			if (getNext != null)
			{
				yield return startVal;
				T val = startVal;
				while (endReached == null || !endReached(val))
				{
					val = getNext(val);
					yield return val;
				}
			}
			yield break;
		}

		public static IEnumerable<T> Clone<T>(this IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			List<T> list2 = new List<T>();
			list.ForEach(new Action<T>(list2.Add));
			return list2;
		}

		public static void CopyTo<T>(this T[] sourceArray, IList<T> distinationList)
		{
			int i = 0;
			int num = sourceArray.Length;
			while (i < num)
			{
				distinationList.Add(sourceArray[i]);
				i++;
			}
		}

		public static string ToPrettyFormat<T>(this IEnumerable<T> list, int amountToShow = 10)
		{
			if (amountToShow < 1)
			{
				throw new Exception("The amount should be bigger than one.");
			}
			StringBuilder sb = new StringBuilder();
			List<T> source = list.ToList<T>();
			source.Take(amountToShow).ForEach(delegate(T s)
			{
				sb.AppendFormat("{0}, ", s);
			});
			sb.Remove(sb.Length - 2, 2);
			sb.Insert(0, "{");
			if (source.Count<T>() > amountToShow)
			{
				sb.Append("...");
			}
			sb.Append("}");
			return sb.ToString();
		}

		public static double Variance(this IEnumerable<double> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			double num = 0.0;
			double num2 = 0.0;
			int num3 = 0;
			using (IEnumerator<double> enumerator = data.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					num3++;
					num2 = enumerator.Current;
				}
				while (enumerator.MoveNext())
				{
					double num4 = enumerator.Current;
					num2 += num4;
					num3++;
					double num5 = (double)num3 * num4 - num2;
					num += num5 * num5 / (double)(num3 * (num3 - 1));
				}
			}
			return num / (double)(num3 - 1);
		}

		public static double StandardDeviation(this IEnumerable<double> data)
		{
			return Math.Sqrt(data.Variance());
		}

		public static IEnumerable<T> Create<T>(Func<int, T> f, int count = 100)
		{
			if (count <= 0)
			{
				throw new ArgumentException("count");
			}
			for (int i = 0; i < count; i++)
			{
				yield return f(i);
			}
			yield break;
		}

		public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> seq, int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (!seq.Any<T>())
			{
				throw new ArgumentException("Sequence is empty.", "index");
			}
			if (index >= seq.Count<T>())
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int i = 0;
			foreach (T x in seq)
			{
				if (i != index)
				{
					yield return x;
				}
				i++;
			}
			yield break;
		}

		public static IEnumerable<T> Drop<T>(this IEnumerable<T> list, int position)
		{
			if (position == 0)
			{
				if (!list.Any<T>())
				{
					throw new Exception("Cannot drop at position zero; the sequence is empty.");
				}
				return list.RemoveAt(0);
			}
			else
			{
				if (position > 0)
				{
					return list.RemoveAt(position);
				}
				if (-position > list.Count<T>())
				{
					throw new ArgumentOutOfRangeException("The negative position is beyond the start of the sequence.", "position");
				}
				return list.RemoveAt(list.Count<T>() + position);
			}
		}

		public static IEnumerable<double> Plus(params IEnumerable<double>[] seqs)
		{
			int c = seqs.First<IEnumerable<double>>().Count<double>();
			if (seqs.Any((IEnumerable<double> l) => l.Count<double>() != c))
			{
				throw new Exception("The length of the sequences are not the same.");
			}
			IEnumerable<IEnumerator<double>> enums = seqs.Map((IEnumerable<double> s) => s.GetEnumerator());
			IEnumerator<double>[] enumerators = (enums as IEnumerator<double>[]) ?? enums.ToArray<IEnumerator<double>>();
			for (;;)
			{
				if (!enumerators.Fold((bool b, IEnumerator<double> e) => e.MoveNext() && b, true))
				{
					break;
				}
				yield return enumerators.Sum((IEnumerator<double> e) => e.Current);
			}
			yield break;
		}

		public static IEnumerable<int> Plus(params IEnumerable<int>[] seqs)
		{
			int c = seqs.First<IEnumerable<int>>().Count<int>();
			if (seqs.Any((IEnumerable<int> l) => l.Count<int>() != c))
			{
				throw new Exception("The length of the sequences are not the same.");
			}
			IEnumerable<IEnumerator<int>> enums = seqs.Map((IEnumerable<int> s) => s.GetEnumerator());
			IEnumerator<int>[] enumerators = (enums as IEnumerator<int>[]) ?? enums.ToArray<IEnumerator<int>>();
			for (;;)
			{
				if (!enumerators.Fold((bool b, IEnumerator<int> e) => e.MoveNext() && b, true))
				{
					break;
				}
				yield return enumerators.Sum((IEnumerator<int> e) => e.Current);
			}
			yield break;
		}

		static readonly Random Rand = new Random(Environment.TickCount);
	}
}
