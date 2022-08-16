using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class Selector : HtmlStyleProperties
	{
		public Selector(string name, SelectorType type)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
			this.type = type;
		}

		public Selector(string name, SelectorType type, string forElementType)
			: this(name, type)
		{
			Guard.ThrowExceptionIfNullOrEmpty(forElementType, "forElementsofType");
			this.forElementType = forElementType;
		}

		public SelectorType Type
		{
			get
			{
				return this.type;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string ForElementType
		{
			get
			{
				return this.forElementType;
			}
		}

		public void CopyFrom(IHtmlExportContext context, Style style)
		{
			if (string.IsNullOrEmpty(style.BasedOnStyleId))
			{
				this.CopyFromIncludingNonHtmlDefaultActualValues(context, style);
				return;
			}
			this.CopyFromInternal(context, style);
		}

		public void CopyTo(IHtmlImportContext context, Style style)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Style>(style, "style");
			base.CopyTo(context, style.CharacterProperties);
			base.CopyTo(context, style.ParagraphProperties);
			base.CopyTo(context, style.TableCellProperties);
			base.CopyTo(context, style.TableProperties);
			base.CopyTo(context, style.TableRowProperties);
		}

		public void Write(CssStyleWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<CssStyleWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			writer.WriteStyleStart(this.Name, this.Type, this.ForElementType);
			base.Write(writer);
			writer.WriteStyleEnd();
		}

		void CopyFromInternal(IHtmlExportContext context, Style style)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Style>(style, "style");
			base.CopyFrom(context, style.CharacterProperties);
			base.CopyFrom(context, style.ParagraphProperties);
			base.CopyFrom(context, style.TableCellProperties);
			base.CopyFrom(context, style.TableProperties);
			base.CopyFrom(context, style.TableRowProperties);
		}

		void CopyFromIncludingNonHtmlDefaultActualValues(IHtmlExportContext context, Style style)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Style>(style, "style");
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, context.Document.DefaultStyle.CharacterProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, context.Document.DefaultStyle.ParagraphProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, style.CharacterProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, style.ParagraphProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, style.TableCellProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, style.TableProperties);
			base.CopyFromIncludingNonHtmlDefaultActualValues(context, style.TableRowProperties);
		}

		readonly string name;

		readonly string forElementType;

		readonly SelectorType type;
	}
}
