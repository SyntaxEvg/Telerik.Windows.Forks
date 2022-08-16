using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class PageSetupElement : WorksheetElementBase
	{
		public PageSetupElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.paperSize = base.RegisterAttribute<ConvertedOpenXmlAttribute<PaperTypes>>(new ConvertedOpenXmlAttribute<PaperTypes>("paperSize", null, XlsxConverters.PaperTypesConverter, PaperTypes.Letter, false));
			this.orientation = base.RegisterAttribute<ConvertedOpenXmlAttribute<PageOrientation>>(new ConvertedOpenXmlAttribute<PageOrientation>("orientation", null, Converters.PageOrientationConverter, PageOrientation.Portrait, false));
			this.pageOrder = base.RegisterAttribute<ConvertedOpenXmlAttribute<PageOrder>>(new ConvertedOpenXmlAttribute<PageOrder>("pageOrder", null, XlsxConverters.PageOrderConverter, PageOrder.DownThenOver, false));
			this.errors = base.RegisterAttribute<ConvertedOpenXmlAttribute<ErrorsPrintStyle>>(new ConvertedOpenXmlAttribute<ErrorsPrintStyle>("errors", null, XlsxConverters.ErrorsPrintStyleConverter, ErrorsPrintStyle.AsDisplayed, false));
			this.firstPageNumber = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("firstPageNumber", 1, false));
			this.fitToHeight = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("fitToHeight", 1, false));
			this.fitToWidth = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("fitToWidth", 1, false));
			this.scale = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("scale", 100, false));
		}

		public PaperTypes PaperSize
		{
			get
			{
				return this.paperSize.Value;
			}
			set
			{
				this.paperSize.Value = value;
			}
		}

		public PageOrientation Orientation
		{
			get
			{
				return this.orientation.Value;
			}
			set
			{
				this.orientation.Value = value;
			}
		}

		public PageOrder PageOrder
		{
			get
			{
				return this.pageOrder.Value;
			}
			set
			{
				this.pageOrder.Value = value;
			}
		}

		public ErrorsPrintStyle Errors
		{
			get
			{
				return this.errors.Value;
			}
			set
			{
				this.errors.Value = value;
			}
		}

		public int FirstPageNumber
		{
			get
			{
				return this.firstPageNumber.Value;
			}
			set
			{
				this.firstPageNumber.Value = value;
			}
		}

		public int FitToHeight
		{
			get
			{
				return this.fitToHeight.Value;
			}
			set
			{
				this.fitToHeight.Value = value;
			}
		}

		public int FitToWidth
		{
			get
			{
				return this.fitToWidth.Value;
			}
			set
			{
				this.fitToWidth.Value = value;
			}
		}

		public int Scale
		{
			get
			{
				return this.scale.Value;
			}
			set
			{
				this.scale.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "pageSetup";
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			PageSetupInfo pageSetupInfo = new PageSetupInfo();
			pageSetupInfo.PaperType = this.PaperSize;
			pageSetupInfo.PageOrientation = this.Orientation;
			if (this.pageOrder.HasValue)
			{
				pageSetupInfo.PageOrder = this.PageOrder;
			}
			if (this.errors.HasValue)
			{
				pageSetupInfo.Errors = this.Errors;
			}
			if (this.firstPageNumber.HasValue)
			{
				pageSetupInfo.FirstPageNumber = new int?(this.FirstPageNumber);
			}
			if (this.fitToHeight.HasValue)
			{
				pageSetupInfo.FitToHeight = this.FitToHeight;
			}
			if (this.fitToWidth.HasValue)
			{
				pageSetupInfo.FitToWidth = this.FitToWidth;
			}
			pageSetupInfo.Scale = (int)SpreadsheetHelper.GetNearestValidValue<double>((double)this.Scale, SpreadsheetDefaultValues.MinScaleFactor * 100.0, SpreadsheetDefaultValues.MaxScaleFactor * 100.0);
			context.ApplyPageSetup(pageSetupInfo);
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			PageSetupInfo pageSetupInfo = context.GetPageSetupInfo();
			bool flag = false;
			if (pageSetupInfo.PageOrder != this.pageOrder.DefaultValue)
			{
				this.PageOrder = pageSetupInfo.PageOrder;
				flag = true;
			}
			if (pageSetupInfo.Errors != this.errors.DefaultValue)
			{
				this.Errors = pageSetupInfo.Errors;
				flag = true;
			}
			if (pageSetupInfo.FirstPageNumber != null && pageSetupInfo.FirstPageNumber != this.firstPageNumber.DefaultValue)
			{
				this.FirstPageNumber = pageSetupInfo.FirstPageNumber.Value;
				flag = true;
			}
			if (pageSetupInfo.FitToHeight != this.fitToHeight.DefaultValue)
			{
				this.FitToHeight = pageSetupInfo.FitToHeight;
				flag = true;
			}
			if (pageSetupInfo.FitToWidth != this.fitToWidth.DefaultValue)
			{
				this.FitToWidth = pageSetupInfo.FitToWidth;
				flag = true;
			}
			if (pageSetupInfo.Scale != this.scale.DefaultValue)
			{
				this.Scale = pageSetupInfo.Scale;
				flag = true;
			}
			if (flag || pageSetupInfo.PaperType != this.paperSize.DefaultValue)
			{
				this.PaperSize = pageSetupInfo.PaperType;
				flag = true;
			}
			if (flag || pageSetupInfo.PageOrientation != this.orientation.DefaultValue)
			{
				this.Orientation = pageSetupInfo.PageOrientation;
			}
		}

		readonly ConvertedOpenXmlAttribute<PaperTypes> paperSize;

		readonly ConvertedOpenXmlAttribute<PageOrientation> orientation;

		readonly ConvertedOpenXmlAttribute<PageOrder> pageOrder;

		readonly ConvertedOpenXmlAttribute<ErrorsPrintStyle> errors;

		readonly IntOpenXmlAttribute firstPageNumber;

		readonly IntOpenXmlAttribute fitToHeight;

		readonly IntOpenXmlAttribute fitToWidth;

		readonly IntOpenXmlAttribute scale;
	}
}
