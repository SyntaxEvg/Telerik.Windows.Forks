using System;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	public class TextBulletNumberingFormat : IBulletNumberingFormat
	{
		public TextBulletNumberingFormat(Func<IListLevelsIndexer, string> getTextBullet)
		{
			Guard.ThrowExceptionIfNull<Func<IListLevelsIndexer, string>>(getTextBullet, "getTextBullet");
			this.getTextBullet = getTextBullet;
		}

		public PositionContentElement GetBulletNumberingElement(IListLevelsIndexer listLevelsIndexer)
		{
			string text = this.getTextBullet(listLevelsIndexer);
			return string.IsNullOrEmpty(text) ? null : new TextFragment(text);
		}

		readonly Func<IListLevelsIndexer, string> getTextBullet;
	}
}
