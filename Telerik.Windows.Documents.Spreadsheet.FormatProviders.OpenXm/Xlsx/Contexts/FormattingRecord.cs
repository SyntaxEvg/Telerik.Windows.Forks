using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	struct FormattingRecord
	{
		public FormattingRecord(FormattingRecord record)
		{
			this = default(FormattingRecord);
			this.FillId = record.FillId;
			this.NumberFormatId = record.NumberFormatId;
			this.FontInfoId = record.FontInfoId;
			this.BordersInfoId = record.BordersInfoId;
			this.HorizontalAlignment = record.HorizontalAlignment;
			this.VerticalAlignment = record.VerticalAlignment;
			this.Indent = record.Indent;
			this.WrapText = record.WrapText;
			this.IsLocked = record.IsLocked;
			this.ApplyNumberFormat = record.ApplyNumberFormat;
			this.ApplyAlignment = record.ApplyAlignment;
			this.ApplyFont = record.ApplyFont;
			this.ApplyBorder = record.ApplyBorder;
			this.ApplyFill = record.ApplyFill;
			this.ApplyProtection = record.ApplyProtection;
			this.StyleFormattingRecordId = record.StyleFormattingRecordId;
		}

		public int? FillId { get; set; }

		public int? NumberFormatId { get; set; }

		public int? FontInfoId { get; set; }

		public int? BordersInfoId { get; set; }

		public RadHorizontalAlignment? HorizontalAlignment { get; set; }

		public RadVerticalAlignment? VerticalAlignment { get; set; }

		public int? Indent { get; set; }

		public bool? WrapText { get; set; }

		public bool? IsLocked { get; set; }

		public bool? ApplyNumberFormat { get; set; }

		public bool? ApplyAlignment { get; set; }

		public bool? ApplyFont { get; set; }

		public bool? ApplyBorder { get; set; }

		public bool? ApplyFill { get; set; }

		public bool? ApplyProtection { get; set; }

		public int? StyleFormattingRecordId { get; set; }

		public bool IsEmpty()
		{
			return this.Equals(FormattingRecord.Empty);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is FormattingRecord))
			{
				return false;
			}
			FormattingRecord formattingRecord = (FormattingRecord)obj;
			return TelerikHelper.EqualsOfT<int?>(this.FillId, formattingRecord.FillId) && TelerikHelper.EqualsOfT<int?>(this.NumberFormatId, formattingRecord.NumberFormatId) && TelerikHelper.EqualsOfT<int?>(this.FontInfoId, formattingRecord.FontInfoId) && TelerikHelper.EqualsOfT<int?>(this.BordersInfoId, formattingRecord.BordersInfoId) && TelerikHelper.EqualsOfT<RadHorizontalAlignment?>(this.HorizontalAlignment, formattingRecord.HorizontalAlignment) && TelerikHelper.EqualsOfT<RadVerticalAlignment?>(this.VerticalAlignment, formattingRecord.VerticalAlignment) && TelerikHelper.EqualsOfT<int?>(this.Indent, formattingRecord.Indent) && TelerikHelper.EqualsOfT<bool?>(this.WrapText, formattingRecord.WrapText) && TelerikHelper.EqualsOfT<bool?>(this.IsLocked, formattingRecord.IsLocked) && TelerikHelper.EqualsOfT<int?>(this.StyleFormattingRecordId, formattingRecord.StyleFormattingRecordId) && TelerikHelper.EqualsOfT<bool?>(this.ApplyNumberFormat, formattingRecord.ApplyNumberFormat) && TelerikHelper.EqualsOfT<bool?>(this.ApplyAlignment, formattingRecord.ApplyAlignment) && TelerikHelper.EqualsOfT<bool?>(this.ApplyFont, formattingRecord.ApplyFont) && TelerikHelper.EqualsOfT<bool?>(this.ApplyBorder, formattingRecord.ApplyBorder) && TelerikHelper.EqualsOfT<bool?>(this.ApplyFill, formattingRecord.ApplyFill) && TelerikHelper.EqualsOfT<bool?>(this.ApplyProtection, formattingRecord.ApplyProtection);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.FillId.GetHashCodeOrZero(), this.NumberFormatId.GetHashCodeOrZero(), new int[]
			{
				this.FontInfoId.GetHashCodeOrZero(),
				this.BordersInfoId.GetHashCodeOrZero(),
				this.HorizontalAlignment.GetHashCodeOrZero(),
				this.VerticalAlignment.GetHashCodeOrZero(),
				this.Indent.GetHashCodeOrZero(),
				this.WrapText.GetHashCodeOrZero(),
				this.IsLocked.GetHashCodeOrZero(),
				this.StyleFormattingRecordId.GetHashCodeOrZero(),
				this.ApplyNumberFormat.GetHashCodeOrZero(),
				this.ApplyAlignment.GetHashCodeOrZero(),
				this.ApplyFont.GetHashCodeOrZero(),
				this.ApplyBorder.GetHashCodeOrZero(),
				this.ApplyFill.GetHashCodeOrZero(),
				this.ApplyProtection.GetHashCodeOrZero()
			});
		}

		public static readonly FormattingRecord Empty = default(FormattingRecord);
	}
}
