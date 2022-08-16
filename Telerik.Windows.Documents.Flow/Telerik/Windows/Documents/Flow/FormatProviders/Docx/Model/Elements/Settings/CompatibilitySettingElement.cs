using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class CompatibilitySettingElement : DocxElementBase
	{
		public CompatibilitySettingElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.uri = base.RegisterAttribute<string>("uri", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.value = base.RegisterAttribute<string>("val", OpenXmlNamespaces.WordprocessingMLNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "compatSetting";
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

		public string Uri
		{
			get
			{
				return this.uri.Value;
			}
			set
			{
				this.uri.Value = value;
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

		readonly OpenXmlAttribute<string> uri;

		readonly OpenXmlAttribute<string> value;
	}
}
