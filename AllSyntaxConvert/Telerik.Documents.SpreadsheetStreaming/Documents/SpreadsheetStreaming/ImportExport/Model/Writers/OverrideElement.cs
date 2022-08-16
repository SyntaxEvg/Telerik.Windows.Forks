using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers
{
	class OverrideElement : DirectElementBase<OverrideContentType>
	{
		public OverrideElement()
		{
			this.partName = base.RegisterAttribute<string>("PartName", true);
			this.contentType = base.RegisterAttribute<string>("ContentType", true);
		}

		public override string ElementName
		{
			get
			{
				return "Override";
			}
		}

		string PartName
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

		protected override void InitializeAttributesOverride(OverrideContentType value)
		{
			this.PartName = value.Path;
			this.ContentType = value.ContentType;
		}

		protected override void CopyAttributesOverride(ref OverrideContentType value)
		{
			value.Path = this.PartName;
			value.ContentType = this.ContentType;
		}

		protected override void WriteChildElementsOverride(OverrideContentType value)
		{
		}

		readonly OpenXmlAttribute<string> partName;

		readonly OpenXmlAttribute<string> contentType;
	}
}
