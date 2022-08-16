using System;
using Telerik.Windows.Documents.Fixed.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Text
{
	public class TextFormatProviderSettings
	{
		public static TextFormatProviderSettings Default
		{
			get
			{
				return new TextFormatProviderSettings();
			}
		}

		public TextFormatProviderSettings()
		{
			this.LinesSeparator = TextFormatProviderSettings.DefaultLinesSeparator;
			this.PagesSeparator = TextFormatProviderSettings.DefaultPagesSeparator;
		}

		public TextFormatProviderSettings(string linesSeparator, string pagesSeparator)
		{
			this.LinesSeparator = linesSeparator;
			this.PagesSeparator = pagesSeparator;
		}

		public string LinesSeparator { get; set; }

		public string PagesSeparator { get; set; }

		internal static readonly string DefaultLinesSeparator = Line.LineSeparator;

		internal static readonly string DefaultPagesSeparator = "----------- Page{0} ------------";
	}
}
