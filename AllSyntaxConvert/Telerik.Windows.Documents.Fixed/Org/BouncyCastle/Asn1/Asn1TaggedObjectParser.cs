using System;

namespace Org.BouncyCastle.Asn1
{
	interface Asn1TaggedObjectParser : IAsn1Convertible
	{
		int TagNo { get; }

		IAsn1Convertible GetObjectParser(int tag, bool isExplicit);
	}
}
