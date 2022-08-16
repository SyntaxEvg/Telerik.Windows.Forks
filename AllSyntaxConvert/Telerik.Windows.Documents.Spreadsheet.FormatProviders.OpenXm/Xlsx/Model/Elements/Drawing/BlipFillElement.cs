using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class BlipFillElement : XlsxElementBase
	{
		public BlipFillElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.blip = base.RegisterChildElement<BlipElement>("blip");
			this.stretch = base.RegisterChildElement<StretchElement>("stretch");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetDrawingMLNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "blipFill";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public BlipElement BlipElement
		{
			get
			{
				return this.blip.Element;
			}
			set
			{
				this.blip.Element = value;
			}
		}

		public StretchElement StretchElement
		{
			get
			{
				return this.stretch.Element;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.blip);
			ImageSource imageSource = image.ImageSource;
			string target = XlsxHelper.CreateResourceName(imageSource);
			this.BlipElement.RelationshipId = base.PartsManager.CreateRelationship(base.Part.Name, target, OpenXmlRelationshipTypes.GetRelationshipTypeByExtension(imageSource.Extension), null);
			base.CreateElement(this.stretch);
			this.StretchElement.CopyPropertiesFrom(context, image);
		}

		public void CopyPropertiesTo(IXlsxWorksheetImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			if (this.BlipElement != null)
			{
				string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, this.BlipElement.RelationshipId);
				string resourceName = base.Part.GetResourceName(relationshipTarget);
				image.ImageSource = (ImageSource)context.GetResourceByResourceKey(resourceName);
				base.ReleaseElement(this.blip);
			}
		}

		readonly OpenXmlChildElement<BlipElement> blip;

		readonly OpenXmlChildElement<StretchElement> stretch;
	}
}
