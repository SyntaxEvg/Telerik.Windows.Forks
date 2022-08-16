using System;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class BulletListInfo
	{
		public BulletListInfo(string htmlName, string symbol, FontFamily symbolFontFamily)
		{
			this.HtmlName = htmlName;
			this.BulletSymbol = symbol;
			this.SymbolFontFamily = symbolFontFamily;
		}

		public string HtmlName { get; set; }

		public string BulletSymbol { get; set; }

		public FontFamily SymbolFontFamily { get; set; }
	}
}
