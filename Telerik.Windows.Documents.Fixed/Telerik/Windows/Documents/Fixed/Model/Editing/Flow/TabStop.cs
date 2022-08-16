using System;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	class TabStop
	{
		public TabStop(double position)
		{
			this.Position = position;
		}

		public double Position { get; set; }

		public TabStopType Type { get; set; }

		public TabStopLeader Leader { get; set; }
	}
}
