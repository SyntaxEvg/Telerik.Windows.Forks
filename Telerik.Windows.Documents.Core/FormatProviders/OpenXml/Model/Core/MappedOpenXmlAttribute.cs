using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class MappedOpenXmlAttribute<T> : OpenXmlAttributeBase<T>
	{
		public MappedOpenXmlAttribute(string name, OpenXmlNamespace ns, ValueMapper<string, T> mapper, bool isRequired = false)
			: base(name, ns, isRequired)
		{
			Guard.ThrowExceptionIfNull<ValueMapper<string, T>>(mapper, "mapper");
			this.mapper = mapper;
			if (this.mapper.DefaultToValue != null)
			{
				base.DefaultValue = mapper.DefaultToValue.Value;
			}
		}

		public override string GetValueAsString()
		{
			return this.mapper.GetFromValue(base.Value);
		}

		public override void SetStringValue(IOpenXmlImportContext context, string value)
		{
			base.Value = this.mapper.GetToValue(value);
		}

		readonly ValueMapper<string, T> mapper;
	}
}
