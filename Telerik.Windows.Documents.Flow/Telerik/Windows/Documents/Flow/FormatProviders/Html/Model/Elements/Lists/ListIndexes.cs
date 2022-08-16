using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class ListIndexes
	{
		public ListIndexes(int listId)
		{
			this.totalListItemsToLevel = new Dictionary<int, ListIndexesPair>();
			this.listId = listId;
		}

		internal int ListId
		{
			get
			{
				return this.listId;
			}
		}

		internal int TotalNumberOfItems
		{
			get
			{
				return this.totalNumberOfItems;
			}
			set
			{
				this.totalNumberOfItems = value;
			}
		}

		internal Dictionary<int, ListIndexesPair> TotalListItemsPerLevel
		{
			get
			{
				return this.totalListItemsToLevel;
			}
		}

		internal void TransferListIndexes(ListIndexes other)
		{
			this.totalListItemsToLevel = new Dictionary<int, ListIndexesPair>(other.totalListItemsToLevel);
			this.TotalNumberOfItems = other.TotalNumberOfItems;
		}

		internal void AddLevelIndex(int levelIndex)
		{
			if (!this.TotalListItemsPerLevel.ContainsKey(levelIndex))
			{
				this.TotalListItemsPerLevel.Add(levelIndex, new ListIndexesPair());
			}
		}

		readonly int listId;

		int totalNumberOfItems;

		Dictionary<int, ListIndexesPair> totalListItemsToLevel;
	}
}
