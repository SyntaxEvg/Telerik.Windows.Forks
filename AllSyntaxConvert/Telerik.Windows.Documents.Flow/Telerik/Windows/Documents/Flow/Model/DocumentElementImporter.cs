using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class DocumentElementImporter
	{
		public DocumentElementImporter(RadFlowDocument targetDocument, RadFlowDocument sourceDocument, ConflictingStylesResolutionMode conflictingStylesResolutionMode)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(targetDocument, "targetDocument");
			Guard.ThrowExceptionIfNull<RadFlowDocument>(sourceDocument, "sourceDocument");
			this.targetDocument = targetDocument;
			this.sourceDocument = sourceDocument;
			this.conflictingStylesResolutionMode = conflictingStylesResolutionMode;
		}

		public T Import<T>(T sourceElement) where T : DocumentElementBase
		{
			Guard.ThrowExceptionIfNull<T>(sourceElement, "sourceElement");
			Guard.ThrowExceptionIfFalse(sourceElement.Document == this.sourceDocument, "The element does not belong to the source document.");
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			T t = (T)((object)sourceElement.CloneCore(this.cloneContext));
			if (t != null)
			{
				sourceElement.OnAfterCloneCore(this.cloneContext, t);
			}
			return t;
		}

		void Initialize()
		{
			MergeOptions mergeOptions = new MergeOptions
			{
				ConflictingStylesResolutionMode = this.conflictingStylesResolutionMode
			};
			this.cloneContext = new CloneContext(this.targetDocument)
			{
				MergeOptions = mergeOptions
			};
			this.targetDocument.MergeWithoutChildren(this.sourceDocument, this.cloneContext);
			this.isInitialized = true;
		}

		readonly RadFlowDocument targetDocument;

		readonly RadFlowDocument sourceDocument;

		readonly ConflictingStylesResolutionMode conflictingStylesResolutionMode;

		bool isInitialized;

		CloneContext cloneContext;
	}
}
