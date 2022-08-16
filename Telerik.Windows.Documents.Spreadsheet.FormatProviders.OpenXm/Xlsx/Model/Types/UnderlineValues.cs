using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class UnderlineValues
	{
		static UnderlineValues()
		{
			UnderlineValues.nameToValueMapper.AddPair(UnderlineValues.Double, UnderlineType.Double);
			UnderlineValues.nameToValueMapper.AddPair(UnderlineValues.DoubleAccounting, UnderlineType.DoubleAccounting);
			UnderlineValues.nameToValueMapper.AddPair(UnderlineValues.Single, UnderlineType.Single);
			UnderlineValues.nameToValueMapper.AddPair(UnderlineValues.SingleAccounting, UnderlineType.SingleAccounting);
			UnderlineValues.nameToValueMapper.AddPair(UnderlineValues.None, UnderlineType.None);
		}

		public static string GetUnderlineValueName(UnderlineType underline)
		{
			return UnderlineValues.nameToValueMapper.GetFromValue(underline);
		}

		public static UnderlineType GetUnderlineValue(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return UnderlineValues.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Double = "double";

		public static readonly string DoubleAccounting = "doubleAccounting";

		public static readonly string None = "none";

		public static readonly string Single = "single";

		public static readonly string SingleAccounting = "singleAccounting";

		static readonly ValueMapper<string, UnderlineType> nameToValueMapper = new ValueMapper<string, UnderlineType>();
	}
}
