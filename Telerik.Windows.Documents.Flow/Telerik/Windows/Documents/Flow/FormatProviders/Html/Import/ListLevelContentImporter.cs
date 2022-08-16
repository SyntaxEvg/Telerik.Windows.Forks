using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class ListLevelContentImporter
	{
		public ListLevelContentImporter()
		{
			this.childIndex = -1;
			this.shouldApplyIndentationOnParagraph = false;
			this.shouldImportOnlyInlines = false;
		}

		internal bool ShouldApplyIndentationOnParagraph
		{
			get
			{
				return this.shouldApplyIndentationOnParagraph;
			}
		}

		internal bool ShouldCreateParagraph
		{
			get
			{
				return !this.shouldImportOnlyInlines && this.childIndex != 0;
			}
		}

		internal void BeginChildRead()
		{
			this.shouldApplyIndentationOnParagraph = ++this.childIndex > 0;
		}

		internal void EndChildRead(bool isReadParagraphEmpty)
		{
			this.shouldImportOnlyInlines = this.childIndex == 0 && isReadParagraphEmpty;
		}

		const int ChildIndexDefaultValue = -1;

		int childIndex;

		bool shouldImportOnlyInlines;

		bool shouldApplyIndentationOnParagraph;
	}
}
