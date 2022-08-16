using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Contains : PseudoSelectorFilter
	{
		public override IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			foreach (IDomObject el in selection)
			{
				if (this.ContainsText((IDomElement)el, this.Parameters[0]))
				{
					yield return el;
				}
			}
			yield break;
		}

		public override bool Matches(IDomObject element)
		{
			return element is IDomContainer && this.ContainsText(element, this.Parameters[0]);
		}

		bool ContainsText(IDomObject source, string text)
		{
			foreach (IDomObject domObject in source.ChildNodes)
			{
				if (domObject.NodeType == NodeType.TEXT_NODE)
				{
					if (((IDomText)domObject).NodeValue.IndexOf(text) >= 0)
					{
						return true;
					}
				}
				else if (domObject.NodeType == NodeType.ELEMENT_NODE && this.ContainsText((IDomElement)domObject, text))
				{
					return true;
				}
			}
			return false;
		}

		public override int MaximumParameterCount
		{
			get
			{
				return 1;
			}
		}

		public override int MinimumParameterCount
		{
			get
			{
				return 1;
			}
		}

		protected override QuotingRule ParameterQuoted(int index)
		{
			return QuotingRule.OptionallyQuoted;
		}
	}
}
