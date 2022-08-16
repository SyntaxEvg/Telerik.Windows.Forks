using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class DerVideotexString : DerStringBase
	{
		public static DerVideotexString GetInstance(object obj)
		{
			if (obj == null || obj is DerVideotexString)
			{
				return (DerVideotexString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerVideotexString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		public static DerVideotexString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerVideotexString)
			{
				return DerVideotexString.GetInstance(@object);
			}
			return new DerVideotexString(((Asn1OctetString)@object).GetOctets());
		}

		public DerVideotexString(byte[] encoding)
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
			derOut.WriteEncoded(21, this.mString);
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerVideotexString derVideotexString = asn1Object as DerVideotexString;
			return derVideotexString != null && Arrays.AreEqual(this.mString, derVideotexString.mString);
		}

		readonly byte[] mString;
	}
}
