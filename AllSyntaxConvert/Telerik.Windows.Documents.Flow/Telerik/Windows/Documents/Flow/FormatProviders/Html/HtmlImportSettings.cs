using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class HtmlImportSettings
	{
		public HtmlImportSettings()
		{
			this.genericFonts = new GenericHtmlFonts();
			this.DefaultStyleSheet = HtmlConstants.DefaultStyleSheet;
		}

		public event EventHandler<LoadFromUriEventArgs> LoadFromUri;

		public GenericHtmlFonts GenericFonts
		{
			get
			{
				return this.genericFonts;
			}
		}

		public bool ReplaceNonBreakingSpaces { get; set; }

		public string DefaultStyleSheet { get; set; }

		internal byte[] GetDataFromUri(string uri)
		{
			Guard.ThrowExceptionIfNullOrEmpty(uri, "uri");
			LoadFromUriEventArgs loadFromUriEventArgs = new LoadFromUriEventArgs(uri);
			this.OnLoadFromUri(loadFromUriEventArgs);
			return loadFromUriEventArgs.GetData();
		}

		void OnLoadFromUri(LoadFromUriEventArgs args)
		{
			if (this.LoadFromUri != null)
			{
				this.LoadFromUri(this, args);
			}
		}

		readonly GenericHtmlFonts genericFonts;
	}
}
