using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class BreakElement : WorksheetElementBase
	{
		public BreakElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.id = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("id", 0, false));
			this.min = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("min", 0, false));
			this.max = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("max", 0, false));
			this.manual = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("man"));
		}

		public PageBreakInfo PageBreakInfo
		{
			get
			{
				return this.pageBreakInfo;
			}
		}

		public override string ElementName
		{
			get
			{
				return "brk";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		int Id
		{
			get
			{
				return this.id.Value;
			}
			set
			{
				this.id.Value = value;
			}
		}

		int Min
		{
			get
			{
				return this.min.Value;
			}
			set
			{
				this.min.Value = value;
			}
		}

		int Max
		{
			get
			{
				return this.max.Value;
			}
			set
			{
				this.max.Value = value;
			}
		}

		bool Manual
		{
			get
			{
				return this.manual.Value;
			}
			set
			{
				this.manual.Value = value;
			}
		}

		public void OnBeforeWrite(PageBreakInfo pageBreakInfo)
		{
			Guard.ThrowExceptionIfNull<PageBreakInfo>(pageBreakInfo, "pageBreakInfo");
			if (pageBreakInfo.Id != this.id.DefaultValue)
			{
				this.Id = pageBreakInfo.Id;
			}
			if (pageBreakInfo.Min != this.min.DefaultValue)
			{
				this.Min = pageBreakInfo.Min;
			}
			if (pageBreakInfo.Max != this.max.DefaultValue)
			{
				this.Max = pageBreakInfo.Max;
			}
			if (pageBreakInfo.Manual != this.manual.DefaultValue)
			{
				this.Manual = pageBreakInfo.Manual;
			}
		}

		protected override void ClearOverride()
		{
			this.pageBreakInfo = null;
			base.ClearOverride();
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			this.pageBreakInfo = new PageBreakInfo();
			if (this.id.HasValue)
			{
				this.pageBreakInfo.Id = this.Id;
			}
			if (this.min.HasValue)
			{
				this.pageBreakInfo.Min = this.Min;
			}
			if (this.max.HasValue)
			{
				this.pageBreakInfo.Max = this.Max;
			}
			if (this.manual.HasValue)
			{
				this.pageBreakInfo.Manual = this.Manual;
			}
		}

		readonly IntOpenXmlAttribute id;

		readonly IntOpenXmlAttribute min;

		readonly IntOpenXmlAttribute max;

		readonly BoolOpenXmlAttribute manual;

		PageBreakInfo pageBreakInfo;
	}
}
