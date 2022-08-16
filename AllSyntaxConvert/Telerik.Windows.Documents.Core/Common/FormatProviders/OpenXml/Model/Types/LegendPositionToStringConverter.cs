using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class LegendPositionToStringConverter : IStringConverter<LegendPosition>
	{
		public LegendPosition ConvertFromString(string value)
		{
			if (value != null)
			{
				if (value == "b")
				{
					return LegendPosition.Bottom;
				}
				if (value == "t")
				{
					return LegendPosition.Top;
				}
				if (value == "r")
				{
					return LegendPosition.Right;
				}
				if (value == "l")
				{
					return LegendPosition.Left;
				}
			}
			throw new NotSupportedException();
		}

		public string ConvertToString(LegendPosition value)
		{
			switch (value)
			{
			case LegendPosition.Right:
				return "r";
			case LegendPosition.Bottom:
				return "b";
			case LegendPosition.Left:
				return "l";
			case LegendPosition.Top:
				return "t";
			default:
				throw new NotSupportedException();
			}
		}
	}
}
