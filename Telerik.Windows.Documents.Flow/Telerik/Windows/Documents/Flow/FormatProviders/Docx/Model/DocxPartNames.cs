using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	static class DocxPartNames
	{
		public const string DocumentPartName = "/word/document.xml";

		public const string HeaderPartName = "/word/header{0}.xml";

		public const string FooterPartName = "/word/footer{0}.xml";

		public const string ResourcePartName = "/word/media/{0}";

		public const string DocumentSettingsPartName = "/word/settings.xml";

		public const string ThemePartName = "/word/theme/theme1.xml";

		public const string StylesPartName = "/word/styles.xml";

		public const string CommentsPartName = "/word/comments.xml";

		public const string ListsPartName = "/word/numbering.xml";
	}
}
