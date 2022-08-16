using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Has : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public override string Arguments
		{
			get
			{
				return base.Arguments;
			}
			set
			{
				base.Arguments = value;
				this.ChildSelector = new Selector(this.Parameters[0]).ToContextSelector();
			}
		}

		public IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			IDomObject first = selection.FirstOrDefault<IDomObject>();
			if (first != null)
			{
				HashSet<IDomObject> subSel = new HashSet<IDomObject>(this.ChildSelector.Select(first.Document, selection));
				foreach (IDomObject item in selection)
				{
					IEnumerator<IDomObject> enumerator2 = this.Descendants(item).GetEnumerator();
					for (;;)
					{
						try
						{
							goto IL_103;
							IL_B9:
							IDomObject descendant = enumerator2.Current;
							if (!subSel.Contains(descendant))
							{
								goto IL_103;
							}
							yield return item;
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						break;
						IL_103:
						if (!enumerator2.MoveNext())
						{
							break;
						}
						goto IL_B9;
					}
				}
			}
			yield break;
		}

		protected IEnumerable<IDomObject> Descendants(IDomObject parent)
		{
			if (parent.HasChildren)
			{
				foreach (IDomObject child in parent.ChildNodes)
				{
					foreach (IDomObject descendant in this.Descendants(child))
					{
						yield return descendant;
					}
					yield return child;
				}
			}
			yield break;
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

		protected Selector ChildSelector;
	}
}
