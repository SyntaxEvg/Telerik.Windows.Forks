using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	abstract class Indexed : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		protected int Index
		{
			get
			{
				if (!this.IndexParsed)
				{
					if (!int.TryParse(this.Parameters[0], out this._Index))
					{
						throw new ArgumentException(string.Format("The {0} selector requires a single integer parameter.", this.Name));
					}
					this.IndexParsed = true;
				}
				return this._Index;
			}
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

		public abstract IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection);

		int _Index;

		bool IndexParsed;
	}
}
