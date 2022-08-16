using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class HeaderFooterSectionRenderer : HeaderFooterSectionTextRendererBase, IRenderer<HeaderSectionRenderable>, IRenderer<FooterSectionRenderable>
	{
		protected sealed override Sheet SheetContext
		{
			get
			{
				return this.renderable.HeaderFooterContext.Worksheet;
			}
		}

		public void Render(HeaderSectionRenderable renderable, ViewportPaneType paneType)
		{
			this.Render<HeaderSectionRenderable>(renderable, new Func<HeaderSectionRenderable, Matrix>(this.CalculateMatrix));
		}

		public void Render(FooterSectionRenderable renderable, ViewportPaneType paneType)
		{
			this.Render<FooterSectionRenderable>(renderable, new Func<FooterSectionRenderable, Matrix>(this.CalculateMatrix));
		}

		public static string GetDateContent(DateTime dateTime)
		{
			return dateTime.ToShortDateString();
		}

		public static string GetTimeContent(DateTime dateTime)
		{
			return dateTime.ToShortTimeString();
		}

		protected abstract void PrepareForRendering(TextAlignment alignment);

		protected abstract double MeasureHeightBeforeRendering(double blockWidth);

		protected abstract void RenderOverride(Matrix positionMarix, double blockWidth);

		protected sealed override void OnAppendingAmpersandOverride()
		{
			this.OnAppendingTextFragmentOverride("&");
		}

		protected sealed override void OnAppendingFileNameOverride()
		{
			this.OnAppendingTextFragmentOverride(this.renderable.HeaderFooterContext.Worksheet.Workbook.Name);
		}

		protected sealed override void OnAppendingPageNumberOverride()
		{
			this.OnAppendingTextFragmentOverride(this.renderable.HeaderFooterContext.PageNumber.ToString());
		}

		protected sealed override void OnAppendingAddToPageNumberOverride()
		{
		}

		protected sealed override void OnAppendingSubstractFromPageNumberOverride()
		{
		}

		protected sealed override void OnAppendingNumberOfPagesOverride()
		{
			this.OnAppendingTextFragmentOverride(this.renderable.HeaderFooterContext.NumberOfPages.ToString());
		}

		protected sealed override void OnAppendingSheetNameOverride()
		{
			this.OnAppendingTextFragmentOverride(this.renderable.HeaderFooterContext.Worksheet.Name);
		}

		protected sealed override void OnAppendingFilePathOverride()
		{
		}

		protected sealed override void OnAppendingPictureOverride()
		{
		}

		protected sealed override void OnAppendingDateOverride()
		{
			string dateContent = HeaderFooterSectionRenderer.GetDateContent(this.renderable.HeaderFooterContext.DateTime);
			this.OnAppendingTextFragmentOverride(dateContent);
		}

		protected sealed override void OnAppendingTimeOverride()
		{
			string timeContent = HeaderFooterSectionRenderer.GetTimeContent(this.renderable.HeaderFooterContext.DateTime);
			this.OnAppendingTextFragmentOverride(timeContent);
		}

		void Render<T>(T renderable, Func<T, Matrix> calculateMatrix) where T : HeaderFooterSectionRenderable
		{
			Guard.ThrowExceptionIfNull<T>(renderable, "renderable");
			Guard.ThrowExceptionIfNotNull<HeaderFooterSectionRenderable>(this.renderable, "this.renderable");
			this.renderable = renderable;
			this.PrepareForRendering(this.renderable.Alignment);
			base.Render(this.renderable.Section.Text);
			Matrix positionMarix = calculateMatrix(renderable);
			this.RenderOverride(positionMarix, this.renderable.BlockWidth);
			this.renderable = null;
		}

		Matrix CalculateMatrix(HeaderSectionRenderable renderable)
		{
			return this.CalculateMatrix(renderable.Left, renderable.Top, renderable.ScaleFactor);
		}

		Matrix CalculateMatrix(FooterSectionRenderable renderable)
		{
			double num = this.MeasureHeightBeforeRendering(renderable.BlockWidth);
			double num2 = num * renderable.ScaleFactor;
			double top = renderable.Bottom - num2;
			return this.CalculateMatrix(renderable.Left, top, renderable.ScaleFactor);
		}

		Matrix CalculateMatrix(double left, double top, double scale)
		{
			return new Matrix(scale, 0.0, 0.0, scale, left, top);
		}

		HeaderFooterSectionRenderable renderable;
	}
}
