using System;

namespace Telerik.Windows.Documents.Flow.Model.Editing
{
	public class InsertDocumentOptions
	{
		public InsertDocumentOptions()
		{
			this.ConflictingStylesResolutionMode = ConflictingStylesResolutionMode.RenameSourceStyle;
			this.InsertLastParagraphMarker = true;
		}

		public ConflictingStylesResolutionMode ConflictingStylesResolutionMode { get; set; }

		public bool InsertLastParagraphMarker { get; set; }
	}
}
