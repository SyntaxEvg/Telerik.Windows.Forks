using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class BorderStyles
	{
		static BorderStyles()
		{
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.DashDot, CellBorderStyle.DashDot);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.DashDotDot, CellBorderStyle.DashDotDot);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Dashed, CellBorderStyle.Dashed);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Dotted, CellBorderStyle.Dotted);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Hair, CellBorderStyle.Hair);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Medium, CellBorderStyle.Medium);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.MediumDashDot, CellBorderStyle.MediumDashDot);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.MediumDashDotDot, CellBorderStyle.MediumDashDotDot);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.MediumDashed, CellBorderStyle.MediumDashed);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Thick, CellBorderStyle.Thick);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Thin, CellBorderStyle.Thin);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.None, CellBorderStyle.None);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.SlantDashDot, CellBorderStyle.SlantDashDot);
			BorderStyles.nameToValueMapper.AddPair(BorderStyles.Double, CellBorderStyle.Double);
		}

		public static string GetBorderStyleName(CellBorderStyle style)
		{
			return BorderStyles.nameToValueMapper.GetFromValue(style);
		}

		public static CellBorderStyle GetBorderStyle(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return BorderStyles.nameToValueMapper.GetToValue(name);
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

		static readonly ValueMapper<string, CellBorderStyle> nameToValueMapper = new ValueMapper<string, CellBorderStyle>(BorderStyles.Thin, CellBorderStyle.Thin);
	}
}
