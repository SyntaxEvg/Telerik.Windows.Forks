using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class BoolOpenXmlAttribute : ConvertedOpenXmlAttribute<bool>
	{
		public BoolOpenXmlAttribute(string name)
			: this(name, false)
		{
		}

		public BoolOpenXmlAttribute(string name, bool isRequired)
			: this(name, null, isRequired)
		{
		}

		public BoolOpenXmlAttribute(string name, bool defaultValue, bool isRequired)
			: this(name, null, defaultValue, isRequired)
		{
		}

		public BoolOpenXmlAttribute(string name, OpenXmlNamespace ns)
			: this(name, ns, false)
		{
		}

		public BoolOpenXmlAttribute(string name, OpenXmlNamespace ns, bool isRequired)
			: this(name, ns, false, isRequired)
		{
		}

		public BoolOpenXmlAttribute(string name, OpenXmlNamespace ns, bool defaultValue, bool isRequired)
			: base(name, ns, Converters.BoolValueConverter, defaultValue, isRequired)
		{
		}
	}
}
