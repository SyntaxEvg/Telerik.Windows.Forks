using System;

namespace System.Collections.Generic
{
	public static class ExtensionMethods
	{
		public static TValue GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
		{
			TValue result = default(TValue);
			if (dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return default(TValue);
		}
	}
}
