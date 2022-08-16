using System;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.Model.Cloning
{
	class ListCollectionMerger
	{
		public ListCollectionMerger(ListCollection target, ListCollection source)
		{
			this.targetListCollection = target;
			this.sourceListCollection = source;
		}

		internal void Merge(CloneContext cloneContext)
		{
			foreach (List list in this.sourceListCollection)
			{
				List list2 = list.Clone();
				this.targetListCollection.Add(list2);
				cloneContext.ReinitializedLists.Add(list.Id, list2.Id);
				if (!string.IsNullOrEmpty(list.StyleId) && cloneContext.RenamedStyles.ContainsKey(list.StyleId))
				{
					list2.StyleId = cloneContext.RenamedStyles[list.StyleId];
				}
			}
		}

		readonly ListCollection targetListCollection;

		readonly ListCollection sourceListCollection;
	}
}
