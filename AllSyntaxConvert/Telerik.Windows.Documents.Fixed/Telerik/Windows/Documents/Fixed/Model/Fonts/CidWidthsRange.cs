using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class CidWidthsRange<T>
	{
		public CidWidthsRange(int startCharCode, int endCharCode, T constantWidth)
		{
			this.startCharCode = startCharCode;
			this.endCharCode = endCharCode;
			this.constantWidth = constantWidth;
			this.widths = null;
		}

		public CidWidthsRange(int startCharCode, IEnumerable<T> widths)
		{
			this.startCharCode = startCharCode;
			this.widths = new List<T>(widths);
			this.endCharCode = this.startCharCode + this.widths.Count - 1;
		}

		public bool IsConstantWidthRange
		{
			get
			{
				return this.widths == null;
			}
		}

		public int StartCharCode
		{
			get
			{
				return this.startCharCode;
			}
		}

		public int EndCharCode
		{
			get
			{
				return this.endCharCode;
			}
		}

		public T ConstantWidth
		{
			get
			{
				return this.constantWidth;
			}
		}

		public IEnumerable<T> Widths
		{
			get
			{
				if (this.IsConstantWidthRange)
				{
					int count = this.EndCharCode - this.StartCharCode + 1;
					return Enumerable.Repeat<T>(this.ConstantWidth, count);
				}
				return this.widths;
			}
		}

		public bool TryGetWidth(int charCode, out T width)
		{
			width = default(T);
			bool flag = this.StartCharCode <= charCode && charCode <= this.EndCharCode;
			if (flag)
			{
				if (this.IsConstantWidthRange)
				{
					width = this.ConstantWidth;
				}
				else
				{
					int index = charCode - this.StartCharCode;
					width = this.widths[index];
				}
			}
			return flag;
		}

		readonly int startCharCode;

		readonly int endCharCode;

		readonly T constantWidth;

		readonly List<T> widths;
	}
}
