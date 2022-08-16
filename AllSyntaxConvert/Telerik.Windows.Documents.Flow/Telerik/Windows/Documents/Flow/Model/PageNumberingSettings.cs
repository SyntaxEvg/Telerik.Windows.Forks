using System;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class PageNumberingSettings
	{
		internal PageNumberingSettings(Section section)
		{
			this.section = section;
		}

		public ChapterSeparatorType? ChapterSeparatorCharacter
		{
			get
			{
				return this.section.Properties.ChapterSeparatorCharacter.GetActualValue();
			}
			set
			{
				this.section.Properties.ChapterSeparatorCharacter.LocalValue = value;
			}
		}

		public int? ChapterHeadingStyleIndex
		{
			get
			{
				return this.section.Properties.ChapterHeadingStyleIndex.GetActualValue();
			}
			set
			{
				if (value != null)
				{
					Guard.ThrowExceptionIfOutOfRange<int>(1, 9, value.Value, "value");
				}
				this.section.Properties.ChapterHeadingStyleIndex.LocalValue = value;
			}
		}

		public NumberingStyle? PageNumberFormat
		{
			get
			{
				return this.section.Properties.PageNumberFormat.GetActualValue();
			}
			set
			{
				this.section.Properties.PageNumberFormat.LocalValue = value;
			}
		}

		public int? StartingPageNumber
		{
			get
			{
				return this.section.Properties.StartingPageNumber.GetActualValue();
			}
			set
			{
				if (value != null)
				{
					Guard.ThrowExceptionIfLessThan<int>(0, value.Value, "value");
				}
				this.section.Properties.StartingPageNumber.LocalValue = value;
			}
		}

		readonly Section section;
	}
}
