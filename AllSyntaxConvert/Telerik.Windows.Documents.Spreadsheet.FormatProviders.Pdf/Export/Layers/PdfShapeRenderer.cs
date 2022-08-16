using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfShapeRenderer : PdfRenderer<ShapeRenderable>
	{
		public IPdfChartRenderer ChartRenderer { get; internal set; }

		public override void RenderOverride(ShapeRenderable renderable, ViewportPaneType paneType)
		{
			if (PdfShapeRenderer.IsZeroSized(renderable))
			{
				return;
			}
			if (renderable is ImageRenderable)
			{
				this.RenderImage(renderable as ImageRenderable);
				return;
			}
			if (renderable is ChartRenderable)
			{
				this.RenderChart(renderable as ChartRenderable);
			}
		}

		static bool IsZeroSized(ShapeRenderable renderable)
		{
			return renderable.Width == 0.0 || renderable.Height == 0.0;
		}

		void RenderChart(ChartRenderable chartRenderable)
		{
			IPdfChartRenderer pdfChartRenderer = this.ChartRenderer;
			if (pdfChartRenderer == null && ChartModelToImageConverterContainer.ChartModelToImageConverter != null)
			{
				pdfChartRenderer = new DefaultPdfChartToImageRenderer(ChartModelToImageConverterContainer.ChartModelToImageConverter);
			}
			if (pdfChartRenderer == null)
			{
				return;
			}
			double x = chartRenderable.TopLeft.X;
			double y = chartRenderable.TopLeft.Y;
			double scaleX = chartRenderable.Width / chartRenderable.Shape.Width;
			double scaleY = chartRenderable.Height / chartRenderable.Shape.Height;
			Matrix matrix = default(Matrix).TranslateMatrix(x, y).ScaleMatrixAt(scaleX, scaleY, x, y).MultiplyBy(base.Editor.Position.Matrix);
			base.Editor.Position = new MatrixPosition(matrix);
			FloatingChartShape chartShape = (chartRenderable.ShapeSource as ChartPdfSource).ChartShape;
			pdfChartRenderer.RenderChart(base.Editor, chartShape);
		}

		void RenderImage(ImageRenderable renderable)
		{
			BitmapShapeSource bitmapShapeSource = (BitmapShapeSource)renderable.ShapeSource;
			BitmapSource bitmap = bitmapShapeSource.Bitmap;
			double x = renderable.TopLeft.X;
			double y = renderable.TopLeft.Y;
			double scaleX = renderable.Width / (double)bitmap.PixelWidth;
			double scaleY = renderable.Height / (double)bitmap.PixelHeight;
			double centerX = x + renderable.Width / 2.0;
			double centerY = y + renderable.Height / 2.0;
			double scaleX2 = (double)(renderable.Shape.IsHorizontallyFlipped ? (-1) : 1);
			double scaleY2 = (double)(renderable.Shape.IsVerticallyFlipped ? (-1) : 1);
			double rotationAngle = renderable.Shape.RotationAngle;
			Matrix matrix = default(Matrix).TranslateMatrix(x, y).ScaleMatrixAt(scaleX, scaleY, x, y).ScaleMatrixAt(scaleX2, scaleY2, centerX, centerY)
				.RotateMatrixAt(rotationAngle, centerX, centerY)
				.MultiplyBy(base.Editor.Position.Matrix);
			base.Editor.Position = new MatrixPosition(matrix);
			base.Editor.DrawImage(new MemoryStream(renderable.Shape.ImageSource.Data));
		}
	}
}
