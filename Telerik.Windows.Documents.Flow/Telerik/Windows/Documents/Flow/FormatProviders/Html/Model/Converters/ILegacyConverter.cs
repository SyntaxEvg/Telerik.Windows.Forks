using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	interface ILegacyConverter
	{
		bool TryGetConvertedValue(string value, out string convertedValue);
	}
}
