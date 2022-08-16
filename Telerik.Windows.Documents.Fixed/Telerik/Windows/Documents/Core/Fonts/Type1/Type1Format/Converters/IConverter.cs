using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters
{
	interface IConverter
	{
		object Convert(Type resultType, object value);
	}
}
