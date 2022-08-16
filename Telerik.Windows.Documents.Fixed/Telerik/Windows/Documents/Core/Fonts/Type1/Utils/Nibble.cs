using System;
using Telerik.Windows.Documents.Core.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Utils
{
	class Nibble
	{
		public static Nibble[] GetNibbles(byte b)
		{
			byte[] bits = BitsHelper.GetBits(b);
			Nibble[] array = new Nibble[]
			{
				null,
				new Nibble(BitsHelper.ToByte(bits, 0, 4))
			};
			array[0] = new Nibble(BitsHelper.ToByte(bits, 4, 4));
			return array;
		}

		public static bool operator ==(Nibble left, Nibble right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return object.ReferenceEquals(right, null);
			}
			return left.Equals(right);
		}

		public static bool operator !=(Nibble left, Nibble right)
		{
			return !(left == right);
		}

		public Nibble(byte value)
		{
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			Nibble nibble = obj as Nibble;
			return !(nibble == null) && this.value == nibble.value;
		}

		public override int GetHashCode()
		{
			int num = 17;
			return num * 23 + this.value.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0:X}", this.value);
		}

		readonly byte value;
	}
}
