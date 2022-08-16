using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class DateGroupItem
	{
		public DateTimeGroupingType DateTimeGroupingType
		{
			get
			{
				return this.dateTimeGroupingType;
			}
		}

		public int Year
		{
			get
			{
				return this.year;
			}
		}

		public int Month
		{
			get
			{
				return this.month;
			}
		}

		public int Day
		{
			get
			{
				return this.day;
			}
		}

		public int Hour
		{
			get
			{
				return this.hour;
			}
		}

		public int Minute
		{
			get
			{
				return this.minute;
			}
		}

		public int Second
		{
			get
			{
				return this.second;
			}
		}

		public DateGroupItem(int year)
			: this(DateTimeGroupingType.Year, year, 0, 0, 0, 0, 0)
		{
		}

		public DateGroupItem(int year, int month)
			: this(DateTimeGroupingType.Month, year, month, 0, 0, 0, 0)
		{
		}

		public DateGroupItem(int year, int month, int day)
			: this(DateTimeGroupingType.Day, year, month, day, 0, 0, 0)
		{
		}

		public DateGroupItem(int year, int month, int day, int hour)
			: this(DateTimeGroupingType.Hour, year, month, day, hour, 0, 0)
		{
		}

		public DateGroupItem(int year, int month, int day, int hour, int minute)
			: this(DateTimeGroupingType.Minute, year, month, day, hour, minute, 0)
		{
		}

		public DateGroupItem(int year, int month, int day, int hour, int minute, int seconds)
			: this(DateTimeGroupingType.Second, year, month, day, hour, minute, seconds)
		{
		}

		internal DateGroupItem(DateTimeGroupingType dateTimeGroupingType, int year, int month, int day, int hour, int minute, int second)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, year, "year");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 12, month, "month");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 31, day, "day");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 23, hour, "hour");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 59, minute, "minute");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 59, second, "second");
			this.dateTimeGroupingType = dateTimeGroupingType;
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
		}

		public bool DateSatisfiesItem(DateTime date)
		{
			return date.Year == this.year && (this.dateTimeGroupingType < DateTimeGroupingType.Month || date.Month == this.month) && (this.dateTimeGroupingType < DateTimeGroupingType.Day || date.Day == this.day) && (this.dateTimeGroupingType < DateTimeGroupingType.Hour || date.Hour == this.hour) && (this.dateTimeGroupingType < DateTimeGroupingType.Minute || date.Minute == this.minute) && (this.dateTimeGroupingType < DateTimeGroupingType.Second || date.Second == this.second);
		}

		public override bool Equals(object obj)
		{
			DateGroupItem dateGroupItem = obj as DateGroupItem;
			return dateGroupItem != null && (this.Year.Equals(dateGroupItem.Year) && this.Month.Equals(dateGroupItem.Month) && this.Day.Equals(dateGroupItem.Day) && this.Hour.Equals(dateGroupItem.Hour) && this.Minute.Equals(dateGroupItem.Minute) && this.Second.Equals(dateGroupItem.Second)) && this.DateTimeGroupingType.Equals(dateGroupItem.DateTimeGroupingType);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Year.GetHashCode(), this.Month.GetHashCode(), this.Day.GetHashCode(), this.Hour.GetHashCode(), this.Minute.GetHashCode(), this.Second.GetHashCode(), this.DateTimeGroupingType.GetHashCode());
		}

		readonly DateTimeGroupingType dateTimeGroupingType;

		readonly int year;

		readonly int month;

		readonly int day;

		readonly int hour;

		readonly int minute;

		readonly int second;
	}
}
