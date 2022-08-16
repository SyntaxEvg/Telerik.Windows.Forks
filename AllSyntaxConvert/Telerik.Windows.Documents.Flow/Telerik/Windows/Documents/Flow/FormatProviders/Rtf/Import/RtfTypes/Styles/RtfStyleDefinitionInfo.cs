using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles
{
	class RtfStyleDefinitionInfo
	{
		public int? StyleId { get; set; }

		public int? BasedOnStyleId { get; set; }

		public int? LinkedStyleId { get; set; }

		public int? NextStyleId { get; set; }

		public bool IsPrimary { get; set; }

		public int? UIPriority { get; set; }

		public Style CurrentStyle { get; set; }

		public Run ImportedSpanProperties { get; set; }

		public Paragraph ImportedParagraphProperties { get; set; }

		public Table ImportedTableProperties { get; set; }

		public TableCell ImportedTableCellProperties { get; set; }

		public TableRow ImportedTableRowProperties { get; set; }

		public void CopyPropertiesFrom(RtfStyleDefinitionInfo other)
		{
			this.CurrentStyle = other.CurrentStyle;
		}
	}
}
