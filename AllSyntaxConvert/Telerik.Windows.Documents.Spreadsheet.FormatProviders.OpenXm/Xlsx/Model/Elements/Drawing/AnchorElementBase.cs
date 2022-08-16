using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	abstract class AnchorElementBase : WorksheetDrawingElementBase
	{
		public AnchorElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.picture = new OpenXmlChildElement<PictureElement>("pic");
			this.graphicFrame = new OpenXmlChildElement<GraphicFrameElement>("graphicFrame");
			this.clientData = new OpenXmlChildElement<ClientDataElement>("clientData");
		}

		public PictureElement PictureElement
		{
			get
			{
				return this.picture.Element;
			}
		}

		public GraphicFrameElement GraphicFrameElement
		{
			get
			{
				return this.graphicFrame.Element;
			}
		}

		public virtual void CopyPropertiesFrom(IXlsxWorksheetExportContext context, FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			Image image = shape.Shape as Image;
			if (image != null)
			{
				base.CreateElement(this.picture);
				this.PictureElement.CopyPropertiesFrom(context, image);
			}
			ChartShape chartShape = shape.Shape as ChartShape;
			if (chartShape != null)
			{
				base.CreateElement(this.graphicFrame);
				this.GraphicFrameElement.CopyPropertiesFrom(context, chartShape);
			}
			base.CreateElement(this.clientData);
		}

		protected FloatingShapeBase CreateShapeAndCopyPropertiesTo(IXlsxWorksheetImportContext context, CellIndex fromIndex, double fromOffsetX, double fromOffsetY)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			FloatingShapeBase result = null;
			if (this.PictureElement != null)
			{
				FloatingImage floatingImage = new FloatingImage(context.Worksheet, fromIndex, fromOffsetX, fromOffsetY);
				this.PictureElement.CopyPropertiesTo(context, floatingImage.Image);
				result = floatingImage;
			}
			else if (this.GraphicFrameElement != null)
			{
				ShapeBase shapeBase = this.GraphicFrameElement.CreateShapeAndCopyPropertiesTo(context);
				if (shapeBase is Image)
				{
					FloatingImage floatingImage2 = new FloatingImage(context.Worksheet, fromIndex, fromOffsetX, fromOffsetY, shapeBase as Image);
					result = floatingImage2;
				}
				else if (shapeBase is ChartShape)
				{
					result = new FloatingChartShape(context.Worksheet, fromIndex, shapeBase as ChartShape)
					{
						OffsetX = fromOffsetX,
						OffsetY = fromOffsetY
					};
				}
			}
			return result;
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			base.ReleaseElement(this.picture);
			base.ReleaseElement(this.graphicFrame);
			base.ReleaseElement(this.clientData);
		}

		protected void RegisterChildElements()
		{
			base.RegisterChildElement<PictureElement>(this.picture);
			base.RegisterChildElement<GraphicFrameElement>(this.graphicFrame);
			base.RegisterChildElement<ClientDataElement>(this.clientData);
		}

		readonly OpenXmlChildElement<PictureElement> picture;

		readonly OpenXmlChildElement<GraphicFrameElement> graphicFrame;

		readonly OpenXmlChildElement<ClientDataElement> clientData;
	}
}
