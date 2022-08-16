using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class DataValidationRuleTypeConverter : IStringConverter<DataValidationRuleType>
	{
		static DataValidationRuleTypeConverter()
		{
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.Custom, DataValidationRuleType.Custom);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.Date, DataValidationRuleType.Date);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.Decimal, DataValidationRuleType.Decimal);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.List, DataValidationRuleType.List);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.None, DataValidationRuleType.None);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.TextLength, DataValidationRuleType.TextLength);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.Time, DataValidationRuleType.Time);
			DataValidationRuleTypeConverter.AddId(DataValidationRuleTypeConverter.Whole, DataValidationRuleType.Whole);
			DataValidationRuleTypeConverter.defaultId = DataValidationRuleTypeConverter.dataValidationRuleTypeToId[DataValidationRuleType.None];
		}

		public DataValidationRuleType ConvertFromString(string value)
		{
			DataValidationRuleType result;
			if (DataValidationRuleTypeConverter.typeIdToDataValidationRuleType.TryGetValue(value, out result))
			{
				return result;
			}
			return DataValidationRuleType.None;
		}

		public string ConvertToString(DataValidationRuleType value)
		{
			string result;
			if (DataValidationRuleTypeConverter.dataValidationRuleTypeToId.TryGetValue(value, out result))
			{
				return result;
			}
			return DataValidationRuleTypeConverter.defaultId;
		}

		static void AddId(string id, DataValidationRuleType dataValidationRuleType)
		{
			DataValidationRuleTypeConverter.typeIdToDataValidationRuleType[id] = dataValidationRuleType;
			if (!DataValidationRuleTypeConverter.dataValidationRuleTypeToId.ContainsKey(dataValidationRuleType))
			{
				DataValidationRuleTypeConverter.dataValidationRuleTypeToId[dataValidationRuleType] = id;
			}
		}

		public const DataValidationRuleType DefaultExportDataValidationRuleType = DataValidationRuleType.None;

		static readonly string Custom = "custom";

		static readonly string Date = "date";

		static readonly string Decimal = "decimal";

		static readonly string List = "list";

		static readonly string None = "none";

		static readonly string TextLength = "textLength";

		static readonly string Time = "time";

		static readonly string Whole = "whole";

		static readonly Dictionary<DataValidationRuleType, string> dataValidationRuleTypeToId = new Dictionary<DataValidationRuleType, string>();

		static readonly Dictionary<string, DataValidationRuleType> typeIdToDataValidationRuleType = new Dictionary<string, DataValidationRuleType>();

		static readonly string defaultId;
	}
}
