using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	class ListLevelsIndexer : IListLevelsIndexer
	{
		public ListLevelsIndexer(ListLevelCollection levels)
		{
			this.levels = levels;
		}

		public int GetCurrentIndex(int listLevel)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(FixedDocumentDefaults.FirstListLevelIndex, this.levels.Count - 1, listLevel, "listLevel");
			return this.levels[listLevel].LastInsertedIndex;
		}

		public void MoveToNextIndex(int listLevel)
		{
			this.levels[listLevel].LastInsertedIndex++;
			for (int i = listLevel + 1; i < this.levels.Count; i++)
			{
				ListLevel listLevel2 = this.levels[i];
				if (listLevel2.RestartAfterLevel < 0 || listLevel <= listLevel2.RestartAfterLevel)
				{
					listLevel2.RestartCurrentNumber();
				}
			}
		}

		readonly ListLevelCollection levels;
	}
}
