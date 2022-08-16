using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Private : Dict
	{
		public Private(Top top, long offset, int length)
			: base(top.File, offset, length)
		{
			this.top = top;
		}

		public static OperatorDescriptor SubrsOperator { get; set; } = new OperatorDescriptor(19);

		public static OperatorDescriptor DefaultWidthXOperator { get; set; } = new OperatorDescriptor(20, 0);

		public static OperatorDescriptor NominalWidthXOperator { get; set; } = new OperatorDescriptor(21, 0);

		public SubrsIndex Subrs
		{
			get
			{
				if (this.subrs == null)
				{
					this.subrs = new SubrsIndex(base.File, this.top.CharstringType, base.Offset + (long)base.GetInt(Private.SubrsOperator));
					base.File.ReadTable(this.subrs);
				}
				return this.subrs;
			}
		}

		public int DefaultWidthX
		{
			get
			{
				if (this.defaultWidthX == null)
				{
					this.defaultWidthX = new int?(base.GetInt(Private.DefaultWidthXOperator));
				}
				return this.defaultWidthX.Value;
			}
		}

		public int NominalWidthX
		{
			get
			{
				if (this.nominalWidthX == null)
				{
					this.nominalWidthX = new int?(base.GetInt(Private.NominalWidthXOperator));
				}
				return this.nominalWidthX.Value;
			}
		}

		readonly Top top;

		SubrsIndex subrs;

		int? defaultWidthX;

		int? nominalWidthX;
	}
}
