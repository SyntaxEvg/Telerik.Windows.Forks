using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class OnlyOfType : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			return (from item in element.ParentNode.ChildElements
				where item.NodeNameID == element.NodeNameID
				select item).SingleOrDefaultAlways<IDomElement>() != null;
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			return this.OnlyChildOfAnyType(element);
		}

		IEnumerable<IDomObject> OnlyChildOfAnyType(IDomObject parent)
		{
			IDictionary<ushort, IDomElement> dictionary = new Dictionary<ushort, IDomElement>();
			foreach (IDomElement domElement in parent.ChildElements)
			{
				if (dictionary.ContainsKey(domElement.NodeNameID))
				{
					dictionary[domElement.NodeNameID] = null;
				}
				else
				{
					dictionary[domElement.NodeNameID] = domElement;
				}
			}
			return from item in dictionary.Values
				where item != null
				select item;
		}
	}
}
