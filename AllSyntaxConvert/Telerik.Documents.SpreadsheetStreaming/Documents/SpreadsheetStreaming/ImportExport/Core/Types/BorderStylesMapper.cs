using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class BorderStylesMapper
	{
		static BorderStylesMapper()
		{
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.DashDot, SpreadBorderStyle.DashDot);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.DashDotDot, SpreadBorderStyle.DashDotDot);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Dashed, SpreadBorderStyle.Dashed);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Dotted, SpreadBorderStyle.Dotted);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Hair, SpreadBorderStyle.Hair);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Medium, SpreadBorderStyle.Medium);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.MediumDashDot, SpreadBorderStyle.MediumDashDot);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.MediumDashDotDot, SpreadBorderStyle.MediumDashDotDot);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.MediumDashed, SpreadBorderStyle.MediumDashed);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Thick, SpreadBorderStyle.Thick);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Thin, SpreadBorderStyle.Thin);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.None, SpreadBorderStyle.None);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.SlantDashDot, SpreadBorderStyle.SlantDashDot);
			BorderStylesMapper.nameToValueMapper.AddPair(BorderStylesMapper.Double, SpreadBorderStyle.Double);
		}

		public static string GetBorderStyleName(SpreadBorderStyle style)
		{
			return BorderStylesMapper.nameToValueMapper.GetFromValue(style);
		}

		public static SpreadBorderStyle GetBorderStyle(string name)
		{
			return BorderStylesMapper.nameToValueMapper.GetToValue(name);
		}

		public static readonly string SlantDashDot = "slantDashDot";

		public static readonly string DashDot = "dashDot";

		public static readonly string DashDotDot = "dashDotDot";

		public static readonly string Dashed = "dashed";

		public static readonly string Double = "double";

		public static readonly string Dotted = "dotted";

		public static readonly string Hair = "hair";

		public static readonly string Medium = "medium";

		public static readonly string MediumDashDot = "mediumDashDot";

		public static readonly string MediumDashDotDot = "mediumDashDotDot";

		public static readonly string MediumDashed = "mediumDashed";

		public static readonly string None = "none";

		public static readonly string Thick = "thick";

		public static readonly string Thin = "thin";

		static readonly ValueMapper<string, SpreadBorderStyle> nameToValueMapper = new ValueMapper<string, SpreadBorderStyle>(BorderStylesMapper.Thin, SpreadBorderStyle.Thin);
	}
}
