using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TabStop
	{
		public TabStop(double position)
			: this(position, TabStopType.Left, TabStopLeader.None)
		{
		}

		public TabStop(double position, TabStopType type)
			: this(position, type, TabStopLeader.None)
		{
		}

		public TabStop(double position, TabStopType type, TabStopLeader leader)
		{
			this.position = position;
			this.type = type;
			this.leader = leader;
		}

		public double Position
		{
			get
			{
				return this.position;
			}
		}

		public TabStopType Type
		{
			get
			{
				return this.type;
			}
		}

		public TabStopLeader Leader
		{
			get
			{
				return this.leader;
			}
		}

		public static bool operator ==(TabStop tabStop, TabStop otherTabStop)
		{
			return object.ReferenceEquals(tabStop, otherTabStop) || (tabStop != null && otherTabStop != null && tabStop.Equals(otherTabStop));
		}

		public static bool operator !=(TabStop tabStop, TabStop otherTabStop)
		{
			return !(tabStop == otherTabStop);
		}

		public override bool Equals(object obj)
		{
			TabStop tabStop = obj as TabStop;
			return !(tabStop == null) && (this.Leader == tabStop.Leader && this.Position == tabStop.Position) && this.Type == tabStop.Type;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Leader.GetHashCode(), this.Position.GetHashCode(), this.Type.GetHashCode());
		}

		readonly double position;

		readonly TabStopType type;

		readonly TabStopLeader leader;
	}
}
