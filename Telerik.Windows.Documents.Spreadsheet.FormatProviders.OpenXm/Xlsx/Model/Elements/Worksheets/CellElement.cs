using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class CellElement : WorksheetElementBase
	{
		public CellElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRef>>(new ConvertedOpenXmlAttribute<CellRef>("r", XlsxConverters.CellRefConverter, false));
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("s", 0, false));
			this.cellDataType = base.RegisterAttribute<string>("t", CellTypes.Number, false);
			this.formula = base.RegisterChildElement<FormulaElement>("f", "x:f");
			this.cellValue = base.RegisterChildElement<CellValueElement>("v", "x:v");
			this.richTextInline = base.RegisterChildElement<RichTextInlineElement>("is");
		}

		public CellRef Reference
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

		public int StyleIndex
		{
			get
			{
				return this.styleIndex.Value;
			}
			set
			{
				this.styleIndex.Value = value;
			}
		}

		public string CellDataType
		{
			get
			{
				return this.cellDataType.Value;
			}
			set
			{
				this.cellDataType.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "c";
			}
		}

		public FormulaElement FormulaElement
		{
			get
			{
				return this.formula.Element;
			}
			set
			{
				this.formula.Element = value;
			}
		}

		public CellValueElement CellValueElement
		{
			get
			{
				return this.cellValue.Element;
			}
			set
			{
				this.cellValue.Element = value;
			}
		}

		public RichTextInlineElement RichTextInlineElement
		{
			get
			{
				return this.richTextInline.Element;
			}
			set
			{
				this.richTextInline.Element = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, CellInfo cell)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CellInfo>(cell, "cell");
			this.Reference = new CellRef(cell.RowIndex, cell.ColumnIndex);
			if (cell.CellValue != null && cell.CellValue.ValueType != CellValueType.Empty)
			{
				this.CellDataType = CellElement.GetCellDataType(cell.CellValue);
				base.CreateElement(this.cellValue);
				this.CellValueElement.InnerText = CellElement.GetValueAsString(context, cell.CellValue);
				if (cell.CellValue.ValueType == CellValueType.Formula)
				{
					base.CreateElement(this.formula);
					this.FormulaElement.CopyPropertiesFrom(context, (FormulaCellValue)cell.CellValue);
				}
			}
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(cell.RowIndex, cell.ColumnIndex);
			bool flag = context.ContainsNonDefaultFormattingRecordIndex(context.Worksheet.Cells, index);
			if (flag)
			{
				int? formattingRecordIndex = context.GetFormattingRecordIndex(context.Worksheet.Cells, index);
				if (formattingRecordIndex != null)
				{
					this.StyleIndex = formattingRecordIndex.Value;
				}
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			CellInfo cellInfo = new CellInfo();
			if (this.Reference != null)
			{
				cellInfo.RowIndex = this.Reference.RowIndex;
				cellInfo.ColumnIndex = this.Reference.ColumnIndex;
			}
			cellInfo.StyleIndex = this.StyleIndex;
			ICellValue cellValue = null;
			if (this.FormulaElement != null)
			{
				if (!this.FormulaElement.CopyPropertiesTo(context, cellInfo.RowIndex, cellInfo.ColumnIndex, out cellValue) && this.CellValueElement != null)
				{
					cellValue = new TextCellValue(this.CellValueElement.InnerText);
				}
			}
			else if (this.CellValueElement != null)
			{
				if (this.CellDataType == CellTypes.SharedString)
				{
					string sharedStringValue = context.GetSharedStringValue(Convert.ToInt32(this.CellValueElement.InnerText));
					if (sharedStringValue == null)
					{
						cellValue = EmptyCellValue.EmptyValue;
					}
					else
					{
						cellValue = new TextCellValue(sharedStringValue);
					}
				}
				else if (this.CellDataType == CellTypes.Boolean)
				{
					byte value;
					bool flag = byte.TryParse(this.CellValueElement.InnerText, out value);
					if (flag)
					{
						cellValue = new BooleanCellValue(Convert.ToBoolean(value));
					}
					else
					{
						cellValue = new BooleanCellValue(Convert.ToBoolean(this.CellValueElement.InnerText));
					}
				}
				else if (this.CellDataType == CellTypes.Error)
				{
					cellValue = this.CellValueElement.InnerText.ToCellValue(context.Worksheet, cellInfo.RowIndex, cellInfo.ColumnIndex);
				}
				else if (this.CellDataType == CellTypes.Number)
				{
					double doubleValue;
					bool flag2 = XlsxHelper.CultureInfo.TryParseDouble(this.CellValueElement.InnerText, out doubleValue) || XlsxHelper.CultureInfo.TryParseScientific(this.CellValueElement.InnerText, out doubleValue);
					if (flag2)
					{
						cellValue = new NumberCellValue(doubleValue);
					}
				}
				else if (this.CellDataType == CellTypes.String)
				{
					cellValue = new TextCellValue(this.CellValueElement.InnerText);
				}
				else if (this.CellDataType == CellTypes.Date)
				{
					DateTime dateTime = DateTime.Parse(this.CellValueElement.InnerText);
					double doubleValue2 = FormatHelper.ConvertDateTimeToDouble(dateTime);
					cellValue = new NumberCellValue(doubleValue2);
				}
			}
			else if (this.RichTextInlineElement != null && this.CellDataType == CellTypes.InlineString)
			{
				cellValue = new TextCellValue(this.RichTextInlineElement.ResultText);
			}
			base.ReleaseElement(this.formula);
			base.ReleaseElement(this.cellValue);
			base.ReleaseElement(this.richTextInline);
			cellInfo.CellValue = cellValue;
			context.ApplyCellInfo(cellInfo);
		}

		static string GetFormulaCellValueType(FormulaCellValue formula)
		{
			Guard.ThrowExceptionIfNull<FormulaCellValue>(formula, "formula");
			ConstantExpression valueAsConstantExpression = formula.Value.GetValue().GetValueAsConstantExpression();
			if (valueAsConstantExpression is StringExpression)
			{
				return CellTypes.String;
			}
			if (valueAsConstantExpression is NumberExpression)
			{
				return CellTypes.Number;
			}
			if (valueAsConstantExpression is ErrorExpression)
			{
				return CellTypes.Error;
			}
			if (valueAsConstantExpression is BooleanExpression)
			{
				return CellTypes.Boolean;
			}
			return CellTypes.Number;
		}

		static string GetFormulaValueAsString(FormulaCellValue formula)
		{
			Guard.ThrowExceptionIfNull<FormulaCellValue>(formula, "formula");
			ConstantExpression valueAsConstantExpression = formula.Value.GetValue().GetValueAsConstantExpression();
			BooleanExpression booleanExpression = valueAsConstantExpression as BooleanExpression;
			if (booleanExpression != null)
			{
				return booleanExpression.Value.ToValueString();
			}
			return valueAsConstantExpression.GetValueAsString(XlsxHelper.CultureInfo);
		}

		static string GetCellDataType(ICellValue value)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(value, "value");
			switch (value.ValueType)
			{
			case CellValueType.Number:
			case CellValueType.Boolean:
			case CellValueType.Text:
			case CellValueType.Error:
				return CellTypes.GetCellTypeName(value.ValueType);
			case CellValueType.Formula:
				return CellElement.GetFormulaCellValueType((FormulaCellValue)value);
			}
			throw new NotSupportedException("Value type is not supported.");
		}

		static string GetValueAsString(IXlsxWorksheetExportContext context, ICellValue value)
		{
			switch (value.ValueType)
			{
			case CellValueType.Number:
				return XlsxHelper.CultureInfo.ToString(((NumberCellValue)value).Value);
			case CellValueType.Boolean:
			{
				BooleanCellValue booleanCellValue = (BooleanCellValue)value;
				return booleanCellValue.Value.ToValueString();
			}
			case CellValueType.Formula:
				return CellElement.GetFormulaValueAsString((FormulaCellValue)value);
			case CellValueType.Text:
				return context.GetSharedStringIndex((TextCellValue)value).ToString();
			case CellValueType.Error:
				return value.RawValue;
			}
			throw new NotSupportedException("Value type is not supported.");
		}

		readonly ConvertedOpenXmlAttribute<CellRef> reference;

		readonly IntOpenXmlAttribute styleIndex;

		readonly OpenXmlAttribute<string> cellDataType;

		readonly OpenXmlChildElement<FormulaElement> formula;

		readonly OpenXmlChildElement<CellValueElement> cellValue;

		readonly OpenXmlChildElement<RichTextInlineElement> richTextInline;
	}
}
