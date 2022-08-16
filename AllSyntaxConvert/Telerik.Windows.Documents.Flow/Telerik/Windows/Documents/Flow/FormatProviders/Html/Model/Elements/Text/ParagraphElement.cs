using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text
{
	class ParagraphElement : ParagraphElementBase
	{
		public ParagraphElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			base.RegisterAttribute(new StyleValueAttribute("dir", "direction", base.Style, null, false, null));
		}

		public override string Name
		{
			get
			{
				return "p";
			}
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			if (!context.ListImportContext.IsInListLevel || context.ListImportContext.CurrentListLevelContentImporter.ShouldCreateParagraph)
			{
				Paragraph paragraph = new Paragraph(context.Document);
				base.SetAssociatedFlowElement(paragraph);
				base.ApplyStyle(context, paragraph);
				base.CopyLocalPropertiesTo(context, paragraph);
				base.CopyLocalPropertiesTo(context, paragraph.Properties.ParagraphMarkerProperties);
				context.BeginParagraph(paragraph);
			}
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			base.ApplyListLevelIndentationOnCurrentParagraphIfNecessary(context);
			context.EndParagraph();
		}
	}
}
