using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class BerOctetString : DerOctetString, IEnumerable
	{
		public static BerOctetString FromSequence(Asn1Sequence seq)
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Encodable value = (Asn1Encodable)obj;
				list.Add(value);
			}
			return new BerOctetString(list);
		}

		static byte[] ToBytes(IEnumerable octs)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in octs)
			{
				DerOctetString derOctetString = (DerOctetString)obj;
				byte[] octets = derOctetString.GetOctets();
				memoryStream.Write(octets, 0, octets.Length);
			}
			return memoryStream.ToArray();
		}

		public BerOctetString(byte[] str)
			: base(str)
		{
		}

		public BerOctetString(IEnumerable octets)
			: base(BerOctetString.ToBytes(octets))
		{
			this.octs = octets;
		}

		public BerOctetString(Asn1Object obj)
			: base(obj)
		{
		}

		public BerOctetString(Asn1Encodable obj)
			: base(obj.ToAsn1Object())
		{
		}

		public override byte[] GetOctets()
		{
			return this.str;
		}

		public IEnumerator GetEnumerator()
		{
			if (this.octs == null)
			{
				return this.GenerateOcts().GetEnumerator();
			}
			return this.octs.GetEnumerator();
		}

		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		IList GenerateOcts()
		{
			IList list = Platform.CreateArrayList();
			for (int i = 0; i < this.str.Length; i += 1000)
			{
				int num = System.Math.Min(this.str.Length, i + 1000);
				byte[] array = new byte[num - i];
				Array.Copy(this.str, i, array, 0, array.Length);
				list.Add(new DerOctetString(array));
			}
			return list;
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(36);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					DerOctetString obj2 = (DerOctetString)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		const int MaxLength = 1000;

		readonly IEnumerable octs;
	}
}
