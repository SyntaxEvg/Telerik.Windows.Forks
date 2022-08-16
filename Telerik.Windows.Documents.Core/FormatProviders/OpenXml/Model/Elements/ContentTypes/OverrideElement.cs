using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.ContentTypes
{
	class OverrideElement : ContentTypeElementBase
	{
		public OverrideElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.partName = base.RegisterAttribute<string>("PartName", true);
			this.contentType = base.RegisterAttribute<string>("ContentType", true);
		}

		public string PartName
		{
			get
			{
				return this.partName.Value;
			}
			set
			{
				this.partName.Value = value;
			}
		}

		public string ContentType
		{
			get
			{
				return this.contentType.Value;
			}
			set
			{
				this.contentType.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "Override";
			}
		}

		readonly OpenXmlAttribute<string> partName;

		readonly OpenXmlAttribute<string> contentType;
	}
}
