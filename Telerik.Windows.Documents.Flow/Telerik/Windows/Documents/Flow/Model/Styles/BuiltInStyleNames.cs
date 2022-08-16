using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public static class BuiltInStyleNames
	{
		public static string GetHeadingStyleIdByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("Heading{0}", index);
		}

		public static string GetHeadingStyleNameByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("Heading {0}", index);
		}

		public static string GetHeadingCharStyleIdByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("Heading{0}Char", index);
		}

		public static string GetHeadingCharStyleNameByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("Heading {0} Char", index);
		}

		public static string GetTocStyleIdByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("TOC{0}", index);
		}

		public static string GetTocStyleNameByIndex(int index)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 9, index, "index");
			return string.Format("toc {0}", index);
		}

		public const string NormalTableStyleId = "TableNormal";

		public const string NormalTableStyleName = "Table Normal";

		public const string TableGridStyleId = "TableGrid";

		public const string TableGridStyleName = "Table Grid";

		public const string NormalStyleId = "Normal";

		public const string NormalStyleName = "Normal";

		public const string NormalWebStyleId = "NormalWeb";

		public const string NormalWebStyleName = "Normal (Web)";

		public const string HyperlinkStyleId = "Hyperlink";

		public const string HyperlinkStyleName = "Hyperlink";

		public const string CaptionStyleId = "Caption";

		public const string CaptionStyleName = "Caption";

		public const string TofStyleId = "TableofFigures";

		public const string TofStyleName = "table of figures";

		public const string FootnoteReferenceStyleId = "FootnoteReference";

		public const string FootnoteReferenceStyleName = "footnote reference";

		public const string FootnoteTextStyleId = "FootnoteText";

		public const string FootnoteTextStyleName = "footnote text";

		public const string FootnoteTextCharStyleId = "FootnoteTextChar";

		public const string FootnoteTextCharStyleName = "Footnote Text Char";

		public const string EndnoteReferenceStyleId = "EndnoteReference";

		public const string EndnoteReferenceStyleName = "endnote reference";

		public const string EndnoteTextStyleId = "EndnoteText";

		public const string EndnoteTextStyleName = "endnote text";

		public const string EndnoteTextCharStyleId = "EndnoteTextChar";

		public const string EndnoteTextCharStyleName = "Endnote Text Char";

		internal const string HeadingStyleIdTemplate = "Heading{0}";

		internal const string HeadingStyleNameTemplate = "Heading {0}";

		internal const string HeadingCharStyleIdTemplate = "Heading{0}Char";

		internal const string HeadingCharStyleNameTemplate = "Heading {0} Char";

		internal const string TocStyleIdTemplate = "TOC{0}";

		internal const string TocStyleNameTemplate = "toc {0}";
	}
}
