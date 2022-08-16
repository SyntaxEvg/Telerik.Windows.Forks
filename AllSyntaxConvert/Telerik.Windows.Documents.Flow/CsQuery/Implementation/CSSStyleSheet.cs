using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	class CSSStyleSheet : ICSSStyleSheet
	{
		public CSSStyleSheet(IDomElement ownerNode)
		{
			this.OwnerNode = ownerNode;
		}

		public bool Disabled { get; set; }

		public string Href
		{
			get
			{
				if (this.OwnerNode != null)
				{
					return this.OwnerNode["href"];
				}
				return null;
			}
			set
			{
				if (this.OwnerNode == null)
				{
					throw new InvalidOperationException("This CSSStyleSheet is not bound to an element node.");
				}
				this.OwnerNode["href"] = value;
			}
		}

		public IDomElement OwnerNode { get; protected set; }

		public string Type
		{
			get
			{
				return "text/css";
			}
		}

		public IList<ICSSRule> CssRules
		{
			get
			{
				if (this._Rules == null)
				{
					this._Rules = new List<ICSSRule>();
				}
				return this._Rules;
			}
		}

		IList<ICSSRule> _Rules;
	}
}
