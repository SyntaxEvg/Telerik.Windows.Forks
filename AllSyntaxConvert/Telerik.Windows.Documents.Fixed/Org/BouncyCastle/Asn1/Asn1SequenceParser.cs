using System;

namespace Org.BouncyCastle.Asn1
{
	interface Asn1SequenceParser : IAsn1Convertible
	{
		IAsn1Convertible ReadObject();
	}
}
