using System;

namespace Org.BouncyCastle.Asn1
{
	class BerNull : DerNull
	{
		[Obsolete("Use static Instance object")]
		public BerNull()
		{
		}

		BerNull(int dummy)
			: base(dummy)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(5);
				return;
			}
			base.Encode(derOut);
		}

		public new static readonly BerNull Instance = new BerNull(0);
	}
}
