using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	interface IStringConverter<T>
	{
		T ConvertFromString(string value);

		string ConvertToString(T value);
	}
}
