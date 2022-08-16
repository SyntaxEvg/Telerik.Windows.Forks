using System;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class ElementContext
	{
		public ElementContext(OpenXmlWriter writer, OpenXmlReader reader, SpreadTheme theme)
		{
			this.Writer = writer;
			this.Reader = reader;
			this.Theme = theme;
		}

		public OpenXmlWriter Writer { get; set; }

		public OpenXmlReader Reader { get; set; }

		public SpreadTheme Theme { get; set; }
	}
}
