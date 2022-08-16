using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class UnderlineValuesMapper
	{
		static UnderlineValuesMapper()
		{
			UnderlineValuesMapper.nameToValueMapper.AddPair(UnderlineValuesMapper.Double, SpreadUnderlineType.Double);
			UnderlineValuesMapper.nameToValueMapper.AddPair(UnderlineValuesMapper.DoubleAccounting, SpreadUnderlineType.DoubleAccounting);
			UnderlineValuesMapper.nameToValueMapper.AddPair(UnderlineValuesMapper.Single, SpreadUnderlineType.Single);
			UnderlineValuesMapper.nameToValueMapper.AddPair(UnderlineValuesMapper.SingleAccounting, SpreadUnderlineType.SingleAccounting);
			UnderlineValuesMapper.nameToValueMapper.AddPair(UnderlineValuesMapper.None, SpreadUnderlineType.None);
		}

		public static string GetUnderlineValueName(SpreadUnderlineType underline)
		{
			return UnderlineValuesMapper.nameToValueMapper.GetFromValue(underline);
		}

		public static SpreadUnderlineType GetUnderlineValue(string name)
		{
			return UnderlineValuesMapper.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Double = "double";

		public static readonly string DoubleAccounting = "doubleAccounting";

		public static readonly string None = "none";

		public static readonly string Single = "single";

		public static readonly string SingleAccounting = "singleAccounting";

		static readonly ValueMapper<string, SpreadUnderlineType> nameToValueMapper = new ValueMapper<string, SpreadUnderlineType>();
	}
}
