using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CsQuery.Implementation
{
	class HtmlFormElement : DomElement, IHTMLFormElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, INodeList<IDomElement>, IReadOnlyList<IDomElement>, IReadOnlyCollection<IDomElement>, IEnumerable<IDomElement>, IEnumerable
	{
		public HtmlFormElement()
			: base(41)
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

		public string AcceptCharset
		{
			get
			{
				return this.GetAttribute("acceptcharset");
			}
			set
			{
				this.SetAttribute("acceptcharset", value);
			}
		}

		public string Action
		{
			get
			{
				return this.GetAttribute("action");
			}
			set
			{
				this.SetAttribute("action", value);
			}
		}

		public string Autocomplete
		{
			get
			{
				return this.GetAttribute("autocomplete");
			}
			set
			{
				this.SetAttribute("autocomplete", value);
			}
		}

		public string Enctype
		{
			get
			{
				return this.GetAttribute("enctype", "application/x-www-form-urlencoded");
			}
			set
			{
				this.SetAttribute("enctype", value);
			}
		}

		public string Encoding
		{
			get
			{
				return this.Enctype;
			}
			set
			{
				this.Enctype = value;
			}
		}

		public string Method
		{
			get
			{
				return this.GetAttribute("method", "GET");
			}
			set
			{
				this.SetAttribute("method", value.ToUpper());
			}
		}

		public bool NoValidate
		{
			get
			{
				return this.HasAttribute("novalidate");
			}
			set
			{
				base.SetProp("novalidate", value);
			}
		}

		public INodeList<IDomElement> Elements
		{
			get
			{
				return new NodeList<IDomElement>(this.Document.QuerySelectorAll(":input"));
			}
		}

		public IList<IDomElement> ToList()
		{
			return new List<IDomElement>(this).AsReadOnly();
		}

		public int Length
		{
			get
			{
				return this.Elements.Length;
			}
		}

		public IDomElement Item(int index)
		{
			return this.Elements[index];
		}

		[IndexerName("Indexer")]
		public IDomElement this[int index]
		{
			get
			{
				return this.Item(index);
			}
		}

		public IEnumerator<IDomElement> GetEnumerator()
		{
			return this.Elements.GetEnumerator();
		}

		int IReadOnlyCollection<IDomElement>.Count
		{
			get
			{
				return this.Elements.Length;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		//IDomElement IReadOnlyList<IDomElement>.get_Item(int A_1)
		//{
		//	return this[A_1];
		//}
	}
}
