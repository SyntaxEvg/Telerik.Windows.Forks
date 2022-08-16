using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.InlineImage;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.MarkedContent;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators
{
	static class ContentStreamOperators
	{
		static ContentStreamOperators()
		{
			ContentStreamOperators.SetGraphicsStateDictionaryOperator = new SetGraphicsStateDictionary();
			ContentStreamOperators.CurveToOperator = new CurveTo();
			ContentStreamOperators.VCurveToOperator = new VCurveTo();
			ContentStreamOperators.YCurveToOperator = new YCurveTo();
			ContentStreamOperators.LineToOperator = new LineTo();
			ContentStreamOperators.AppendRectangleOperator = new AppendRectangle();
			ContentStreamOperators.MoveToOperator = new MoveTo();
			ContentStreamOperators.ClosePathOperator = new ClosePath();
			ContentStreamOperators.FillPathOperator = new FillPath();
			ContentStreamOperators.FillPathAliasOperator = new FillPathAlias();
			ContentStreamOperators.FillPathEvenOddOperator = new FillPathEvenOdd();
			ContentStreamOperators.StrokePathOperator = new StrokePath();
			ContentStreamOperators.CloseAndStrokePathOperator = new CloseAndStrokePath();
			ContentStreamOperators.FillAndStrokePathOperator = new FillAndStrokePath();
			ContentStreamOperators.FillAndStrokePathEvenOddOperator = new FillAndStrokePathEvenOdd();
			ContentStreamOperators.ModifyClippingPathNonzeroOperator = new ModifyClippingPathNonzero();
			ContentStreamOperators.ModifyClippingPathEvenOddOperator = new ModifyClippingPathEvenOdd();
			ContentStreamOperators.EndPathOperator = new EndPath();
			ContentStreamOperators.SetLineDashPatternOperator = new SetLineDashPattern();
			ContentStreamOperators.CloseFillAndStrokePathOperator = new CloseFillAndStrokePath();
			ContentStreamOperators.CloseFillAndStrokePathEvenOddOperator = new CloseFillAndStrokePathEvenOdd();
			ContentStreamOperators.PaintOperator = new Paint();
			ContentStreamOperators.BeginMarkedContentOperator = new BeginMarkedContent();
			ContentStreamOperators.EndMarkedContentOperator = new EndMarkedContent();
			ContentStreamOperators.BeginInlineImageOperator = new BeginInlineImage();
			ContentStreamOperators.EndInlineImageOperator = new EndInlineImage();
		}

		public static GraphicsStatePusher PushGraphicsState(PdfWriter writer, IPdfContentExportContext context)
		{
			return new GraphicsStatePusher(writer, context);
		}

		public static readonly BeginText BeginTextOperator = new BeginText();

		public static readonly EndText EndTextOperator = new EndText();

		public static readonly Font FontOperator = new Font();

		public static readonly CharSpace CharSpaceOperator = new CharSpace();

		public static readonly RenderingMode RenderingModeOperator = new RenderingMode();

		public static readonly Rise RiseOperator = new Rise();

		public static readonly HorizontalScale HorizontalScaleOperator = new HorizontalScale();

		public static readonly WordSpace WordSpaceOperator = new WordSpace();

		public static readonly ShowText ShowTextOperator = new ShowText();

		public static readonly ShowTextArray ShowTextArrayOperator = new ShowTextArray();

		public static readonly MoveToNextLineAndShowText MoveToNextLineAndShowTextOperator = new MoveToNextLineAndShowText();

		public static readonly SetSpacingMoveToNextLineAndShowText SetSpacingMoveToNextLineAndShowTextOperator = new SetSpacingMoveToNextLineAndShowText();

		public static readonly MoveToNextLine MoveToNextLineOperator = new MoveToNextLine();

		public static readonly TextLeading TextLeadingOperator = new TextLeading();

		public static readonly TextMatrix TextMatrixOperator = new TextMatrix();

		public static readonly TranslateText TranslateTextOperator = new TranslateText();

		public static readonly TranslateTextAndSetLeading TranslateTextAndSetLeadingOperator = new TranslateTextAndSetLeading();

		public static readonly SetRgbColor SetRgbColorOperator = new SetRgbColor();

		public static readonly SetStrokeRgbColor SetStrokeRgbColorOperator = new SetStrokeRgbColor();

		public static readonly SetGrayColor SetGrayColorOperator = new SetGrayColor();

		public static readonly SetStrokeGrayColor SetStrokeGrayColorOperator = new SetStrokeGrayColor();

		public static readonly SetFillColorN SetColorN = new SetFillColorN();

		public static readonly SetFillColorNAlias SetColorNAlias = new SetFillColorNAlias();

		public static readonly SetStrokeColorN SetStrokeColorN = new SetStrokeColorN();

		public static readonly SetStrokeColorNAlias SetStrokeColorNAlias = new SetStrokeColorNAlias();

		public static readonly SetFillColorSpace SetColorSpace = new SetFillColorSpace();

		public static readonly SetStrokeColorSpace SetStrokeColorSpace = new SetStrokeColorSpace();

		public static readonly SetCmykColor SetCmykColor = new SetCmykColor();

		public static readonly SetStrokeCmykColor SetCmykStrokeColor = new SetStrokeCmykColor();

		public static readonly ConcatMatrix ConcatMatrixOperator = new ConcatMatrix();

		public static readonly SetLineJoin SetLineJoinOperator = new SetLineJoin();

		public static readonly SetLineCap SetLineCapOperator = new SetLineCap();

		public static readonly SetLineWidth SetLineWidthOperator = new SetLineWidth();

		public static readonly SaveGraphicsState SaveGraphicsStateOperator = new SaveGraphicsState();

		public static readonly RestoreGraphicsState RestoreGraphicsStateOperator = new RestoreGraphicsState();

		public static readonly SetGraphicsStateDictionary SetGraphicsStateDictionaryOperator;

		public static readonly CurveTo CurveToOperator;

		public static readonly VCurveTo VCurveToOperator;

		public static readonly YCurveTo YCurveToOperator;

		public static readonly LineTo LineToOperator;

		public static readonly AppendRectangle AppendRectangleOperator;

		public static readonly MoveTo MoveToOperator;

		public static readonly ClosePath ClosePathOperator;

		public static readonly FillPath FillPathOperator;

		public static readonly FillPathAlias FillPathAliasOperator;

		public static readonly FillPathEvenOdd FillPathEvenOddOperator;

		public static readonly StrokePath StrokePathOperator;

		public static readonly CloseAndStrokePath CloseAndStrokePathOperator;

		public static readonly FillAndStrokePath FillAndStrokePathOperator;

		public static readonly FillAndStrokePathEvenOdd FillAndStrokePathEvenOddOperator;

		public static readonly ModifyClippingPathNonzero ModifyClippingPathNonzeroOperator;

		public static readonly ModifyClippingPathEvenOdd ModifyClippingPathEvenOddOperator;

		public static readonly EndPath EndPathOperator;

		public static readonly SetLineDashPattern SetLineDashPatternOperator;

		public static readonly SetMiterLimit SetMiterLimitOperator = new SetMiterLimit();

		public static readonly CloseFillAndStrokePath CloseFillAndStrokePathOperator;

		public static readonly CloseFillAndStrokePathEvenOdd CloseFillAndStrokePathEvenOddOperator;

		public static readonly Paint PaintOperator;

		public static readonly BeginMarkedContent BeginMarkedContentOperator;

		public static readonly EndMarkedContent EndMarkedContentOperator;

		public static readonly BeginInlineImage BeginInlineImageOperator;

		public static readonly EndInlineImage EndInlineImageOperator;
	}
}
