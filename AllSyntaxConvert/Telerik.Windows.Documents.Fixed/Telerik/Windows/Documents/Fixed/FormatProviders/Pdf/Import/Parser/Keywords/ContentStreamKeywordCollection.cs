using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class ContentStreamKeywordCollection : KeywordCollection
	{
		public ContentStreamKeywordCollection()
		{
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SaveGraphicsStateOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ConcatMatrixOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.BeginTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.CharSpaceOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.RenderingModeOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.HorizontalScaleOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.RiseOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.WordSpaceOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetRgbColorOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetStrokeRgbColorOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetGrayColorOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetStrokeGrayColorOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FontOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ShowTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ShowTextArrayOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.MoveToNextLineAndShowTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetSpacingMoveToNextLineAndShowTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.MoveToNextLineOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.TextLeadingOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.TextMatrixOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.TranslateTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.TranslateTextAndSetLeadingOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.EndTextOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.RestoreGraphicsStateOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.AppendRectangleOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FillPathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FillPathAliasOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FillPathEvenOddOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ClosePathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.CurveToOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.VCurveToOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.YCurveToOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.EndPathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FillAndStrokePathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.FillAndStrokePathEvenOddOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.CloseFillAndStrokePathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.CloseFillAndStrokePathEvenOddOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.LineToOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ModifyClippingPathNonzeroOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.ModifyClippingPathEvenOddOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.MoveToOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.StrokePathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.CloseAndStrokePathOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetLineCapOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetLineJoinOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetLineWidthOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.PaintOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.BeginMarkedContentOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.EndMarkedContentOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetLineDashPatternOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetMiterLimitOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetGraphicsStateDictionaryOperator));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetColorSpace));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetStrokeColorSpace));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetColorN));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetColorNAlias));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetStrokeColorN));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetStrokeColorNAlias));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetCmykColor));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.SetCmykStrokeColor));
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.BeginInlineImageOperator));
			base.RegisterKeyword(new ImageDataKeyword());
			base.RegisterKeyword(new OperatorKeyword(ContentStreamOperators.EndInlineImageOperator));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("i")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("ri")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("sh")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("MP")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("DP")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("BDC")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("d0")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("d1")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("BX")));
			base.RegisterKeyword(new OperatorKeyword(new NotSupportedOperator("EX")));
		}
	}
}
