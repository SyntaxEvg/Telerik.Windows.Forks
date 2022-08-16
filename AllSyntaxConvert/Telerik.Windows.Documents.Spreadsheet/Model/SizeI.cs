using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public struct SizeI
	{
		public static SizeI Empty
		{
			get
			{
				return SizeI.emptySizeI;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.width < 0;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Height must be greater or equal to 0");
				}
				this.height = value;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Width must be greater or equal to 0");
				}
				this.width = value;
			}
		}

		public SizeI(int width, int height)
		{
			if (width < 0 || height < 0)
			{
				throw new ArgumentException("Width and height must be greater or equal to 0");
			}
			this.width = width;
			this.height = height;
		}

		static SizeI CreateEmptySize()
		{
			return new SizeI
			{
				width = int.MinValue,
				height = int.MinValue
			};
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is SizeI && SizeI.Equals(this, (SizeI)obj);
		}

		public bool Equals(SizeI value)
		{
			return SizeI.Equals(this, value);
		}

		static bool Equals(SizeI size1, SizeI size2)
		{
			if (size1.IsEmpty)
			{
				return size2.IsEmpty;
			}
			return size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);
		}

		public override int GetHashCode()
		{
			if (this.IsEmpty)
			{
				return 0;
			}
			int hashCode = this.Width.GetHashCode();
			return hashCode ^ this.Height.GetHashCode();
		}

		public static bool operator ==(SizeI size1, SizeI size2)
		{
			return size1.Width == size2.Width && size1.Height == size2.Height;
		}

		public static bool operator !=(SizeI size1, SizeI size2)
		{
			return !(size1 == size2);
		}

		public override string ToString()
		{
			if (this.IsEmpty)
			{
				return "Empty";
			}
			return string.Format("{0},{1}", this.width, this.height);
		}

		static readonly SizeI emptySizeI = SizeI.CreateEmptySize();

		internal int width;

		internal int height;
	}
}
