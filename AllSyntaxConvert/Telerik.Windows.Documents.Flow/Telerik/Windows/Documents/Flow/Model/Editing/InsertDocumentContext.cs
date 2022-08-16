using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;

namespace Telerik.Windows.Documents.Flow.Model.Editing
{
	class InsertDocumentContext
	{
		public InsertDocumentContext(RadFlowDocument targetDocument, RadFlowDocument sourceDocument, InsertDocumentOptions insertDocumentOptions)
		{
			this.insertDocumentOptions = insertDocumentOptions;
			MergeOptions mergeOptions = new MergeOptions
			{
				ConflictingStylesResolutionMode = this.InsertDocumentOptions.ConflictingStylesResolutionMode
			};
			this.cloneContext = new CloneContext(targetDocument)
			{
				MergeOptions = mergeOptions
			};
			targetDocument.MergeWithoutChildren(sourceDocument, this.cloneContext);
		}

		public InsertDocumentOptions InsertDocumentOptions
		{
			get
			{
				return this.insertDocumentOptions;
			}
		}

		public CloneContext CloneContext
		{
			get
			{
				return this.cloneContext;
			}
		}

		public Paragraph LastParagraphMarker { get; set; }

		readonly InsertDocumentOptions insertDocumentOptions;

		readonly CloneContext cloneContext;
	}
}
