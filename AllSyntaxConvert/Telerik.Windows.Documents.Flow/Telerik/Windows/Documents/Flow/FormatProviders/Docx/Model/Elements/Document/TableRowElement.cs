using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TableRowElement : DocumentElementBase
	{
		public TableRowElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.tableRowPropertiesChildElement = base.RegisterChildElement<TableRowPropertiesElement>("trPr");
		}

		public override string ElementName
		{
			get
			{
				return "tr";
			}
		}

		public List<TableCellData> TableCellDatas { get; set; }

		public TableRow TableRow
		{
			get
			{
				return this.tableRow;
			}
		}

		public TableRowPropertiesElement TableRowPropertiesElement
		{
			get
			{
				return this.tableRowPropertiesChildElement.Element;
			}
		}

		public void SetAssociatedFlowModelElement(TableRow tableRow, List<TableCellData> tableCellDatas)
		{
			Guard.ThrowExceptionIfNull<TableRow>(tableRow, "tableRow");
			Guard.ThrowExceptionIfNull<List<TableCellData>>(tableCellDatas, "tableCellDatas");
			this.tableRow = tableRow;
			this.TableCellDatas = tableCellDatas;
		}

		protected override void ClearOverride()
		{
			this.TableCellDatas = null;
			this.tableRow = null;
			base.ClearOverride();
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return this.tableRow.Cells.Any<TableCell>() || base.ShouldExport(context);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (this.TableRowPropertiesElement != null)
			{
				base.ReleaseElement(this.tableRowPropertiesChildElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.tableRow.Properties.HasLocalValues())
			{
				base.CreateElement(this.tableRowPropertiesChildElement);
				this.TableRowPropertiesElement.SetAssociatedFlowModelElement(this.tableRow.Properties);
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (TableCellData tableCellData in this.TableCellDatas)
			{
				if (tableCellData.Type != TableCellType.HorizontalyMerged)
				{
					TableCellElement tableCellElement = base.CreateElement<TableCellElement>("tc");
					tableCellElement.SetAssociatedFlowModelElement(tableCellData.Cell);
					tableCellElement.TableCellData = tableCellData;
					yield return tableCellElement;
				}
			}
			yield break;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "tc")
				{
					TableCellElement tableCellElement = (TableCellElement)childElement;
					tableCellElement.SetAssociatedFlowModelElement(new TableCell(this.tableRow.Document));
					return;
				}
				if (!(elementName == "trPr"))
				{
					return;
				}
				this.TableRowPropertiesElement.SetAssociatedFlowModelElement(this.tableRow.Properties);
			}
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			DocxHelper.AddHangingAnnotation(context, childElement);
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "tc"))
				{
					return;
				}
				TableCellElement tableCellElement = (TableCellElement)childElement;
				this.TableCellDatas.Add(tableCellElement.TableCellData);
			}
		}

		readonly OpenXmlChildElement<TableRowPropertiesElement> tableRowPropertiesChildElement;

		TableRow tableRow;
	}
}
