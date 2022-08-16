using System;

namespace Org.BouncyCastle.Asn1
{
	class BerSetParser : Asn1SetParser, IAsn1Convertible
	{
		internal BerSetParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		public Asn1Object ToAsn1Object()
		{
			return new BerSet(this._parser.ReadVector(), false);
		}

		readonly Asn1StreamParser _parser;
	}
}
