using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	interface IProperty
	{
		IPropertyDefinition PropertyDefinition { get; }

		void ClearValue(CellRange cellRange);

		void SetDefaultValue(CellRange cellRange);
	}
}
