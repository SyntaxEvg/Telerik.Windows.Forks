using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class PatternTypes
	{
		static PatternTypes()
		{
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.Solid, PatternType.Solid);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkGray, PatternType.Gray75Percent);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.MediumGray, PatternType.Gray50Percent);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightGray, PatternType.Gray25Percent);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.Gray125, PatternType.Gray12Percent);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.Gray0625, PatternType.Gray6Percent);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkHorizontal, PatternType.HorizontalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkVertical, PatternType.VerticalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkDown, PatternType.ReverseDiagonalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkUp, PatternType.DiagonalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkGrid, PatternType.DiagonalCrosshatch);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.DarkTrellis, PatternType.ThickDiagonalCrosshatch);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightHorizontal, PatternType.ThinHorizontalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightVertical, PatternType.ThinVerticalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightDown, PatternType.ThinReverseDiagonalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightUp, PatternType.ThinDiagonalStripe);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightGrid, PatternType.ThinHorizontalCrossHatch);
			PatternTypes.nameToValueMapper.AddPair(PatternTypes.LightTrellis, PatternType.ThinDiagonalCrosshatch);
		}

		public static string GetPatternTypeName(PatternType pattern)
		{
			return PatternTypes.nameToValueMapper.GetFromValue(pattern);
		}

		public static PatternType GetPatternType(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return PatternTypes.nameToValueMapper.GetToValue(name);
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

		static readonly ValueMapper<string, PatternType> nameToValueMapper = new ValueMapper<string, PatternType>();
	}
}
