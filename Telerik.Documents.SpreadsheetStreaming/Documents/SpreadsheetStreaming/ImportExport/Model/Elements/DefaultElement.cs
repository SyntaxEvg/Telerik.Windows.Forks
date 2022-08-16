using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	class DefaultElement : DirectElementBase<DefaultContentType>
	{
		public DefaultElement()
		{
			this.extension = base.RegisterAttribute<string>("Extension", true);
			this.contentType = base.RegisterAttribute<string>("ContentType", true);
		}

		public override string ElementName
		{
			get
			{
				return "Default";
			}
		}

		string Extension
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

		string ContentType
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

		protected override void InitializeAttributesOverride(DefaultContentType value)
		{
			this.Extension = value.Extension;
			this.ContentType = value.ContentType;
		}

		protected override void WriteChildElementsOverride(DefaultContentType value)
		{
		}

		protected override void CopyAttributesOverride(ref DefaultContentType value)
		{
			value.Extension = this.Extension;
			value.ContentType = this.ContentType;
		}

		readonly OpenXmlAttribute<string> extension;

		readonly OpenXmlAttribute<string> contentType;
	}
}
