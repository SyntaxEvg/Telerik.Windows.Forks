using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfHeaderFooterSectionRenderer : HeaderFooterSectionRenderer, IPdfRenderer
	{
		public FixedContentEditor Editor { get; set; }

		protected override void PrepareForRendering(TextAlignment textAlignment)
		{
			Guard.ThrowExceptionIfNotNull<Block>(this.block, "block");
			this.block = new Block();
			this.block.GraphicProperties.IsFilled = true;
			this.block.GraphicProperties.IsStroked = false;
			this.block.HorizontalAlignment = textAlignment.ToPdfHorizontalTextAlignment();
		}

		protected override void OnAppendingTextFragmentOverride(string text)
		{
			this.block.TextProperties.TrySetFont(base.FontFamily, base.FontStyle, base.FontWeight);
			this.block.TextProperties.UnderlinePattern = base.UnderlineType.ToPdfUnderlinePattern();
			this.block.GraphicProperties.FillColor = base.FontColorActualValue.ToPdfRgbColor();
			this.block.TextProperties.UnderlineColor = this.block.GraphicProperties.FillColor;
			this.block.TextProperties.FontSize = base.FontSize;
			this.block.TextProperties.BaselineAlignment = base.BaselineAlignment.ToPdfBaselineAlignment();
			PdfTextRenderer.InsertText(this.block, text);
		}

		protected override double MeasureHeightBeforeRendering(double blockWidth)
		{
			return this.block.Measure(new Size(blockWidth, double.MaxValue)).Height;
		}

		protected override void RenderOverride(Matrix positionMatrix, double blockWidth)
		{
			using (this.Editor.SavePosition())
			{
				this.Editor.Position = new MatrixPosition(positionMatrix);
				this.Editor.DrawBlock(this.block, new Size(blockWidth, double.MaxValue));
			}
			this.block = null;
		}

		Block block;
	}
}
