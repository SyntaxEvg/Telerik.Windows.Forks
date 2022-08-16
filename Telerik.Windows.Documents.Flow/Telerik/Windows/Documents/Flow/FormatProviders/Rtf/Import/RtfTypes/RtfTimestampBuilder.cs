using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	sealed class RtfTimestampBuilder : RtfElementIteratorBase
	{
		public DateTime CreateTimestamp(RtfGroup group)
		{
			this.Reset();
			base.VisitGroupChildren(group, false);
			DateTime result;
			try
			{
				result = new DateTime(this.year, this.month, this.day, this.hour, this.minutes, this.seconds);
			}
			catch (ArgumentException)
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		public void Reset()
		{
			this.year = 0;
			this.month = 1;
			this.day = 1;
			this.hour = 0;
			this.minutes = 0;
			this.seconds = 0;
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "yr")
				{
					this.year = tag.ValueAsNumber;
					return;
				}
				if (name == "mo")
				{
					this.month = tag.ValueAsNumber;
					return;
				}
				if (name == "dy")
				{
					this.day = tag.ValueAsNumber;
					return;
				}
				if (name == "hr")
				{
					this.hour = tag.ValueAsNumber;
					return;
				}
				if (name == "min")
				{
					this.minutes = tag.ValueAsNumber;
					return;
				}
				if (!(name == "sec"))
				{
					return;
				}
				this.seconds = tag.ValueAsNumber;
			}
		}

		int year;

		int month;

		int day;

		int hour;

		int minutes;

		int seconds;
	}
}
