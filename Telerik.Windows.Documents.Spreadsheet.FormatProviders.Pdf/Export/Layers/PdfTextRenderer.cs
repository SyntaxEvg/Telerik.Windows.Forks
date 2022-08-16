using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfTextRenderer : PdfRenderer<TextBlockRenderable>
	{
		public override void RenderOverride(TextBlockRenderable renderable, ViewportPaneType paneType)
		{
			Matrix matrix = default(Matrix).ScaleMatrix(renderable.ScaleFactor.Width, renderable.ScaleFactor.Height).TranslateMatrix(renderable.TopLeft.X * renderable.ScaleFactor.Width, renderable.TopLeft.Y * renderable.ScaleFactor.Height).TranslateMatrix(renderable.Padding.Left * renderable.ScaleFactor.Width, renderable.Padding.Top * renderable.ScaleFactor.Height)
				.MultiplyBy(base.Editor.Position.Matrix);
			base.Editor.Position = new MatrixPosition(matrix);
			Block block = new Block();
			block.GraphicProperties.FillColor = renderable.Foreground.ToPdfRgbColor();
			TextProperties textProperties = block.TextProperties;
			textProperties.TrySetFont(renderable.FontFamily, renderable.FontStyle, renderable.FontWeight);
			textProperties.FontSize = renderable.FontSize;
			textProperties.UnderlinePattern = renderable.TextDecorations.ToPdfTextDecorations();
			Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment horizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
			double wrappingWidth = double.PositiveInfinity;
			if (renderable.TextWrapping == TextWrapping.Wrap)
			{
				wrappingWidth = renderable.CellBox.Width - renderable.Padding.Left - renderable.Padding.Right;
				horizontalAlignment = renderable.TextAlignment.ToPdfHorizontalTextAlignment();
			}
			block.HorizontalAlignment = horizontalAlignment;
			Rect clip = renderable.Clip;
			if (!clip.IsEmpty)
			{
				clip = new Rect(clip.X - renderable.Padding.Left, clip.Y, clip.Width, clip.Height);
				base.Editor.PushTransformedClipping(clip);
				this.DrawText(renderable, block, wrappingWidth);
				base.Editor.PopClipping();
				return;
			}
			this.DrawText(renderable, block, wrappingWidth);
		}

		internal static void InsertText(Block parentBlock, string text)
		{
			string[] array = text.Split(PdfTextRenderer.environmentNewLineSplitter, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				parentBlock.InsertText(array[i]);
				if (i < array.Length - 1)
				{
					parentBlock.InsertLineBreak();
				}
			}
		}

		void DrawText(TextBlockRenderable renderable, Block block, double wrappingWidth)
		{
			if (renderable.Text == null)
			{
				using (IEnumerator<RunRenderable> enumerator = renderable.Runs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RunRenderable run = enumerator.Current;
						this.DrawRun(block, run);
					}
					goto IL_45;
				}
			}
			PdfTextRenderer.InsertText(block, renderable.Text);
			IL_45:
			base.Editor.DrawBlock(block, new Size(wrappingWidth, double.PositiveInfinity));
		}

		void DrawRun(Block block, RunRenderable run)
		{
			using (block.SaveProperties())
			{
				if (run.FontFamily != null)
				{
					block.TextProperties.TrySetFont(run.FontFamily);
				}
				if (run.FontSize != null)
				{
					block.TextProperties.FontSize = run.FontSize.Value;
				}
				if (run.Foreground != null)
				{
					block.GraphicProperties.FillColor = run.Foreground.Color.ToPdfRgbColor();
				}
				if (run.Foreground == null || !(run.Foreground.Color == Colors.Transparent))
				{
					PdfTextRenderer.InsertText(block, run.Text);
				}
			}
		}

		static readonly string[] environmentNewLineSplitter = new string[] { "\r\n", "\n", "\r" };
	}
}
