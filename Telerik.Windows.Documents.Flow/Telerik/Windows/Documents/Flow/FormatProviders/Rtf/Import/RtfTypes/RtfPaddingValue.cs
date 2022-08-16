using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	class RtfPaddingValue
	{
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				this.HasValue = true;
			}
		}

		public int UnitType
		{
			get
			{
				return this.unitType;
			}
			set
			{
				this.unitType = value;
				this.HasValue = true;
			}
		}

		public bool HasValue { get; set; }

		int value;

		int unitType;
	}
}
