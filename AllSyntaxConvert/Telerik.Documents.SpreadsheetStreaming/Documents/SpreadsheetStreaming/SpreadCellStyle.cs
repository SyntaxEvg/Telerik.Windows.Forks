using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadCellStyle : SpreadCellFormatBase
	{
		internal SpreadCellStyle(string styleName, int? builtinId)
		{
			this.styleName = styleName;
			this.builtinId = builtinId;
			this.ApplyBorder = true;
			this.ApplyFill = true;
			this.ApplyFont = true;
			this.ApplyNumberFormat = true;
			this.ApplyAlignment = true;
			this.ApplyProtection = true;
		}

		public bool ApplyBorder
		{
			get
			{
				return base.ApplyBorderInternal.Value;
			}
			set
			{
				base.ApplyBorderInternal = new bool?(value);
			}
		}

		public bool ApplyFill
		{
			get
			{
				return base.ApplyFillInternal.Value;
			}
			set
			{
				base.ApplyFillInternal = new bool?(value);
			}
		}

		public bool ApplyFont
		{
			get
			{
				return base.ApplyFontInternal.Value;
			}
			set
			{
				base.ApplyFontInternal = new bool?(value);
			}
		}

		public bool ApplyNumberFormat
		{
			get
			{
				return base.ApplyNumberFormatInternal.Value;
			}
			set
			{
				base.ApplyNumberFormatInternal = new bool?(value);
			}
		}

		public bool ApplyAlignment
		{
			get
			{
				return base.ApplyAlignmentInternal.Value;
			}
			set
			{
				base.ApplyAlignmentInternal = new bool?(value);
			}
		}

		public bool ApplyProtection
		{
			get
			{
				return base.ApplyProtectionInternal.Value;
			}
			set
			{
				base.ApplyProtectionInternal = new bool?(value);
			}
		}

		public string Name
		{
			get
			{
				return this.styleName;
			}
		}

		internal int? BuiltinId
		{
			get
			{
				return this.builtinId;
			}
		}

		readonly string styleName;

		readonly int? builtinId;
	}
}
