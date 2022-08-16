using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model
{
	class DiferentialFormat
	{
		public int? BorderId { get; set; }

		public int? FillId { get; set; }

		public int? FontId { get; set; }

		public int? NumFmtId { get; set; }

		public int? FormatId { get; set; }

		public bool? ApplyAlignment { get; set; }

		public bool? ApplyBorder { get; set; }

		public bool? ApplyFill { get; set; }

		public bool? ApplyFont { get; set; }

		public bool? ApplyNumberFormat { get; set; }

		public bool? ApplyProtection { get; set; }

		public SpreadHorizontalAlignment? HorizontalAlignment { get; set; }

		public SpreadVerticalAlignment? VerticalAlignment { get; set; }

		public int? Indent { get; set; }

		public bool? WrapText { get; set; }

		public bool? IsLocked { get; set; }

		public override bool Equals(object obj)
		{
			DiferentialFormat diferentialFormat = obj as DiferentialFormat;
			return diferentialFormat != null && (ObjectExtensions.EqualsOfT<int?>(this.BorderId, diferentialFormat.BorderId) && ObjectExtensions.EqualsOfT<int?>(this.FillId, diferentialFormat.FillId) && ObjectExtensions.EqualsOfT<int?>(this.FontId, diferentialFormat.FontId) && ObjectExtensions.EqualsOfT<int?>(this.NumFmtId, diferentialFormat.NumFmtId) && ObjectExtensions.EqualsOfT<int?>(this.FormatId, diferentialFormat.FormatId) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyAlignment, diferentialFormat.ApplyAlignment) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyBorder, diferentialFormat.ApplyBorder) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyFill, diferentialFormat.ApplyFill) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyFont, diferentialFormat.ApplyFont) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyNumberFormat, diferentialFormat.ApplyNumberFormat) && ObjectExtensions.EqualsOfT<bool?>(this.ApplyProtection, diferentialFormat.ApplyProtection) && ObjectExtensions.EqualsOfT<SpreadHorizontalAlignment?>(this.HorizontalAlignment, diferentialFormat.HorizontalAlignment) && ObjectExtensions.EqualsOfT<SpreadVerticalAlignment?>(this.VerticalAlignment, diferentialFormat.VerticalAlignment) && ObjectExtensions.EqualsOfT<int?>(this.Indent, diferentialFormat.Indent) && ObjectExtensions.EqualsOfT<bool?>(this.WrapText, diferentialFormat.WrapText)) && ObjectExtensions.EqualsOfT<bool?>(this.IsLocked, diferentialFormat.IsLocked);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.BorderId.GetHashCodeOrZero(), this.FillId.GetHashCodeOrZero(), new int[]
			{
				this.FontId.GetHashCodeOrZero(),
				this.NumFmtId.GetHashCodeOrZero(),
				this.FormatId.GetHashCodeOrZero(),
				this.ApplyAlignment.GetHashCodeOrZero(),
				this.ApplyBorder.GetHashCodeOrZero(),
				this.ApplyFill.GetHashCodeOrZero(),
				this.ApplyFont.GetHashCodeOrZero(),
				this.ApplyNumberFormat.GetHashCodeOrZero(),
				this.ApplyProtection.GetHashCodeOrZero(),
				this.HorizontalAlignment.GetHashCodeOrZero(),
				this.VerticalAlignment.GetHashCodeOrZero(),
				this.Indent.GetHashCodeOrZero(),
				this.WrapText.GetHashCodeOrZero(),
				this.IsLocked.GetHashCodeOrZero()
			});
		}
	}
}
