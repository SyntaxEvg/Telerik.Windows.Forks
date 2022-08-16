using System;
using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class DerBitString : DerStringBase
	{
		public static DerBitString GetInstance(object obj)
		{
			if (obj == null || obj is DerBitString)
			{
				return (DerBitString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		public static DerBitString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBitString)
			{
				return DerBitString.GetInstance(@object);
			}
			return DerBitString.FromAsn1Octets(((Asn1OctetString)@object).GetOctets());
		}

		public DerBitString(byte[] data, int padBits)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (padBits < 0 || padBits > 7)
			{
				throw new ArgumentException("must be in the range 0 to 7", "padBits");
			}
			if (data.Length == 0 && padBits != 0)
			{
				throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");
			}
			this.mData = Arrays.Clone(data);
			this.mPadBits = padBits;
		}

		public DerBitString(byte[] data)
			: this(data, 0)
		{
		}

		public DerBitString(int namedBits)
		{
			if (namedBits == 0)
			{
				this.mData = new byte[0];
				this.mPadBits = 0;
				return;
			}
			int num = BigInteger.BitLen(namedBits);
			int num2 = (num + 7) / 8;
			byte[] array = new byte[num2];
			num2--;
			for (int i = 0; i < num2; i++)
			{
				array[i] = (byte)namedBits;
				namedBits >>= 8;
			}
			array[num2] = (byte)namedBits;
			int num3 = 0;
			while ((namedBits & (1 << num3)) == 0)
			{
				num3++;
			}
			this.mData = array;
			this.mPadBits = num3;
		}

		public DerBitString(Asn1Encodable obj)
			: this(obj.GetDerEncoded())
		{
		}

		public virtual byte[] GetOctets()
		{
			if (this.mPadBits != 0)
			{
				throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
			}
			return Arrays.Clone(this.mData);
		}

		public virtual byte[] GetBytes()
		{
			byte[] array = Arrays.Clone(this.mData);
			if (this.mPadBits > 0)
			{
				byte[] array2 = array;
				int num = array.Length - 1;
				array2[num] &= (byte)(255 << this.mPadBits);
			}
			return array;
		}

		public virtual int PadBits
		{
			get
			{
				return this.mPadBits;
			}
		}

		public virtual int IntValue
		{
			get
			{
				int num = 0;
				int num2 = System.Math.Min(4, this.mData.Length);
				for (int i = 0; i < num2; i++)
				{
					num |= (int)this.mData[i] << 8 * i;
				}
				if (this.mPadBits > 0 && num2 == this.mData.Length)
				{
					int num3 = (1 << this.mPadBits) - 1;
					num &= ~(num3 << 8 * (num2 - 1));
				}
				return num;
			}
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (this.mPadBits > 0)
			{
				int num = (int)this.mData[this.mData.Length - 1];
				int num2 = (1 << this.mPadBits) - 1;
				int num3 = num & num2;
				if (num3 != 0)
				{
					byte[] array = Arrays.Prepend(this.mData, (byte)this.mPadBits);
					array[array.Length - 1] = (byte)(num ^ num3);
					derOut.WriteEncoded(3, array);
					return;
				}
			}
			derOut.WriteEncoded(3, (byte)this.mPadBits, this.mData);
		}

		protected override int Asn1GetHashCode()
		{
			return this.mPadBits.GetHashCode() ^ Arrays.GetHashCode(this.mData);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBitString derBitString = asn1Object as DerBitString;
			return derBitString != null && this.mPadBits == derBitString.mPadBits && Arrays.AreEqual(this.mData, derBitString.mData);
		}

		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder("#");
			byte[] derEncoded = base.GetDerEncoded();
			for (int num = 0; num != derEncoded.Length; num++)
			{
				uint num2 = (uint)derEncoded[num];
				stringBuilder.Append(DerBitString.table[(int)((UIntPtr)((num2 >> 4) & 15U))]);
				stringBuilder.Append(DerBitString.table[(int)(derEncoded[num] & 15)]);
			}
			return stringBuilder.ToString();
		}

		internal static DerBitString FromAsn1Octets(byte[] octets)
		{
			if (octets.Length < 1)
			{
				throw new ArgumentException("truncated BIT STRING detected", "octets");
			}
			int num = (int)octets[0];
			byte[] array = Arrays.CopyOfRange(octets, 1, octets.Length);
			if (num > 0 && num < 8 && array.Length > 0)
			{
				int num2 = (int)array[array.Length - 1];
				int num3 = (1 << num) - 1;
				if ((num2 & num3) != 0)
				{
					return new BerBitString(array, num);
				}
			}
			return new DerBitString(array, num);
		}

		static readonly char[] table = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		protected readonly byte[] mData;

		protected readonly int mPadBits;
	}
}
