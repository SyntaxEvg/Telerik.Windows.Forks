using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
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
