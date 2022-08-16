using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	static class SheetFactory
	{
		public static Sheet Create(SheetType type, string name, Workbook workbook)
		{
			if (type == SheetType.Worksheet)
			{
				return new Worksheet(name, workbook);
			}
			throw new NotSupportedException();
		}
	}
}
