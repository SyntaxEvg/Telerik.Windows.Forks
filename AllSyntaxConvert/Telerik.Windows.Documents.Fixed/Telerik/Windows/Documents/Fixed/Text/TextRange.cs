using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	public class TextRange
	{
		internal TextRange(TextPosition startPosition, TextPosition endPosition)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(startPosition, "startPosition");
			Guard.ThrowExceptionIfNull<TextPosition>(endPosition, "endPosition");
			this.startPosition = new TextPosition(startPosition);
			this.endPosition = new TextPosition(endPosition);
		}

		TextRange()
		{
			this.startPosition = null;
			this.endPosition = null;
		}

		public static TextRange Empty
		{
			get
			{
				return new TextRange();
			}
		}

		public TextPosition StartPosition
		{
			get
			{
				return this.startPosition;
			}
		}

		public TextPosition EndPosition
		{
			get
			{
				return this.endPosition;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.startPosition == null && this.endPosition == null;
			}
		}

		readonly TextPosition startPosition;

		readonly TextPosition endPosition;
	}
}
