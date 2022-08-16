using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class DerGraphicString : DerStringBase
	{
		public static DerGraphicString GetInstance(object obj)
		{
			if (obj == null || obj is DerGraphicString)
			{
				return (DerGraphicString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerGraphicString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		public static DerGraphicString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGraphicString)
			{
				return DerGraphicString.GetInstance(@object);
			}
			return new DerGraphicString(((Asn1OctetString)@object).GetOctets());
		}

		public DerGraphicString(byte[] encoding)
		{
			this.mString = Arrays.Clone(encoding);
		}

		public override string GetString()
		{
			return Strings.FromByteArray(this.mString);
		}

		public byte[] GetOctets()
		{
			return Arrays.Clone(this.mString);
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(25, this.mString);
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGraphicString derGraphicString = asn1Object as DerGraphicString;
			return derGraphicString != null && Arrays.AreEqual(this.mString, derGraphicString.mString);
		}

		readonly byte[] mString;
	}
}
