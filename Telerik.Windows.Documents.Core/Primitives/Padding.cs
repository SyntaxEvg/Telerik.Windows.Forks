using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Primitives
{
	public class Padding
	{
		public Padding(double all)
		{
			this.top = all;
			this.left = all;
			this.right = all;
			this.bottom = all;
		}

		public Padding(double left, double top, double right, double bottom)
		{
			this.top = top;
			this.left = left;
			this.right = right;
			this.bottom = bottom;
		}

		public double Top
		{
			get
			{
				return this.top;
			}
		}

		public double Bottom
		{
			get
			{
				return this.bottom;
			}
		}

		public double Left
		{
			get
			{
				return this.left;
			}
		}

		public double Right
		{
			get
			{
				return this.right;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				return this.Left == this.Right && this.Right == this.Top && this.top == this.Bottom && this.Bottom == 0.0;
			}
		}

		public override bool Equals(object obj)
		{
			Padding padding = obj as Padding;
			return !(padding == null) && (this.Left.Equals(padding.Left) && this.Top.Equals(padding.Top) && this.Right.Equals(padding.Right)) && this.Bottom.Equals(padding.Bottom);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Left.GetHashCode(), this.Top.GetHashCode(), this.Right.GetHashCode(), this.Bottom.GetHashCode());
		}

		public static bool operator ==(Padding a, Padding b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(Padding a, Padding b)
		{
			return !(a == b);
		}

		internal Padding Clone()
		{
			return new Padding(this.Left, this.Top, this.Right, this.Bottom);
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[] { this.Left, this.Top, this.Right, this.Bottom });
		}

		const double EmptyValue = 0.0;

		public static readonly Padding Empty = new Padding(0.0);

		readonly double top;

		readonly double left;

		readonly double right;

		readonly double bottom;
	}
}
