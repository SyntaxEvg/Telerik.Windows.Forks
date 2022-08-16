using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class RichTextRunElement : DrawingElementBase
	{
		public RichTextRunElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.text = base.RegisterChildElement<TextElement>("t", "a:t");
		}

		public override string ElementName
		{
			get
			{
				return "r";
			}
		}

		public TextElement TextElement
		{
			get
			{
				return this.text.Element;
			}
			set
			{
				this.text.Element = value;
			}
		}

		public void CopyPropertiesFrom(string text)
		{
			base.CreateElement(this.text);
			this.TextElement.InnerText = text;
		}

		public string GetPlainText()
		{
			string result = string.Empty;
			if (this.TextElement != null)
			{
				result = this.TextElement.InnerText;
				base.ReleaseElement(this.text);
			}
			return result;
		}

		readonly OpenXmlChildElement<TextElement> text;
	}
}
