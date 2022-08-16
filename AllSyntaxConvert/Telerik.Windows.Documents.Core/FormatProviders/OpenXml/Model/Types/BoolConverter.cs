using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class BoolConverter : IStringConverter<bool>
	{
		public bool ConvertFromString(string str)
		{
			return !(str == "false") && !(str == "0") && !(str == "off");
		}

		public string ConvertToString(bool obj)
		{
			return obj.ToString().ToLowerInvariant();
		}
	}
}
