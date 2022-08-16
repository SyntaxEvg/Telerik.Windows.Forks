using System;

namespace CsQuery.Implementation
{
	class DomComment : DomObject<DomComment>, IDomComment, IDomSpecialElement, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public DomComment()
		{
			this._NonAttributeData = "";
		}

		public DomComment(string text)
		{
			this.NodeValue = text;
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.COMMENT_NODE;
			}
		}

		public override string NodeName
		{
			get
			{
				return "#comment";
			}
		}

		public bool IsQuoted { get; set; }

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

		public override DomComment Clone()
		{
			return new DomComment
			{
				NonAttributeData = this.NonAttributeData,
				IsQuoted = this.IsQuoted
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
