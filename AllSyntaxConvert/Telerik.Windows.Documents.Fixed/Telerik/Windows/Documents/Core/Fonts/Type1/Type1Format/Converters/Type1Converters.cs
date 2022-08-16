using System;
using Telerik.Windows.Documents.Fixed.Fonts.Type1.Type1Format.Converters;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters
{
	static class Type1Converters
	{
		public static EncodingConverter EncodingConverter { get; set; } = new EncodingConverter();

		public static PostScriptObjectConverter PostScriptObjectConverter { get; set; } = new PostScriptObjectConverter();

		public static DoubleConverter DoubleConverter { get; set; } = new DoubleConverter();
	}
}
