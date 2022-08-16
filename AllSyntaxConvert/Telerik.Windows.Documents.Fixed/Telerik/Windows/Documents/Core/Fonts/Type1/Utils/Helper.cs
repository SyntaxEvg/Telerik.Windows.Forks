using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Utils
{
	static class Helper
	{
		public static bool UnboxInteger(object number, out int res)
		{
			return Helper.Unbox<int>(number, out res);
		}

		public static bool UnboxReal(object number, out double res)
		{
			return Helper.Unbox<double>(number, out res);
		}

		public static byte[] CreateByteArray(params byte[] bytes)
		{
			return bytes;
		}

		public static bool Unbox<T>(object value, out T res) where T : struct
		{
			res = default(T);
			bool result;
			try
			{
				object obj = Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
				if (obj != null)
				{
					res = (T)((object)obj);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
	}
}
