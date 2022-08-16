﻿using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class DerIA5String : DerStringBase
	{
		public static DerIA5String GetInstance(object obj)
		{
			if (obj == null || obj is DerIA5String)
			{
				return (DerIA5String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		public static DerIA5String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerIA5String)
			{
				return DerIA5String.GetInstance(@object);
			}
			return new DerIA5String(((Asn1OctetString)@object).GetOctets());
		}

		public DerIA5String(byte[] str)
			: this(Strings.FromAsciiByteArray(str), false)
		{
		}

		public DerIA5String(string str)
			: this(str, false)
		{
		}

		public DerIA5String(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerIA5String.IsIA5String(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		public override string GetString()
		{
			return this.str;
		}

		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(22, this.GetOctets());
		}

		protected override int Asn1GetHashCode()
		{
			return this.str.GetHashCode();
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerIA5String derIA5String = asn1Object as DerIA5String;
			return derIA5String != null && this.str.Equals(derIA5String.str);
		}

		public static bool IsIA5String(string str)
		{
			foreach (char c in str)
			{
				if (c > '\u007f')
				{
					return false;
				}
			}
			return true;
		}

		readonly string str;
	}
}
