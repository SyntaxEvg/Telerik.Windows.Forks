using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class HtmlExportListManager : HtmlListManager
	{
		public List<IStyleProperty> ExportedCharacterProperties
		{
			get
			{
				return this.exportedCharacterProperties;
			}
		}

		public bool IsInList
		{
			get
			{
				return this.isInListCounter > 0;
			}
		}

		internal void BeginList()
		{
			this.isInListCounter++;
		}

		internal void EndList()
		{
			this.isInListCounter--;
		}

		internal bool ShouldRestartLevel(List list, int listLevelIndex)
		{
			Guard.ThrowExceptionIfNull<List>(list, "list");
			int restartAfterLevel = list.Levels[listLevelIndex].RestartAfterLevel;
			if (restartAfterLevel != DocumentDefaultStyleSettings.RestartAfterLevel)
			{
				int verticalDepthSerialIndex = base.ListIndexes.TotalListItemsPerLevel[restartAfterLevel].VerticalDepthSerialIndex;
				if (base.ListIndexes.TotalListItemsPerLevel.ContainsKey(listLevelIndex))
				{
					int verticalDepthSerialIndex2 = base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].VerticalDepthSerialIndex;
					if (verticalDepthSerialIndex > verticalDepthSerialIndex2)
					{
						return true;
					}
				}
				return false;
			}
			return listLevelIndex != 0 || !base.ListIndexes.TotalListItemsPerLevel.ContainsKey(listLevelIndex) || base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].VerticalDepthSerialIndex <= 0;
		}

		internal void UpdateListIndexes(List list, int listLevelIndex, bool shouldRestartLevel)
		{
			Guard.ThrowExceptionIfNull<List>(list, "list");
			Guard.ThrowExceptionIfLessThan<int>(DocumentDefaultStyleSettings.ListLevel, listLevelIndex, "listLevelIndex");
			base.ListIndexes.TotalNumberOfItems++;
			base.ListIndexes.AddLevelIndex(listLevelIndex);
			if (shouldRestartLevel)
			{
				base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].BulletSerialIndex = list.Levels[listLevelIndex].StartIndex;
			}
			else
			{
				base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].BulletSerialIndex = base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].BulletSerialIndex + 1;
			}
			base.ListIndexes.TotalListItemsPerLevel[listLevelIndex].VerticalDepthSerialIndex = base.ListIndexes.TotalNumberOfItems;
		}

		readonly List<IStyleProperty> exportedCharacterProperties = new List<IStyleProperty>();

		int isInListCounter;
	}
}
