using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class DynamicFilterTypeConverter : IStringConverter<DynamicFilterType>
	{
		static DynamicFilterTypeConverter()
		{
			DynamicFilterTypeConverter.AddId("null", DynamicFilterType.None);
			DynamicFilterTypeConverter.AddId("aboveAverage", DynamicFilterType.AboveAverage);
			DynamicFilterTypeConverter.AddId("belowAverage", DynamicFilterType.BelowAverage);
			DynamicFilterTypeConverter.AddId("tomorrow", DynamicFilterType.Tomorrow);
			DynamicFilterTypeConverter.AddId("today", DynamicFilterType.Today);
			DynamicFilterTypeConverter.AddId("yesterday", DynamicFilterType.Yesterday);
			DynamicFilterTypeConverter.AddId("nextWeek", DynamicFilterType.NextWeek);
			DynamicFilterTypeConverter.AddId("thisWeek", DynamicFilterType.ThisWeek);
			DynamicFilterTypeConverter.AddId("lastWeek", DynamicFilterType.LastWeek);
			DynamicFilterTypeConverter.AddId("nextMonth", DynamicFilterType.NextMonth);
			DynamicFilterTypeConverter.AddId("thisMonth", DynamicFilterType.ThisMonth);
			DynamicFilterTypeConverter.AddId("lastMonth", DynamicFilterType.LastMonth);
			DynamicFilterTypeConverter.AddId("nextQuarter", DynamicFilterType.NextQuarter);
			DynamicFilterTypeConverter.AddId("thisQuarter", DynamicFilterType.ThisQuarter);
			DynamicFilterTypeConverter.AddId("lastQuarter", DynamicFilterType.LastQuarter);
			DynamicFilterTypeConverter.AddId("nextYear", DynamicFilterType.NextYear);
			DynamicFilterTypeConverter.AddId("thisYear", DynamicFilterType.ThisYear);
			DynamicFilterTypeConverter.AddId("lastYear", DynamicFilterType.LastYear);
			DynamicFilterTypeConverter.AddId("yearToDate", DynamicFilterType.YearToDate);
			DynamicFilterTypeConverter.AddId("Q1", DynamicFilterType.Quarter1);
			DynamicFilterTypeConverter.AddId("Q2", DynamicFilterType.Quarter2);
			DynamicFilterTypeConverter.AddId("Q3", DynamicFilterType.Quarter3);
			DynamicFilterTypeConverter.AddId("Q4", DynamicFilterType.Quarter4);
			DynamicFilterTypeConverter.AddId("M1", DynamicFilterType.January);
			DynamicFilterTypeConverter.AddId("M2", DynamicFilterType.February);
			DynamicFilterTypeConverter.AddId("M3", DynamicFilterType.March);
			DynamicFilterTypeConverter.AddId("M4", DynamicFilterType.April);
			DynamicFilterTypeConverter.AddId("M5", DynamicFilterType.May);
			DynamicFilterTypeConverter.AddId("M6", DynamicFilterType.June);
			DynamicFilterTypeConverter.AddId("M7", DynamicFilterType.July);
			DynamicFilterTypeConverter.AddId("M8", DynamicFilterType.August);
			DynamicFilterTypeConverter.AddId("M9", DynamicFilterType.September);
			DynamicFilterTypeConverter.AddId("M10", DynamicFilterType.October);
			DynamicFilterTypeConverter.AddId("M11", DynamicFilterType.November);
			DynamicFilterTypeConverter.AddId("M12", DynamicFilterType.December);
			DynamicFilterTypeConverter.defaultId = DynamicFilterTypeConverter.dynamicFilterTypeToId[DynamicFilterType.None];
		}

		public DynamicFilterType ConvertFromString(string value)
		{
			DynamicFilterType result;
			if (DynamicFilterTypeConverter.filterIdToDynamicFilterType.TryGetValue(value, out result))
			{
				return result;
			}
			return DynamicFilterType.None;
		}

		public string ConvertToString(DynamicFilterType value)
		{
			string result;
			if (DynamicFilterTypeConverter.dynamicFilterTypeToId.TryGetValue(value, out result))
			{
				return result;
			}
			return DynamicFilterTypeConverter.defaultId;
		}

		static void AddId(string id, DynamicFilterType dynamicFilterType)
		{
			DynamicFilterTypeConverter.filterIdToDynamicFilterType[id] = dynamicFilterType;
			if (!DynamicFilterTypeConverter.dynamicFilterTypeToId.ContainsKey(dynamicFilterType))
			{
				DynamicFilterTypeConverter.dynamicFilterTypeToId[dynamicFilterType] = id;
			}
		}

		public const DynamicFilterType DefaultExportDynamicFilterType = DynamicFilterType.None;

		static readonly Dictionary<DynamicFilterType, string> dynamicFilterTypeToId = new Dictionary<DynamicFilterType, string>();

		static readonly Dictionary<string, DynamicFilterType> filterIdToDynamicFilterType = new Dictionary<string, DynamicFilterType>();

		static readonly string defaultId;
	}
}
