using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class HtmlExportSettings
	{
		public HtmlExportSettings()
		{
			this.StylesExportMode = StylesExportMode.Embedded;
			this.ImagesExportMode = ImagesExportMode.Embedded;
			this.IndentDocument = false;
			this.BordersMinimalThickness = 0.0;
		}

		public event EventHandler<ExternalStylesExportingEventArgs> ExternalStylesExporting;

		public event EventHandler<ImageExportingEventArgs> ImageExporting;

		public string ImagesFolderPath { get; set; }

		public string ImagesSourceBasePath { get; set; }

		public string StylesFilePath { get; set; }

		public string StylesSourcePath { get; set; }

		public ImagesExportMode ImagesExportMode { get; set; }

		public StylesExportMode StylesExportMode { get; set; }

		public DocumentExportLevel DocumentExportLevel { get; set; }

		public bool IndentDocument { get; set; }

		public double BordersMinimalThickness { get; set; }

		internal void OnExternalStylesExporting(ExternalStylesExportingEventArgs e)
		{
			Guard.ThrowExceptionIfNull<ExternalStylesExportingEventArgs>(e, "e");
			if (this.ExternalStylesExporting != null)
			{
				this.ExternalStylesExporting(this, e);
			}
		}

		internal void OnImageExporting(ImageExportingEventArgs e)
		{
			Guard.ThrowExceptionIfNull<ImageExportingEventArgs>(e, "e");
			if (this.ImageExporting != null)
			{
				this.ImageExporting(this, e);
			}
		}
	}
}
