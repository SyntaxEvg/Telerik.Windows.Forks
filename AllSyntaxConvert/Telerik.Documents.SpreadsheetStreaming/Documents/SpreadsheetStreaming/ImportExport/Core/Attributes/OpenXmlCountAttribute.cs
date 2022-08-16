using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes
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
				base.EnsureSetAttributeAllowed();
				this.value = value;
				this.hasValue = true;
			}
		}

		public override string GetValueAsString()
		{
			return this.Value.ToString();
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

		protected override void SetStringValueOverride(string value)
		{
			this.Value = int.Parse(value);
		}

		int value;

		bool hasValue;
	}
}
