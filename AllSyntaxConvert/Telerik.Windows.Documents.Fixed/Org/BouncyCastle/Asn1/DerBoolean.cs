using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class DerBoolean : Asn1Object
	{
		public static DerBoolean GetInstance(object obj)
		{
			if (obj == null || obj is DerBoolean)
			{
				return (DerBoolean)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		public static DerBoolean GetInstance(bool value)
		{
			if (!value)
			{
				return DerBoolean.False;
			}
			return DerBoolean.True;
		}

		public static DerBoolean GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBoolean)
			{
				return DerBoolean.GetInstance(@object);
			}
			return DerBoolean.FromOctetString(((Asn1OctetString)@object).GetOctets());
		}

		public DerBoolean(byte[] val)
		{
			if (val.Length != 1)
			{
				throw new ArgumentException("byte value should have 1 byte in it", "val");
			}
			this.value = val[0];
		}

		DerBoolean(bool value)
		{
			this.value = ((byte)(value ? byte.MaxValue : 0));
		}

		public bool IsTrue
		{
			get
			{
				return this.value != 0;
			}
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(1, new byte[] { this.value });
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBoolean derBoolean = asn1Object as DerBoolean;
			return derBoolean != null && this.IsTrue == derBoolean.IsTrue;
		}

		protected override int Asn1GetHashCode()
		{
			return this.IsTrue.GetHashCode();
		}

		public override string ToString()
		{
			if (!this.IsTrue)
			{
				return "FALSE";
			}
			return "TRUE";
		}

		internal static DerBoolean FromOctetString(byte[] value)
		{
			if (value.Length != 1)
			{
				throw new ArgumentException("BOOLEAN value should have 1 byte in it", "value");
			}
			byte b = value[0];
			if (b == 0)
			{
				return DerBoolean.False;
			}
			if (b != 255)
			{
				return new DerBoolean(value);
			}
			return DerBoolean.True;
		}

		readonly byte value;

		public static readonly DerBoolean False = new DerBoolean(false);

		public static readonly DerBoolean True = new DerBoolean(true);
	}
}
