using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class GradientTypes
	{
		static GradientTypes()
		{
			GradientTypes.nameToValueMapper.AddPair(GradientTypes.Linear, GradientInfoType.Linear);
			GradientTypes.nameToValueMapper.AddPair(GradientTypes.Path, GradientInfoType.Path);
		}

		public static string GetGradientTypeName(GradientInfoType gradient)
		{
			return GradientTypes.nameToValueMapper.GetFromValue(gradient);
		}

		public static GradientInfoType GetGradientType(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return GradientTypes.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Linear = "linear";

		public static readonly string Path = "path";

		static readonly ValueMapper<string, GradientInfoType> nameToValueMapper = new ValueMapper<string, GradientInfoType>();
	}
}
