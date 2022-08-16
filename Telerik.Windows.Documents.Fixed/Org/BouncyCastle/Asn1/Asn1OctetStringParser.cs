using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	interface Asn1OctetStringParser : IAsn1Convertible
	{
		Stream GetOctetStream();
	}
}
