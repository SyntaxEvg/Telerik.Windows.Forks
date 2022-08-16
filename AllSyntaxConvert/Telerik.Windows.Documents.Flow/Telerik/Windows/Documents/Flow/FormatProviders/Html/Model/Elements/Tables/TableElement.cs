using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables
{
	class TableElement : BodyElementBase
	{
		public TableElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.align = base.RegisterAttribute<string>("align", false);
			this.cellPadding = base.RegisterAttribute<string>("cellpadding", false);
			base.RegisterAttribute(new StyleValueAttribute("width", base.Style, null, false, LegacyConverters.NumericAttributeToPixelUnitTypeConverter));
			base.RegisterAttribute(new StyleValueAttribute("bgcolor", "background-color", base.Style, null, false, null));
			base.RegisterAttribute(new StyleValueAttribute("bordercolor", "border-color", base.Style, null, false, null));
			base.RegisterAttribute(new StyleValueAttribute("border", base.Style, null, false, LegacyConverters.NumericAttributeToPixelUnitTypeConverter));
			base.RegisterAttribute(new StyleValueAttribute("cellspacing", "border-spacing", base.Style, null, false, LegacyConverters.NumericAttributeToPixelUnitTypeConverter));
		}

		public override string Name
		{
			get
			{
				return "table";
			}
		}

		protected override bool CanHaveStyle
		{
			get
			{
				return true;
			}
		}

		public void SetAssociatedFlowElement(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			this.table = table;
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			base.OnBeforeWrite(writer, context);
			context.BeginExportTable(this.table);
			base.CopyStyleFrom(context, this.table);
			base.CopyLocalPropertiesFrom(context, this.table);
			base.RemoveLocalPropertiesAlreadyInAppliedStyles(context, "table");
			string value;
			if (this.table.Alignment != DocumentDefaultStyleSettings.TableAlignment && HtmlValueMappers.AlignmentValueMapper.TryGetFromValue(this.table.Alignment, out value))
			{
				this.align.Value = value;
			}
		}

		protected override void OnAfterWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			base.OnAfterWrite(writer, context);
			context.EndExportTable(this.table);
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Table table = new Table(context.Document);
			base.ApplyStyle(context, table);
			base.CopyLocalPropertiesTo(context, table);
			Alignment alignment;
			if (this.align.HasValue && HtmlValueMappers.AlignmentValueMapper.TryGetToValue(this.align.Value, out alignment))
			{
				table.Alignment = alignment;
			}
			if (this.cellPadding.HasValue && !string.IsNullOrEmpty(this.cellPadding.Value))
			{
				HtmlStyleProperty property = new HtmlStyleProperty("cellpadding", this.cellPadding.Value);
				Padding tableCellPadding;
				if (HtmlConverters.PaddingConverter.Convert(context, property, table.Properties, out tableCellPadding))
				{
					table.TableCellPadding = tableCellPadding;
				}
			}
			context.PushTable(table);
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			context.PopTable();
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (TableRow row in this.table.Rows)
			{
				TableRowElement rowElement = base.CreateElement<TableRowElement>("tr");
				rowElement.SetAssociatedFlowElement(row);
				yield return rowElement;
			}
			yield break;
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.table = null;
		}

		protected override bool GetDefaultStyleId(IHtmlExportContext context, out string styleId)
		{
			styleId = context.Document.StyleRepository.GetDefaultStyle(StyleType.Table).Id;
			return true;
		}

		readonly HtmlAttribute<string> align;

		readonly HtmlAttribute<string> cellPadding;

		Table table;
	}
}
