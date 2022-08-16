using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class MarkerElement : WorksheetDrawingElementBase
	{
		public MarkerElement(XlsxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			this.elementName = elementName;
			this.column = base.RegisterChildElement<ColumnRowElement>("col");
			this.columnOffset = base.RegisterChildElement<OneDimentionOffsetElement>("colOff");
			this.row = base.RegisterChildElement<ColumnRowElement>("row");
			this.rowOffset = base.RegisterChildElement<OneDimentionOffsetElement>("rowOff");
		}

		public ColumnRowElement ColumnElement
		{
			get
			{
				return this.column.Element;
			}
		}

		public OneDimentionOffsetElement ColumnOffsetElement
		{
			get
			{
				return this.columnOffset.Element;
			}
		}

		public ColumnRowElement RowElement
		{
			get
			{
				return this.row.Element;
			}
		}

		public OneDimentionOffsetElement RowOffsetElement
		{
			get
			{
				return this.rowOffset.Element;
			}
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, CellIndex cellIndex, double offsetX, double offsetY)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			base.CreateElement(this.column);
			this.ColumnElement.InnerText = cellIndex.ColumnIndex.ToString();
			base.CreateElement(this.columnOffset);
			this.ColumnOffsetElement.InnerText = ((int)Unit.DipToEmu(offsetX)).ToString();
			base.CreateElement(this.row);
			this.RowElement.InnerText = cellIndex.RowIndex.ToString();
			base.CreateElement(this.rowOffset);
			this.RowOffsetElement.InnerText = ((int)Unit.DipToEmu(offsetY)).ToString();
		}

		public void CopyPropertiesTo(IXlsxWorksheetImportContext context, out CellIndex cellIndex, out double offsetX, out double offsetY)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			int rowIndex = int.Parse(this.RowElement.InnerText);
			int columnIndex = int.Parse(this.ColumnElement.InnerText);
			cellIndex = new CellIndex(rowIndex, columnIndex);
			offsetX = Unit.EmuToDip((double)int.Parse(this.ColumnOffsetElement.InnerText));
			offsetY = Unit.EmuToDip((double)int.Parse(this.RowOffsetElement.InnerText));
			base.ReleaseElement(this.column);
			base.ReleaseElement(this.columnOffset);
			base.ReleaseElement(this.row);
			base.ReleaseElement(this.rowOffset);
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			if (elementName != null)
			{
				if (elementName == "col")
				{
					return new ColumnRowElement(base.PartsManager, "col")
					{
						Part = base.Part
					};
				}
				if (elementName == "row")
				{
					return new ColumnRowElement(base.PartsManager, "row")
					{
						Part = base.Part
					};
				}
			}
			return base.CreateElement(elementName);
		}

		protected override void ReleaseElementOverride(OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			string a;
			if ((a = element.ElementName) != null && (a == "col" || a == "row"))
			{
				return;
			}
			base.ReleaseElementOverride(element);
		}

		readonly string elementName;

		readonly OpenXmlChildElement<ColumnRowElement> column;

		readonly OpenXmlChildElement<OneDimentionOffsetElement> columnOffset;

		readonly OpenXmlChildElement<ColumnRowElement> row;

		readonly OpenXmlChildElement<OneDimentionOffsetElement> rowOffset;
	}
}
