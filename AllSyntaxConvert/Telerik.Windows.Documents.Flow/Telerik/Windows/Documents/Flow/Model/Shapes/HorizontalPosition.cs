using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public class HorizontalPosition
	{
		public HorizontalPosition()
			: this(HorizontalRelativeFrom.Column, 0.0)
		{
		}

		public HorizontalPosition(HorizontalRelativeFrom relativeFrom, double offset)
		{
			this.RelativeFrom = relativeFrom;
			this.ValueType = PositionValueType.Offset;
			this.Offset = offset;
		}

		public HorizontalPosition(HorizontalRelativeFrom relativeFrom, RelativeHorizontalAlignment alignment)
		{
			this.RelativeFrom = relativeFrom;
			this.ValueType = PositionValueType.Alignment;
			this.Alignment = alignment;
		}

		public PositionValueType ValueType { get; set; }

		public HorizontalRelativeFrom RelativeFrom { get; set; }

		public RelativeHorizontalAlignment Alignment { get; set; }

		public double Offset { get; set; }

		public override bool Equals(object obj)
		{
			HorizontalPosition horizontalPosition = obj as HorizontalPosition;
			return horizontalPosition != null && (this.ValueType == horizontalPosition.ValueType && this.RelativeFrom == horizontalPosition.RelativeFrom && this.Alignment == horizontalPosition.Alignment) && this.Offset == horizontalPosition.Offset;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.ValueType.GetHashCode(), this.RelativeFrom.GetHashCode(), this.Alignment.GetHashCode(), this.Offset.GetHashCode());
		}

		internal HorizontalPosition Clone()
		{
			return new HorizontalPosition
			{
				Alignment = this.Alignment,
				Offset = this.Offset,
				RelativeFrom = this.RelativeFrom,
				ValueType = this.ValueType
			};
		}
	}
}
