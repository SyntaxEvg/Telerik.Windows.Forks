using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class HyperlinkElement : BodyElementBase, IPhrasingElement
	{
		public HyperlinkElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.innerElements = new List<HtmlElementBase>();
			this.uri = base.RegisterAttribute<string>("href", false);
			this.target = base.RegisterAttribute<string>("target", false);
			this.tooltip = base.RegisterAttribute<string>("title", false);
		}

		public override string Name
		{
			get
			{
				return "a";
			}
		}

		public Hyperlink Hyperlink { get; set; }

		protected override bool CanHaveStyle
		{
			get
			{
				return true;
			}
		}

		public void CopyPropertiesFrom(IHtmlExportContext context, Hyperlink hyperlink)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Hyperlink>(hyperlink, "hyperlink");
			this.Hyperlink = hyperlink;
			this.uri.Value = this.Hyperlink.Uri;
			if (!string.IsNullOrEmpty(this.Hyperlink.ToolTip))
			{
				this.tooltip.Value = this.Hyperlink.ToolTip;
			}
		}

		public void AppendInnerElement(HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			this.innerElements.Add(innerElement);
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Hyperlink hyperlink = new Hyperlink(context.Document);
			if (this.uri.HasValue)
			{
				hyperlink.Uri = this.uri.Value;
			}
			bool hasValue = this.target.HasValue;
			if (this.tooltip.HasValue)
			{
				hyperlink.ToolTip = this.tooltip.Value;
			}
			this.hyperlinkField = new FieldInfo(context.Document, hyperlink);
			context.InsertInline(this.hyperlinkField.Start);
			context.InsertInline(new Run(context.Document)
			{
				Text = hyperlink.CreateHyperlinkCode()
			});
			context.InsertInline(this.hyperlinkField.Separator);
			context.BeginHyperlink();
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			context.EndHyperlink();
			context.InsertInline(this.hyperlinkField.End);
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return this.innerElements;
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.hyperlinkField = null;
			this.Hyperlink = null;
			this.innerElements.Clear();
		}

		readonly List<HtmlElementBase> innerElements;

		readonly HtmlAttribute<string> uri;

		readonly HtmlAttribute<string> target;

		readonly HtmlAttribute<string> tooltip;

		FieldInfo hyperlinkField;
	}
}
