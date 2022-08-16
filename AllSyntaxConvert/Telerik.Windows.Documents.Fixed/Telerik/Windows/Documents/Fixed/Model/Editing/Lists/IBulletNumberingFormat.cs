using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	public interface IBulletNumberingFormat
	{
		PositionContentElement GetBulletNumberingElement(IListLevelsIndexer listLevelsIndexer);
	}
}
