using System;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Empty : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			return !element.HasChildren || Empty.IsEmpty(element);
		}

		public static bool IsEmpty(IDomObject element)
		{
			return !(from item in element.ChildNodes
				where item.NodeType == NodeType.ELEMENT_NODE || (item.NodeType == NodeType.TEXT_NODE && !string.IsNullOrEmpty(item.NodeValue))
				select item).Any<IDomObject>();
		}
	}
}
