using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.ContentTypes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts
{
	class ContentTypesPart : OpenXmlPartBase
	{
		public ContentTypesPart(OpenXmlPartsManager partsManager)
			: base(partsManager, "/[Content_Types].xml")
		{
			this.typesElement = new TypesElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.typesElement;
			}
		}

		public override string ContentType
		{
			get
			{
				throw new InvalidOperationException("This part don't have content type.");
			}
		}

		public override int Level
		{
			get
			{
				return 0;
			}
		}

		public void RegisterPart(OpenXmlPartBase part)
		{
			if (part.OverrideDefaultContentType)
			{
				this.typesElement.RegisterOverride(part.Name, part.ContentType);
				return;
			}
			string extension = OpenXmlHelper.GetExtension(part.Name);
			string contentType;
			if (OpenXmlHelper.TryGetContentTypeByExtension(extension, out contentType))
			{
				this.typesElement.RegisterDefault(extension, contentType);
			}
		}

		public void RegisterResource(string extension)
		{
			string contentType;
			if (OpenXmlHelper.TryGetContentTypeByExtension(extension, out contentType))
			{
				this.typesElement.RegisterDefault(OpenXmlHelper.GetExtension(extension), contentType);
			}
		}

		public OpenXmlPartBase CreatePart(string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			return this.typesElement.CreatePart(partName);
		}

		public override void Export(IOpenXmlWriter writer, IOpenXmlExportContext context)
		{
			this.RootElement.Write(writer, context);
		}

		readonly TypesElement typesElement;
	}
}
