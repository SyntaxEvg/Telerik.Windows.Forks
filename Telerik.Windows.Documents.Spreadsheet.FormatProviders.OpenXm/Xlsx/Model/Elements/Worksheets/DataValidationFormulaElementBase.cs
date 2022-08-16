using System;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	abstract class DataValidationFormulaElementBase : WorksheetElementBase
	{
		public DataValidationFormulaElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.formulaElement = base.RegisterChildElement<FormulaElement>("f", "x:f");
		}

		public FormulaElement FormulaElement
		{
			get
			{
				return this.formulaElement.Element;
			}
			set
			{
				this.formulaElement.Element = value;
			}
		}

		public string GetValue()
		{
			string text = base.InnerText;
			if (string.IsNullOrEmpty(text))
			{
				text = this.FormulaElement.InnerText;
			}
			double num;
			if (!double.TryParse(text, out num))
			{
				text = this.PrepareFormulaText(text);
			}
			return text;
		}

		string PrepareFormulaText(string formulaText)
		{
			if (formulaText.Contains('"'))
			{
				formulaText = formulaText.Trim(new char[] { '"' });
			}
			else
			{
				formulaText = SpreadsheetCultureHelper.PrepareFormulaValue(formulaText);
			}
			formulaText = formulaText.Replace(SpreadsheetCultureHelper.InvariantSpreadsheetCultureInfo.ListSeparator, FormatHelper.DefaultSpreadsheetCulture.ListSeparator);
			return formulaText;
		}

		OpenXmlChildElement<FormulaElement> formulaElement;
	}
}
