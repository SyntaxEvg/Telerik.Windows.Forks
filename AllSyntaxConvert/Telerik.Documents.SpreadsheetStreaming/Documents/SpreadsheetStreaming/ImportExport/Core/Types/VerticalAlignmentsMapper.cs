using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class VerticalAlignmentsMapper
	{
		static VerticalAlignmentsMapper()
		{
			VerticalAlignmentsMapper.nameToValueMapper.AddPair(VerticalAlignmentsMapper.Bottom, SpreadVerticalAlignment.Bottom);
			VerticalAlignmentsMapper.nameToValueMapper.AddPair(VerticalAlignmentsMapper.Center, SpreadVerticalAlignment.Center);
			VerticalAlignmentsMapper.nameToValueMapper.AddPair(VerticalAlignmentsMapper.Distributed, SpreadVerticalAlignment.Distributed);
			VerticalAlignmentsMapper.nameToValueMapper.AddPair(VerticalAlignmentsMapper.Justify, SpreadVerticalAlignment.Justify);
			VerticalAlignmentsMapper.nameToValueMapper.AddPair(VerticalAlignmentsMapper.Top, SpreadVerticalAlignment.Top);
		}

		public static string GetVerticalAlignmentName(SpreadVerticalAlignment verticalAlignment)
		{
			return VerticalAlignmentsMapper.nameToValueMapper.GetFromValue(verticalAlignment);
		}

		public static SpreadVerticalAlignment? GetVerticalAlignmentValue(string name)
		{
			return new SpreadVerticalAlignment?(VerticalAlignmentsMapper.nameToValueMapper.GetToValue(name));
		}

		public static readonly string Bottom = "bottom";

		public static readonly string Center = "center";

		public static readonly string Distributed = "distributed";

		public static readonly string Justify = "justify";

		public static readonly string Top = "top";

		static readonly ValueMapper<string, SpreadVerticalAlignment> nameToValueMapper = new ValueMapper<string, SpreadVerticalAlignment>();
	}
}
