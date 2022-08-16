using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo
{
	class AbstractListInfo
	{
		public AbstractListInfo()
		{
			this.Levels = new List<ListLevelInfo>();
		}

		public AbstractListInfo(List list, bool isAbstractListForStyle)
			: this()
		{
			this.IsAbstractListForStyle = isAbstractListForStyle;
			this.Id = list.Id;
			this.MultilevelType = new MultilevelType?(list.MultilevelType);
			this.StyleLink = list.StyleId;
			this.Name = list.Name;
			if (this.IsAbstractListForStyle)
			{
				this.NumStyleLink = list.StyleId;
			}
			if (list.MultilevelType == Telerik.Windows.Documents.Flow.Model.Lists.MultilevelType.SingleLevel)
			{
				this.Levels.Add(new ListLevelInfo(list.Levels[0]));
				return;
			}
			foreach (ListLevel listLevel in list.Levels)
			{
				this.Levels.Add(new ListLevelInfo(listLevel));
			}
		}

		public AbstractListInfo(AbstractListInfo abstractListInfo)
			: this()
		{
			this.Id = abstractListInfo.Id;
			this.MultilevelType = abstractListInfo.MultilevelType;
			this.StyleLink = abstractListInfo.StyleLink;
			this.NumStyleLink = abstractListInfo.NumStyleLink;
			this.Name = abstractListInfo.Name;
			foreach (ListLevelInfo listLevelInfo in abstractListInfo.Levels)
			{
				ListLevelInfo item = new ListLevelInfo(listLevelInfo);
				this.Levels.Add(item);
			}
		}

		public bool IsAbstractListForStyle { get; set; }

		public int Id { get; set; }

		public MultilevelType? MultilevelType { get; set; }

		public string Name { get; set; }

		public string NumStyleLink { get; set; }

		public string StyleLink { get; set; }

		public List<ListLevelInfo> Levels { get; set; }

		public List GetList()
		{
			List list = new List();
			list.MultilevelType = this.MultilevelType ?? Telerik.Windows.Documents.Flow.Model.Lists.MultilevelType.HybridMultilevel;
			list.Name = this.Name;
			list.StyleId = this.StyleLink;
			if (list.MultilevelType == Telerik.Windows.Documents.Flow.Model.Lists.MultilevelType.SingleLevel)
			{
				if (this.Levels.Count > 0)
				{
					ListLevel listLevel = this.Levels[0].GetListLevel();
					listLevel.OwnerList = list;
					list.Levels[this.Levels[0].ListLevelId] = listLevel;
				}
			}
			else
			{
				foreach (ListLevelInfo listLevelInfo in this.Levels)
				{
					ListLevel listLevel2 = listLevelInfo.GetListLevel();
					listLevel2.OwnerList = list;
					list.Levels[listLevelInfo.ListLevelId] = listLevel2;
				}
			}
			return list;
		}
	}
}
