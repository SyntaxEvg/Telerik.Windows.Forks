using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core
{
	class OpenXmlCountAttribute : OpenXmlAttributeBase
	{
		public OpenXmlCountAttribute()
			: base("count", null, false)
		{
			this.hasValue = false;
		}

		public override bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		public override string GetValueAsString()
		{
			return this.Value.ToString();
		}

		public override void SetStringValue(IOpenXmlImportContext context, string value)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.Value = int.Parse(value);
		}

		public override bool ShouldExport()
		{
			return true;
		}

		public override void ResetValue()
		{
			this.value = 0;
			this.hasValue = false;
		}

		int value;

		bool hasValue;
	}
}
