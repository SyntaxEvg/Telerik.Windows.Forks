using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters
{
	class EncodingConverter : IConverter
	{
		public object Convert(Type resultType, object value)
		{
			if (value is PostScriptArray)
			{
				return value;
			}
			if (value is string)
			{
				return PredefinedEncodings.CreateEncoding((string)value);
			}
			return null;
		}
	}
}
