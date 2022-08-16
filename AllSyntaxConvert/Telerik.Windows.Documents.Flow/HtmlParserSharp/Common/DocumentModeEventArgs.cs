using System;

namespace HtmlParserSharp.Common
{
	class DocumentModeEventArgs : EventArgs
	{
		public DocumentMode Mode { get; set; }

		public string PublicIdentifier { get; set; }

		public string SystemIdentifier { get; set; }

		public bool Html4SpecificAdditionalErrorChecks { get; set; }

		public DocumentModeEventArgs(DocumentMode mode, string publicIdentifier, string systemIdentifier, bool html4SpecificAdditionalErrorChecks)
		{
			this.Mode = mode;
			this.PublicIdentifier = publicIdentifier;
			this.SystemIdentifier = systemIdentifier;
			this.Html4SpecificAdditionalErrorChecks = html4SpecificAdditionalErrorChecks;
		}
	}
}
