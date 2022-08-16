using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class WorksheetLayerBase : LayerBase
	{
		protected sealed override void UpdateRenderOverride(RenderUpdateContext updateContext)
		{
			this.UpdateRenderOverride(updateContext as WorksheetRenderUpdateContext);
		}

		protected sealed override void TranslateAndScale(RenderUpdateContext updateContext)
		{
			this.TranslateAndScale(updateContext as WorksheetRenderUpdateContext);
		}

		protected virtual void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			Guard.ThrowExceptionIfNull<WorksheetRenderUpdateContext>(worksheetUpdateContext, "worksheetUpdateContext");
		}

		protected virtual void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			Guard.ThrowExceptionIfNull<WorksheetRenderUpdateContext>(worksheetUpdateContext, "worksheetUpdateContext");
		}
	}
}
