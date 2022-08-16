using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public class VerticalPosition
	{
		public VerticalPosition()
			: this(VerticalRelativeFrom.Paragraph, 0.0)
		{
		}

		public VerticalPosition(VerticalRelativeFrom relativeFrom, double offset)
		{
			this.RelativeFrom = relativeFrom;
			this.ValueType = PositionValueType.Offset;
			this.Offset = offset;
		}

		public VerticalPosition(VerticalRelativeFrom relativeFrom, RelativeVerticalAlignment alignment)
		{
			this.RelativeFrom = relativeFrom;
			this.ValueType = PositionValueType.Alignment;
			this.Alignment = alignment;
		}

		public PositionValueType ValueType { get; set; }

		public VerticalRelativeFrom RelativeFrom { get; set; }

		public RelativeVerticalAlignment Alignment { get; set; }

		public double Offset { get; set; }

		public override bool Equals(object obj)
		{
			VerticalPosition verticalPosition = obj as VerticalPosition;
			return verticalPosition != null && (this.ValueType == verticalPosition.ValueType && this.RelativeFrom == verticalPosition.RelativeFrom && this.Alignment == verticalPosition.Alignment) && this.Offset == verticalPosition.Offset;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.ValueType.GetHashCode(), this.RelativeFrom.GetHashCode(), this.Alignment.GetHashCode(), this.Offset.GetHashCode());
		}

		internal VerticalPosition Clone()
		{
			return new VerticalPosition
			{
				Alignment = this.Alignment,
				Offset = this.Offset,
				RelativeFrom = this.RelativeFrom,
				ValueType = this.ValueType
			};
		}
	}
}
