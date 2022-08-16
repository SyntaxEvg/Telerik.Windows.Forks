using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class PatternTypesMapper
	{
		static PatternTypesMapper()
		{
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.Solid, SpreadPatternType.Solid);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkGray, SpreadPatternType.Gray75Percent);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.MediumGray, SpreadPatternType.Gray50Percent);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightGray, SpreadPatternType.Gray25Percent);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.Gray125, SpreadPatternType.Gray12Percent);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.Gray0625, SpreadPatternType.Gray6Percent);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkHorizontal, SpreadPatternType.HorizontalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkVertical, SpreadPatternType.VerticalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkDown, SpreadPatternType.ReverseDiagonalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkUp, SpreadPatternType.DiagonalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkGrid, SpreadPatternType.DiagonalCrosshatch);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.DarkTrellis, SpreadPatternType.ThickDiagonalCrosshatch);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightHorizontal, SpreadPatternType.ThinHorizontalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightVertical, SpreadPatternType.ThinVerticalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightDown, SpreadPatternType.ThinReverseDiagonalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightUp, SpreadPatternType.ThinDiagonalStripe);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightGrid, SpreadPatternType.ThinHorizontalCrossHatch);
			PatternTypesMapper.nameToValueMapper.AddPair(PatternTypesMapper.LightTrellis, SpreadPatternType.ThinDiagonalCrosshatch);
		}

		public static string GetPatternTypeName(SpreadPatternType pattern)
		{
			return PatternTypesMapper.nameToValueMapper.GetFromValue(pattern);
		}

		public static SpreadPatternType GetPatternType(string name)
		{
			return PatternTypesMapper.nameToValueMapper.GetToValue(name);
		}

		public static readonly string DarkDown = "darkDown";

		public static readonly string DarkGray = "darkGray";

		public static readonly string DarkGrid = "darkGrid";

		public static readonly string DarkHorizontal = "darkHorizontal";

		public static readonly string DarkTrellis = "darkTrellis";

		public static readonly string DarkUp = "darkUp";

		public static readonly string DarkVertical = "darkVertical";

		public static readonly string Gray0625 = "gray0625";

		public static readonly string Gray125 = "gray125";

		public static readonly string LightDown = "lightDown";

		public static readonly string LightGray = "lightGray";

		public static readonly string LightGrid = "lightGrid";

		public static readonly string LightHorizontal = "lightHorizontal";

		public static readonly string LightTrellis = "lightTrellis";

		public static readonly string LightUp = "lightUp";

		public static readonly string LightVertical = "lightVertical";

		public static readonly string MediumGray = "mediumGray";

		public static readonly string None = "none";

		public static readonly string Solid = "solid";

		static readonly ValueMapper<string, SpreadPatternType> nameToValueMapper = new ValueMapper<string, SpreadPatternType>();
	}
}
