using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators
{
	static class ContentStreamOperatorNames
	{
		public const string CurveTo = "c";

		public const string VCurveTo = "v";

		public const string YCurveTo = "y";

		public const string AppendRectangle = "re";

		public const string LineTo = "l";

		public const string MoveTo = "m";

		public const string ClosePath = "h";

		public const string FillPath = "f";

		public const string FillPathAlias = "F";

		public const string FillPathEvenOdd = "f*";

		public const string StrokePath = "S";

		public const string CloseAndStrokePath = "s";

		public const string FillAndStrokePath = "B";

		public const string FillAndStrokePathEvenOdd = "B*";

		public const string CloseFillAndStrokePath = "b";

		public const string CloseFillAndStrokePathEvenOdd = "b*";

		public const string ModifyClippingPathNonzero = "W";

		public const string ModifyClippingPathEvenOdd = "W*";

		public const string EndPath = "n";

		public const string BeginText = "BT";

		public const string EndText = "ET";

		public const string Font = "Tf";

		public const string CharSpace = "Tc";

		public const string RenderingMode = "Tr";

		public const string Rise = "Ts";

		public const string HorizontalScale = "Tz";

		public const string WordSpace = "Tw";

		public const string TextLeading = "TL";

		public const string TranslateText = "Td";

		public const string TranslateTextAndSetLeading = "TD";

		public const string TextMatrix = "Tm";

		public const string MoveToNextLine = "T*";

		public const string ShowText = "Tj";

		public const string ShowTextArray = "TJ";

		public const string MoveToNextLineAndShowText = "'";

		public const string SetSpacingMoveToNextLineAndShowText = "\"";

		public const string SetRgbColor = "rg";

		public const string SetStrokeRgbColor = "RG";

		public const string SetGrayColor = "g";

		public const string SetStrokeGrayColor = "G";

		public const string SetFillColorN = "scn";

		public const string SetFillColorNAlias = "sc";

		public const string SetStrokeColorN = "SCN";

		public const string SetStrokeColorNAlias = "SC";

		public const string SetFillColorSpace = "cs";

		public const string SetStrokeColorSpace = "CS";

		public const string SetCmykStrokeColor = "K";

		public const string SetCmykColor = "k";

		public const string ShadingPattern = "sh";

		public const string SetLineCap = "J";

		public const string SetLineJoin = "j";

		public const string SetLineWidth = "w";

		public const string SetLineDashPattern = "d";

		public const string SetMiterLimit = "M";

		public const string SetGraphicsStateDictionary = "gs";

		public const string SetFlatnessState = "i";

		public const string SetRenderingIntent = "ri";

		public const string ConcatMatrix = "cm";

		public const string SaveGraphicsState = "q";

		public const string RestorGraphicsState = "Q";

		public const string Paint = "Do";

		public const string MarkedContentPoint = "MP";

		public const string MarkedContentPointWithProperties = "DP";

		public const string BeginMarkedContent = "BMC";

		public const string EndMarkedContentWithProperties = "BDC";

		public const string EndMarkedContent = "EMC";

		public const string SetWidthInfo = "d0";

		public const string SetWidthAndBBInfo = "d1";

		public const string BeginInlineImage = "BI";

		public const string ImageData = "ID";

		public const string EndInlineImage = "EI";

		public const string BeginCompatibility = "BX";

		public const string EndCompatibility = "EX";
	}
}
