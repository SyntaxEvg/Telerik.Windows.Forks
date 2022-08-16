using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class IntOpenXmlAttribute : ConvertedOpenXmlAttribute<int>
	{
		public IntOpenXmlAttribute(string name, bool isRequired = false)
			: this(name, null, isRequired)
		{
		}

		public IntOpenXmlAttribute(string name, int defaultValue, bool isRequired = false)
			: this(name, null, defaultValue, isRequired)
		{
		}

		public IntOpenXmlAttribute(string name, OpenXmlNamespace ns, bool isRequired = false)
			: this(name, ns, 0, isRequired)
		{
		}

		public IntOpenXmlAttribute(string name, OpenXmlNamespace ns, int defaultValue, bool isRequired = false)
			: base(name, ns, Converters.IntValueConverter, defaultValue, isRequired)
		{
		}
	}
}
