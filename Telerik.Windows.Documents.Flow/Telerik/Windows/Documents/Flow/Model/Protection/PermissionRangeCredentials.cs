using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	public class PermissionRangeCredentials
	{
		public PermissionRangeCredentials(string editor)
		{
			Guard.ThrowExceptionIfNullOrEmpty(editor, "editor");
			Guard.ThrowExceptionIfContainsWhitespace(editor, "editor");
			this.editor = editor;
		}

		public PermissionRangeCredentials(EditingGroup editingGroup)
		{
			Guard.ThrowExceptionIfNull<EditingGroup>(editingGroup, "editingGroup");
			this.editingGroup = new EditingGroup?(editingGroup);
		}

		PermissionRangeCredentials()
		{
		}

		public string Editor
		{
			get
			{
				return this.editor;
			}
			set
			{
				this.editor = value;
			}
		}

		public EditingGroup? EditingGroup
		{
			get
			{
				return this.editingGroup;
			}
			set
			{
				this.editingGroup = value;
			}
		}

		internal PermissionRangeCredentials Clone()
		{
			return new PermissionRangeCredentials
			{
				Editor = this.Editor,
				EditingGroup = this.EditingGroup
			};
		}

		string editor;

		EditingGroup? editingGroup;
	}
}
