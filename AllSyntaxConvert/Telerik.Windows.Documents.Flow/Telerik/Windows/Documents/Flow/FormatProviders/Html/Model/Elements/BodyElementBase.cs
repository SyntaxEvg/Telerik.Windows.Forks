using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	abstract class BodyElementBase : HtmlElementBase
	{
		public BodyElementBase(HtmlContentManager contentManager)
			: base(contentManager)
		{
		}

		protected static void InsertInline(IHtmlImportContext context, InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			context.InsertInline(inline);
		}

		protected static void InsertRun(IHtmlImportContext context, Run run)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			context.InsertRun(run);
		}

		protected override void OnReadInnerText(IHtmlReader reader, IHtmlImportContext context, string text)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			Run run = new Run(context.Document);
			run.Text = text;
			this.PrepareRunOverride(context, run);
			BodyElementBase.InsertRun(context, run);
		}

		protected override void OnAfterReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			if (innerElement is TextElement)
			{
				Run run = new Run(context.Document);
				run.Text = innerElement.InnerText;
				this.PrepareRunOverride(context, run);
				BodyElementBase.InsertRun(context, run);
			}
		}

		protected virtual void PrepareRunOverride(IHtmlImportContext context, Run run)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			base.CopyLocalPropertiesTo(context, run);
		}

		protected override HtmlElementBase CreateElement(string elementName)
		{
			return base.ContentManager.CreateElement(elementName, true);
		}

		protected override void OnBeforeReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			if (innerElement is IPhrasingElement || innerElement is TextElement)
			{
				this.EnsureParagraph(context);
			}
			base.OnBeforeReadChildElement(reader, context, innerElement);
		}

		protected Paragraph EnsureParagraph(IHtmlImportContext context)
		{
			if (!context.HasCurrentParagraph())
			{
				Paragraph paragraph = new Paragraph(context.Document);
				base.ApplyStyle(context, paragraph);
				base.CopyLocalPropertiesTo(context, paragraph);
				base.CopyLocalPropertiesTo(context, paragraph.Properties.ParagraphMarkerProperties);
				context.BeginParagraph(paragraph);
			}
			return context.CurrentParagraph;
		}
	}
}
