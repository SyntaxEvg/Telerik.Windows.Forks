﻿using System;

namespace Org.BouncyCastle.Asn1
{
	class DerOctetString : Asn1OctetString
	{
		public DerOctetString(byte[] str)
			: base(str)
		{
		}

		public DerOctetString(Asn1Encodable obj)
			: base(obj)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(4, this.str);
		}

		internal static void Encode(DerOutputStream derOut, byte[] bytes, int offset, int length)
		{
			derOut.WriteEncoded(4, bytes, offset, length);
		}
	}
}
