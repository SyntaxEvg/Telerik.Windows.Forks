using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class FirstOfType : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			return (from item in element.ParentNode.ChildElements
				where item.NodeNameID == element.NodeNameID
				select item).FirstOrDefault<IDomElement>() == element;
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			HashSet<ushort> Types = new HashSet<ushort>();
			foreach (IDomElement child in element.ChildElements)
			{
				if (!Types.Contains(child.NodeNameID))
				{
					Types.Add(child.NodeNameID);
					yield return child;
				}
			}
			yield break;
		}
	}
}
