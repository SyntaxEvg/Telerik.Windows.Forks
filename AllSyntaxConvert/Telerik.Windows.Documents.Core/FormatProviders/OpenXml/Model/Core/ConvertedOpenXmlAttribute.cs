using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
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
			Guard.ThrowExceptionIfNull<IStringConverter<T>>(converter, "converter");
			this.converter = converter;
		}

		public override string GetValueAsString()
		{
			return this.converter.ConvertToString(base.Value);
		}

		public override void SetStringValue(IOpenXmlImportContext context, string value)
		{
			base.Value = this.converter.ConvertFromString(value);
		}

		readonly IStringConverter<T> converter;
	}
}
