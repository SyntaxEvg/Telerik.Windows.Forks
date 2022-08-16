using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class ListsImportContext
	{
		public Dictionary<int, NumberingListInfo> NumberingListInfoCollection
		{
			get
			{
				return this.numberingListInfoCollection;
			}
		}

		public Dictionary<int, AbstractListInfo> AbstractListInfoCollection
		{
			get
			{
				return this.abstractListInfoCollection;
			}
		}

		public void AddAbstractListToListMapping(int abstractListInfoId, int listId, out int nextAbstractListInfoId)
		{
			if (!this.abstractListToListMapper.ContainsKey(abstractListInfoId))
			{
				this.abstractListToListMapper.Add(abstractListInfoId, listId);
				nextAbstractListInfoId = abstractListInfoId;
				return;
			}
			int num = abstractListInfoId;
			while (this.abstractListToListMapper.ContainsKey(num))
			{
				num++;
			}
			nextAbstractListInfoId = num;
			this.abstractListToListMapper.Add(nextAbstractListInfoId, listId);
		}

		public int? GetListIdByNumberedListId(int numberedListId)
		{
			if (!this.numberingListInfoCollection.ContainsKey(numberedListId))
			{
				return null;
			}
			int abstractListId = this.numberingListInfoCollection[numberedListId].AbstractListId;
			int? listIdByAbstractListId = this.GetListIdByAbstractListId(abstractListId);
			if (listIdByAbstractListId != null)
			{
				return listIdByAbstractListId;
			}
			if (this.AbstractListInfoCollection.ContainsKey(abstractListId))
			{
				string numStyleLink = this.AbstractListInfoCollection[abstractListId].NumStyleLink;
				foreach (KeyValuePair<int, AbstractListInfo> keyValuePair in this.AbstractListInfoCollection)
				{
					if (keyValuePair.Value.StyleLink == numStyleLink)
					{
						listIdByAbstractListId = this.GetListIdByAbstractListId(keyValuePair.Value.Id);
						if (listIdByAbstractListId != null)
						{
							return listIdByAbstractListId;
						}
						break;
					}
				}
			}
			return null;
		}

		int? GetListIdByAbstractListId(int abstractListId)
		{
			if (this.abstractListToListMapper.ContainsKey(abstractListId))
			{
				int value = this.abstractListToListMapper[abstractListId];
				return new int?(value);
			}
			return null;
		}

		readonly Dictionary<int, AbstractListInfo> abstractListInfoCollection = new Dictionary<int, AbstractListInfo>();

		readonly Dictionary<int, NumberingListInfo> numberingListInfoCollection = new Dictionary<int, NumberingListInfo>();

		readonly Dictionary<int, int> abstractListToListMapper = new Dictionary<int, int>();
	}
}
