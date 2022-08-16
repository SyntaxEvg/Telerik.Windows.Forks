using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	sealed class StackNode<T>
	{
		public TaintableLocator Locator
		{
			get
			{
				return this.locator;
			}
		}

		public int Flags
		{
			get
			{
				return this.flags;
			}
		}

		public DispatchGroup Group
		{
			get
			{
				return (DispatchGroup)(this.flags & 127);
			}
		}

		public bool IsScoping
		{
			get
			{
				return (this.flags & 134217728) != 0;
			}
		}

		public bool IsSpecial
		{
			get
			{
				return (this.flags & 536870912) != 0;
			}
		}

		public bool IsFosterParenting
		{
			get
			{
				return (this.flags & 268435456) != 0;
			}
		}

		public bool IsHtmlIntegrationPoint
		{
			get
			{
				return (this.flags & 16777216) != 0;
			}
		}

		public bool IsOptionalEndTag
		{
			get
			{
				return (this.flags & 8388608) != 0;
			}
		}

		internal StackNode(int flags, [NsUri] string ns, [Local] string name, T node, [Local] string popName, HtmlAttributes attributes, TaintableLocator locator)
		{
			this.flags = flags;
			this.name = name;
			this.popName = popName;
			this.ns = ns;
			this.node = node;
			this.attributes = attributes;
			this.refcount = 1;
			this.locator = locator;
		}

		internal StackNode(ElementName elementName, T node, TaintableLocator locator)
		{
			this.flags = elementName.Flags;
			this.name = elementName.name;
			this.popName = elementName.name;
			this.ns = "http://www.w3.org/1999/xhtml";
			this.node = node;
			this.attributes = null;
			this.refcount = 1;
			this.locator = locator;
		}

		internal StackNode(ElementName elementName, T node, HtmlAttributes attributes, TaintableLocator locator)
		{
			this.flags = elementName.Flags;
			this.name = elementName.name;
			this.popName = elementName.name;
			this.ns = "http://www.w3.org/1999/xhtml";
			this.node = node;
			this.attributes = attributes;
			this.refcount = 1;
			this.locator = locator;
		}

		internal StackNode(ElementName elementName, T node, [Local] string popName, TaintableLocator locator)
		{
			this.flags = elementName.Flags;
			this.name = elementName.name;
			this.popName = popName;
			this.ns = "http://www.w3.org/1999/xhtml";
			this.node = node;
			this.attributes = null;
			this.refcount = 1;
			this.locator = locator;
		}

		internal StackNode(ElementName elementName, [Local] string popName, T node, TaintableLocator locator)
		{
			this.flags = StackNode<T>.PrepareSvgFlags(elementName.Flags);
			this.name = elementName.name;
			this.popName = popName;
			this.ns = "http://www.w3.org/2000/svg";
			this.node = node;
			this.attributes = null;
			this.refcount = 1;
			this.locator = locator;
		}

		internal StackNode(ElementName elementName, T node, [Local] string popName, bool markAsIntegrationPoint, TaintableLocator locator)
		{
			this.flags = StackNode<T>.PrepareMathFlags(elementName.Flags, markAsIntegrationPoint);
			this.name = elementName.name;
			this.popName = popName;
			this.ns = "http://www.w3.org/1998/Math/MathML";
			this.node = node;
			this.attributes = null;
			this.refcount = 1;
			this.locator = locator;
		}

		static int PrepareSvgFlags(int flags)
		{
			flags &= -947912705;
			if ((flags & 67108864) != 0)
			{
				flags |= 687865856;
			}
			return flags;
		}

		static int PrepareMathFlags(int flags, bool markAsIntegrationPoint)
		{
			flags &= -947912705;
			if ((flags & 33554432) != 0)
			{
				flags |= 671088640;
			}
			if (markAsIntegrationPoint)
			{
				flags |= 16777216;
			}
			return flags;
		}

		public void DropAttributes()
		{
			this.attributes = null;
		}

		public override string ToString()
		{
			return this.name;
		}

		public void Retain()
		{
			this.refcount++;
		}

		public void Release()
		{
			this.refcount--;
		}

		readonly int flags;

		[Local]
		internal readonly string name;

		[Local]
		internal readonly string popName;

		[NsUri]
		internal readonly string ns;

		internal readonly T node;

		internal HtmlAttributes attributes;

		int refcount = 1;

		readonly TaintableLocator locator;
	}
}
