using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	abstract class TxElement : ChartElementBase
	{
		public TxElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.stringReference = base.RegisterChildElement<StringReferenceElement>("strRef");
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

		public void CopyPropertiesFrom(Title title)
		{
			if (title.TitleType == TitleType.Text)
			{
				this.CopyPropertiesFromTextTitle(title as TextTitle);
				return;
			}
			if (title.TitleType == TitleType.Formula)
			{
				base.CreateElement(this.stringReference);
				this.StringReferenceElement.CopyPropertiesFrom((title as FormulaTitle).Formula);
				return;
			}
			throw new NotSupportedException();
		}

		protected abstract void CopyPropertiesFromTextTitle(TextTitle title);

		public Title CreateChartTitle(IOpenXmlImportContext context)
		{
			Title result;
			if (this.StringReferenceElement != null)
			{
				FormulaChartData formulaChartData = this.StringReferenceElement.CreateFormulaChartData(context);
				result = new FormulaTitle(formulaChartData);
				base.ReleaseElement(this.stringReference);
			}
			else
			{
				result = this.CreateTextChartTitle(context);
			}
			return result;
		}

		protected abstract Title CreateTextChartTitle(IOpenXmlImportContext context);

		readonly OpenXmlChildElement<StringReferenceElement> stringReference;
	}
}
