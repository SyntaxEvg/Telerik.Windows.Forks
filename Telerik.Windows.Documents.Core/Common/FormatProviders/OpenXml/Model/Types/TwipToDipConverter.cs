using System;
using System.Globalization;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class TwipToDipConverter : IStringConverter<double>
	{
		public double ConvertFromString(string value)
		{
			int num;
			if (int.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out num))
			{
				return Unit.TwipToDip((double)num);
			}
			return 0.0;
		}

		public string ConvertToString(double value)
		{
			return Unit.DipToTwipI(value).ToString(CultureInfo.InvariantCulture);
		}
	}
}
