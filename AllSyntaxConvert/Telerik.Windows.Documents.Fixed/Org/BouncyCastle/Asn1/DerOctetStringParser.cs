﻿using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		internal DerOctetStringParser(DefiniteLengthInputStream stream)
		{
			this.stream = stream;
		}

		public Stream GetOctetStream()
		{
			return this.stream;
		}

		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new DerOctetString(this.stream.ToArray());
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		readonly DefiniteLengthInputStream stream;
	}
}
