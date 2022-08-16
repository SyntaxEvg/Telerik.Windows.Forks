using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Web.Spreadsheet
{
	public static class RadVerticalAlignmentExtensions
	{
		public static string AsString(this RadVerticalAlignment alignment)
		{
			if (alignment == RadVerticalAlignment.Bottom)
			{
				return "bottom";
			}
			if (alignment == RadVerticalAlignment.Top)
			{
				return "top";
			}
			return "center";
		}

		public static RadVerticalAlignment ToVerticalAlignment(this string alignment)
		{
			if (alignment != null)
			{
				if (alignment == "top")
				{
					return RadVerticalAlignment.Top;
				}
				if (alignment == "bottom")
				{
					return RadVerticalAlignment.Bottom;
				}
			}
			return RadVerticalAlignment.Center;
		}
	}
}
