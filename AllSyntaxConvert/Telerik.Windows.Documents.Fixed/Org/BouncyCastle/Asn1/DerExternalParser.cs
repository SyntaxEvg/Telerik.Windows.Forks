using System;

namespace Org.BouncyCastle.Asn1
{
	class DerExternalParser : Asn1Encodable
	{
		public DerExternalParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerExternal(this._parser.ReadVector());
		}

		readonly Asn1StreamParser _parser;
	}
}
