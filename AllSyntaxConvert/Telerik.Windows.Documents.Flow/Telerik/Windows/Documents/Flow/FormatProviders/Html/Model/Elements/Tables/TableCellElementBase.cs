using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	abstract class TableCellElementBase : BlockContainerElementBase
	{
		public TableCellElementBase(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.columnSpan = base.RegisterAttribute<int>("colspan", false);
			this.rowSpan = base.RegisterAttribute<int>("rowspan", false);
			base.RegisterAttribute(new StyleValueAttribute("bgcolor", "background-color", base.Style, null, false, null));
			base.RegisterAttribute(new StyleValueAttribute("height", base.Style, null, false, null));
			base.RegisterAttribute(new StyleValueAttribute("width", base.Style, null, false, null));
			base.RegisterAttribute(new StyleValueAttribute("valign", "vertical-align", base.Style, null, false, null));
		}

		protected TableCell AssociatedTableCell
		{
			get
			{
				return base.AssociatedBlockContainer as TableCell;
			}
		}

		protected abstract CellType CellType { get; }

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			TableCell tableCell = new TableCell(context.Document);
			base.SetAssociatedFlowElement(tableCell);
			base.CopyLocalPropertiesTo(context, tableCell);
			if (this.columnSpan.HasValue)
			{
				tableCell.ColumnSpan = this.columnSpan.Value;
			}
			if (this.rowSpan.HasValue)
			{
				tableCell.RowSpan = this.rowSpan.Value;
			}
			context.InsertTableCell(tableCell, this.CellType);
		}

		protected override void EvaluatePropertiesAfterReadAttributes(IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			this.RuleOutBubblingProperties(context, this.AssociatedTableCell);
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			context.PopBlockContainer();
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			TableCell associatedTableCell = this.AssociatedTableCell;
			Guard.ThrowExceptionIfFalse(associatedTableCell != null, "No associated table cell.");
			if ((context.Settings.StylesExportMode == StylesExportMode.External || context.Settings.StylesExportMode == StylesExportMode.Embedded) && associatedTableCell.Parent != null && associatedTableCell.Parent.Parent != null)
			{
				string styleId = (associatedTableCell.Parent.Parent as IElementWithStyle).StyleId;
				if (!string.IsNullOrEmpty(styleId))
				{
					string styleId2 = StyleNamesConverter.ConvertStyleNameOnExport(styleId);
					if (context.HtmlStyleRepository.ContainsStyle(styleId2, "td"))
					{
						base.CopyStylePropertiesFrom(context, styleId);
					}
				}
			}
			base.CopyLocalPropertiesFrom(context, associatedTableCell);
			base.RemoveLocalPropertiesAlreadyInAppliedStyles(context, "td");
			if (associatedTableCell.ColumnSpan != 1)
			{
				this.columnSpan.Value = associatedTableCell.ColumnSpan;
			}
			if (associatedTableCell.RowSpan != 1)
			{
				this.rowSpan.Value = associatedTableCell.RowSpan;
			}
		}

		readonly HtmlAttribute<int> columnSpan;

		readonly HtmlAttribute<int> rowSpan;
	}
}
