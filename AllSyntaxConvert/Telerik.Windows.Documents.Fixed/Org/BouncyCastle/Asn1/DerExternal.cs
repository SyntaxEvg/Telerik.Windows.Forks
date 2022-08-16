using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	class DerExternal : Asn1Object
	{
		public DerExternal(Asn1EncodableVector vector)
		{
			int num = 0;
			Asn1Object objFromVector = DerExternal.GetObjFromVector(vector, num);
			if (objFromVector is DerObjectIdentifier)
			{
				this.directReference = (DerObjectIdentifier)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (objFromVector is DerInteger)
			{
				this.indirectReference = (DerInteger)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				this.dataValueDescriptor = objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (vector.Count != num + 1)
			{
				throw new ArgumentException("input vector too large", "vector");
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				throw new ArgumentException("No tagged object found in vector. Structure doesn't seem to be of type External", "vector");
			}
			Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)objFromVector;
			this.Encoding = asn1TaggedObject.TagNo;
			if (this.encoding < 0 || this.encoding > 2)
			{
				throw new InvalidOperationException("invalid encoding value");
			}
			this.externalContent = asn1TaggedObject.GetObject();
		}

		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, DerTaggedObject externalData)
			: this(directReference, indirectReference, dataValueDescriptor, externalData.TagNo, externalData.ToAsn1Object())
		{
		}

		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, int encoding, Asn1Object externalData)
		{
			this.DirectReference = directReference;
			this.IndirectReference = indirectReference;
			this.DataValueDescriptor = dataValueDescriptor;
			this.Encoding = encoding;
			this.ExternalContent = externalData.ToAsn1Object();
		}

		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerExternal.WriteEncodable(memoryStream, this.directReference);
			DerExternal.WriteEncodable(memoryStream, this.indirectReference);
			DerExternal.WriteEncodable(memoryStream, this.dataValueDescriptor);
			DerExternal.WriteEncodable(memoryStream, new DerTaggedObject(8, this.externalContent));
			derOut.WriteEncoded(32, 8, memoryStream.ToArray());
		}

		protected override int Asn1GetHashCode()
		{
			int num = this.externalContent.GetHashCode();
			if (this.directReference != null)
			{
				num ^= this.directReference.GetHashCode();
			}
			if (this.indirectReference != null)
			{
				num ^= this.indirectReference.GetHashCode();
			}
			if (this.dataValueDescriptor != null)
			{
				num ^= this.dataValueDescriptor.GetHashCode();
			}
			return num;
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (this == asn1Object)
			{
				return true;
			}
			DerExternal derExternal = asn1Object as DerExternal;
			return derExternal != null && (object.Equals(this.directReference, derExternal.directReference) && object.Equals(this.indirectReference, derExternal.indirectReference) && object.Equals(this.dataValueDescriptor, derExternal.dataValueDescriptor)) && this.externalContent.Equals(derExternal.externalContent);
		}

		public Asn1Object DataValueDescriptor
		{
			get
			{
				return this.dataValueDescriptor;
			}
			set
			{
				this.dataValueDescriptor = value;
			}
		}

		public DerObjectIdentifier DirectReference
		{
			get
			{
				return this.directReference;
			}
			set
			{
				this.directReference = value;
			}
		}

		public int Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (this.encoding < 0 || this.encoding > 2)
				{
					throw new InvalidOperationException("invalid encoding value: " + this.encoding);
				}
				this.encoding = value;
			}
		}

		public Asn1Object ExternalContent
		{
			get
			{
				return this.externalContent;
			}
			set
			{
				this.externalContent = value;
			}
		}

		public DerInteger IndirectReference
		{
			get
			{
				return this.indirectReference;
			}
			set
			{
				this.indirectReference = value;
			}
		}

		static Asn1Object GetObjFromVector(Asn1EncodableVector v, int index)
		{
			if (v.Count <= index)
			{
				throw new ArgumentException("too few objects in input vector", "v");
			}
			return v[index].ToAsn1Object();
		}

		static void WriteEncodable(MemoryStream ms, Asn1Encodable e)
		{
			if (e != null)
			{
				byte[] derEncoded = e.GetDerEncoded();
				ms.Write(derEncoded, 0, derEncoded.Length);
			}
		}

		DerObjectIdentifier directReference;

		DerInteger indirectReference;

		Asn1Object dataValueDescriptor;

		int encoding;

		Asn1Object externalContent;
	}
}
