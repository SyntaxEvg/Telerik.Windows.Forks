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

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphOld> GetAllNonWhiteSpaceGlyphs()
		{
			return global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils.Extensions.EnumerateChildrenOfType<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphOld>(this).Where((global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphOld g) => !global::Telerik.Windows.Documents.Utilities.Extensions.IsNullEmptyOrWhiteSpace(g.ToUnicode));
		}

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphOld> GetAllGlyphs()
		{
			return global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils.Extensions.EnumerateChildrenOfType<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphOld>(this);
		}


		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphsLayoutBox> GetAllGlyphLayoutBoxes()
		{
			return global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils.Extensions.EnumerateChildrenOfType<global::Telerik.Windows.Documents.Fixed.Model.Internal.GlyphsLayoutBox>(this);
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
