using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class OneCellAnchorElement : AnchorElementBase
	{
		public OneCellAnchorElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.from = base.RegisterChildElement<MarkerElement>("from");
			this.ext = base.RegisterChildElement<SizeElement>("ext");
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

		public SizeElement SizeElement
		{
			get
			{
				return this.ext.Element;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			CellIndex fromIndex = new CellIndex(0, 0);
			double fromOffsetX = 0.0;
			double fromOffsetY = 0.0;
			if (this.FromElement != null)
			{
				this.FromElement.CopyPropertiesTo(context, out fromIndex, out fromOffsetX, out fromOffsetY);
			}
			FloatingShapeBase floatingShapeBase = base.CreateShapeAndCopyPropertiesTo(context, fromIndex, fromOffsetX, fromOffsetY);
			if (floatingShapeBase != null)
			{
				if (this.SizeElement != null)
				{
					floatingShapeBase.Width = this.SizeElement.ExtentWidth;
					floatingShapeBase.Height = this.SizeElement.ExtentHeight;
				}
				this.AdjustShapeForRotation(floatingShapeBase);
				context.Worksheet.Shapes.Add(floatingShapeBase);
			}
			base.ReleaseElement(this.from);
			base.ReleaseElement(this.ext);
			base.OnAfterRead(context);
		}

		void AdjustShapeForRotation(FloatingShapeBase shape)
		{
			double num = SpreadsheetHelper.RestrictRotationAngle(shape.RotationAngle);
			bool flag = (num >= 45.0 && num < 135.0) || (num > 225.0 && num < 315.0);
			if (flag)
			{
				RadWorksheetLayout worksheetLayout = shape.Worksheet.Workbook.GetWorksheetLayout(shape.Worksheet, false);
				Point pointFromCellIndexAndOffset = worksheetLayout.GetPointFromCellIndexAndOffset(shape.CellIndex, shape.OffsetY, shape.OffsetY);
				double width = shape.Width;
				double height = shape.Height;
				Matrix identity = Matrix.Identity;
				Point point = identity.RotateMatrixAt(-90.0, pointFromCellIndexAndOffset.X + width / 2.0, pointFromCellIndexAndOffset.Y + height / 2.0).Transform(pointFromCellIndexAndOffset);
				point.Y -= width;
				double offsetX;
				double offsetY;
				CellIndex cellIndexAndOffsetFromPoint = worksheetLayout.GetCellIndexAndOffsetFromPoint(point, out offsetX, out offsetY);
				shape.CellIndex = cellIndexAndOffsetFromPoint;
				shape.OffsetX = offsetX;
				shape.OffsetY = offsetY;
				shape.Shape.Width = height;
				shape.Shape.Height = width;
			}
		}

		readonly OpenXmlChildElement<MarkerElement> from;

		readonly OpenXmlChildElement<SizeElement> ext;
	}
}
