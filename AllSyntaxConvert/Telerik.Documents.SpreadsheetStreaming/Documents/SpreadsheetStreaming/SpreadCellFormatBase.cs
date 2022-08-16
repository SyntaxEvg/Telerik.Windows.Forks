using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public abstract class SpreadCellFormatBase
	{
		public SpreadBorder LeftBorder
		{
			get
			{
				return this.leftBorder;
			}
			set
			{
				if (this.leftBorder != value)
				{
					this.leftBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public SpreadBorder RightBorder
		{
			get
			{
				return this.rightBorder;
			}
			set
			{
				if (this.rightBorder != value)
				{
					this.rightBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public SpreadBorder TopBorder
		{
			get
			{
				return this.topBorder;
			}
			set
			{
				if (this.topBorder != value)
				{
					this.topBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public SpreadBorder BottomBorder
		{
			get
			{
				return this.bottomBorder;
			}
			set
			{
				if (this.bottomBorder != value)
				{
					this.bottomBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public SpreadBorder DiagonalUpBorder
		{
			get
			{
				return this.diagonalUpBorder;
			}
			set
			{
				if (this.diagonalUpBorder != value)
				{
					this.diagonalUpBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public SpreadBorder DiagonalDownBorder
		{
			get
			{
				return this.diagonalDownBorder;
			}
			set
			{
				if (this.diagonalDownBorder != value)
				{
					this.diagonalDownBorder = value;
					this.ApplyBorderInternal = new bool?(true);
				}
			}
		}

		public ISpreadFill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				if (this.fill != value)
				{
					this.fill = value;
					this.ApplyFillInternal = new bool?(true);
				}
			}
		}

		public string NumberFormat
		{
			get
			{
				return this.numberFormat;
			}
			set
			{
				if (this.numberFormat != value)
				{
					this.numberFormat = value;
					this.ApplyNumberFormatInternal = new bool?(true);
				}
			}
		}

		public double? FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				double? num = this.fontSize;
				double? num2 = value;
				if (num.GetValueOrDefault() != num2.GetValueOrDefault() || num != null != (num2 != null))
				{
					this.fontSize = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public SpreadThemableColor ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				if (this.foreColor != value)
				{
					this.foreColor = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public bool? IsBold
		{
			get
			{
				return this.isBold;
			}
			set
			{
				if (this.isBold != value)
				{
					this.isBold = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public bool? IsItalic
		{
			get
			{
				return this.isItalic;
			}
			set
			{
				if (this.isItalic != value)
				{
					this.isItalic = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public SpreadUnderlineType? Underline
		{
			get
			{
				return this.underline;
			}
			set
			{
				if (this.underline != value)
				{
					this.underline = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public SpreadThemableFontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
			set
			{
				if (this.fontFamily != value)
				{
					this.fontFamily = value;
					this.ApplyFontInternal = new bool?(true);
				}
			}
		}

		public SpreadHorizontalAlignment? HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignment;
			}
			set
			{
				if (this.horizontalAlignment != value)
				{
					this.horizontalAlignment = value;
					this.ApplyAlignmentInternal = new bool?(true);
				}
			}
		}

		public SpreadVerticalAlignment? VerticalAlignment
		{
			get
			{
				return this.verticalAlignment;
			}
			set
			{
				if (this.verticalAlignment != value)
				{
					this.verticalAlignment = value;
					this.ApplyAlignmentInternal = new bool?(true);
				}
			}
		}

		public int? Indent
		{
			get
			{
				return this.indent;
			}
			set
			{
				if (this.indent != value)
				{
					this.indent = value;
					this.ApplyAlignmentInternal = new bool?(true);
				}
			}
		}

		public bool? WrapText
		{
			get
			{
				return this.wrapText;
			}
			set
			{
				if (this.wrapText != value)
				{
					this.wrapText = value;
					this.ApplyAlignmentInternal = new bool?(true);
				}
			}
		}

		internal bool? ApplyBorderInternal { get; set; }

		internal bool? ApplyFillInternal { get; set; }

		internal bool? ApplyFontInternal { get; set; }

		internal bool? ApplyNumberFormatInternal { get; set; }

		internal bool? ApplyAlignmentInternal { get; set; }

		internal bool? ApplyProtectionInternal { get; set; }

		internal bool HasTextPropertiesSet
		{
			get
			{
				return this.FontSize != null || this.ForeColor != null || this.IsBold != null || this.IsItalic != null || this.Underline != null || this.FontFamily != null;
			}
		}

		internal bool HasAnyBorderSet
		{
			get
			{
				return this.BottomBorder != null || this.DiagonalDownBorder != null || this.DiagonalUpBorder != null || this.LeftBorder != null || this.RightBorder != null || this.TopBorder != null;
			}
		}

		SpreadBorder leftBorder;

		SpreadBorder rightBorder;

		SpreadBorder topBorder;

		SpreadBorder bottomBorder;

		SpreadBorder diagonalUpBorder;

		SpreadBorder diagonalDownBorder;

		ISpreadFill fill;

		string numberFormat;

		double? fontSize;

		SpreadThemableColor foreColor;

		bool? isBold;

		bool? isItalic;

		SpreadUnderlineType? underline;

		SpreadThemableFontFamily fontFamily;

		SpreadHorizontalAlignment? horizontalAlignment;

		SpreadVerticalAlignment? verticalAlignment;

		int? indent;

		bool? wrapText;
	}
}
