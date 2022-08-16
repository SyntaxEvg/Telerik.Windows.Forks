using System;

namespace Telerik.Windows.Documents.Flow.Model.Editing
{
	public static class EditingExtensions
	{
		public static RadFlowDocumentEditor GetEditorAfter(this InlineBase inline)
		{
			RadFlowDocumentEditor radFlowDocumentEditor = new RadFlowDocumentEditor(inline.Document);
			radFlowDocumentEditor.MoveToInlineEnd(inline);
			return radFlowDocumentEditor;
		}

		public static RadFlowDocumentEditor GetEditorBefore(this InlineBase inline)
		{
			RadFlowDocumentEditor radFlowDocumentEditor = new RadFlowDocumentEditor(inline.Document);
			radFlowDocumentEditor.MoveToInlineStart(inline);
			return radFlowDocumentEditor;
		}
	}
}
