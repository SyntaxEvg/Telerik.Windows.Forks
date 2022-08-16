using System;
using CsQuery.Utility;

namespace CsQuery.Implementation
{
	class HtmlAnchorElement : DomElement, IHTMLAnchorElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		public HtmlAnchorElement()
			: base(39)
		{
		}

		public string Target
		{
			get
			{
				return this.GetAttribute("target");
			}
			set
			{
				this.SetAttribute("target", value);
			}
		}

		public string Href
		{
			get
			{
				return this.GetAttribute("href");
			}
			set
			{
				this.SetAttribute("href", value);
			}
		}

		public RelAnchor Rel
		{
			get
			{
				return Support.AttributeToEnum<RelAnchor>(this.GetAttribute("rel"));
			}
			set
			{
				this.SetAttribute("rel", Support.EnumToAttribute(value));
			}
		}

		public string Hreflang
		{
			get
			{
				return this.GetAttribute("hreflang");
			}
			set
			{
				this.SetAttribute("hreflang", value);
			}
		}

		public string Media
		{
			get
			{
				return this.GetAttribute("media");
			}
			set
			{
				this.SetAttribute("media", value);
			}
		}
	}
}
