using System;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class EmuToDipConverter : IStringConverter<double>
	{
		public double ConvertFromString(string value)
		{
			int num;
			if (int.TryParse(value, out num))
			{
				return (double)((int)Unit.EmuToDip((double)num));
			}
			return 0.0;
		}

		public string ConvertToString(double dip)
		{
			return ((int)Unit.DipToEmu(dip)).ToString();
		}
	}
}
