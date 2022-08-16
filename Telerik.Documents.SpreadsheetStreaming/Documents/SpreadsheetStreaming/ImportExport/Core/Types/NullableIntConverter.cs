using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class NullableIntConverter : IStringConverter<int?>
	{
		public int? ConvertFromString(string value)
		{
			return new int?(Converters.IntValueConverter.ConvertFromString(value));
		}

		public string ConvertToString(int? value)
		{
			if (value != null)
			{
				return Converters.IntValueConverter.ConvertToString(value.Value);
			}
			return null;
		}
	}
}
