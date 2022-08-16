using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class GlyphsLayoutBox : Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement
	{
		public GlyphsLayoutBox()
		{
			this.Glyphs = new GlyphsCollection();
		}

		public GlyphsCollection Glyphs { get; set; }

		public override bool HasChildren
		{
			get
			{
				return true;
			}
		}

		public override IEnumerable<IContentElement> Children
		{
			get
			{
				return this.Glyphs;
			}
		}

		public override IContentElement Clone()
		{
			GlyphsLayoutBox glyphsLayoutBox = new GlyphsLayoutBox();
			glyphsLayoutBox.Size = base.Size;
			glyphsLayoutBox.TransformMatrix = base.TransformMatrix;
			glyphsLayoutBox.ZIndex = base.ZIndex;
			foreach (GlyphOld glyphOld in this.Glyphs)
			{
				glyphsLayoutBox.Glyphs.Add((GlyphOld)glyphOld.Clone());
			}
			return glyphsLayoutBox;
		}

		public override Rect Arrange(Matrix transformMatrix)
		{
			foreach (GlyphOld glyphOld in this.Glyphs)
			{
				glyphOld.Arrange(transformMatrix);
			}
			base.BoundingRect = Helper.GetBoundingRect(this.Glyphs.GetBoundingRect(), Matrix.Identity);
			return base.BoundingRect;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GlyphOld glyphOld in this.Glyphs)
			{
				stringBuilder.Append(glyphOld.ToUnicode);
			}
			return stringBuilder.ToString();
		}

		public override ContentElementTypeOld Type
		{
			get
			{
				return ContentElementTypeOld.GlyphLayoutBox;
			}
		}
	}
}
