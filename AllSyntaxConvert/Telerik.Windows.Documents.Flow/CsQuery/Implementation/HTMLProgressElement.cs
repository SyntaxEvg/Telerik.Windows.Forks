using System;
using CsQuery.Utility;

namespace CsQuery.Implementation
{
	class HTMLProgressElement : DomElement, IHTMLProgressElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		public HTMLProgressElement()
			: base(45)
		{
		}

		public new int Value
		{
			get
			{
				return Support.IntOrZero(base.GetAttribute(4));
			}
			set
			{
				base.SetAttribute(4, value.ToString());
			}
		}

		public double Max
		{
			get
			{
				return Support.DoubleOrZero(this.GetAttribute("max"));
			}
			set
			{
				this.SetAttribute("max", value.ToString());
			}
		}

		public double Position
		{
			get
			{
				if (!this.HasAttribute("value"))
				{
					return 1.0;
				}
				return (double)this.Value / this.Max;
			}
		}

		public INodeList<IHTMLLabelElement> Labels
		{
			get
			{
				return new NodeList<IHTMLLabelElement>(base.ChildElementsOfTag<IHTMLLabelElement>(46));
			}
		}
	}
}
