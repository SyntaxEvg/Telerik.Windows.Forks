using System;

namespace CsQuery.Implementation
{
	class DomText : DomObject<DomText>, IDomText, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public DomText()
		{
		}

		public DomText(string nodeValue)
		{
			this.NodeValue = nodeValue;
		}

		public override string NodeName
		{
			get
			{
				return "#text";
			}
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.TEXT_NODE;
			}
		}

		public override string NodeValue
		{
			get
			{
				return this._NodeValue ?? "";
			}
			set
			{
				this._NodeValue = value;
			}
		}

		public override DomText Clone()
		{
			return new DomText(this.NodeValue);
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

		public override string ToString()
		{
			return this.NodeValue;
		}

		protected string _NodeValue;
	}
}
