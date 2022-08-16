using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements
{
	class OverlapElement : DocxElementBase
	{
		public OverlapElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("val", OpenXmlNamespaces.WordprocessingMLNamespace, true));
		}

		public override string ElementName
		{
			get
			{
				return "tblOverlap";
			}
		}

		public bool Value
		{
			get
			{
				return !(this.valueAttribute.Value == "never");
			}
			set
			{
				this.valueAttribute.Value = (value ? "overlap" : "never");
			}
		}

		readonly OpenXmlAttribute<string> valueAttribute;
	}
}
