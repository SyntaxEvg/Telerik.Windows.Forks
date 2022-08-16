using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadColor
	{
		public SpreadColor(byte r, byte g, byte b)
			: this(byte.MaxValue, r, g, b)
		{
		}

		internal SpreadColor(byte a, byte r, byte g, byte b)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, 255, (int)a, "a");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 255, (int)r, "r");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 255, (int)g, "g");
			Guard.ThrowExceptionIfOutOfRange<int>(0, 255, (int)b, "b");
			this.a = a;
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public byte G
		{
			get
			{
				return this.g;
			}
		}

		public byte R
		{
			get
			{
				return this.r;
			}
		}

		public byte B
		{
			get
			{
				return this.b;
			}
		}

		internal byte A
		{
			get
			{
				return this.a;
			}
		}

		public override bool Equals(object obj)
		{
			SpreadColor spreadColor = obj as SpreadColor;
			return spreadColor != null && (this.A == spreadColor.A && this.R == spreadColor.R && this.G == spreadColor.G) && this.B == spreadColor.B;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes((int)this.A, (int)this.R, (int)this.G, (int)this.B);
		}

		readonly byte a;

		readonly byte r;

		readonly byte g;

		readonly byte b;
	}
}
