using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class VerticalAlignments
	{
		static VerticalAlignments()
		{
			VerticalAlignments.nameToValueMapper.AddPair(VerticalAlignments.Bottom, RadVerticalAlignment.Bottom);
			VerticalAlignments.nameToValueMapper.AddPair(VerticalAlignments.Center, RadVerticalAlignment.Center);
			VerticalAlignments.nameToValueMapper.AddPair(VerticalAlignments.Distributed, RadVerticalAlignment.Distributed);
			VerticalAlignments.nameToValueMapper.AddPair(VerticalAlignments.Justify, RadVerticalAlignment.Justify);
			VerticalAlignments.nameToValueMapper.AddPair(VerticalAlignments.Top, RadVerticalAlignment.Top);
		}

		public static string GetVerticalAlignmentName(RadVerticalAlignment verticalAlignment)
		{
			return VerticalAlignments.nameToValueMapper.GetFromValue(verticalAlignment);
		}

		public static RadVerticalAlignment GetVerticalAlignment(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return VerticalAlignments.nameToValueMapper.GetToValue(name);
		}

		public static readonly string Bottom = "bottom";

		public static readonly string Center = "center";

		public static readonly string Distributed = "distributed";

		public static readonly string Justify = "justify";

		public static readonly string Top = "top";

		static readonly ValueMapper<string, RadVerticalAlignment> nameToValueMapper = new ValueMapper<string, RadVerticalAlignment>();
	}
}
