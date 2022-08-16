using System;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	public class ParagraphProperties
	{
		public ParagraphProperties()
		{
			this.BackgroundColor = FixedDocumentDefaults.BackgroundColor;
			this.HorizontalAlignment = FixedDocumentDefaults.HorizontalAlignment;
			this.SpacingBefore = FixedDocumentDefaults.SpacingBefore;
			this.SpacingAfter = FixedDocumentDefaults.SpacingAfter;
			this.LineSpacing = FixedDocumentDefaults.LineSpacing;
			this.LineSpacingType = FixedDocumentDefaults.LineSpacingType;
			this.LeftIndent = FixedDocumentDefaults.LeftIndent;
			this.RightIndent = FixedDocumentDefaults.RightIndent;
			this.FirstLineIndent = FixedDocumentDefaults.FirstLineIndent;
			this.ListLevel = FixedDocumentDefaults.FirstListLevelIndex;
			this.ListId = null;
		}

		public HorizontalAlignment HorizontalAlignment { get; set; }

		public double SpacingBefore { get; set; }

		public double SpacingAfter { get; set; }

		public double LineSpacing { get; set; }

		public HeightType LineSpacingType { get; set; }

		public double FirstLineIndent { get; set; }

		public double LeftIndent { get; set; }

		public double RightIndent { get; set; }

		public ColorBase BackgroundColor { get; set; }

		public int? ListId { get; set; }

		public int ListLevel { get; set; }

		public void CopyPropertiesFrom(ParagraphProperties fromProperties)
		{
			Guard.ThrowExceptionIfNull<ParagraphProperties>(fromProperties, "other");
			this.HorizontalAlignment = fromProperties.HorizontalAlignment;
			this.SpacingBefore = fromProperties.SpacingBefore;
			this.SpacingAfter = fromProperties.SpacingAfter;
			this.LineSpacing = fromProperties.LineSpacing;
			this.LineSpacingType = fromProperties.LineSpacingType;
			this.FirstLineIndent = fromProperties.FirstLineIndent;
			this.LeftIndent = fromProperties.LeftIndent;
			this.RightIndent = fromProperties.RightIndent;
			this.BackgroundColor = fromProperties.BackgroundColor;
		}

		internal void CopyTo(Block block)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			block.HorizontalAlignment = this.HorizontalAlignment;
			block.SpacingBefore = this.SpacingBefore;
			block.SpacingAfter = this.SpacingAfter;
			block.LineSpacing = this.LineSpacing;
			block.LineSpacingType = this.LineSpacingType;
			block.FirstLineIndent = this.FirstLineIndent;
			block.LeftIndent = this.LeftIndent;
			block.RightIndent = this.RightIndent;
			block.BackgroundColor = this.BackgroundColor;
		}
	}
}
