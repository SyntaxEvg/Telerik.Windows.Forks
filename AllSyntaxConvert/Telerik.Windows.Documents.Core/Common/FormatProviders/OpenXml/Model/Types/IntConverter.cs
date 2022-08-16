using System;
using System.Globalization;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Globalization;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class IntConverter : IStringConverter<int>
	{
		public int ConvertFromString(string value)
		{
			int result;
			if (int.TryParse(value, out result))
			{
				return result;
			}
			double num;
			if (IntConverter.cultureInfo.TryParseDouble(value, out num))
			{
				return (int)num;
			}
			return 0;
		}

		public string ConvertToString(int value)
		{
			return value.ToString();
		}

		static readonly OpenXmlCultureInfo cultureInfo = new OpenXmlCultureInfo(CultureInfo.InvariantCulture);
	}
}
