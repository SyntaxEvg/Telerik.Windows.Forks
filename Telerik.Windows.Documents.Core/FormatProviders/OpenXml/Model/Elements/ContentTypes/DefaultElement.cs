using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.ContentTypes
{
	class DefaultElement : ContentTypeElementBase
	{
		public DefaultElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.extension = base.RegisterAttribute<string>("Extension", true);
			this.contentType = base.RegisterAttribute<string>("ContentType", true);
		}

		public string Extension
		{
			get
			{
				return this.extension.Value;
			}
			set
			{
				this.extension.Value = value;
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
				return "Default";
			}
		}

		readonly OpenXmlAttribute<string> extension;

		readonly OpenXmlAttribute<string> contentType;
	}
}
