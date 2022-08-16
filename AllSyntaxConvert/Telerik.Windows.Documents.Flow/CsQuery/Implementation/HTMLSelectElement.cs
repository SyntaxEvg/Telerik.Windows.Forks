using System;
using System.Linq;

namespace CsQuery.Implementation
{
	class HTMLSelectElement : FormSubmittableElement, IHTMLSelectElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		public HTMLSelectElement()
			: base(10)
		{
		}

		public IHTMLOptionsCollection Options
		{
			get
			{
				return this.SelectOptions();
			}
		}

		public int Length
		{
			get
			{
				return this.SelectOptions().Count<IDomObject>();
			}
		}

		public override string Type
		{
			get
			{
				if (!this.Multiple)
				{
					return "select-one";
				}
				return "select-multiple";
			}
			set
			{
				throw new InvalidOperationException("You can't set the type for a SELECT element.");
			}
		}

		HTMLOptionsCollection SelectOptions()
		{
			if (this.NodeNameID != 10)
			{
				throw new InvalidOperationException("This property is only applicable to SELECT elements.");
			}
			return new HTMLOptionsCollection(this);
		}

		public bool Multiple
		{
			get
			{
				return base.HasAttribute(38);
			}
			set
			{
				base.SetAttribute(38);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return this.SelectOptions().SelectedIndex;
			}
			set
			{
				this.SelectOptions().SelectedIndex = value;
			}
		}

		public IDomElement SelectedItem
		{
			get
			{
				return this.SelectOptions().SelectedItem;
			}
			set
			{
				this.SelectOptions().SelectedItem = value;
			}
		}

		public override string Value
		{
			get
			{
				IDomElement selectedItem = this.SelectedItem;
				if (selectedItem != null)
				{
					return selectedItem.Value;
				}
				return "";
			}
			set
			{
				foreach (IDomObject domObject in this.Options)
				{
					if (domObject.Value == value)
					{
						this.SelectedIndex = domObject.Index;
						return;
					}
				}
				this.SelectedIndex = -1;
			}
		}
	}
}
