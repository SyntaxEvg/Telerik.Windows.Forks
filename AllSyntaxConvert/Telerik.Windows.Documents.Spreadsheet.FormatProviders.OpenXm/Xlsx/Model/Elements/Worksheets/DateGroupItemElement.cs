using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DateGroupItemElement : WorksheetElementBase
	{
		public DateGroupItemElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.year = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("year", true));
			this.month = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("month", false));
			this.day = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("day", false));
			this.hour = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("hour", false));
			this.minute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("minute", false));
			this.second = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("second", false));
			this.dateTimeGrouping = base.RegisterAttribute<ConvertedOpenXmlAttribute<DateTimeGroupingType>>(new ConvertedOpenXmlAttribute<DateTimeGroupingType>("dateTimeGrouping", null, XlsxConverters.DateTimeGroupingTypeConverter, DateTimeGroupingType.Year, false));
		}

		public override string ElementName
		{
			get
			{
				return "dateGroupItem";
			}
		}

		public DateTimeGroupingType DateTimeGrouping
		{
			get
			{
				return this.dateTimeGrouping.Value;
			}
			set
			{
				this.dateTimeGrouping.Value = value;
			}
		}

		public int Year
		{
			get
			{
				return this.year.Value;
			}
			set
			{
				this.year.Value = value;
			}
		}

		public int Month
		{
			get
			{
				return this.month.Value;
			}
			set
			{
				this.month.Value = value;
			}
		}

		public int Day
		{
			get
			{
				return this.day.Value;
			}
			set
			{
				this.day.Value = value;
			}
		}

		public int Hour
		{
			get
			{
				return this.hour.Value;
			}
			set
			{
				this.hour.Value = value;
			}
		}

		public int Minute
		{
			get
			{
				return this.minute.Value;
			}
			set
			{
				this.minute.Value = value;
			}
		}

		public int Second
		{
			get
			{
				return this.second.Value;
			}
			set
			{
				this.second.Value = value;
			}
		}

		public void CopyPropertiesFrom(DateGroupItemInfo dateFilter)
		{
			this.DateTimeGrouping = dateFilter.DateTimeGroupingType;
			this.Year = dateFilter.Year;
			if (dateFilter.Month != 0)
			{
				this.Month = dateFilter.Month;
			}
			if (dateFilter.Day != 0)
			{
				this.Day = dateFilter.Day;
			}
			if (dateFilter.Hour != 0)
			{
				this.Hour = dateFilter.Hour;
			}
			if (dateFilter.Minute != 0)
			{
				this.Minute = dateFilter.Minute;
			}
			if (dateFilter.Second != 0)
			{
				this.Second = dateFilter.Second;
			}
		}

		readonly ConvertedOpenXmlAttribute<DateTimeGroupingType> dateTimeGrouping;

		readonly IntOpenXmlAttribute year;

		readonly IntOpenXmlAttribute month;

		readonly IntOpenXmlAttribute day;

		readonly IntOpenXmlAttribute hour;

		readonly IntOpenXmlAttribute minute;

		readonly IntOpenXmlAttribute second;
	}
}
