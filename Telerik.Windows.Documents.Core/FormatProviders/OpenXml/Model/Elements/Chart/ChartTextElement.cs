using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ChartTextElement : TxElement
	{
		public ChartTextElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.richElement = base.RegisterChildElement<RichTextElement>("rich");
		}

		public override string ElementName
		{
			get
			{
				return "tx";
			}
		}

		public RichTextElement RichTextElement
		{
			get
			{
				return this.richElement.Element;
			}
			set
			{
				this.richElement.Element = value;
			}
		}

		protected override void CopyPropertiesFromTextTitle(TextTitle title)
		{
			base.CreateElement(this.richElement);
			this.RichTextElement.CopyPropertiesFrom(title);
		}

		protected override Title CreateTextChartTitle(IOpenXmlImportContext context)
		{
			Title result = null;
			if (this.RichTextElement != null)
			{
				string plainText = this.RichTextElement.GetPlainText();
				result = new TextTitle(plainText);
			}
			return result;
		}

		readonly OpenXmlChildElement<RichTextElement> richElement;
	}
}
