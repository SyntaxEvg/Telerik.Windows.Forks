using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class CellValuesLayer : CellBasedLayerBase
	{
		public CellValuesLayer(IRenderer<TextBlockRenderable> textRenderer)
		{
			this.textRenderer = textRenderer;
		}

		public override string Name
		{
			get
			{
				return "CellValues";
			}
		}

		internal IRenderer<TextBlockRenderable> TextRenderer
		{
			get
			{
				return this.textRenderer;
			}
		}

		internal static bool ShouldCreateElementForBox(CellLayoutBox box, WorksheetRenderUpdateContext updateContext)
		{
			CellValueFormat cellFormat = updateContext.GetCellFormat(box.LongIndex);
			string asStringOrNull = updateContext.GetCellValue(box.LongIndex).GetAsStringOrNull(cellFormat);
			return !string.IsNullOrEmpty(asStringOrNull);
		}

		protected override IRenderable CreateRenderableElementForBox(ViewportPaneType viewportPaneType, CellLayoutBox box, WorksheetRenderUpdateContext updateContext)
		{
			if (!CellValuesLayer.ShouldCreateElementForBox(box, updateContext))
			{
				return null;
			}
			return CellValuesLayer.CreateTextBlockRenderableForBox(box, updateContext);
		}

		internal static TextBlockRenderable CreateTextBlockRenderableForBox(CellLayoutBox box, WorksheetRenderUpdateContext updateContext)
		{
			TextBlockRenderable textBlockRenderable = new TextBlockRenderable();
			ICellValue cellValue = updateContext.GetCellValue(box.LongIndex);
			CellValueFormat cellFormat = updateContext.GetCellFormat(box.LongIndex);
			CellValuesLayer.SetTextBlockContent(textBlockRenderable, cellValue, cellFormat, box, updateContext);
			FontProperties fontProperties = updateContext.GetFontProperties(box.LongIndex);
			RadHorizontalAlignment horizontalAlignment = updateContext.GetHorizontalAlignment(box.LongIndex);
			textBlockRenderable.FontFamily = fontProperties.FontFamily;
			textBlockRenderable.FontSize = fontProperties.FontSize;
			textBlockRenderable.FontWeight = (fontProperties.IsBold ? FontWeights.Bold : FontWeights.Normal);
			textBlockRenderable.FontStyle = (fontProperties.IsItalic ? FontStyles.Italic : FontStyles.Normal);
			textBlockRenderable.TextDecorations = ((fontProperties.Underline != UnderlineType.None) ? TextDecorations.Underline : null);
			TextAlignment textAlignment = CellBoxRenderHelper.GetHorizontalAlignment(horizontalAlignment, cellValue);
			if (textAlignment == TextAlignment.Justify)
			{
				textAlignment = TextAlignment.Left;
			}
			textBlockRenderable.TextAlignment = textAlignment;
			bool isWrapped = updateContext.GetIsWrapped(box.LongIndex);
			if (isWrapped)
			{
				textBlockRenderable.TextWrapping = TextWrapping.Wrap;
			}
			else
			{
				textBlockRenderable.TextWrapping = TextWrapping.NoWrap;
			}
			Thickness thickness = CellBoxRenderHelper.CalculateIndentPadding(box.LongIndex, updateContext);
			Thickness padding = new Thickness(thickness.Left, 0.0, thickness.Right, 0.0);
			textBlockRenderable.Padding = padding;
			return textBlockRenderable;
		}

		static void SetTextBlockContent(TextBlockRenderable textBlockRenderable, ICellValue cellValue, CellValueFormat format, CellLayoutBox cellBox, WorksheetRenderUpdateContext updateContext)
		{
			double width = cellBox.Width;
			FontProperties fontProperties = updateContext.GetFontProperties(cellBox.LongIndex);
			textBlockRenderable.Foreground = fontProperties.ForeColor.GetActualValue(updateContext.CurrentTheme);
			try
			{
				FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
				if (formulaCellValue != null)
				{
					cellValue = formulaCellValue.GetResultValueAsCellValue();
				}
				CellValueFormatResult formatResult = format.GetFormatResult(cellValue);
				IEnumerable<CellValueFormatResultItem> infos = formatResult.Infos;
				string infosText = formatResult.InfosText;
				int num = infos.Count<CellValueFormatResultItem>();
				bool containsExpandableInfo = formatResult.ContainsExpandableInfo;
				Size cellContentSize = updateContext.GetCellContentSize(cellBox);
				if (CellBoxRenderHelper.ShouldDisplayContentAsSharps(cellBox.LongIndex, updateContext))
				{
					textBlockRenderable.Text = FormatHelper.ExpandSymbol("#", width, fontProperties);
				}
				else
				{
					if (formatResult.Foreground != null)
					{
						textBlockRenderable.Foreground = formatResult.Foreground.Value;
					}
					textBlockRenderable.ShouldFitToCellSize = false;
					if (num == 1)
					{
						textBlockRenderable.Text = infosText;
						if (formatResult.Foreground != null)
						{
							textBlockRenderable.Foreground = formatResult.Foreground.Value;
						}
					}
					else
					{
						RunRenderable runRenderable = null;
						textBlockRenderable.ClearRuns();
						foreach (CellValueFormatResultItem cellValueFormatResultItem in infos)
						{
							RunRenderable runRenderable2 = new RunRenderable();
							runRenderable2.Text = cellValueFormatResultItem.Text;
							if (cellValueFormatResultItem.IsTransparent)
							{
								runRenderable2.Foreground = new SolidColorBrush(Colors.Transparent);
							}
							textBlockRenderable.AddRun(runRenderable2);
							if (cellValueFormatResultItem.ShouldExpand)
							{
								runRenderable = runRenderable2;
							}
							if (!cellValueFormatResultItem.ApplyFormat && containsExpandableInfo)
							{
								textBlockRenderable.AddRun(new RunRenderable
								{
									Text = FormatHelper.InvisibleDivider,
									FontSize = new double?(FormatHelper.InvisibleDividerFontsize)
								});
							}
						}
						if (runRenderable != null)
						{
							double width2 = RadTextMeasurer.Measure(runRenderable.Text, fontProperties, null).Size.Width;
							double num2 = Math.Max(0.0, width - (cellContentSize.Width - width2 + CellBorder.Default.Thickness));
							runRenderable.Text = FormatHelper.ExpandSymbol(runRenderable.Text, num2, fontProperties);
							double num3 = num2 - RadTextMeasurer.Measure(runRenderable.Text, fontProperties, null).Size.Width;
							num3 = Math.Round(num3, 4);
							if (num3 > 0.0)
							{
								RunRenderable run = RenderHelper.CreateSpaceRun(num3);
								textBlockRenderable.InsertRunAfter(runRenderable, run);
							}
							textBlockRenderable.ShouldFitToCellSize = true;
						}
					}
				}
			}
			finally
			{
				textBlockRenderable.CellValue = cellValue;
				textBlockRenderable.CellValueFormat = format;
				textBlockRenderable.CellBox = cellBox;
			}
		}

		protected override void TranslateAndScale(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			ScaleTransform scaleTransform = new ScaleTransform
			{
				ScaleX = worksheetUpdateContext.ScaleFactor.Width,
				ScaleY = worksheetUpdateContext.ScaleFactor.Height
			};
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in worksheetUpdateContext.VisibleCellLayoutBoxes)
			{
				ViewportPaneType key = keyValuePair.Key;
				foreach (IRenderable renderable in base.ContainerManager.GetElementsForViewportPane(key))
				{
					TextBlockRenderable textBlockRenderable = (TextBlockRenderable)renderable;
					CellLayoutBox cellBox = textBlockRenderable.CellBox;
					Rect rect = worksheetUpdateContext.GetActualBoundingRectangleByContentAlignment(cellBox);
					Rect cellClipping = worksheetUpdateContext.GetCellClipping(cellBox);
					if (textBlockRenderable.ShouldFitToCellSize)
					{
						rect.X = cellBox.BoundingRectangle.Left;
						rect.Width = cellBox.BoundingRectangle.Width;
						cellClipping.X = 0.0;
						cellClipping.Width = rect.Width;
					}
					textBlockRenderable.Clip = cellClipping;
					rect = base.Translate(rect, keyValuePair.Key, worksheetUpdateContext);
					textBlockRenderable.TopLeft = new Point(rect.Left, rect.Top);
					textBlockRenderable.ScaleFactor = worksheetUpdateContext.ScaleFactor;
					this.textRenderer.Render(textBlockRenderable, key);
				}
			}
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair2 in worksheetUpdateContext.VisibleCellLayoutBoxes)
			{
				foreach (CellLayoutBox cellLayoutBox in keyValuePair2.Value)
				{
					long longIndex = cellLayoutBox.LongIndex;
					HyperlinkInfo hyperlinkInfo;
					if (worksheetUpdateContext.CellIndexToHyperlinkInfo.TryGetValue(longIndex, out hyperlinkInfo))
					{
						Rect value = worksheetUpdateContext.CellIndexToHyperlinkArea[longIndex];
						Point point = new Point(value.Left, value.Top);
						point = base.Translate(point, keyValuePair2.Key, worksheetUpdateContext);
						point = scaleTransform.Transform(point);
						Point point2 = scaleTransform.Transform(new Point(value.Width, value.Height));
						value = new Rect(point.X, point.Y, point2.X, point2.Y);
						worksheetUpdateContext.CellIndexToHyperlinkArea[longIndex] = value;
					}
				}
			}
		}

		readonly IRenderer<TextBlockRenderable> textRenderer;
	}
}
