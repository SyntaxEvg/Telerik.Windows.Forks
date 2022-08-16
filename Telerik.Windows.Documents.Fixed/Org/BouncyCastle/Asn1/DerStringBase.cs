using System;

namespace Org.BouncyCastle.Asn1
{
	abstract class DerStringBase : Asn1Object, IAsn1String
	{
		public abstract string GetString();

		public override string ToString()
		{
			return this.GetString();
		}

		protected override int Asn1GetHashCode()
		{
			return this.GetString().GetHashCode();
		}
	}
}
