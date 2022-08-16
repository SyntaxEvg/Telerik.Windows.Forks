﻿using System;

namespace Org.BouncyCastle.Asn1
{
	class DerNull : Asn1Null
	{
		[Obsolete("Use static Instance object")]
		public DerNull()
		{
		}

		protected internal DerNull(int dummy)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(5, this.zeroBytes);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			return asn1Object is DerNull;
		}

		protected override int Asn1GetHashCode()
		{
			return -1;
		}

		public static readonly DerNull Instance = new DerNull(0);

		byte[] zeroBytes = new byte[0];
	}
}
