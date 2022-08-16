using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace CsQuery.ExtensionMethods
{
	static class ExtensionMethods
	{
		public static string RegexReplace(this string input, string pattern, string replacement)
		{
			return input.RegexReplace(Objects.Enumerate<string>(pattern), Objects.Enumerate<string>(replacement));
		}

		public static string RegexReplace(this string input, IEnumerable<string> patterns, IEnumerable<string> replacements)
		{
			List<string> list = new List<string>(patterns);
			List<string> list2 = new List<string>(replacements);
			if (list2.Count != list.Count)
			{
				throw new ArgumentException("Mismatched pattern and replacement lists.");
			}
			for (int i = 0; i < list.Count; i++)
			{
				input = Regex.Replace(input, list[i], list2[i]);
			}
			return input;
		}

		public static string RegexReplace(this string input, string pattern, MatchEvaluator evaluator)
		{
			return Regex.Replace(input, pattern, evaluator);
		}

		public static bool RegexTest(this string input, string pattern)
		{
			return Regex.IsMatch(input, pattern);
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> list, T element)
		{
			if (list != null)
			{
				foreach (T item in list)
				{
					yield return item;
				}
			}
			if (element != null)
			{
				yield return element;
			}
			yield break;
		}

		public static int IndexOf<T>(this IEnumerable<T> list, Func<T, bool> predicate)
		{
			int num = 0;
			IEnumerator<T> enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (predicate(enumerator.Current))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static int IndexOf<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T item)
		{
			int num = 0;
			IEnumerator<T> enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (predicate(enumerator.Current))
				{
					item = enumerator.Current;
					return num;
				}
				num++;
			}
			item = default(T);
			return -1;
		}

		public static int LastIndexOf<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T item)
		{
			int num = 0;
			int result = -1;
			item = default(T);
			IEnumerator<T> enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (predicate(enumerator.Current))
				{
					item = enumerator.Current;
					result = num;
				}
				num++;
			}
			return result;
		}

		public static int IndexOf<T>(this IEnumerable<T> list, T target)
		{
			int num = 0;
			foreach (T t in list)
			{
				if (t.Equals(target))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (T obj in list)
			{
				action(obj);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action)
		{
			int num = 0;
			foreach (T arg in list)
			{
				action(arg, num++);
			}
		}

		public static bool HasProperty(this DynamicObject obj, string propertyName)
		{
			return ((IDictionary<string, object>)obj).ContainsKey(propertyName);
		}

		public static T Get<T>(this DynamicObject obj, string name)
		{
			if (obj == null)
			{
				return default(T);
			}
			IDictionary<string, object> dictionary = (IDictionary<string, object>)obj;
			object value;
			if (dictionary.TryGetValue(name, out value))
			{
				return Objects.Convert<T>(value);
			}
			return default(T);
		}

		public static IEnumerable<IDomObject> Clone(this IEnumerable<IDomObject> source)
		{
			foreach (IDomObject item in source)
			{
				yield return item.Clone();
			}
			yield break;
		}

		public static Array Slice(this Array array, int start, int end)
		{
			if (start < 0)
			{
				start = array.Length + start;
				if (start < 0)
				{
					start = 0;
				}
			}
			if (end < 0)
			{
				end = array.Length + end;
				if (end < 0)
				{
					end = 0;
				}
			}
			if (end >= array.Length)
			{
				end = array.Length;
			}
			int length = end - start;
			Type elementType = array.GetType().GetElementType();
			Array array2 = Array.CreateInstance(elementType, length);
			int num = 0;
			for (int i = start; i < end; i++)
			{
				array2.SetValue(array.GetValue(i), num++);
			}
			return array2;
		}

		public static Array Slice(this Array array, int start)
		{
			return array.Slice(start, array.Length);
		}
	}
}
