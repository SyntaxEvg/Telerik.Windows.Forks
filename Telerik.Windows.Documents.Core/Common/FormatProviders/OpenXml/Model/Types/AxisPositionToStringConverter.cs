using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class AxisPositionToStringConverter : IStringConverter<AxisPosition>
	{
		public AxisPosition ConvertFromString(string value)
		{
			if (value != null)
			{
				if (value == "b")
				{
					return AxisPosition.Bottom;
				}
				if (value == "t")
				{
					return AxisPosition.Top;
				}
				if (value == "r")
				{
					return AxisPosition.Right;
				}
				if (value == "l")
				{
					return AxisPosition.Left;
				}
			}
			throw new NotSupportedException();
		}

		public string ConvertToString(AxisPosition value)
		{
			switch (value)
			{
			case AxisPosition.Bottom:
				return "b";
			case AxisPosition.Left:
				return "l";
			case AxisPosition.Right:
				return "r";
			case AxisPosition.Top:
				return "t";
			default:
				throw new NotSupportedException();
			}
		}
	}
}
