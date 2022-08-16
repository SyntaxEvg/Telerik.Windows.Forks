using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class ReferenceDataElementBase : ChartElementBase
	{
		public ReferenceDataElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.formula = base.RegisterChildElement<FormulaElement>("f", "c:f");
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

		public void CopyPropertiesFrom(FormulaChartData formulaChartData)
		{
			base.CreateElement(this.formula);
			this.FormulaElement.InnerText = formulaChartData.Formula;
		}

		public FormulaChartData CreateFormulaChartData(IOpenXmlImportContext context)
		{
			string innerText = this.FormulaElement.InnerText;
			return context.GetFormulaChartData(innerText);
		}

		readonly OpenXmlChildElement<FormulaElement> formula;
	}
}
