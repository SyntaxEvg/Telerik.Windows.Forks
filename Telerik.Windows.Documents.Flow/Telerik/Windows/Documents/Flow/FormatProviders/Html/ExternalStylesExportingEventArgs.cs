using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class ExternalStylesExportingEventArgs : EventArgs
	{
		internal ExternalStylesExportingEventArgs(string css)
		{
			this.css = css;
		}

		public bool Handled { get; set; }

		public string Reference { get; set; }

		public string Css
		{
			get
			{
				return this.css;
			}
		}

		readonly string css;
	}
}
