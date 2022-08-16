using System;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	interface IRenderer<T> where T : IRenderable
	{
		void Render(T renderable, ViewportPaneType paneType);
	}
}
