using System;

namespace CsQuery.Implementation
{
	class HTMLOptionElement : DomElement, IHTMLOptionElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		public HTMLOptionElement()
			: base(11)
		{
		}

		public override string Value
		{
			get
			{
				string attribute = base.GetAttribute(4, null);
				return attribute ?? this.InnerText;
			}
			set
			{
				base.Value = value;
			}
		}

		public override bool Disabled
		{
			get
			{
				if (base.HasAttribute(47))
				{
					return true;
				}
				if (this.ParentNode.NodeNameID == 11 || this.ParentNode.NodeNameID == 24)
				{
					bool flag = ((DomElement)this.ParentNode).HasAttribute(47);
					return flag || (this.ParentNode.ParentNode.NodeNameID == 24 && ((DomElement)this.ParentNode.ParentNode).HasAttribute(47));
				}
				return false;
			}
			set
			{
				base.SetProp(47, value);
			}
		}

		public IHTMLFormElement Form
		{
			get
			{
				IHTMLSelectElement ihtmlselectElement = this.OptionOwner();
				if (ihtmlselectElement != null)
				{
					return ihtmlselectElement.Form;
				}
				return null;
			}
		}

		public string Label
		{
			get
			{
				return base.GetAttribute(46);
			}
			set
			{
				base.SetAttribute(46, value);
			}
		}

		public override bool Selected
		{
			get
			{
				IHTMLSelectElement ihtmlselectElement = this.OptionOwner();
				if (ihtmlselectElement != null)
				{
					return base.HasAttribute(6) || this.OwnerSelectOptions(ihtmlselectElement).SelectedItem == this;
				}
				return base.HasAttribute(6);
			}
			set
			{
				IHTMLSelectElement ihtmlselectElement = this.OptionOwner();
				if (ihtmlselectElement == null)
				{
					base.SetAttribute(6);
					return;
				}
				if (value)
				{
					this.OwnerSelectOptions(ihtmlselectElement).SelectedItem = this;
					return;
				}
				base.RemoveAttribute(6);
			}
		}

		IHTMLSelectElement OptionOwner()
		{
			IDomContainer domContainer = ((this.ParentNode == null) ? null : ((this.ParentNode.NodeNameID == 10) ? this.ParentNode : ((this.ParentNode.ParentNode == null) ? null : ((this.ParentNode.ParentNode.NodeNameID == 10) ? this.ParentNode.ParentNode : null))));
			return (IHTMLSelectElement)domContainer;
		}

		HTMLOptionsCollection OwnerSelectOptions()
		{
			IHTMLSelectElement ihtmlselectElement = this.OptionOwner();
			if (ihtmlselectElement == null)
			{
				throw new InvalidOperationException("The selected property only applies to valid OPTION nodes within a SELECT node");
			}
			return this.OwnerSelectOptions(ihtmlselectElement);
		}

		HTMLOptionsCollection OwnerSelectOptions(IDomElement owner)
		{
			return new HTMLOptionsCollection(owner);
		}
	}
}
