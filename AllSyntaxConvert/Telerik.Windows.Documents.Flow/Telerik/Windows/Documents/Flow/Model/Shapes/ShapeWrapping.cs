using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public class ShapeWrapping
	{
		public ShapeWrapping(ShapeWrappingType wrappingType, TextWrap textWrap)
		{
			this.WrappingType = wrappingType;
			this.TextWrap = textWrap;
		}

		public ShapeWrapping()
			: this(ShapeWrappingType.Square, TextWrap.BothSides)
		{
		}

		public ShapeWrappingType WrappingType { get; set; }

		public TextWrap TextWrap { get; set; }

		public override bool Equals(object obj)
		{
			ShapeWrapping shapeWrapping = obj as ShapeWrapping;
			return shapeWrapping != null && this.WrappingType == shapeWrapping.WrappingType && this.TextWrap == shapeWrapping.TextWrap;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.WrappingType.GetHashCode(), this.TextWrap.GetHashCode());
		}

		internal ShapeWrapping Clone()
		{
			return new ShapeWrapping(this.WrappingType, this.TextWrap);
		}
	}
}
