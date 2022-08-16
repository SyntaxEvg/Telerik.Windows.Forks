using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector
{
	class TextpathElement : VectorElementBase
	{
		public TextpathElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.stringAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("string", false));
			this.style = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("style", false));
		}

		public override string ElementName
		{
			get
			{
				return "textpath";
			}
		}

		public string StringAttribute
		{
			get
			{
				return this.stringAttribute.Value;
			}
			set
			{
				this.stringAttribute.Value = value;
			}
		}

		public string Style
		{
			get
			{
				return this.style.Value;
			}
			set
			{
				this.style.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> stringAttribute;

		readonly OpenXmlAttribute<string> style;
	}
}
