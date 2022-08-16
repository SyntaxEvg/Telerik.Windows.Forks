using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	interface IStringConverter<T>
	{
		T ConvertFromString(string value);

		string ConvertToString(T value);
	}
}
