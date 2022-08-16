using System;
using System.Collections.Generic;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class OnlyChild : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			return this.OnlyChildOrNull(element.ParentNode) == element;
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			IDomObject child = this.OnlyChildOrNull(element);
			if (child != null)
			{
				yield return child;
			}
			yield break;
		}

		IDomObject OnlyChildOrNull(IDomObject parent)
		{
			if (parent.NodeType == NodeType.DOCUMENT_NODE)
			{
				return null;
			}
			return parent.ChildElements.SingleOrDefaultAlways<IDomElement>();
		}
	}
}
