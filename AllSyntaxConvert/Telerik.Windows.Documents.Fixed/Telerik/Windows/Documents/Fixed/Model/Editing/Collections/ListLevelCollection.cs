using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public class ListLevelCollection : CollectionBase<ListLevel>
	{
		public ListLevel AddListLevel()
		{
			ListLevel listLevel = new ListLevel();
			base.Add(listLevel);
			return listLevel;
		}

		internal override void OnAddingElement(ListLevel element)
		{
			if (element.IsAddedToListLevelCollection)
			{
				throw new InvalidOperationException("Cannot add ListLevel to ListLevelCollection twice!");
			}
			element.IsAddedToListLevelCollection = true;
		}
	}
}
