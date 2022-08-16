using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class BarDirectionToStringConverter : IStringConverter<BarDirection>
	{
		public BarDirection ConvertFromString(string value)
		{
			if (value != null)
			{
				if (value == "col")
				{
					return BarDirection.Column;
				}
				if (value == "bar")
				{
					return BarDirection.Bar;
				}
			}
			throw new NotSupportedException("This type of bar direction is not supported.");
		}

		public string ConvertToString(BarDirection value)
		{
			switch (value)
			{
			case BarDirection.Bar:
				return "bar";
			case BarDirection.Column:
				return "col";
			default:
				throw new NotSupportedException("This type of bar direction is not supported.");
			}
		}
	}
}
