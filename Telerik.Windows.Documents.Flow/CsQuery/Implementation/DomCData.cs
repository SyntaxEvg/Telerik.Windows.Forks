using System;

namespace CsQuery.Implementation
{
	class DomCData : DomObject<DomCData>, IDomCData, IDomSpecialElement, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public DomCData()
		{
			this._NonAttributeData = "";
		}

		public DomCData(string value)
		{
			this.NodeValue = value;
		}

		public override string NodeValue
		{
			get
			{
				return this.NonAttributeData;
			}
			set
			{
				this.NonAttributeData = value;
			}
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.CDATA_SECTION_NODE;
			}
		}

		public string NonAttributeData
		{
			get
			{
				return this._NonAttributeData;
			}
			set
			{
				this._NonAttributeData = value ?? "";
			}
		}

		public override bool InnerHtmlAllowed
		{
			get
			{
				return false;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.NonAttributeData;
			}
			set
			{
				this.NonAttributeData = value;
			}
		}

		public override DomCData Clone()
		{
			return new DomCData
			{
				NonAttributeData = this.NonAttributeData
			};
		}

		IDomNode IDomNode.Clone()
		{
			return this.Clone();
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		string _NonAttributeData;
	}
}
