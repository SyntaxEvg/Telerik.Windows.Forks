using System;
using System.Windows;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class TwoCellAnchorElement : AnchorElementBase
	{
		public TwoCellAnchorElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.editAs = base.RegisterAttribute<string>("editAs", "oneCell", false);
			this.from = base.RegisterChildElement<MarkerElement>("from");
			this.to = base.RegisterChildElement<MarkerElement>("to");
			base.RegisterChildElements();
		}

		public override string ElementName
		{
			get
			{
				return "twoCellAnchor";
			}
		}

		public MarkerElement FromElement
		{
			get
			{
				return this.from.Element;
			}
		}

		public MarkerElement ToElement
		{
			get
			{
				return this.to.Element;
			}
		}

		public string EditAs
		{
			get
			{
				return this.editAs.Value;
			}
			set
			{
				this.editAs.Value = value;
			}
		}

		public override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			CellIndex cellIndex = shape.CellIndex;
			double offsetX = shape.OffsetX;
			double offsetY = shape.OffsetY;
			base.CreateElement(this.from);
			this.FromElement.CopyPropertiesFrom(context, cellIndex, offsetX, offsetY);
			CellIndex cellIndex2;
			double offsetX2;
			double offsetY2;
			this.GetToCellIndex(context.Worksheet, shape, out cellIndex2, out offsetX2, out offsetY2);
			base.CreateElement(this.to);
			this.ToElement.CopyPropertiesFrom(context, cellIndex2, offsetX2, offsetY2);
			this.EditAs = "oneCell";
			base.CopyPropertiesFrom(context, shape);
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			CellIndex fromIndex;
			double fromOffsetX;
			double fromOffsetY;
			this.FromElement.CopyPropertiesTo(context, out fromIndex, out fromOffsetX, out fromOffsetY);
			CellIndex toIndex;
			double toOffsetX;
			double toOffsetY;
			this.ToElement.CopyPropertiesTo(context, out toIndex, out toOffsetX, out toOffsetY);
			FloatingShapeBase floatingShapeBase = base.CreateShapeAndCopyPropertiesTo(context, fromIndex, fromOffsetX, fromOffsetY);
			if (floatingShapeBase != null && floatingShapeBase.Shape.SizeInternal.IsEmpty)
			{
				TwoCellAnchorInfo value = new TwoCellAnchorInfo
				{
					FromIndex = fromIndex,
					ToIndex = toIndex,
					FromOffsetX = fromOffsetX,
					FromOffsetY = fromOffsetY,
					ToOffsetX = toOffsetX,
					ToOffsetY = toOffsetY
				};
				context.ShapesAnchorsWaitingSize.Add(floatingShapeBase, value);
			}
			if (floatingShapeBase != null)
			{
				context.Worksheet.Shapes.Add(floatingShapeBase);
			}
			base.ReleaseElement(this.from);
			base.ReleaseElement(this.to);
			base.OnAfterRead(context);
		}

		void GetToCellIndex(Worksheet worksheet, FloatingShapeBase shape, out CellIndex toCellIndex, out double toOffsetX, out double toOffsetY)
		{
			RadWorksheetLayout worksheetLayout = worksheet.Workbook.GetWorksheetLayout(worksheet, false);
			Point topLeftPointFromCellIndex = worksheetLayout.GetTopLeftPointFromCellIndex(shape.CellIndex);
			if (shape.DoesRotationAngleRequireCellIndexChange())
			{
				topLeftPointFromCellIndex.X += shape.OffsetX + shape.Height;
				topLeftPointFromCellIndex.Y += shape.OffsetY + shape.Width;
			}
			else
			{
				topLeftPointFromCellIndex.X += shape.OffsetX + shape.Width;
				topLeftPointFromCellIndex.Y += shape.OffsetY + shape.Height;
			}
			toCellIndex = worksheetLayout.GetCellIndexAndOffsetFromPoint(topLeftPointFromCellIndex, out toOffsetX, out toOffsetY);
		}

		readonly OpenXmlChildElement<MarkerElement> from;

		readonly OpenXmlChildElement<MarkerElement> to;

		readonly OpenXmlAttribute<string> editAs;
	}
}
