using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	class RtfTabStop
	{
		public double Position { get; set; }

		public TabStopType Type { get; set; }

		public TabStopLeader Leader { get; set; }

		public TabStop GetTabStop()
		{
			return new TabStop(this.Position, this.Type, this.Leader);
		}

		public void Reset()
		{
			this.Type = TabStopType.Left;
			this.Position = 0.0;
			this.Leader = TabStopLeader.None;
		}
	}
}
