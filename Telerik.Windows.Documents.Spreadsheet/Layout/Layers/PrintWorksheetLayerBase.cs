using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class PrintWorksheetLayerBase : LayerBase
	{
		protected sealed override void UpdateRenderOverride(RenderUpdateContext updateContext)
		{
			this.UpdateRenderOverride(updateContext as PrintWorksheetRenderUpdateContext);
		}

		protected sealed override void TranslateAndScale(RenderUpdateContext updateContext)
		{
			this.TranslateAndScale(updateContext as PrintWorksheetRenderUpdateContext);
		}

		protected virtual void UpdateRenderOverride(PrintWorksheetRenderUpdateContext worksheetUpdateContext)
		{
			Guard.ThrowExceptionIfNull<PrintWorksheetRenderUpdateContext>(worksheetUpdateContext, "worksheetUpdateContext");
		}

		protected virtual void TranslateAndScale(PrintWorksheetRenderUpdateContext worksheetUpdateContext)
		{
			Guard.ThrowExceptionIfNull<PrintWorksheetRenderUpdateContext>(worksheetUpdateContext, "worksheetUpdateContext");
		}
	}
}
