using System;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	class RtfCellSpacingValue
	{
		public RtfCellSpacingValue()
		{
			this.UnitType = 3;
		}

		public int UnitType { get; set; }

		public int RtfValue { get; set; }

		public double ValueInDips
		{
			get
			{
				if (this.UnitType == 3)
				{
					return Unit.TwipToDip((double)this.RtfValue);
				}
				return 0.0;
			}
		}
	}
}
