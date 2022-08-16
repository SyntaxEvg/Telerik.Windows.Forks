using System;

namespace Org.BouncyCastle.Asn1
{
	interface IAsn1ApplicationSpecificParser : IAsn1Convertible
	{
		IAsn1Convertible ReadObject();
	}
}
