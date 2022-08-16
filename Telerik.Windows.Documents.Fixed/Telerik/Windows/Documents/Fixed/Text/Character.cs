using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class Character
	{
		public Character(Rect boundingRect, string toUnicode, double charSpacing)
		{
			this.boundingRect = boundingRect;
			this.toUnicode = toUnicode;
			this.charSpacing = charSpacing;
		}

		public char Char
		{
			get
			{
				return this.toUnicode[0];
			}
		}

		public Rect BoundingRect
		{
			get
			{
				return this.boundingRect;
			}
		}

		public double Height
		{
			get
			{
				return this.boundingRect.Height;
			}
		}

		public string ToUnicode
		{
			get
			{
				return this.toUnicode;
			}
		}

		public double CharSpacing
		{
			get
			{
				return this.charSpacing;
			}
		}

		public override string ToString()
		{
			return this.Char.ToString();
		}

		readonly Rect boundingRect;

		readonly string toUnicode;

		readonly double charSpacing;
	}
}
