using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class TextMatrix : MatrixOperatorBase
	{
		public override string Name
		{
			get
			{
				return "Tm";
			}
		}

		protected override void InterpretMatrixOverride(ContentStreamInterpreter interpreter, Matrix matrix)
		{
			interpreter.TextState.TextLineMatrix = matrix;
			interpreter.TextState.TextMatrix = interpreter.TextState.TextLineMatrix;
		}
	}
}
