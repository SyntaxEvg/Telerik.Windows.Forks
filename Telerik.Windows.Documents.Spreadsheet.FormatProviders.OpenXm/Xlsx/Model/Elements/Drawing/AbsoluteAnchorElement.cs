using System;
using System.Windows;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class AbsoluteAnchorElement : AnchorElementBase
	{
		public AbsoluteAnchorElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.pos = base.RegisterChildElement<OffsetElement>("pos");
			this.ext = base.RegisterChildElement<SizeElement>("ext");
			base.RegisterChildElements();
		}

		public override string ElementName
		{
			get
			{
				return "absoluteAnchor";
			}
		}

		public OffsetElement PositionElement
		{
			get
			{
				return this.pos.Element;
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
			if (this.PositionElement != null)
			{
				RadWorksheetLayout worksheetLayout = context.Worksheet.Workbook.GetWorksheetLayout(context.Worksheet, false);
				fromIndex = worksheetLayout.GetCellIndexAndOffsetFromPoint(new Point((double)this.PositionElement.X, (double)this.PositionElement.Y), out fromOffsetX, out fromOffsetY);
				base.ReleaseElement(this.pos);
			}
			double width = 0.0;
			double height = 0.0;
			if (this.SizeElement != null)
			{
				width = this.SizeElement.ExtentWidth;
				height = this.SizeElement.ExtentHeight;
				base.ReleaseElement(this.ext);
			}
			FloatingShapeBase floatingShapeBase = base.CreateShapeAndCopyPropertiesTo(context, fromIndex, fromOffsetX, fromOffsetY);
			if (floatingShapeBase != null)
			{
				floatingShapeBase.Shape.Width = width;
				floatingShapeBase.Shape.Height = height;
				context.Worksheet.Shapes.Add(floatingShapeBase);
			}
			base.OnAfterRead(context);
		}

		readonly OpenXmlChildElement<OffsetElement> pos;

		readonly OpenXmlChildElement<SizeElement> ext;
	}
}
