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
	class TableElement : DocumentElementBase
	{
		public TableElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.tablePropertiesElementChildElement = base.RegisterChildElement<TablePropertiesElement>("tblPr");
		}

		public override string ElementName
		{
			get
			{
				return "tbl";
			}
		}

		TablePropertiesElement TablePropertiesElement
		{
			get
			{
				return this.tablePropertiesElementChildElement.Element;
			}
		}

		public void SetAssociatedFlowModelElement(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			this.table = table;
			this.tableRowDatas = new List<TableRowData>();
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.table.Rows.Count > 0 || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			TableCellDataGrid tableCellDataGrid = TableCellDataGrid.CreateFromTable(this.table);
			return this.table.Rows.Zip(tableCellDataGrid.Data, new Func<TableRow, TableRowData, TableRowElement>(this.CreateTableRowElement));
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "tr")
				{
					((TableRowElement)childElement).SetAssociatedFlowModelElement(new TableRow(this.table.Document), new List<TableCellData>());
					return;
				}
				if (!(elementName == "tblPr"))
				{
					return;
				}
				this.TablePropertiesElement.SetAssociatedFlowModelElement(this.table.Properties);
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
				if (!(elementName == "tr"))
				{
					return;
				}
				TableRowElement tableRowElement = (TableRowElement)childElement;
				this.tableRowDatas.Add(new TableRowData(tableRowElement.TableRow, tableRowElement.TableCellDatas));
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			TableCellDataGrid.FillTableFromTableCellsData(this.table, this.tableRowDatas);
			if (this.tablePropertiesElementChildElement != null)
			{
				base.ReleaseElement(this.tablePropertiesElementChildElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (this.table.Properties.HasLocalValues())
			{
				base.CreateElement(this.tablePropertiesElementChildElement);
				this.TablePropertiesElement.SetAssociatedFlowModelElement(this.table.Properties);
			}
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.table = null;
		}

		TableRowElement CreateTableRowElement(TableRow tableRow, TableRowData tableRowData)
		{
			TableRowElement tableRowElement = base.CreateElement<TableRowElement>("tr");
			tableRowElement.SetAssociatedFlowModelElement(tableRow, tableRowData.TableCellDatas);
			return tableRowElement;
		}

		readonly OpenXmlChildElement<TablePropertiesElement> tablePropertiesElementChildElement;

		Table table;

		List<TableRowData> tableRowDatas;
	}
}
