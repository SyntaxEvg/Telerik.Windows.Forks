using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableRowElement : BodyElementBase
	{
		public TableRowElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.rowType = RowType.Body;
			base.RegisterAttribute(new StyleValueAttribute("height", base.Style, null, false, null));
		}

		public override string Name
		{
			get
			{
				return "tr";
			}
		}

		public void SetAssociatedFlowElement(TableRow row)
		{
			Guard.ThrowExceptionIfNull<TableRow>(row, "row");
			this.row = row;
		}

		public void SetRowType(RowType rowType)
		{
			this.rowType = rowType;
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			TableRow tableRow = new TableRow(context.Document);
			this.SetAssociatedFlowElement(tableRow);
			base.CopyLocalPropertiesTo(context, tableRow);
			context.PushTableRow(tableRow, this.rowType);
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (TableCell cell in this.row.Cells)
			{
				TableCellElement cellElement = base.CreateElement<TableCellElement>("td");
				cellElement.SetAssociatedFlowElement(cell);
				yield return cellElement;
			}
			yield break;
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			base.BubblingProperties.CopyTo(context, this.row);
			base.BubblingProperties.RemoveProperty(HtmlStylePropertyDescriptors.RowHeightPropertyDescriptor.Name);
			context.PopTableRow();
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			base.OnBeforeWrite(writer, context);
			base.CopyLocalPropertiesFrom(context, this.row);
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.rowType = RowType.Body;
			this.row = null;
		}

		TableRow row;

		RowType rowType;
	}
}
