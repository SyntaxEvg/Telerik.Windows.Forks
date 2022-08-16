using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public abstract class TilingBase : PatternColor, IContentRootElement, IContainerElement, IFixedDocumentElement, ITiling, IPatternColor
	{
		public abstract TilingType TilingType { get; set; }

		public abstract double VerticalSpacing { get; set; }

		public abstract Rect BoundingBox { get; set; }

		public abstract double HorizontalSpacing { get; set; }

		public abstract Size Size { get; }

		public abstract ContentElementCollection Content { get; }

		public IFixedDocumentElement Parent
		{
			get
			{
				return null;
			}
		}

		public bool SupportsAnnotations
		{
			get
			{
				return false;
			}
		}

		public AnnotationCollection Annotations
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		internal override PatternType PatternType
		{
			get
			{
				return PatternType.Tiling;
			}
		}

		internal abstract PaintType PaintType { get; }
	}
}
