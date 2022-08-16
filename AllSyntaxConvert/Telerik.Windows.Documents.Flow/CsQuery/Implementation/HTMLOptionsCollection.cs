using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CsQuery.ExtensionMethods;

namespace CsQuery.Implementation
{
	class HTMLOptionsCollection : IHTMLOptionsCollection, IEnumerable<IDomObject>, IEnumerable
	{
		public HTMLOptionsCollection(IDomElement parent)
		{
			if (parent.NodeNameID != 10)
			{
				throw new ArgumentException("The parent node for an HtmlOptionsCollection must be a SELECT node.");
			}
			this.Parent = (IHTMLSelectElement)parent;
		}

		public IHTMLSelectElement Parent { get; protected set; }

		public IDomElement Item(int index)
		{
			return this.Children().ElementAt(index);
		}

		[IndexerName("Indexer")]
		public IDomElement this[int index]
		{
			get
			{
				return this.Item(index);
			}
		}

		public IDomElement NamedItem(string name)
		{
			return (from item in this.Children()
				where item.Name == name
				select item).FirstOrDefault<DomElement>();
		}

		[IndexerName("Indexer")]
		public IDomElement this[string name]
		{
			get
			{
				return this.NamedItem(name);
			}
		}

		internal int SelectedIndex
		{
			get
			{
				HTMLOptionsCollection.OptionElement optionElement;
				return this.GetSelectedItem(out optionElement);
			}
			set
			{
				this.Children().ForEach(delegate(DomElement item, int i)
				{
					if (i == value)
					{
						item.SetAttribute(6);
						return;
					}
					if (item.Selected)
					{
						item.RemoveAttribute(6);
					}
				});
			}
		}

		int GetSelectedItem(out HTMLOptionsCollection.OptionElement el)
		{
			int num = this.Children(this.Parent).LastIndexOf((HTMLOptionsCollection.OptionElement item) => item.Element.HasAttribute("selected"), out el);
			if (this.Parent.Multiple || num >= 0)
			{
				return num;
			}
			return this.Children(this.Parent).IndexOf((HTMLOptionsCollection.OptionElement item) => !item.Disabled, out el);
		}

		internal IDomElement SelectedItem
		{
			get
			{
				HTMLOptionsCollection.OptionElement optionElement;
				this.GetSelectedItem(out optionElement);
				return optionElement.Element;
			}
			set
			{
				this.Children().ForEach(delegate(DomElement item)
				{
					if (item == value)
					{
						item.SetAttribute(6);
						return;
					}
					if (item.Selected)
					{
						item.RemoveAttribute(6);
					}
				});
			}
		}

		public IEnumerator<IDomObject> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		protected IEnumerable<DomElement> Children()
		{
			return from item in this.Children(this.Parent)
				select item.Element;
		}

		IEnumerable<HTMLOptionsCollection.OptionElement> Children(IDomElement parent)
		{
			return this.Children(parent, false);
		}

		IEnumerable<HTMLOptionsCollection.OptionElement> Children(IDomElement parent, bool disabled)
		{
			foreach (IDomElement item in parent.ChildElements)
			{
				ushort nodeNameID = item.NodeNameID;
				if (nodeNameID != 11)
				{
					if (nodeNameID == 24)
					{
						foreach (HTMLOptionsCollection.OptionElement child in this.Children(item, disabled || item.HasAttribute("disabled")))
						{
							yield return child;
						}
					}
				}
				else
				{
					yield return new HTMLOptionsCollection.OptionElement
					{
						Element = (DomElement)item,
						Disabled = (disabled || item.HasAttribute("disabled"))
					};
				}
			}
			yield break;
		}

		struct OptionElement
		{
			public DomElement Element;

			public bool Disabled;
		}
	}
}
