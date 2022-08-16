using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class LayerBase : INamedObject
	{
		public LayerBase()
		{
			this.containerManager = new LayerContainerManager();
		}

		public abstract string Name { get; }

		public LayerContainerManager ContainerManager
		{
			get
			{
				return this.containerManager;
			}
		}

		public void UpdateRender(RenderUpdateContext updateContext)
		{
			this.Clear();
			this.UpdateRenderOverride(updateContext);
			this.OnRenderUpdated();
			this.TranslateAndScale(updateContext);
		}

		protected virtual void UpdateRenderOverride(RenderUpdateContext updateContext)
		{
		}

		protected virtual void OnRenderUpdated()
		{
		}

		protected virtual void TranslateAndScale(RenderUpdateContext updateContext)
		{
		}

		protected virtual void Clear()
		{
			this.ContainerManager.Clear();
		}

		protected Point TranslateAndScale(Point point, ViewportPaneType viewportPaneType, RenderUpdateContext updateContext)
		{
			point = this.Translate(point, viewportPaneType, updateContext);
			point = updateContext.ScaleTransform.Transform(point);
			return point;
		}

		protected Size Scale(Size size, RenderUpdateContext updateContext)
		{
			size.Width = SpreadsheetHelper.Transform(updateContext.ScaleTransform, size.Width);
			size.Height = SpreadsheetHelper.Transform(updateContext.ScaleTransform, size.Height);
			return size;
		}

		protected Point Scale(Point point, RenderUpdateContext updateContext)
		{
			point = updateContext.ScaleTransform.Transform(point);
			return point;
		}

		protected double Scale(double value, RenderUpdateContext updateContext)
		{
			return SpreadsheetHelper.Transform(updateContext.ScaleTransform, value);
		}

		protected Point Translate(Point point, ViewportPaneType viewportPaneType, RenderUpdateContext updateContext)
		{
			return updateContext.Translate(point, viewportPaneType);
		}

		protected Rect Translate(Rect rect, ViewportPaneType viewportPaneType, RenderUpdateContext updateContext)
		{
			return updateContext.Translate(rect, viewportPaneType);
		}

		protected Rect TranslateAndScale(Rect rect, ViewportPaneType viewportPaneType, RenderUpdateContext updateContext)
		{
			return updateContext.TranslateAndScale(rect, viewportPaneType);
		}

		readonly LayerContainerManager containerManager;
	}
}
