using System;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class ImageExportingEventArgs : EventArgs
	{
		internal ImageExportingEventArgs(Image imageInline)
		{
			Guard.ThrowExceptionIfNull<Image>(imageInline, "imageInline");
			this.image = imageInline;
			this.ExportSize = true;
		}

		public Image Image
		{
			get
			{
				return this.image;
			}
		}

		public bool ExportSize { get; set; }

		public string Source { get; set; }

		public string AlternativeText { get; set; }

		public string Title { get; set; }

		public bool Handled { get; set; }

		readonly Image image;
	}
}
