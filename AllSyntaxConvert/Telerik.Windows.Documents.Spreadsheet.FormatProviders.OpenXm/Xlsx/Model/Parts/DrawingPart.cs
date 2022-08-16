using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	class DrawingPart : XlsxPartBase
	{
		public DrawingPart(XlsxPartsManager partsManager, IXlsxWorksheetExportContext context)
			: this(partsManager, string.Format("/xl/drawings/drawing{0}.xml", context.SheetNo))
		{
		}

		public DrawingPart(XlsxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.worksheetDrawingElement = new WorksheetDrawingElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.worksheetDrawingElement;
			}
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.DrawingContentType;
			}
		}

		public override int Level
		{
			get
			{
				return 5;
			}
		}

		readonly WorksheetDrawingElement worksheetDrawingElement;
	}
}
