using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters;

namespace Telerik.Windows.Documents.Fixed.Fonts.Type1.Type1Format.Converters
{
	class DoubleConverter : IConverter
	{
		public object Convert(Type resultType, object value)
		{
			object result;
			try
			{
				result = System.Convert.ToDouble(value);
			}
			catch
			{
				result = value;
			}
			return result;
		}
	}
}
