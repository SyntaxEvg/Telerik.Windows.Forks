using System;

namespace Org.BouncyCastle.Asn1
{
	interface Asn1SetParser : IAsn1Convertible
	{
		IAsn1Convertible ReadObject();
	}
}
