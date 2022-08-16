using System;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	static class StyleNamesConverter
	{
		public static string ConvertStyleNameOnExport(string styleId)
		{
			if (BuiltInStyles.IsBuiltInStyle(styleId))
			{
				return "Telerik" + styleId;
			}
			return styleId;
		}

		public static bool TryGetTelerikExportedBuiltInStyle(string styleId, out string originalStyleId)
		{
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			if (styleId.StartsWith("Telerik"))
			{
				originalStyleId = styleId.Substring("Telerik".Length);
				if (BuiltInStyles.IsBuiltInStyle(originalStyleId))
				{
					return true;
				}
			}
			originalStyleId = null;
			return false;
		}

		public static bool IsTelerikExportedDefaultBuiltInStyle(string styleId)
		{
			string a;
			return StyleNamesConverter.TryGetTelerikExportedBuiltInStyle(styleId, out a) && (a == "Normal" || a == "TableNormal");
		}

		const string BuiltInStyleNamesPrefix = "Telerik";

		public static readonly string PrefixedNormalStyleId = StyleNamesConverter.ConvertStyleNameOnExport("Normal");

		public static readonly string PrefixedNormalTableStyleId = StyleNamesConverter.ConvertStyleNameOnExport("TableNormal");

		public static readonly string PrefixedNormalWebStyleId = StyleNamesConverter.ConvertStyleNameOnExport("NormalWeb");
	}
}
