﻿using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class BerTaggedObject : DerTaggedObject
	{
		public BerTaggedObject(int tagNo, Asn1Encodable obj)
			: base(tagNo, obj)
		{
		}

		public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
			: base(explicitly, tagNo, obj)
		{
		}

		public BerTaggedObject(int tagNo)
			: base(false, tagNo, BerSequence.Empty)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteTag(160, this.tagNo);
				derOut.WriteByte(128);
				if (!base.IsEmpty())
				{
					if (!this.explicitly)
					{
						IEnumerable enumerable;
						if (this.obj is Asn1OctetString)
						{
							if (this.obj is BerOctetString)
							{
								enumerable = (BerOctetString)this.obj;
							}
							else
							{
								Asn1OctetString asn1OctetString = (Asn1OctetString)this.obj;
								enumerable = new BerOctetString(asn1OctetString.GetOctets());
							}
						}
						else if (this.obj is Asn1Sequence)
						{
							enumerable = (Asn1Sequence)this.obj;
						}
						else
						{
							if (!(this.obj is Asn1Set))
							{
								throw Platform.CreateNotImplementedException(Platform.GetTypeName(this.obj));
							}
							enumerable = (Asn1Set)this.obj;
						}
                        foreach (var obj in enumerable)
                        {
							Asn1Encodable obj2 = (Asn1Encodable)obj;
							derOut.WriteObject(obj2);
						}
						
					}
					derOut.WriteObject(this.obj);
				}
				IL_114:
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}
	}
}
