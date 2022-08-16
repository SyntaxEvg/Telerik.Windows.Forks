using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class CellFillLayer : WorksheetLayerBase
	{
		public CellFillLayer(IRenderer<PatternFillRenderable> patternFillRenderer, IRenderer<GradientFillRenderable> gradientFillRenderer)
		{
			this.patternFillRenderer = patternFillRenderer;
			this.gradientFillRenderer = gradientFillRenderer;
		}

		public override string Name
		{
			get
			{
				return "CellFill";
			}
		}

		internal IRenderer<PatternFillRenderable> PatternFillRenderer
		{
			get
			{
				return this.patternFillRenderer;
			}
		}

		internal IRenderer<GradientFillRenderable> GradientFillRenderer
		{
			get
			{
				return this.gradientFillRenderer;
			}
		}

		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			this.UpdatePatternFillsRender(worksheetUpdateContext);
			this.UpdateGradientFillsRender(worksheetUpdateContext);
		}

		void UpdatePatternFillsRender(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (ViewportPane viewportPane in worksheetUpdateContext.SheetViewport.ViewportPanes)
			{
				Dictionary<IFill, PatternFillRenderable> dictionary = new Dictionary<IFill, PatternFillRenderable>();
				foreach (CellLayoutBox cellLayoutBox in worksheetUpdateContext.VisibleCellLayoutBoxes[viewportPane.ViewportPaneType])
				{
					if (cellLayoutBox.MergeState != CellMergeState.NonTopLeftCellInMergedRange)
					{
						IFill fill = worksheetUpdateContext.GetFill(cellLayoutBox.LongIndex);
						if (fill != CellPropertyDefinitions.FillProperty.DefaultValue)
						{
							PatternFill patternFill = fill as PatternFill;
							if (patternFill != null)
							{
								PatternFillRenderable patternFillRenderable;
								if (!dictionary.TryGetValue(fill, out patternFillRenderable))
								{
									patternFillRenderable = new PatternFillRenderable
									{
										ColorScheme = worksheetUpdateContext.CurrentTheme.ColorScheme,
										PatternFill = patternFill
									};
									dictionary.Add(fill, patternFillRenderable);
									base.ContainerManager.Add(patternFillRenderable, viewportPane.ViewportPaneType);
								}
								patternFillRenderable.AddVisibleBox(cellLayoutBox.BoundingRectangle);
							}
						}
					}
				}
			}
		}

		void UpdateGradientFillsRender(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (ViewportPane viewportPane in worksheetUpdateContext.SheetViewport.ViewportPanes)
			{
				foreach (CellLayoutBox cellLayoutBox in worksheetUpdateContext.VisibleCellLayoutBoxes[viewportPane.ViewportPaneType])
				{
					if (cellLayoutBox.MergeState != CellMergeState.NonTopLeftCellInMergedRange)
					{
						IFill fill = worksheetUpdateContext.GetFill(cellLayoutBox.LongIndex);
						if (fill != CellPropertyDefinitions.FillProperty.DefaultValue)
						{
							GradientFill gradientFill = fill as GradientFill;
							if (gradientFill != null)
							{
								GradientFillRenderable renderable = new GradientFillRenderable
								{
									GradientFill = gradientFill,
									ColorScheme = worksheetUpdateContext.CurrentTheme.ColorScheme,
									BoundingRectangle = cellLayoutBox.BoundingRectangle
								};
								base.ContainerManager.Add(renderable, viewportPane.ViewportPaneType);
							}
						}
					}
				}
			}
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (ViewportPane viewportPane in worksheetUpdateContext.SheetViewport.ViewportPanes)
			{
				ViewportPaneType viewportPaneType = viewportPane.ViewportPaneType;
				foreach (IRenderable renderable in base.ContainerManager.GetElementsForViewportPane(viewportPaneType))
				{
					PatternFillRenderable patternFillRenderable = renderable as PatternFillRenderable;
					GradientFillRenderable gradientFillRenderable = renderable as GradientFillRenderable;
					if (patternFillRenderable != null)
					{
						this.TranslateAndScale(patternFillRenderable, viewportPaneType, worksheetUpdateContext);
					}
					else
					{
						if (gradientFillRenderable == null)
						{
							throw new InvalidCastException("Invalid fill renderable type!");
						}
						this.TranslateAndScale(gradientFillRenderable, viewportPaneType, worksheetUpdateContext);
					}
				}
			}
		}

		void TranslateAndScale(PatternFillRenderable patternFillRenderable, ViewportPaneType paneType, WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			List<Rect> list = new List<Rect>();
			foreach (Rect rect in patternFillRenderable.VisibleBoxes)
			{
				list.Add(base.TranslateAndScale(rect, paneType, worksheetUpdateContext));
			}
			patternFillRenderable.ClearVisibleBoxes();
			patternFillRenderable.AddVisibleBoxes(list);
			this.patternFillRenderer.Render(patternFillRenderable, paneType);
		}

		void TranslateAndScale(GradientFillRenderable gradientFillRenderable, ViewportPaneType paneType, WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			gradientFillRenderable.BoundingRectangle = base.TranslateAndScale(gradientFillRenderable.BoundingRectangle, paneType, worksheetUpdateContext);
			this.gradientFillRenderer.Render(gradientFillRenderable, paneType);
		}

		readonly IRenderer<PatternFillRenderable> patternFillRenderer;

		readonly IRenderer<GradientFillRenderable> gradientFillRenderer;
	}
}
