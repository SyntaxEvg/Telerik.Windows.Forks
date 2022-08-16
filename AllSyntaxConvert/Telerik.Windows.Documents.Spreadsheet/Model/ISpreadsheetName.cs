using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface ISpreadsheetName
	{
		string Name { get; }

		SpreadsheetNameCollectionScope Scope { get; }

		string Comment { get; }

		string RefersTo { get; }

		string Value { get; }

		bool IsVisible { get; }
	}
}
