using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesTextElement : TxElement
	{
		public SeriesTextElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterChildElement<ValueElement>("v", "c:v");
		}

		public override string ElementName
		{
			get
			{
				return "tx";
			}
		}

		public ValueElement ValueElement
		{
			get
			{
				return this.value.Element;
			}
			set
			{
				this.value.Element = value;
			}
		}

		protected override void CopyPropertiesFromTextTitle(TextTitle title)
		{
			base.CreateElement(this.value);
			this.ValueElement.InnerText = title.Text;
		}

		protected override Title CreateTextChartTitle(IOpenXmlImportContext context)
		{
			Title result = null;
			if (this.ValueElement != null)
			{
				result = new TextTitle(this.ValueElement.InnerText);
				base.ReleaseElement(this.value);
			}
			return result;
		}

		readonly OpenXmlChildElement<ValueElement> value;
	}
}
