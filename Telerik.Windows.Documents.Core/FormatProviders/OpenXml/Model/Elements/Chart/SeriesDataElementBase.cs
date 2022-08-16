using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class SeriesDataElementBase : ChartElementBase
	{
		public SeriesDataElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.stringReference = base.RegisterChildElement<StringReferenceElement>("strRef");
			this.numberReference = base.RegisterChildElement<NumberReferenceElement>("numRef");
			this.stringLiterals = base.RegisterChildElement<StringLiteralsElement>("strLit");
			this.numberLiterals = base.RegisterChildElement<NumberLiteralsElement>("numLit");
		}

		public StringReferenceElement StringReferenceElement
		{
			get
			{
				return this.stringReference.Element;
			}
			set
			{
				this.stringReference.Element = value;
			}
		}

		public NumberReferenceElement NumberReferenceElement
		{
			get
			{
				return this.numberReference.Element;
			}
			set
			{
				this.numberReference.Element = value;
			}
		}

		public StringLiteralsElement StringLiteralsElement
		{
			get
			{
				return this.stringLiterals.Element;
			}
			set
			{
				this.stringLiterals.Element = value;
			}
		}

		public NumberLiteralsElement NumberLiteralsElement
		{
			get
			{
				return this.numberLiterals.Element;
			}
			set
			{
				this.numberLiterals.Element = value;
			}
		}

		public void CopyPropertiesFrom(IChartData chartData)
		{
			switch (chartData.ChartDataType)
			{
			case ChartDataType.Formula:
			{
				FormulaChartData formulaChartData = chartData as FormulaChartData;
				base.CreateElement(this.numberReference);
				this.NumberReferenceElement.CopyPropertiesFrom(formulaChartData);
				return;
			}
			case ChartDataType.NumericLiteral:
			{
				NumericChartData numericChartData = chartData as NumericChartData;
				base.CreateElement(this.numberLiterals);
				this.NumberLiteralsElement.CopyPropertiesFrom(numericChartData);
				return;
			}
			case ChartDataType.StringLiteral:
			{
				StringChartData stringChartData = chartData as StringChartData;
				base.CreateElement(this.stringLiterals);
				this.StringLiteralsElement.CopyPropertiesFrom(stringChartData);
				return;
			}
			default:
				throw new NotSupportedException();
			}
		}

		public IChartData CreateChartData(IOpenXmlImportContext context)
		{
			IChartData result = null;
			if (this.NumberReferenceElement != null)
			{
				result = this.NumberReferenceElement.CreateFormulaChartData(context);
				base.ReleaseElement(this.numberReference);
			}
			else if (this.StringReferenceElement != null)
			{
				result = this.StringReferenceElement.CreateFormulaChartData(context);
				base.ReleaseElement(this.stringReference);
			}
			else if (this.NumberLiteralsElement != null)
			{
				result = this.NumberLiteralsElement.CreateLiteralChartData();
				base.ReleaseElement(this.numberLiterals);
			}
			else if (this.StringLiteralsElement != null)
			{
				result = this.StringLiteralsElement.CreateLiteralChartData();
				base.ReleaseElement(this.stringLiterals);
			}
			return result;
		}

		readonly OpenXmlChildElement<StringReferenceElement> stringReference;

		readonly OpenXmlChildElement<NumberReferenceElement> numberReference;

		readonly OpenXmlChildElement<StringLiteralsElement> stringLiterals;

		readonly OpenXmlChildElement<NumberLiteralsElement> numberLiterals;
	}
}
