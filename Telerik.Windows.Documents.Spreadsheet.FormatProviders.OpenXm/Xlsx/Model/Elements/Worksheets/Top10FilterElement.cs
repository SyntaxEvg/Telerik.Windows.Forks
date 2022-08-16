using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class Top10FilterElement : FilterElementBase
	{
		public Top10FilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.fitlerValue = base.RegisterAttribute<double>("filterVal", false);
			this.percent = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("percent"));
			this.top = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("top", true, false));
			this.value = base.RegisterAttribute<double>("val", true);
		}

		public override string ElementName
		{
			get
			{
				return "top10";
			}
		}

		public double FitlerValue
		{
			get
			{
				return this.fitlerValue.Value;
			}
			set
			{
				this.fitlerValue.Value = value;
			}
		}

		public bool Percent
		{
			get
			{
				return this.percent.Value;
			}
			set
			{
				this.percent.Value = value;
			}
		}

		public bool Top
		{
			get
			{
				return this.top.Value;
			}
			set
			{
				this.top.Value = value;
			}
		}

		public double Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		internal override IFilterInfo GetInfo()
		{
			return new Top10FilterInfo
			{
				Value = this.Value,
				Top = this.Top,
				Percent = this.Percent
			};
		}

		internal override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			Top10FilterInfo top10FilterInfo = context.GetTop10FilterInfo(columnId);
			this.Value = top10FilterInfo.Value;
			if (top10FilterInfo.Top != this.top.DefaultValue)
			{
				this.Top = top10FilterInfo.Top;
			}
			if (top10FilterInfo.Percent != this.percent.DefaultValue)
			{
				this.Percent = top10FilterInfo.Percent;
			}
		}

		readonly OpenXmlAttribute<double> fitlerValue;

		readonly BoolOpenXmlAttribute percent;

		readonly BoolOpenXmlAttribute top;

		readonly OpenXmlAttribute<double> value;
	}
}
