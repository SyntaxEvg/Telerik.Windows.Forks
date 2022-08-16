using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
{
	class ConvertedOpenXmlAttribute<T> : OpenXmlAttributeBase<T>
	{
		public ConvertedOpenXmlAttribute(string name, IStringConverter<T> converter, T defaultValue, bool isRequired = false)
			: this(name, null, converter, defaultValue, isRequired)
		{
		}

		public ConvertedOpenXmlAttribute(string name, IStringConverter<T> converter, bool isRequired = false)
			: this(name, null, converter, isRequired)
		{
		}

		public ConvertedOpenXmlAttribute(string name, OpenXmlNamespace ns, IStringConverter<T> converter, bool isRequired = false)
			: this(name, ns, converter, default(T), isRequired)
		{
		}

		public ConvertedOpenXmlAttribute(string name, OpenXmlNamespace ns, IStringConverter<T> converter, T defaultValue, bool isRequired = false)
			: base(name, ns, defaultValue, isRequired)
		{
			this.converter = converter;
		}

		public override string GetValueAsString()
		{
			return this.converter.ConvertToString(base.Value);
		}

		protected override void SetStringValueOverride(string value)
		{
			base.Value = this.converter.ConvertFromString(value);
		}

		readonly IStringConverter<T> converter;
	}
}
