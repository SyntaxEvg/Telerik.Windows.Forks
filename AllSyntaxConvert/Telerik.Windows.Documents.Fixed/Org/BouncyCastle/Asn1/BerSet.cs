﻿using System;

namespace Org.BouncyCastle.Asn1
{
	class BerSet : DerSet
	{
		public new static BerSet FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v);
			}
			return BerSet.Empty;
		}

		internal new static BerSet FromVector(Asn1EncodableVector v, bool needsSorting)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v, needsSorting);
			}
			return BerSet.Empty;
		}

		public BerSet()
		{
		}

		public BerSet(Asn1Encodable obj)
			: base(obj)
		{
		}

		public BerSet(Asn1EncodableVector v)
			: base(v, false)
		{
		}

		internal BerSet(Asn1EncodableVector v, bool needsSorting)
			: base(v, needsSorting)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(49);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		public new static readonly BerSet Empty = new BerSet();
	}
}
