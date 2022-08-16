using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class SeriesGroupingToStringConverter : IStringConverter<SeriesGrouping>
	{
		public SeriesGrouping ConvertFromString(string value)
		{
			if (value != null)
			{
				if (value == "standard")
				{
					return SeriesGrouping.Standard;
				}
				if (value == "stacked")
				{
					return SeriesGrouping.Stacked;
				}
				if (value == "percentStacked")
				{
					return SeriesGrouping.PercentStacked;
				}
			}
			throw new NotSupportedException();
		}

		public string ConvertToString(SeriesGrouping value)
		{
			switch (value)
			{
			case SeriesGrouping.Standard:
				return "standard";
			case SeriesGrouping.Stacked:
				return "stacked";
			case SeriesGrouping.PercentStacked:
				return "percentStacked";
			default:
				throw new NotSupportedException();
			}
		}
	}
}
