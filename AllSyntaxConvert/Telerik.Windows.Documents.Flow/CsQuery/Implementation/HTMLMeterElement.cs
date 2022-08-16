using System;
using CsQuery.Utility;

namespace CsQuery.Implementation
{
	class HTMLMeterElement : DomElement, IHTMLMeterElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		public HTMLMeterElement()
			: base(48)
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

		public double Min
		{
			get
			{
				return Support.DoubleOrZero(this.GetAttribute("min"));
			}
			set
			{
				this.SetAttribute("min", value.ToString());
			}
		}

		public double Low
		{
			get
			{
				return Support.DoubleOrZero(this.GetAttribute("low"));
			}
			set
			{
				this.SetAttribute("low", value.ToString());
			}
		}

		public double High
		{
			get
			{
				return Support.DoubleOrZero(this.GetAttribute("high"));
			}
			set
			{
				this.SetAttribute("high", value.ToString());
			}
		}

		public double Optimum
		{
			get
			{
				return Support.DoubleOrZero(this.GetAttribute("optimum"));
			}
			set
			{
				this.SetAttribute("optimum", value.ToString());
			}
		}

		public INodeList<IDomElement> Labels
		{
			get
			{
				return new NodeList<IDomElement>(base.ChildElementsOfTag<IDomElement>(46));
			}
		}
	}
}
