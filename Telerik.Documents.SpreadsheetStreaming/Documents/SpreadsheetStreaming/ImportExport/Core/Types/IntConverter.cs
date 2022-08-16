using System;
using System.Globalization;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
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
