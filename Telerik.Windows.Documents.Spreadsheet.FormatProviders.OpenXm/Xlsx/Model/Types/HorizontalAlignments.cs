using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class HorizontalAlignments
	{
		static HorizontalAlignments()
		{
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Center, RadHorizontalAlignment.Center);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.CenterContinuous, RadHorizontalAlignment.CenterContinuous);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Distributed, RadHorizontalAlignment.Distributed);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Fill, RadHorizontalAlignment.Fill);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.General, RadHorizontalAlignment.General);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Justify, RadHorizontalAlignment.Justify);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Left, RadHorizontalAlignment.Left);
			HorizontalAlignments.nameToValueMapper.AddPair(HorizontalAlignments.Right, RadHorizontalAlignment.Right);
		}

		public static string GetHorizontalAlignmentName(RadHorizontalAlignment horizontalAlignment)
		{
			return HorizontalAlignments.nameToValueMapper.GetFromValue(horizontalAlignment);
		}

		public static RadHorizontalAlignment GetHorizontalAlignment(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return HorizontalAlignments.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Center = "center";

		public static readonly string CenterContinuous = "centerContinuous";

		public static readonly string Distributed = "distributed";

		public static readonly string Fill = "fill";

		public static readonly string General = "general";

		public static readonly string Justify = "justify";

		public static readonly string Left = "left";

		public static readonly string Right = "right";

		static readonly ValueMapper<string, RadHorizontalAlignment> nameToValueMapper = new ValueMapper<string, RadHorizontalAlignment>();
	}
}
