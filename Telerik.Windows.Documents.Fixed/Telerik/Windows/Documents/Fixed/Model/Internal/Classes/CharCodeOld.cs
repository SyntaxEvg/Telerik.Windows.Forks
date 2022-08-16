using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Classes
{
	struct CharCodeOld
	{
		public CharCodeOld(byte[] bytes)
		{
			this.bytes = bytes;
			this.intValue = BytesHelper.GetInt(this.bytes);
		}

		public CharCodeOld(byte b)
		{
			this.bytes = new byte[1];
			this.bytes[0] = b;
			this.intValue = (int)b;
		}

		public CharCodeOld(ushort us)
		{
			byte[] array = BitConverter.GetBytes(us);
			this.bytes = new byte[array.Length];
			int num = array.Length - 1;
			for (int i = 0; i < array.Length; i++)
			{
				this.bytes[num - i] = array[i];
			}
			this.intValue = (int)us;
		}

		public CharCodeOld(int ii)
		{
			byte[] array = BitConverter.GetBytes(ii);
			this.bytes = new byte[array.Length];
			int num = array.Length - 1;
			for (int i = 0; i < array.Length; i++)
			{
				this.bytes[num - i] = array[i];
			}
			this.intValue = ii;
		}

		public int BytesCount
		{
			get
			{
				if (this.bytes == null)
				{
					return 0;
				}
				return this.bytes.Length;
			}
		}

		public byte[] Bytes
		{
			get
			{
				return this.bytes;
			}
		}

		public int IntValue
		{
			get
			{
				return this.intValue;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.bytes == null;
			}
		}

		static void InsureCharCodes(CharCodeOld left, CharCodeOld right)
		{
			if (left.bytes == null || right.bytes == null)
			{
				throw new ArgumentException("Bytes cannot be null.");
			}
			if (left.bytes.Length != right.bytes.Length)
			{
				throw new InvalidOperationException("Cannot compare CharCodes with different length.");
			}
		}

		public static CharCodeOld operator ++(CharCodeOld cc)
		{
			byte[] array = (byte[])cc.bytes.Clone();
			int num = 1;
			for (int i = array.Length - 1; i >= 0; i--)
			{
				int num2 = (int)array[i] + num;
				num = num2 / 256;
				array[i] = (byte)(num2 % 256);
				if (num == 0)
				{
					break;
				}
			}
			if (num > 0)
			{
				throw new OverflowException();
			}
			return new CharCodeOld(array);
		}

		public static bool operator <(CharCodeOld left, CharCodeOld right)
		{
			CharCodeOld.InsureCharCodes(left, right);
			for (int i = 0; i < left.bytes.Length; i++)
			{
				if (left.bytes[i] > right.bytes[i])
				{
					return false;
				}
				if (left.bytes[i] == right.bytes[i] && left.IntValue == right.IntValue)
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator ==(CharCodeOld left, CharCodeOld right)
		{
			CharCodeOld.InsureCharCodes(left, right);
			for (int i = 0; i < left.bytes.Length; i++)
			{
				if (left.bytes[i] != right.bytes[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator >(CharCodeOld left, CharCodeOld right)
		{
			CharCodeOld.InsureCharCodes(left, right);
			for (int i = 0; i < left.bytes.Length; i++)
			{
				if (left.bytes[i] <= right.bytes[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator !=(CharCodeOld left, CharCodeOld right)
		{
			return !(left == right);
		}

		public static bool operator <=(CharCodeOld left, CharCodeOld right)
		{
			return left < right || left == right;
		}

		public static bool operator >=(CharCodeOld left, CharCodeOld right)
		{
			return left > right || left == right;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<");
			for (int i = 0; i < this.bytes.Length; i++)
			{
				stringBuilder.AppendFormat("{0:X2}", this.bytes[i]);
			}
			stringBuilder.Append("> ");
			stringBuilder.Append(this.GetHashCode());
			return stringBuilder.ToString();
		}

		public override bool Equals(object obj)
		{
			if (this.bytes == null)
			{
				return false;
			}
			if (obj is CharCodeOld)
			{
				CharCodeOld right = (CharCodeOld)obj;
				return this.BytesCount == right.BytesCount && this == right;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this.IntValue;
		}

		readonly byte[] bytes;

		readonly int intValue;
	}
}
