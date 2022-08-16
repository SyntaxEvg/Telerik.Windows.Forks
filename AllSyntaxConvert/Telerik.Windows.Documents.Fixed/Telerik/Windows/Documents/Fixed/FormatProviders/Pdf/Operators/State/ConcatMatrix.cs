using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	class ConcatMatrix : MatrixOperatorBase
	{
		public override string Name
		{
			get
			{
				return "cm";
			}
		}

		protected override void InterpretMatrixOverride(ContentStreamInterpreter interpreter, Matrix matrix)
		{
			interpreter.GraphicState.ConcatenateMatrix(matrix);
		}
	}
}
