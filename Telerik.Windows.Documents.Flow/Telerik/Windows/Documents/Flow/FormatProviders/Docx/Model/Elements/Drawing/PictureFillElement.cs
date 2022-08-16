using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class PictureFillElement : PictureFillElementBase
	{
		public PictureFillElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public DocxPartsManager DocxPartsManager
		{
			get
			{
				return (DocxPartsManager)base.PartsManager;
			}
		}

		protected override ImageSource CreateImageSourceFromRelationshipId(IOpenXmlImportContext context, string relationshipId)
		{
			string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, relationshipId);
			string resourceName = base.Part.GetResourceName(relationshipTarget);
			return (ImageSource)context.GetResourceByResourceKey(resourceName);
		}

		protected override string CreateRelationshipIdFromImageSource(IOpenXmlExportContext context, ImageSource resource)
		{
			string target = DocxHelper.CreateResourceName(resource);
			return base.PartsManager.CreateRelationship(base.Part.Name, target, OpenXmlRelationshipTypes.GetRelationshipTypeByExtension(resource.Extension), null);
		}
	}
}
