using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class PageOrientationConverter : IStringConverter<PageOrientation>
	{
		public PageOrientation ConvertFromString(string str)
		{
			if (string.Equals(str, "landscape", StringComparison.OrdinalIgnoreCase))
			{
				return PageOrientation.Landscape;
			}
			return PageOrientation.Portrait;
		}

		public string ConvertToString(PageOrientation orientation)
		{
			if (orientation == PageOrientation.Landscape || orientation == PageOrientation.Rotate270)
			{
				return "landscape";
			}
			return "portrait";
		}
	}
}
