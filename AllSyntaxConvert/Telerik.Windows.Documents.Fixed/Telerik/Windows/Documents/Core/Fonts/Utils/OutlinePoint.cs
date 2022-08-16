using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class OutlinePoint
	{
		public OutlinePoint(byte flags)
		{
			this.Flags = flags;
		}

		public OutlinePoint(double x, double y, byte flags)
			: this(flags)
		{
			this.Point = new Point(x, y);
		}

		public Point Point { get; set; }

		public byte Flags { get; set; }

		public byte Instruction { get; set; }

		public bool IsOnCurve
		{
			get
			{
				return (this.Flags & 1) != 0;
			}
		}
	}
}
