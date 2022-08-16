using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;

namespace Org.BouncyCastle.Utilities
{
	abstract class Platform
	{
		static string GetNewLine()
		{
			return Environment.NewLine;
		}

		internal static bool EqualsIgnoreCase(string a, string b)
		{
			return Platform.ToUpperInvariant(a) == Platform.ToUpperInvariant(b);
		}

		internal static string GetEnvironmentVariable(string variable)
		{
			string result;
			try
			{
				result = Environment.GetEnvironmentVariable(variable);
			}
			catch (SecurityException)
			{
				result = null;
			}
			return result;
		}

		internal static Exception CreateNotImplementedException(string message)
		{
			return new NotImplementedException(message);
		}

		internal static IList CreateArrayList()
		{
			return new ArrayList();
		}

		internal static IList CreateArrayList(int capacity)
		{
			return new ArrayList(capacity);
		}

		internal static IList CreateArrayList(ICollection collection)
		{
			return new ArrayList(collection);
		}

		internal static IList CreateArrayList(IEnumerable collection)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object value in collection)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		internal static IDictionary CreateHashtable()
		{
			return new Hashtable();
		}

		internal static IDictionary CreateHashtable(int capacity)
		{
			return new Hashtable(capacity);
		}

		internal static IDictionary CreateHashtable(IDictionary dictionary)
		{
			return new Hashtable(dictionary);
		}

		internal static string ToLowerInvariant(string s)
		{
			return s.ToLower(CultureInfo.InvariantCulture);
		}

		internal static string ToUpperInvariant(string s)
		{
			return s.ToUpper(CultureInfo.InvariantCulture);
		}

		internal static void Dispose(Stream s)
		{
			s.Close();
		}

		internal static void Dispose(TextWriter t)
		{
			t.Close();
		}

		internal static int IndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.IndexOf(source, value, CompareOptions.Ordinal);
		}

		internal static int LastIndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.LastIndexOf(source, value, CompareOptions.Ordinal);
		}

		internal static bool StartsWith(string source, string prefix)
		{
			return Platform.InvariantCompareInfo.IsPrefix(source, prefix, CompareOptions.Ordinal);
		}

		internal static bool EndsWith(string source, string suffix)
		{
			return Platform.InvariantCompareInfo.IsSuffix(source, suffix, CompareOptions.Ordinal);
		}

		internal static string GetTypeName(object obj)
		{
			return obj.GetType().FullName;
		}

		static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

		internal static readonly string NewLine = Platform.GetNewLine();
	}
}
