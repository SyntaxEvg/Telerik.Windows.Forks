using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	abstract class FixedContentElementEditorBase
	{
		public FixedContentElementEditorBase(FixedContentEditorBase contentEditor)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditorBase>(contentEditor, "contentEditor");
			this.contentEditor = contentEditor;
		}

		protected FixedContentEditorBase Editor
		{
			get
			{
				return this.contentEditor;
			}
		}

		readonly FixedContentEditorBase contentEditor;
	}
}
