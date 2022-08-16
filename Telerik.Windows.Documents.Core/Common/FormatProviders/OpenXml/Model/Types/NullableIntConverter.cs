using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
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
