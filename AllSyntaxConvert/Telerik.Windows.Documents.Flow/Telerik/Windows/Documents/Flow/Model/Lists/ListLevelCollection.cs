using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Lists
{
	public sealed class ListLevelCollection : IEnumerable<ListLevel>, IEnumerable
	{
		internal ListLevelCollection(List ownerList)
		{
			this.ownerList = ownerList;
			this.listLevelCollection = new List<ListLevel>(9);
			this.InitializeListLevels(9);
		}

		public int Count
		{
			get
			{
				return this.listLevelCollection.Count;
			}
		}

		internal List OwnerList
		{
			get
			{
				return this.ownerList;
			}
		}

		public ListLevel this[int index]
		{
			get
			{
				Guard.ThrowExceptionIfLessThan<int>(0, index, "index");
				Guard.ThrowExceptionIfGreaterThan<int>(8, index, "index");
				return this.listLevelCollection[index];
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(0, index, "index");
				Guard.ThrowExceptionIfGreaterThan<int>(8, index, "index");
				if (value.OwnerList != null && value.OwnerList != this.OwnerList)
				{
					throw new InvalidOperationException("List level is already added to another list.");
				}
				ListLevel listLevel = this.listLevelCollection[index];
				this.listLevelCollection[index] = value;
				this.listLevelCollection[index].OwnerList = this.OwnerList;
				listLevel.OwnerList = null;
			}
		}

		public IEnumerator<ListLevel> GetEnumerator()
		{
			return this.listLevelCollection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.listLevelCollection.GetEnumerator();
		}

		void InitializeListLevels(int collectionCount)
		{
			for (int i = 0; i < collectionCount; i++)
			{
				this.listLevelCollection.Add(new ListLevel(this.OwnerList));
			}
		}

		const int LevelsCount = 9;

		readonly List<ListLevel> listLevelCollection;

		readonly List ownerList;
	}
}
