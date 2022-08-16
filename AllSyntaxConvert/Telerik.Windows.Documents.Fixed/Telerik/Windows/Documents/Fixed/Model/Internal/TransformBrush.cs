using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class TransformBrush
	{
		internal TransformBrush(Brush brush)
			: this(brush, Matrix.Identity)
		{
		}

		internal TransformBrush(Brush brush, Matrix parentMatrixTransform)
		{
			Guard.ThrowExceptionIfNull<Brush>(brush, "brush");
			this.Brush = brush;
			this.ParentMatrixTransform = parentMatrixTransform;
		}

		public Brush Brush { get; set; }

		public Matrix ParentMatrixTransform { get; set; }

		public override bool Equals(object obj)
		{
			TransformBrush transformBrush = obj as TransformBrush;
			return transformBrush != null && this.Brush.Equals(transformBrush.Brush) && this.ParentMatrixTransform.Equals(transformBrush.ParentMatrixTransform);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Brush.GetHashCode(), this.ParentMatrixTransform.GetHashCode());
		}
	}
}
