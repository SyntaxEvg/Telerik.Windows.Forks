using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfStyleImporter : RtfElementIteratorBase
	{
		static RtfStyleImporter()
		{
			SpanStyleHandlers.InitializeSpanStyleHandlers(RtfStyleImporter.TagHandlers);
			ParagraphStyleHandlers.InitializeParagraphStyleHandlers(RtfStyleImporter.TagHandlers);
			StyleSheetHandlers.InitializeStyleDefinitionHandlers(RtfStyleImporter.TagHandlers);
			TableStyleHandlers.InitializeTableStyleHandlers(RtfStyleImporter.TagHandlers);
		}

		public RtfStyleImporter(RtfImportContext context, StyleType type)
		{
			this.context = context;
			this.style = new Style("TempName", type);
		}

		public RtfStyleDefinitionInfo ImportStyleGroup(RtfGroup group)
		{
			base.VisitGroupChildren(group, true);
			RtfStyleDefinitionInfo styleDefinitionInfo = this.context.CurrentStyle.StyleDefinitionInfo;
			styleDefinitionInfo.ImportedSpanProperties = new Run(this.context.Document);
			this.context.CurrentStyle.ApplyCharacterStyle(styleDefinitionInfo.ImportedSpanProperties);
			styleDefinitionInfo.ImportedParagraphProperties = new Paragraph(this.context.Document);
			this.context.CurrentStyle.ApplyParagraphStyle(styleDefinitionInfo.ImportedParagraphProperties);
			if (this.context.CurrentRowStyle != null)
			{
				Table table = new Table(this.context.Document);
				this.context.CurrentRowStyle.ApplyTableStyle(table);
				styleDefinitionInfo.ImportedTableProperties = table;
				TableRow tableRow = new TableRow(this.context.Document);
				this.context.CurrentRowStyle.ApplyRowStyle(tableRow);
				styleDefinitionInfo.ImportedTableRowProperties = tableRow;
				TableCell tableCell = new TableCell(this.context.Document);
				if (this.context.CurrentRowStyle.CurrentCellStyle != null)
				{
					this.context.CurrentRowStyle.CurrentCellStyle.ApplyCellStyle(tableCell);
				}
				styleDefinitionInfo.ImportedTableCellProperties = tableCell;
			}
			this.style.IsPrimary = styleDefinitionInfo.IsPrimary;
			if (styleDefinitionInfo.UIPriority != null)
			{
				this.style.UIPriority = styleDefinitionInfo.UIPriority.Value;
			}
			styleDefinitionInfo.CurrentStyle = this.style;
			return styleDefinitionInfo;
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			ControlTagHandler controlTagHandler;
			RtfStyleImporter.TagHandlers.TryGetValue(tag.Name, out controlTagHandler);
			if (controlTagHandler != null)
			{
				controlTagHandler(tag, this.context);
			}
		}

		protected override void DoVisitText(RtfText text)
		{
			string inputStyleName = text.Text.Trim().TrimEnd(new char[] { ';' });
			ReplaceStyleNameContext replaceStyleNameContext = new ReplaceStyleNameContext
			{
				InputStyleName = inputStyleName,
				StyleDefinition = this.style
			};
			ReplaceStyleNameHandler.Replace(replaceStyleNameContext);
		}

		static readonly Dictionary<string, ControlTagHandler> TagHandlers = new Dictionary<string, ControlTagHandler>();

		readonly RtfImportContext context;

		readonly Style style;
	}
}
