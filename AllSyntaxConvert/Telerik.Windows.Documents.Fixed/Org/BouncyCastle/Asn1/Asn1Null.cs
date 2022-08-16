using System;

namespace Org.BouncyCastle.Asn1
{
	abstract class Asn1Null : Asn1Object
	{
		internal Asn1Null()
		{
		}

		public override string ToString()
		{
			return "NULL";
		}
	}
}
