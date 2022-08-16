using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsQuery.StringScanner;
using CsQuery.Utility;

namespace CsQuery.ExtensionMethods.Internal
{
	static class ExtensionMethods
	{
		public static bool IsOneOf(this Enum theEnum, params Enum[] values)
		{
			return values.Any((Enum item) => item.Equals(theEnum));
		}

		public static bool IsOneOf(this string match, params string[] values)
		{
			return match.IsOneOf(true, values);
		}

		public static bool IsOneOf(this string match, bool matchCase = true, params string[] values)
		{
			return values.Any((string item) => match.Equals(item, matchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));
		}

		public static int GetValue(this Enum value)
		{
			return Convert.ToInt32(value);
		}

		public static string GetValueAsString(this Enum value)
		{
			return value.GetValue().ToString();
		}

		public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> elements)
		{
			IList<T> list = new List<T>(elements);
			foreach (T item in list)
			{
				target.Add(item);
			}
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> baseList)
		{
			return baseList == null || (baseList is ICollection<T> && ((ICollection<T>)baseList).Count == 0) || !baseList.Any<T>();
		}

		public static bool TryGetFirst<T>(this IEnumerable<T> baseList, out T firstElement)
		{
			if (baseList == null)
			{
				firstElement = default(T);
				return false;
			}
			bool result = false;
			firstElement = default(T);
			using (IEnumerator<T> enumerator = baseList.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					result = true;
					firstElement = t;
				}
			}
			return result;
		}

		public static T SingleOrDefaultAlways<T>(this IEnumerable<T> list)
		{
			T result = default(T);
			bool flag = false;
			foreach (T t in list)
			{
				if (flag)
				{
					return default(T);
				}
				result = t;
				flag = true;
			}
			return result;
		}

		public static Stream ToStream(this string input, Encoding encoding = null)
		{
			encoding = encoding ?? new UTF8Encoding(false);
			return Support.GetEncodedStream(input ?? "", encoding);
		}

		public static string AsString(this char[] text)
		{
			return new string(text);
		}

		public static int OccurrencesOf(this string text, char find)
		{
			int num = 0;
			int num2 = 0;
			while ((num = text.IndexOf(find, num)) >= 0)
			{
				num2++;
				num++;
			}
			return num2;
		}

		public static string ListAdd(this string list, string value, string separator)
		{
			if (string.IsNullOrEmpty(value))
			{
				return list.Trim();
			}
			if (list == null)
			{
				list = string.Empty;
			}
			else
			{
				list = list.Trim();
			}
			int num = (list + separator).IndexOf(value + separator);
			if (num >= 0)
			{
				return list;
			}
			if (list.LastIndexOf(separator) == list.Length - separator.Length)
			{
				return list + value;
			}
			return list + ((list == "") ? "" : separator) + value;
		}

		public static string ListRemove(this string list, string value, string separator)
		{
			string text = (separator + list).Replace(separator + value, "");
			if (text.Substring(0, 1) == separator)
			{
				text = text.Remove(0, 1);
			}
			return text;
		}

		public static string SubstringBetween(this string text, int startIndex, int endIndex)
		{
			if (endIndex > text.Length || endIndex < 0)
			{
				return "";
			}
			return text.Substring(startIndex, endIndex - startIndex);
		}

		public static string RemoveWhitespace(this string text)
		{
			if (text != null)
			{
				return Regex.Replace(text, "\\s+", " ");
			}
			return null;
		}

		public static string BeforeLast(this string text, string find)
		{
			int num = text.LastIndexOf(find);
			if (num >= 0)
			{
				return text.Substring(0, num);
			}
			return string.Empty;
		}

		public static string After(this string text, string find)
		{
			int num = text.IndexOf(find);
			if (num < 0 || num + find.Length >= text.Length)
			{
				return string.Empty;
			}
			return text.Substring(num + find.Length);
		}

		public static string AfterLast(this string text, string find)
		{
			int num = text.LastIndexOf(find);
			if (num < 0 || num + find.Length >= text.Length)
			{
				return string.Empty;
			}
			return text.Substring(num + find.Length);
		}

		public static string Before(this string text, string find)
		{
			int num = text.IndexOf(find);
			if (num < 0 || num == text.Length)
			{
				return string.Empty;
			}
			return text.Substring(0, num);
		}

		public static string CleanUp(this string value)
		{
			return (value ?? string.Empty).Trim();
		}

		public static IEnumerable<string> SplitClean(this string text)
		{
			return text.SplitClean(CharacterData.charsHtmlSpaceArray);
		}

		public static IEnumerable<string> SplitClean(this string text, char separator)
		{
			return text.SplitClean(new char[] { separator });
		}

		public static IEnumerable<string> SplitClean(this string text, char[] separator)
		{
			string[] list = (text ?? "").Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (list.Length > 0)
			{
				HashSet<string> UniqueList = new HashSet<string>();
				for (int i = 0; i < list.Length; i++)
				{
					if (UniqueList.Add(list[i]))
					{
						yield return list[i].Trim();
					}
				}
			}
			yield break;
		}

		public static StringBuilder Reverse(this StringBuilder text)
		{
			if (text.Length > 1)
			{
				int num = text.Length / 2;
				for (int i = 0; i < num; i++)
				{
					int index = text.Length - (i + 1);
					char value = text[i];
					text[i] = text[index];
					text[index] = value;
				}
			}
			return text;
		}

		public static string Reverse(this string text)
		{
			if (text.Length > 1)
			{
				StringBuilder text2 = new StringBuilder(text);
				return text2.Reverse().ToString();
			}
			return text;
		}

		public static string Substring(this char[] text, int startIndex, int length)
		{
			StringBuilder stringBuilder = new StringBuilder(length);
			stringBuilder.Append(text, startIndex, length);
			return stringBuilder.ToString();
		}

		public static string Substring(this char[] text, int startIndex)
		{
			int num = text.Length - startIndex;
			StringBuilder stringBuilder = new StringBuilder(num);
			stringBuilder.Append(text, startIndex, num);
			return stringBuilder.ToString();
		}

		public static int Seek(this char[] text, string seek)
		{
			return text.Seek(seek, 0);
		}

		public static int Seek(this char[] text, string seek, int startIndex)
		{
			int i = startIndex;
			char value = seek[0];
			while (i >= 0)
			{
				i = Array.IndexOf<char>(text, value, i);
				if (i > 0)
				{
					bool flag = true;
					for (int j = 0; j < seek.Length; j++)
					{
						if (text[i + j] != seek[j])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return i;
					}
					i++;
				}
			}
			return -1;
		}

		public static char ToLower(this char character)
		{
			if (character >= 'A' && character <= 'Z')
			{
				return character + ' ';
			}
			return character;
		}

		public static char ToUpper(this char character)
		{
			if (character >= 'a' && character <= 'z')
			{
				return character - ' ';
			}
			return character;
		}

		public static byte[] Concatenate(this byte[] source1, byte[] source2)
		{
			byte[] array = new byte[source1.Length + source2.Length];
			Buffer.BlockCopy(source1, 0, array, 0, source1.Length);
			Buffer.BlockCopy(source2, 0, array, source1.Length, source2.Length);
			return array;
		}

		public static byte[] ToByteArray(this ushort[] source)
		{
			int num = source.Length << 1;
			byte[] array = new byte[num];
			Buffer.BlockCopy(source, 0, array, 0, num);
			return array;
		}

		public static byte[] ToByteArray(this ushort source)
		{
			return new byte[]
			{
				source.LowByte(),
				source.HighByte()
			};
		}

		public static byte HighByte(this ushort source)
		{
			return (byte)(source >> 8);
		}

		public static byte HighByte(this int source)
		{
			return (byte)(source >> 8);
		}

		public static byte LowByte(this ushort source)
		{
			return (byte)(source & 255);
		}

		public static byte LowByte(this int source)
		{
			return (byte)(source & 255);
		}

		public static int IndexOf<T>(this T[] arr, T item, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if ((arr[i] == null && item == null) || arr[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		public static StringComparer ComparerFor(this StringComparison comparison)
		{
			switch (comparison)
			{
			case StringComparison.CurrentCulture:
				return StringComparer.CurrentCulture;
			case StringComparison.CurrentCultureIgnoreCase:
				return StringComparer.CurrentCultureIgnoreCase;
			case StringComparison.InvariantCulture:
				return StringComparer.InvariantCulture;
			case StringComparison.InvariantCultureIgnoreCase:
				return StringComparer.InvariantCultureIgnoreCase;
			case StringComparison.Ordinal:
				return StringComparer.Ordinal;
			case StringComparison.OrdinalIgnoreCase:
				return StringComparer.OrdinalIgnoreCase;
			default:
				throw new NotImplementedException("Unknown StringComparer enum value");
			}
		}

		public static IEnumerable CloneList(this IEnumerable obj)
		{
			return obj.CloneList(false);
		}

		public static IEnumerable CloneList(this IEnumerable obj, bool deep)
		{
			IEnumerable enumerable;
			if (Objects.IsExpando(obj))
			{
				enumerable = new JsObject();
				IDictionary<string, object> dictionary = (IDictionary<string, object>)enumerable;
				using (IEnumerator<KeyValuePair<string, object>> enumerator = ((IDictionary<string, object>)obj).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, object> keyValuePair = enumerator.Current;
						dictionary.Add(keyValuePair.Key, deep ? Objects.CloneObject(keyValuePair.Value, true) : keyValuePair.Value);
					}
					return enumerable;
				}
			}
			enumerable = new List<object>();
			foreach (object obj2 in obj)
			{
				((List<object>)enumerable).Add(deep ? Objects.CloneObject(obj2, true) : obj2);
			}
			return enumerable;
		}
	}
}
