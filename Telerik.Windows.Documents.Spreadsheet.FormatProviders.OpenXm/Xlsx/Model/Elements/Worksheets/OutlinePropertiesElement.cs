using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class OutlinePropertiesElement : WorksheetElementBase
	{
		public OutlinePropertiesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.summaryBelow = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("summaryBelow", true, false));
			this.summaryRight = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("summaryRight", true, false));
		}

		public override string ElementName
		{
			get
			{
				return "outlinePr";
			}
		}

		public bool SummaryBelow
		{
			get
			{
				return this.summaryBelow.Value;
			}
			set
			{
				this.summaryBelow.Value = value;
			}
		}

		public bool SummaryRight
		{
			get
			{
				return this.summaryRight.Value;
			}
			set
			{
				this.summaryRight.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return !context.Worksheet.GroupingProperties.SummaryRowIsBelow || !context.Worksheet.GroupingProperties.SummaryColumnIsToRight;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.SummaryBelow = context.Worksheet.GroupingProperties.SummaryRowIsBelow;
			this.SummaryRight = context.Worksheet.GroupingProperties.SummaryColumnIsToRight;
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.Worksheet.GroupingProperties.SummaryRowIsBelow = this.SummaryBelow;
			context.Worksheet.GroupingProperties.SummaryColumnIsToRight = this.SummaryRight;
		}

		readonly BoolOpenXmlAttribute summaryBelow;

		readonly BoolOpenXmlAttribute summaryRight;
	}
}
