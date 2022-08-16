using System;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Lang : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			throw new NotImplementedException(":lang is not currently implemented.");
		}
	}
}
