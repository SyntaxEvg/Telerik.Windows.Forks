using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	abstract class PdfRenderer<T> : PdfRendererBase, IRenderer<T> where T : IRenderable
	{
		public void Render(T renderable, ViewportPaneType paneType)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(base.Editor, "Editor");
			Matrix oldEditorMatrix = base.Editor.PreserveStates();
			this.RenderOverride(renderable, paneType);
			base.Editor.RestoreStates(oldEditorMatrix);
		}

		public abstract void RenderOverride(T renderable, ViewportPaneType paneType);
	}
}
