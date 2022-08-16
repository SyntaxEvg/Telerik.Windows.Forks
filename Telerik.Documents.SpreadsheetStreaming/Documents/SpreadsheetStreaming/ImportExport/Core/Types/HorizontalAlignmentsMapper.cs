using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class HorizontalAlignmentsMapper
	{
		static HorizontalAlignmentsMapper()
		{
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Center, SpreadHorizontalAlignment.Center);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.CenterContinuous, SpreadHorizontalAlignment.CenterContinuous);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Distributed, SpreadHorizontalAlignment.Distributed);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Fill, SpreadHorizontalAlignment.Fill);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.General, SpreadHorizontalAlignment.General);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Justify, SpreadHorizontalAlignment.Justify);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Left, SpreadHorizontalAlignment.Left);
			HorizontalAlignmentsMapper.nameToValueMapper.AddPair(HorizontalAlignmentsMapper.Right, SpreadHorizontalAlignment.Right);
		}

		public static string GetHorizontalAlignmentName(SpreadHorizontalAlignment horizontalAlignment)
		{
			return HorizontalAlignmentsMapper.nameToValueMapper.GetFromValue(horizontalAlignment);
		}

		public static SpreadHorizontalAlignment GetHorizontalAlignmentValue(string name)
		{
			return HorizontalAlignmentsMapper.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Center = "center";

		public static readonly string CenterContinuous = "centerContinuous";

		public static readonly string Distributed = "distributed";

		public static readonly string Fill = "fill";

		public static readonly string General = "general";

		public static readonly string Justify = "justify";

		public static readonly string Left = "left";

		public static readonly string Right = "right";

		static readonly ValueMapper<string, SpreadHorizontalAlignment> nameToValueMapper = new ValueMapper<string, SpreadHorizontalAlignment>();
	}
}
