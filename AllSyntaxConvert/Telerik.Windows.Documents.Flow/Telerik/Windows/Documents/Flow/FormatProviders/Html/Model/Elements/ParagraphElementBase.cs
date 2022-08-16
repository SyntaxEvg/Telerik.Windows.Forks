using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	abstract class ParagraphElementBase : BodyElementBase
	{
		public ParagraphElementBase(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		protected Paragraph Paragraph
		{
			get
			{
				return this.paragraph;
			}
		}

		protected override bool CanHaveStyle
		{
			get
			{
				return true;
			}
		}

		public void SetAssociatedFlowElement(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.paragraph = paragraph;
		}

		protected void ApplyListLevelIndentationOnCurrentParagraphIfNecessary(IHtmlImportContext context)
		{
			if (context.HasCurrentParagraph() && context.ListImportContext.IsInListLevel && context.ListImportContext.CurrentListLevelContentImporter.ShouldApplyIndentationOnParagraph)
			{
				Paragraph currentParagraph = context.CurrentParagraph;
				this.CopyStyleToOverride(context, currentParagraph);
				if (currentParagraph.ListId == DocumentDefaultStyleSettings.ListId && currentParagraph.ListLevel == DocumentDefaultStyleSettings.ListLevel)
				{
					ListLevel level = base.ContentManager.ImportListManager.GetLevel();
					currentParagraph.Indentation.LeftIndent = level.ParagraphProperties.LeftIndent.GetActualValue().Value;
				}
			}
		}

		protected override void CopyStyleToOverride(IHtmlImportContext context, IElementWithStyle element)
		{
			if (element is Paragraph && string.IsNullOrEmpty(element.StyleId))
			{
				element.StyleId = "NormalWeb";
				context.ShouldImportNormalWeb = true;
			}
		}

		protected override bool GetDefaultStyleId(IHtmlExportContext context, out string styleId)
		{
			styleId = context.Document.StyleRepository.GetDefaultStyle(StyleType.Paragraph).Id;
			return true;
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			base.OnBeforeWrite(writer, context);
			base.CopyStyleFrom(context, this.Paragraph);
			base.CopyLocalPropertiesFrom(context, this.Paragraph);
			context.ClearParagraphClasses(base.Classes);
		}

		protected override void WriteContent(IHtmlWriter writer, IHtmlExportContext context)
		{
			base.WriteContent(writer, context);
			if (this.ShouldExportNonBreakingSpace())
			{
				writer.WriteValue(" ", true);
			}
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (HtmlElementBase element in base.ContentManager.ExportParagraphInlines(context, this.Paragraph))
			{
				yield return element;
			}
			yield break;
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.paragraph = null;
		}

		bool ShouldExportNonBreakingSpace()
		{
			List<Run> list = this.paragraph.Inlines.OfType<Run>().ToList<Run>();
			bool result;
			if (this.paragraph.Inlines.Count != 0)
			{
				if (list.Count == this.paragraph.Inlines.Count)
				{
					result = list.All((Run run) => string.IsNullOrEmpty(HtmlTextProcessor.TrimEnd(run.Text)));
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		const string EmptyParagraphExportCharacter = " ";

		Paragraph paragraph;
	}
}
