using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	class Asn1EncodableVector : IEnumerable
	{
		public static Asn1EncodableVector FromEnumerable(IEnumerable e)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[0]);
			foreach (object obj in e)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				asn1EncodableVector.Add(new Asn1Encodable[] { asn1Encodable });
			}
			return asn1EncodableVector;
		}

		public Asn1EncodableVector(params Asn1Encodable[] v)
		{
			this.Add(v);
		}

		public void Add(params Asn1Encodable[] objs)
		{
			foreach (Asn1Encodable value in objs)
			{
				this.v.Add(value);
			}
		}

		public void AddOptional(params Asn1Encodable[] objs)
		{
			if (objs != null)
			{
				foreach (Asn1Encodable asn1Encodable in objs)
				{
					if (asn1Encodable != null)
					{
						this.v.Add(asn1Encodable);
					}
				}
			}
		}

		public Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this.v[index];
			}
		}

		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable Get(int index)
		{
			return this[index];
		}

		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.v.Count;
			}
		}

		public int Count
		{
			get
			{
				return this.v.Count;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return this.v.GetEnumerator();
		}

		IList v = Platform.CreateArrayList();
	}
}
