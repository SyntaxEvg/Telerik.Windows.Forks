using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector
{
	class ImageDataElement : VectorElementBase
	{
		public ImageDataElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.id = base.RegisterAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "imagedata";
			}
		}

		public string Id
		{
			get
			{
				return this.id.Value;
			}
			set
			{
				this.id.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> id;
	}
}
