using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Functional
	{
		public static FunctionalList<T> Append<T>(this FunctionalList<T> list, FunctionalList<T> otherList)
		{
			if (!list.IsEmpty)
			{
				return list.Reverse<T>().Aggregate(otherList, (FunctionalList<T> current, T element) => current.Cons(element));
			}
			return otherList;
		}

		public static bool AreAllDifferent<T>(this IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return !list.Any((T m) => list.Count((T l) => EqualityComparer<T>.Default.Equals(l, m)) > 1);
		}

		public static IEnumerable<TRange> Collect<TDomain, TRange>(this IEnumerable<TDomain> list, Converter<TDomain, IEnumerable<TRange>> converter)
		{
			IEnumerable<IEnumerable<TRange>> sequences = list.Map(converter);
			return Functional.Concat<TRange>(sequences);
		}

		public static IEnumerable<T> Concat<T>(IEnumerable<IEnumerable<T>> sequences)
		{
			return (from s in sequences
				where s != null
				select s).SelectMany((IEnumerable<T> sequence) => sequence);
		}

		public static IEnumerable<T> Filter<T>(this IEnumerable<T> list, Predicate<T> predicate)
		{
			return from val in list
				where predicate(val)
				select val;
		}

		public static TRange Fold<TDomain, TRange>(this IEnumerable<TDomain> list, Func<TRange, TDomain, TRange> accumulator, TRange accumulatorStartValue)
		{
			return list.Aggregate(accumulatorStartValue, accumulator);
		}

		public static IEnumerable<TTarget> FoldList<TDomain, TTarget>(this IEnumerable<TDomain> list, Func<TTarget, TDomain, TTarget> accumulator, TTarget seed)
		{
			IEnumerator<TDomain> e = list.GetEnumerator();
			TTarget t = seed;
			while (e.MoveNext())
			{
				TDomain arg = e.Current;
				t = accumulator(t, arg);
				yield return t;
			}
			yield break;
		}

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T obj in source)
			{
				action(obj);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> source, Action action)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			source.ToList<T>().ForEach(delegate(T m)
			{
				action();
			});
		}

		public static bool HaveSameContent<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
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
			IOrderedEnumerable<T> source = from m in list1
				orderby m
				select m;
			IOrderedEnumerable<T> source2 = from m in list2
				orderby m
				select m;
			for (int i = 0; i < source.Count<T>(); i++)
			{
				if (!EqualityComparer<T>.Default.Equals(source.ElementAt(i), source2.ElementAt(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static T[] InitArray<T>(int length, Func<int, T> elementInit)
		{
			T[] array = new T[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = elementInit(i);
			}
			return array;
		}

		public static FunctionalLazy<T> Lazy<T>(this Func<T> func)
		{
			return new FunctionalLazy<T>(func);
		}

		public static FunctionalLazy<T> Lazy<T>(T value)
		{
			return new FunctionalLazy<T>(value);
		}

		public static IEnumerable<TMapped> Map<TOriginal, TMapped>(this IEnumerable<TOriginal> list, Converter<TOriginal, TMapped> function)
		{
			return from sourceVal in list
				select function(sourceVal);
		}

		public static FunctionalList<T> Remove<T>(FunctionalList<T> list, T element)
		{
			FunctionalList<T> functionalList = FunctionalList<T>.Empty;
			FunctionalList<T> functionalList2 = list;
			while (!functionalList2.IsEmpty && !EqualityComparer<T>.Default.Equals(functionalList2.Head, element))
			{
				functionalList = functionalList.Cons(functionalList2.Head);
				functionalList2 = functionalList2.Tail;
			}
			if (functionalList2.IsEmpty)
			{
				return list;
			}
			return functionalList.Aggregate(functionalList2.Tail, (FunctionalList<T> current, T item) => current.Cons(item));
		}

		public static IEnumerable<T> Reverse<T>(IEnumerable<T> source)
		{
			for (FunctionalList<T> stack = source.Aggregate(FunctionalList<T>.Empty, (FunctionalList<T> current, T item) => current.Cons(item)); stack != FunctionalList<T>.Empty; stack = stack.Tail)
			{
				yield return stack.Head;
			}
			yield break;
		}

		public static IEnumerable<T> Scramble<T>(this IEnumerable<T> source)
		{
			List<T> clone = source.ToList<T>();
			while (clone.Any<T>())
			{
				int index = Functional.Rand.Next(clone.Count);
				T item = clone.ElementAt(index);
				clone.RemoveAt(index);
				yield return item;
			}
			yield break;
		}

		public static Func<int, Tuple<double, double>> Interpolate(this Tuple<double, double> range, Tuple<double, double> target, int subdivision = 100)
		{
			if (subdivision <= 0)
			{
				throw new ArgumentException("subdivision");
			}
			double a = range.Item1;
			double b = range.Item2;
			double A = target.Item1;
			double B = target.Item2;
			double delta = (b - a) / (double)subdivision;
			double Delta = (B - A) / (double)subdivision;
			return delegate(int e)
			{
				if (e < 0)
				{
					return Tuple.Create<double, double>(a, A);
				}
				if (e > subdivision)
				{
					return Tuple.Create<double, double>(b, B);
				}
				return Tuple.Create<double, double>(a + (double)e * delta, A + (double)e * Delta);
			};
		}

		public static Func<TDomain2, Func<TDomain1, TRange>> Swap<TDomain1, TDomain2, TRange>(Func<TDomain1, Func<TDomain2, TRange>> func)
		{
			return (TDomain2 p2) => (TDomain1 p1) => func(p1)(p2);
		}

		public static FunctionalList<T> ToFunctionalList<T>(this IEnumerable<T> range)
		{
			return new FunctionalList<T>(range);
		}

		static readonly Random Rand = new Random(Environment.TickCount);
	}
}
