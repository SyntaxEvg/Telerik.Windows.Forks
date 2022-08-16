using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class TileBrush : PatternBrush, ITiling, IPatternColor
	{
		public TileBrush(Container container, Rect boundingBox, Matrix transform)
			: base(transform)
		{
			Guard.ThrowExceptionIfNull<Container>(container, "container");
			this.element = container;
			this.boundingBox = boundingBox;
		}

		public Container Element
		{
			get
			{
				return this.element;
			}
		}

		public Rect BoundingBox
		{
			get
			{
				return this.boundingBox;
			}
		}

		public override Brush Clone()
		{
			return new TileBrush(this.element, this.boundingBox, base.Transform);
		}

		readonly Container element;

		readonly Rect boundingBox;
	}
}
