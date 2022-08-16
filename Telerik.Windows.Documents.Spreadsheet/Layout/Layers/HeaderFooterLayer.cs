using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class HeaderFooterLayer : PrintWorksheetLayerBase
	{
		public HeaderFooterLayer(HeaderFooterSectionRenderer renderer)
		{
			Guard.ThrowExceptionIfNull<HeaderFooterSectionRenderer>(renderer, "renderer");
			this.sectionRenderer = renderer;
		}

		public override string Name
		{
			get
			{
				return "HeaderFooter";
			}
		}

		protected override void UpdateRenderOverride(PrintWorksheetRenderUpdateContext worksheetUpdateContext)
		{
			base.UpdateRenderOverride(worksheetUpdateContext);
			HeaderFooterRenderContext headerFooterContext = worksheetUpdateContext.HeaderFooterContext;
			HeaderFooterSettings headerFooterSettings = headerFooterContext.Worksheet.WorksheetPageSetup.HeaderFooterSettings;
			int pageNumber = headerFooterContext.PageNumber;
			HeaderFooterContent content;
			HeaderFooterContent content2;
			if (headerFooterSettings.DifferentFirstPage && pageNumber == 1)
			{
				content = headerFooterSettings.FirstPageHeader;
				content2 = headerFooterSettings.FirstPageFooter;
			}
			else if (headerFooterSettings.DifferentOddAndEvenPages && (pageNumber & 1) == 0)
			{
				content = headerFooterSettings.EvenPageHeader;
				content2 = headerFooterSettings.EvenPageFooter;
			}
			else
			{
				content = headerFooterSettings.Header;
				content2 = headerFooterSettings.Footer;
			}
			this.UpdateHeaderContentRender(content, headerFooterContext);
			this.UpdateFooterContentRender(content2, headerFooterContext);
		}

		void UpdateHeaderContentRender(HeaderFooterContent content, HeaderFooterRenderContext context)
		{
			this.UpdateHeaderSectionRender(content.LeftSection, TextAlignment.Left, context);
			this.UpdateHeaderSectionRender(content.CenterSection, TextAlignment.Center, context);
			this.UpdateHeaderSectionRender(content.RightSection, TextAlignment.Right, context);
		}

		void UpdateFooterContentRender(HeaderFooterContent content, HeaderFooterRenderContext context)
		{
			this.UpdateFooterSectionRender(content.LeftSection, TextAlignment.Left, context);
			this.UpdateFooterSectionRender(content.CenterSection, TextAlignment.Center, context);
			this.UpdateFooterSectionRender(content.RightSection, TextAlignment.Right, context);
		}

		void UpdateHeaderSectionRender(HeaderFooterSection headerSection, TextAlignment textAlignment, HeaderFooterRenderContext context)
		{
			if (!headerSection.IsEmpty)
			{
				double header = context.Worksheet.WorksheetPageSetup.Margins.Header;
				HeaderSectionRenderable renderable = new HeaderSectionRenderable
				{
					Top = header
				};
				HeaderFooterLayer.SetHeaderFooterSectionProperties(renderable, headerSection, textAlignment, context);
				this.sectionRenderer.Render(renderable, ViewportPaneType.Scrollable);
			}
		}

		void UpdateFooterSectionRender(HeaderFooterSection footerSection, TextAlignment textAlignment, HeaderFooterRenderContext context)
		{
			if (!footerSection.IsEmpty)
			{
				double bottom = context.Worksheet.WorksheetPageSetup.RotatedPageSize.Height - context.Worksheet.WorksheetPageSetup.Margins.Footer;
				FooterSectionRenderable renderable = new FooterSectionRenderable
				{
					Bottom = bottom
				};
				HeaderFooterLayer.SetHeaderFooterSectionProperties(renderable, footerSection, textAlignment, context);
				this.sectionRenderer.Render(renderable, ViewportPaneType.Scrollable);
			}
		}

		static void SetHeaderFooterSectionProperties(HeaderFooterSectionRenderable renderable, HeaderFooterSection headerSection, TextAlignment textAlignment, HeaderFooterRenderContext context)
		{
			renderable.Alignment = textAlignment;
			renderable.BlockWidth = HeaderFooterLayer.CalculateHeaderFooterSectionScaledWidth(context.Worksheet.WorksheetPageSetup);
			renderable.HeaderFooterContext = context;
			renderable.Left = HeaderFooterLayer.GetHeaderFooterLeft(context.Worksheet.WorksheetPageSetup);
			renderable.ScaleFactor = HeaderFooterLayer.GetHeaderFooterScale(context.Worksheet.WorksheetPageSetup);
			renderable.Section = headerSection;
		}

		static double CalculateHeaderFooterSectionScaledWidth(WorksheetPageSetup pageSetup)
		{
			double headerFooterLeft = HeaderFooterLayer.GetHeaderFooterLeft(pageSetup);
			double headerFooterRigth = HeaderFooterLayer.GetHeaderFooterRigth(pageSetup);
			double width = pageSetup.RotatedPageSize.Width;
			double num = width - headerFooterLeft - headerFooterRigth;
			double headerFooterScale = HeaderFooterLayer.GetHeaderFooterScale(pageSetup);
			return num / headerFooterScale;
		}

		static double GetHeaderFooterScale(WorksheetPageSetup pageSetup)
		{
			if (!pageSetup.HeaderFooterSettings.ScaleWithDocument)
			{
				return 1.0;
			}
			return pageSetup.ScaleFactor.Width;
		}

		static double GetHeaderFooterLeft(WorksheetPageSetup pageSetup)
		{
			PageMargins pageMargins = (pageSetup.HeaderFooterSettings.AlignWithPageMargins ? pageSetup.Margins : PageMargins.NormalMargins);
			return pageMargins.Left;
		}

		static double GetHeaderFooterRigth(WorksheetPageSetup pageSetup)
		{
			PageMargins pageMargins = (pageSetup.HeaderFooterSettings.AlignWithPageMargins ? pageSetup.Margins : PageMargins.NormalMargins);
			return pageMargins.Right;
		}

		readonly HeaderFooterSectionRenderer sectionRenderer;
	}
}
