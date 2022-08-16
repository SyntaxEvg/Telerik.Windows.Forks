using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	class BerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		internal BerOctetStringParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		public Stream GetOctetStream()
		{
			return new ConstructedOctetStream(this._parser);
		}

		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new BerOctetString(Streams.ReadAll(this.GetOctetStream()));
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		readonly Asn1StreamParser _parser;
	}
}
