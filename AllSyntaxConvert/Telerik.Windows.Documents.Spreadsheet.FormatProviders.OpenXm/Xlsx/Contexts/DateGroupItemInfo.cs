using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class DateGroupItemInfo
	{
		public DateGroupItemInfo(DateGroupItem item)
			: this(item.DateTimeGroupingType, item.Year, item.Month, item.Day, item.Hour, item.Minute, item.Second)
		{
		}

		public DateGroupItemInfo(DateTimeGroupingType dateTimeGroupingType, int year, int month, int day, int hour, int minute, int second)
		{
			this.DateTimeGroupingType = dateTimeGroupingType;
			this.Year = year;
			this.Month = month;
			this.Day = day;
			this.Hour = hour;
			this.Minute = minute;
			this.Second = second;
		}

		public DateTimeGroupingType DateTimeGroupingType { get; set; }

		public int Year { get; set; }

		public int Month { get; set; }

		public int Day { get; set; }

		public int Hour { get; set; }

		public int Minute { get; set; }

		public int Second { get; set; }
	}
}
