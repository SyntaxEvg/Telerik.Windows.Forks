using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
{
	class MappedOpenXmlAttribute<T> : OpenXmlAttributeBase<T>
	{
		public MappedOpenXmlAttribute(string name, OpenXmlNamespace ns, ValueMapper<string, T> mapper, bool isRequired = false)
			: base(name, ns, isRequired)
		{
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

		protected override void SetStringValueOverride(string value)
		{
			base.Value = this.mapper.GetToValue(value);
		}

		readonly ValueMapper<string, T> mapper;
	}
}
