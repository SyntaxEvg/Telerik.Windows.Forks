using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class DocumentVariableElement : DocxElementBase
	{
		public DocumentVariableElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.value = base.RegisterAttribute<string>("val", OpenXmlNamespaces.WordprocessingMLNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "docVar";
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public string Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlAttribute<string> value;
	}
}
