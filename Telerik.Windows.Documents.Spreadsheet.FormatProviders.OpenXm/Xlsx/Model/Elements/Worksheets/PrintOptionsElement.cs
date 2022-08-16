using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class PrintOptionsElement : WorksheetElementBase
	{
		public PrintOptionsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.horizontalCentered = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("horizontalCentered"));
			this.verticalCentered = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("verticalCentered"));
			this.headings = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("headings"));
			this.gridLines = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("gridLines"));
		}

		public bool HorizontalCentered
		{
			get
			{
				return this.horizontalCentered.Value;
			}
			set
			{
				this.horizontalCentered.Value = value;
			}
		}

		public bool VerticalCentered
		{
			get
			{
				return this.verticalCentered.Value;
			}
			set
			{
				this.verticalCentered.Value = value;
			}
		}

		public bool Headings
		{
			get
			{
				return this.headings.Value;
			}
			set
			{
				this.headings.Value = value;
			}
		}

		public bool GridLines
		{
			get
			{
				return this.gridLines.Value;
			}
			set
			{
				this.gridLines.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "printOptions";
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.ApplyPrintOptions(new PrintOptionsInfo
			{
				HorizontalCentered = this.HorizontalCentered,
				VerticalCentered = this.VerticalCentered,
				GridLines = this.GridLines,
				Headings = this.Headings
			});
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			PrintOptionsInfo printOptionsInfo = context.GetPrintOptionsInfo();
			if (printOptionsInfo.HorizontalCentered != this.horizontalCentered.DefaultValue)
			{
				this.HorizontalCentered = printOptionsInfo.HorizontalCentered;
			}
			if (printOptionsInfo.VerticalCentered != this.verticalCentered.DefaultValue)
			{
				this.VerticalCentered = printOptionsInfo.VerticalCentered;
			}
			if (printOptionsInfo.GridLines != this.gridLines.DefaultValue)
			{
				this.GridLines = printOptionsInfo.GridLines;
			}
			if (printOptionsInfo.Headings != this.headings.DefaultValue)
			{
				this.Headings = printOptionsInfo.Headings;
			}
		}

		readonly BoolOpenXmlAttribute horizontalCentered;

		readonly BoolOpenXmlAttribute verticalCentered;

		readonly BoolOpenXmlAttribute headings;

		readonly BoolOpenXmlAttribute gridLines;
	}
}
