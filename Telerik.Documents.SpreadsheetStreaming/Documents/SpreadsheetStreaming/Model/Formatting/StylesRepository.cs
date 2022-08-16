using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Formatting
{
	class StylesRepository
	{
		public StylesRepository()
		{
			this.fonts = new UniqueItemsStore<FontProperties>(0);
			this.fills = new UniqueItemsStore<ISpreadFill>(0);
			this.borders = new UniqueItemsStore<SpreadCellBorders>(0);
			this.cellStyles = new UniqueItemsStore<CellStyleInfo>(0);
			this.cellStyleFormats = new UniqueItemsStore<DiferentialFormat>(0);
			this.cellFormats = new UniqueItemsStore<DiferentialFormat>(0);
			this.numberFormats = new UniqueItemsStore<NumberFormat>(192);
		}

		public StylesRepository(SpreadCellStyle normalStyle)
			: this()
		{
			this.normalStyle = normalStyle;
			this.InitializeBuiltInStyles();
		}

		public IEnumerable<FontProperties> Fonts
		{
			get
			{
				return this.fonts;
			}
		}

		public IEnumerable<ISpreadFill> Fills
		{
			get
			{
				return this.fills;
			}
		}

		public IEnumerable<SpreadCellBorders> Borders
		{
			get
			{
				return this.borders;
			}
		}

		public IEnumerable<CellStyleInfo> CellStyles
		{
			get
			{
				return this.cellStyles;
			}
		}

		public IEnumerable<DiferentialFormat> CellStyleFormats
		{
			get
			{
				return this.cellStyleFormats;
			}
		}

		public IEnumerable<DiferentialFormat> CellFormats
		{
			get
			{
				return this.cellFormats;
			}
		}

		public IEnumerable<NumberFormat> NumberFormats
		{
			get
			{
				return this.numberFormats;
			}
		}

		public int NumberFormatsStartIndex
		{
			get
			{
				return this.numberFormats.StartIndex;
			}
			set
			{
				int startIndex = Math.Max(value, 192);
				this.numberFormats.StartIndex = startIndex;
			}
		}

		public int FontsStartIndex
		{
			get
			{
				return this.fonts.StartIndex;
			}
			set
			{
				this.fonts.StartIndex = value;
			}
		}

		public int FillsStartIndex
		{
			get
			{
				return this.fills.StartIndex;
			}
			set
			{
				this.fills.StartIndex = value;
			}
		}

		public int BordersStartIndex
		{
			get
			{
				return this.borders.StartIndex;
			}
			set
			{
				this.borders.StartIndex = value;
			}
		}

		public int CellStylesStartIndex
		{
			get
			{
				return this.cellStyles.StartIndex;
			}
			set
			{
				this.cellStyles.StartIndex = value;
			}
		}

		public int CellStyleFormatsStartIndex
		{
			get
			{
				return this.cellStyleFormats.StartIndex;
			}
			set
			{
				this.cellStyleFormats.StartIndex = value;
			}
		}

		public int CellFormatsStartIndex
		{
			get
			{
				return this.cellFormats.StartIndex;
			}
			set
			{
				this.cellFormats.StartIndex = value;
			}
		}

		public int RegisterCellFormat(SpreadCellFormat cellFormat)
		{
			DiferentialFormat value = this.CellFormatToDiferentialFormat(cellFormat);
			return this.cellFormats.Add(value);
		}

		public void AddNumberFormat(NumberFormat format)
		{
			if (format.NumberFormatId != null)
			{
				this.numberFormats.Add(format.NumberFormatId.Value, format);
				return;
			}
			this.numberFormats.Add(format);
		}

		public void AddFont(FontProperties font)
		{
			this.fonts.Add(font);
		}

		public void AddFill(ISpreadFill fill)
		{
			this.fills.Add(fill);
		}

		public void AddBorders(SpreadCellBorders borders)
		{
			this.borders.Add(borders);
		}

		public void AddCellStyleFormat(DiferentialFormat format)
		{
			this.cellStyleFormats.Add(format);
		}

		public void AddCellFormat(DiferentialFormat format)
		{
			this.cellFormats.Add(format);
		}

		public void AddCellStyle(CellStyleInfo style)
		{
			this.cellStyles.Add(style);
		}

		static DiferentialFormat CopyAsCellDiferentialFormat(DiferentialFormat cellStyleFormat)
		{
			DiferentialFormat diferentialFormat = new DiferentialFormat();
			if (cellStyleFormat.ApplyBorder != null && cellStyleFormat.ApplyBorder.Value)
			{
				diferentialFormat.BorderId = cellStyleFormat.BorderId;
			}
			if (cellStyleFormat.ApplyFill != null && cellStyleFormat.ApplyFill.Value)
			{
				diferentialFormat.FillId = cellStyleFormat.FillId;
			}
			if (cellStyleFormat.ApplyFont != null && cellStyleFormat.ApplyFont.Value)
			{
				diferentialFormat.FontId = cellStyleFormat.FontId;
			}
			if (cellStyleFormat.ApplyNumberFormat != null && cellStyleFormat.ApplyNumberFormat.Value)
			{
				diferentialFormat.NumFmtId = cellStyleFormat.NumFmtId;
			}
			diferentialFormat.FormatId = cellStyleFormat.FormatId;
			if (cellStyleFormat.ApplyAlignment != null && cellStyleFormat.ApplyAlignment.Value)
			{
				diferentialFormat.HorizontalAlignment = cellStyleFormat.HorizontalAlignment;
				diferentialFormat.VerticalAlignment = cellStyleFormat.VerticalAlignment;
				diferentialFormat.Indent = cellStyleFormat.Indent;
				diferentialFormat.WrapText = cellStyleFormat.WrapText;
			}
			if (cellStyleFormat.ApplyProtection != null && cellStyleFormat.ApplyProtection.Value)
			{
				diferentialFormat.IsLocked = cellStyleFormat.IsLocked;
			}
			diferentialFormat.ApplyAlignment = new bool?(false);
			diferentialFormat.ApplyBorder = new bool?(false);
			diferentialFormat.ApplyFill = new bool?(false);
			diferentialFormat.ApplyFont = new bool?(false);
			diferentialFormat.ApplyNumberFormat = new bool?(false);
			diferentialFormat.ApplyProtection = new bool?(false);
			return diferentialFormat;
		}

		DiferentialFormat CellFormatToDiferentialFormat(SpreadCellFormat cellFormat)
		{
			DiferentialFormat diferentialFormat = this.AddCellStyle(cellFormat);
			this.CopyFormatProperties(cellFormat, diferentialFormat);
			return diferentialFormat;
		}

		DiferentialFormat AddCellStyle(SpreadCellFormat cellFormat)
		{
			DiferentialFormat diferentialFormat = new DiferentialFormat();
			SpreadCellStyle cellStyle = cellFormat.CellStyle;
			if (cellStyle != null)
			{
				DiferentialFormat diferentialFormat2 = this.CellStyleToDiferentialFormat(cellStyle);
				diferentialFormat = StylesRepository.CopyAsCellDiferentialFormat(diferentialFormat2);
				CellStyleInfo cellStyleInfo = (from p in this.cellStyles
					where p.Name == cellStyle.Name
					select p).FirstOrDefault<CellStyleInfo>();
				int num;
				if (cellStyleInfo == null)
				{
					num = this.cellStyleFormats.Add(diferentialFormat2);
					cellStyleInfo = new CellStyleInfo(cellStyle.Name, num, cellStyle.BuiltinId);
					this.cellStyles.Add(cellStyleInfo);
				}
				else
				{
					num = cellStyleInfo.FormattingRecordId;
					DiferentialFormat valueByIndex = this.cellStyleFormats.GetValueByIndex(num);
					if (!ObjectExtensions.EqualsOfT<DiferentialFormat>(valueByIndex, diferentialFormat2))
					{
						this.cellStyleFormats.RemoveAtIndex(num);
						this.cellStyleFormats.Add(num, diferentialFormat2);
						this.UpdateOldCellsDiferentialFormats(num, valueByIndex, diferentialFormat2);
					}
				}
				diferentialFormat.FormatId = new int?(num);
			}
			return diferentialFormat;
		}

		DiferentialFormat CellStyleToDiferentialFormat(SpreadCellStyle cellFormat)
		{
			DiferentialFormat diferentialFormat = new DiferentialFormat();
			this.CopyFormatProperties(cellFormat, diferentialFormat);
			return diferentialFormat;
		}

		void CopyFormatProperties(SpreadCellFormatBase sourceFormat, DiferentialFormat destinationFormat)
		{
			if (sourceFormat.HasAnyBorderSet || (sourceFormat.ApplyBorderInternal != null && sourceFormat.ApplyBorderInternal.Value))
			{
				SpreadCellBorders spreadCellBorders = new SpreadCellBorders();
				spreadCellBorders.Bottom = sourceFormat.BottomBorder;
				spreadCellBorders.DiagonalDown = sourceFormat.DiagonalDownBorder;
				spreadCellBorders.DiagonalUp = sourceFormat.DiagonalUpBorder;
				spreadCellBorders.Left = sourceFormat.LeftBorder;
				spreadCellBorders.Right = sourceFormat.RightBorder;
				spreadCellBorders.Top = sourceFormat.TopBorder;
				destinationFormat.BorderId = new int?(this.borders.Add(spreadCellBorders));
			}
			if (sourceFormat.ApplyFillInternal != null && sourceFormat.ApplyFillInternal.Value)
			{
				ISpreadFill fill = DefaultCellFormatPropertyValues.Fill;
				if (sourceFormat.Fill != null)
				{
					fill = sourceFormat.Fill;
				}
				destinationFormat.FillId = new int?(this.fills.Add(fill));
			}
			if (sourceFormat.HasTextPropertiesSet || (sourceFormat.ApplyFontInternal != null && sourceFormat.ApplyFontInternal.Value))
			{
				destinationFormat.FontId = new int?(this.fonts.Add(FontProperties.FromCellFormat(sourceFormat)));
			}
			if (sourceFormat.NumberFormat != null && sourceFormat.ApplyNumberFormatInternal != null && sourceFormat.ApplyNumberFormatInternal.Value)
			{
				NumberFormat numberFormat = new NumberFormat();
				numberFormat.NumberFormatString = sourceFormat.NumberFormat;
				if (ObjectExtensions.EqualsOfT<string>(sourceFormat.NumberFormat, DefaultCellFormatPropertyValues.NumberFormat))
				{
					numberFormat.NumberFormatId = new int?(0);
				}
				else
				{
					numberFormat.NumberFormatId = new int?(this.numberFormats.GetNextIndex(numberFormat));
				}
				this.numberFormats.Add(numberFormat.NumberFormatId.Value, numberFormat);
				destinationFormat.NumFmtId = new int?(numberFormat.NumberFormatId.Value);
			}
			if (sourceFormat.HorizontalAlignment != null)
			{
				destinationFormat.HorizontalAlignment = sourceFormat.HorizontalAlignment;
			}
			if (sourceFormat.VerticalAlignment != null)
			{
				destinationFormat.VerticalAlignment = sourceFormat.VerticalAlignment;
			}
			if (sourceFormat.Indent != null)
			{
				destinationFormat.Indent = sourceFormat.Indent;
			}
			if (sourceFormat.WrapText != null)
			{
				destinationFormat.WrapText = sourceFormat.WrapText;
			}
			if (sourceFormat.ApplyBorderInternal != null)
			{
				destinationFormat.ApplyBorder = sourceFormat.ApplyBorderInternal;
			}
			if (sourceFormat.ApplyFillInternal != null)
			{
				destinationFormat.ApplyFill = sourceFormat.ApplyFillInternal;
			}
			if (sourceFormat.ApplyFontInternal != null)
			{
				destinationFormat.ApplyFont = sourceFormat.ApplyFontInternal;
			}
			if (sourceFormat.ApplyNumberFormatInternal != null)
			{
				destinationFormat.ApplyNumberFormat = sourceFormat.ApplyNumberFormatInternal;
			}
			if (sourceFormat.ApplyAlignmentInternal != null)
			{
				destinationFormat.ApplyAlignment = sourceFormat.ApplyAlignmentInternal;
			}
			if (sourceFormat.ApplyProtectionInternal != null)
			{
				destinationFormat.ApplyProtection = sourceFormat.ApplyProtectionInternal;
			}
		}

		void UpdateOldCellsDiferentialFormats(int formattingRecordId, DiferentialFormat oldFormattingRecord, DiferentialFormat cellStyleFormat)
		{
			DiferentialFormat diferentialFormat = (from p in this.cellFormats
				where p.FormatId == formattingRecordId
				select p).FirstOrDefault<DiferentialFormat>();
			if (diferentialFormat != null)
			{
				int indexByValue = this.cellFormats.GetIndexByValue(diferentialFormat);
				this.cellFormats.RemoveAtIndex(indexByValue);
				if (diferentialFormat.BorderId == oldFormattingRecord.BorderId && diferentialFormat.BorderId != cellStyleFormat.BorderId)
				{
					diferentialFormat.BorderId = cellStyleFormat.BorderId;
				}
				if (diferentialFormat.FillId == oldFormattingRecord.FillId && diferentialFormat.FillId != cellStyleFormat.FillId)
				{
					diferentialFormat.FillId = cellStyleFormat.FillId;
				}
				if (diferentialFormat.FontId == oldFormattingRecord.FontId && diferentialFormat.FontId != cellStyleFormat.FontId)
				{
					diferentialFormat.FontId = cellStyleFormat.FontId;
				}
				if (diferentialFormat.FormatId == oldFormattingRecord.FormatId && diferentialFormat.FormatId != cellStyleFormat.FormatId)
				{
					diferentialFormat.FormatId = cellStyleFormat.FormatId;
				}
				if (diferentialFormat.HorizontalAlignment == oldFormattingRecord.HorizontalAlignment && diferentialFormat.HorizontalAlignment != cellStyleFormat.HorizontalAlignment)
				{
					diferentialFormat.HorizontalAlignment = cellStyleFormat.HorizontalAlignment;
				}
				if (diferentialFormat.Indent == oldFormattingRecord.Indent && diferentialFormat.Indent != cellStyleFormat.Indent)
				{
					diferentialFormat.Indent = cellStyleFormat.Indent;
				}
				if (diferentialFormat.IsLocked == oldFormattingRecord.IsLocked && diferentialFormat.IsLocked != cellStyleFormat.IsLocked)
				{
					diferentialFormat.IsLocked = cellStyleFormat.IsLocked;
				}
				if (diferentialFormat.NumFmtId == oldFormattingRecord.NumFmtId && diferentialFormat.NumFmtId != cellStyleFormat.NumFmtId)
				{
					diferentialFormat.NumFmtId = cellStyleFormat.NumFmtId;
				}
				if (diferentialFormat.VerticalAlignment == oldFormattingRecord.VerticalAlignment && diferentialFormat.VerticalAlignment != cellStyleFormat.VerticalAlignment)
				{
					diferentialFormat.VerticalAlignment = cellStyleFormat.VerticalAlignment;
				}
				if (diferentialFormat.WrapText == oldFormattingRecord.WrapText && diferentialFormat.WrapText != cellStyleFormat.WrapText)
				{
					diferentialFormat.WrapText = cellStyleFormat.WrapText;
				}
				this.cellFormats.Add(indexByValue, diferentialFormat);
			}
		}

		void InitializeBuiltInStyles()
		{
			this.RegisterCellFormat(new SpreadCellFormat
			{
				CellStyle = this.normalStyle
			});
			this.fills.Add(new SpreadPatternFill(SpreadPatternType.Gray12Percent));
		}

		const int DefaultNumberFormatsStartIndex = 192;

		readonly UniqueItemsStore<FontProperties> fonts;

		readonly UniqueItemsStore<ISpreadFill> fills;

		readonly UniqueItemsStore<SpreadCellBorders> borders;

		readonly UniqueItemsStore<CellStyleInfo> cellStyles;

		readonly UniqueItemsStore<DiferentialFormat> cellStyleFormats;

		readonly UniqueItemsStore<DiferentialFormat> cellFormats;

		readonly UniqueItemsStore<NumberFormat> numberFormats;

		readonly SpreadCellStyle normalStyle;
	}
}
