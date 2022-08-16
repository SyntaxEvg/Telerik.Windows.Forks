using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	class LinkElement : HtmlElementBase
	{
		public LinkElement(HtmlContentManager contentManger)
			: base(contentManger)
		{
			this.referenceAttribute = base.RegisterAttribute<string>("href", false);
			this.relationAttribute = base.RegisterAttribute<string>("rel", false);
			this.typeAttribute = base.RegisterAttribute<string>("type", false);
		}

		public override string Name
		{
			get
			{
				return "link";
			}
		}

		public string Reference
		{
			get
			{
				return this.referenceAttribute.Value;
			}
			set
			{
				this.referenceAttribute.Value = value;
			}
		}

		public string Relation
		{
			get
			{
				return this.relationAttribute.Value;
			}
			set
			{
				this.relationAttribute.Value = value;
			}
		}

		public string Type
		{
			get
			{
				return this.typeAttribute.Value;
			}
			set
			{
				this.typeAttribute.Value = value;
			}
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			if (this.Relation == "stylesheet" && this.Type == "text/css" && !string.IsNullOrEmpty(this.Reference))
			{
				byte[] dataFromUri = context.Settings.GetDataFromUri(this.Reference);
				Guard.ThrowExceptionIfNull<byte[]>(dataFromUri, string.Format("Can not get the data from {0}", this.Reference));
				base.InnerText = LinkElement.cssEncoding.GetString(dataFromUri);
			}
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			string css = StyleExporter.Export(context);
			ExternalStylesExportingEventArgs externalStylesExportingEventArgs = new ExternalStylesExportingEventArgs(css);
			context.Settings.OnExternalStylesExporting(externalStylesExportingEventArgs);
			if (externalStylesExportingEventArgs.Handled)
			{
				Guard.ThrowExceptionIfNullOrEmpty(externalStylesExportingEventArgs.Reference, "Reference");
				this.Reference = externalStylesExportingEventArgs.Reference;
			}
			else
			{
				if (context.Settings.StylesFilePath == null)
				{
					throw new NotSupportedException("When exporting styles to external file, HtmlExportSettings.StylesFileName should be set, or HtmlExportSettings.ImageExport should be marked as handled.");
				}
				byte[] bytes = LinkElement.cssEncoding.GetBytes(externalStylesExportingEventArgs.Css);
				File.WriteAllBytes(Path.Combine(new string[] { context.Settings.StylesFilePath }), bytes);
				this.Reference = context.Settings.StylesSourcePath;
			}
			this.Type = "text/css";
			this.Relation = "stylesheet";
		}

		static readonly Encoding cssEncoding = Encoding.UTF8;

		readonly HtmlAttribute<string> referenceAttribute;

		readonly HtmlAttribute<string> relationAttribute;

		readonly HtmlAttribute<string> typeAttribute;
	}
}
