using System;

namespace Org.BouncyCastle.Asn1
{
	class BerApplicationSpecific : DerApplicationSpecific
	{
		public BerApplicationSpecific(int tagNo, Asn1EncodableVector vec)
			: base(tagNo, vec)
		{
		}
	}
}
