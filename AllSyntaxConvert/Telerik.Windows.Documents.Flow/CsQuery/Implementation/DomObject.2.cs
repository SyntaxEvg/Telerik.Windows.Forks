using System;

namespace CsQuery.Implementation
{
	abstract class DomObject<T> : DomObject, IDomObject<T>, IDomObject, IDomNode, ICloneable, IComparable<IDomObject> where T : IDomObject, new()
	{
		public DomObject()
		{
		}

		public new abstract T Clone();

		protected override IDomObject CloneImplementation()
		{
			return this.Clone();
		}

		IDomNode IDomNode.Clone()
		{
			return this.Clone();
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}
	}
}
