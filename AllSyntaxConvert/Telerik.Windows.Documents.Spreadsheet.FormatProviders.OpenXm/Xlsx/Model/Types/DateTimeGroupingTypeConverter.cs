using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class DateTimeGroupingTypeConverter : IStringConverter<DateTimeGroupingType>
	{
		static DateTimeGroupingTypeConverter()
		{
			DateTimeGroupingTypeConverter.AddId("year", DateTimeGroupingType.Year);
			DateTimeGroupingTypeConverter.AddId("month", DateTimeGroupingType.Month);
			DateTimeGroupingTypeConverter.AddId("day", DateTimeGroupingType.Day);
			DateTimeGroupingTypeConverter.AddId("hour", DateTimeGroupingType.Hour);
			DateTimeGroupingTypeConverter.AddId("minute", DateTimeGroupingType.Minute);
			DateTimeGroupingTypeConverter.AddId("second", DateTimeGroupingType.Second);
			DateTimeGroupingTypeConverter.defaultId = DateTimeGroupingTypeConverter.dateTimeGroupingTypeToId[DateTimeGroupingType.Year];
		}

		public DateTimeGroupingType ConvertFromString(string value)
		{
			DateTimeGroupingType result;
			if (DateTimeGroupingTypeConverter.typeIdToDateTimeGroupingType.TryGetValue(value, out result))
			{
				return result;
			}
			return DateTimeGroupingType.Year;
		}

		public string ConvertToString(DateTimeGroupingType value)
		{
			string result;
			if (DateTimeGroupingTypeConverter.dateTimeGroupingTypeToId.TryGetValue(value, out result))
			{
				return result;
			}
			return DateTimeGroupingTypeConverter.defaultId;
		}

		static void AddId(string id, DateTimeGroupingType dateTimeGroupingType)
		{
			DateTimeGroupingTypeConverter.typeIdToDateTimeGroupingType[id] = dateTimeGroupingType;
			if (!DateTimeGroupingTypeConverter.dateTimeGroupingTypeToId.ContainsKey(dateTimeGroupingType))
			{
				DateTimeGroupingTypeConverter.dateTimeGroupingTypeToId[dateTimeGroupingType] = id;
			}
		}

		public const DateTimeGroupingType DefaultExportDateTimeGroupingType = DateTimeGroupingType.Year;

		static readonly Dictionary<DateTimeGroupingType, string> dateTimeGroupingTypeToId = new Dictionary<DateTimeGroupingType, string>();

		static readonly Dictionary<string, DateTimeGroupingType> typeIdToDateTimeGroupingType = new Dictionary<string, DateTimeGroupingType>();

		static readonly string defaultId;
	}
}
