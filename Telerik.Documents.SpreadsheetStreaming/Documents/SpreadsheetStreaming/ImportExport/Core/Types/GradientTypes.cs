using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
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

		internal static GradientInfoType? GetGradientTypeValue(string name)
		{
			return new GradientInfoType?(GradientTypes.nameToValueMapper.GetToValue(name));
		}

		public static readonly string Linear = "linear";

		public static readonly string Path = "path";

		static readonly ValueMapper<string, GradientInfoType> nameToValueMapper = new ValueMapper<string, GradientInfoType>();
	}
}
