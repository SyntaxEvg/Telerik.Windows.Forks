using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class SharedFormulaInfo
	{
		public SharedFormulaInfo(Ref reference, FormulaCellValue formulaCellValue)
		{
			Guard.ThrowExceptionIfNull<Ref>(reference, "reference");
			Guard.ThrowExceptionIfNull<FormulaCellValue>(formulaCellValue, "formulaCellValue");
			this.range = reference.ToCellRange();
			this.formulaCellValue = formulaCellValue;
		}

		public CellRange Range
		{
			get
			{
				return this.range;
			}
		}

		public FormulaCellValue FormulaCellValue
		{
			get
			{
				return this.formulaCellValue;
			}
		}

		readonly CellRange range;

		readonly FormulaCellValue formulaCellValue;
	}
}
