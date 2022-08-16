using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class FormulaElement : XlsxElementBase
	{
		public FormulaElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.formulaType = base.RegisterAttribute<string>("t", FormulaTypes.Normal, false);
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", XlsxConverters.RefConverter, false));
			this.sharedGroupIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("si", false));
		}

		public string FormulaType
		{
			get
			{
				return this.formulaType.Value;
			}
			set
			{
				this.formulaType.Value = value;
			}
		}

		public Ref Reference
		{
			get
			{
				return this.reference.Value;
			}
			set
			{
				this.reference.Value = value;
			}
		}

		public int SharedGroupIndex
		{
			get
			{
				return this.sharedGroupIndex.Value;
			}
			set
			{
				this.sharedGroupIndex.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "f";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, FormulaCellValue formula)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormulaCellValue>(formula, "formula");
			base.InnerText = SpreadsheetCultureHelper.ClearFormulaValue(formula.GetExportString());
		}

		public bool CopyPropertiesTo(IXlsxWorksheetImportContext context, int rowIndex, int columnIndex, out ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			if (this.FormulaType == FormulaTypes.Shared)
			{
				cellValue = this.CreateSharedCellValue(context, rowIndex, columnIndex);
			}
			else
			{
				if (string.IsNullOrEmpty(base.InnerText))
				{
					cellValue = null;
					return false;
				}
				cellValue = CellValueFactory.Create(SpreadsheetCultureHelper.PrepareFormulaValue(base.InnerText), context.Worksheet, rowIndex, columnIndex, XlsxHelper.CultureInfo);
			}
			return true;
		}

		ICellValue CreateSharedCellValue(IXlsxWorksheetImportContext context, int rowIndex, int columnIndex)
		{
			ICellValue cellValue = null;
			if (this.reference.HasValue)
			{
				cellValue = CellValueFactory.Create(SpreadsheetCultureHelper.PrepareFormulaValue(base.InnerText), context.Worksheet, rowIndex, columnIndex, XlsxHelper.CultureInfo);
				FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
				if (formulaCellValue != null)
				{
					context.RegisterSharedFormula(this.SharedGroupIndex, new SharedFormulaInfo(this.Reference, formulaCellValue.CloneAndTranslate(context.Worksheet, rowIndex, columnIndex, false)));
				}
				else
				{
					CellValueFormat cellValueFormat;
					CellValueFactory.Create(ErrorExpressions.NameError.Value, context.Worksheet, rowIndex, columnIndex, CellValueFormat.GeneralFormat, out cellValue, out cellValueFormat);
				}
			}
			else
			{
				SharedFormulaInfo sharedFormula = context.GetSharedFormula(this.SharedGroupIndex);
				if (sharedFormula != null)
				{
					cellValue = sharedFormula.FormulaCellValue.CloneAndTranslate(context.Worksheet, rowIndex, columnIndex, false);
				}
				else
				{
					CellValueFormat cellValueFormat2;
					CellValueFactory.Create(ErrorExpressions.NameError.Value, context.Worksheet, rowIndex, columnIndex, CellValueFormat.GeneralFormat, out cellValue, out cellValueFormat2);
				}
			}
			return cellValue;
		}

		readonly OpenXmlAttribute<string> formulaType;

		readonly IntOpenXmlAttribute sharedGroupIndex;

		readonly ConvertedOpenXmlAttribute<Ref> reference;
	}
}
