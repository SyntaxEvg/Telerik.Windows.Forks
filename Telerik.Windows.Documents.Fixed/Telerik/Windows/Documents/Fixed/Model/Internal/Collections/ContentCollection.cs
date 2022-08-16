using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Collections
{
	class ContentCollection : List<IContentElement>
	{
		public ContentCollection()
		{
			this.currentZIndex = 0;
		}

		public void AddChild(IContentElement child)
		{
			Guard.ThrowExceptionIfNull<IContentElement>(child, "child");
			child.ZIndex = this.currentZIndex++;
			base.Add(child);
		}

		public IEnumerable<GlyphOld> GetAllNonWhiteSpaceGlyphs()
		{
			return from g in Extensions.EnumerateChildrenOfType<GlyphOld>(this)
				where !Extensions.IsNullEmptyOrWhiteSpace(g.ToUnicode)
				select g;
		}

		public IEnumerable<GlyphOld> GetAllGlyphs()
		{
			return Extensions.EnumerateChildrenOfType<GlyphOld>(this);
		}

		public IEnumerable<GlyphsLayoutBox> GetAllGlyphLayoutBoxes()
		{
			return Extensions.EnumerateChildrenOfType<GlyphsLayoutBox>(this);
		}

		public void Arrange(Matrix matrix)
		{
			foreach (IContentElement contentElement in this)
			{
				ContentElement contentElement2 = (ContentElement)contentElement;
				contentElement2.Arrange(matrix);
			}
		}

		int currentZIndex;
	}
}
