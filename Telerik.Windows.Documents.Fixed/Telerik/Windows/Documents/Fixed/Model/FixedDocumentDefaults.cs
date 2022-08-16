using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Fixed.Model
{
	static class FixedDocumentDefaults
	{
		public static readonly double FontSize = Unit.PointToDip(10.0);

		public static readonly double StrokeThickness = 1.0;

		public static readonly bool Extend = true;

		public static readonly bool IsFilled = true;

		public static readonly bool IsStroked = true;

		public static readonly LineCap StrokeLineCap = LineCap.Flat;

		public static readonly LineJoin StrokeLineJoin = LineJoin.Miter;

		public static readonly IEnumerable<double> StrokeDashArray = null;

		public static readonly double StrokeDashOffset = 0.0;

		public static readonly double? MiterLimit = null;

		public static readonly Point StartPoint = new Point(0.0, 0.0);

		public static readonly Point EndPoint = new Point(1.0, 0.0);

		public static readonly RenderingMode TextRenderingMode = RenderingMode.Fill;

		public static readonly FillRule FillRule = FillRule.Nonzero;

		public static readonly Padding PageMargins = new Padding(96.0);

		public static readonly Size TilingSize = new Size(10.0, 10.0);

		public static readonly Rect TilingBoundingBox = new Rect(new Point(0.0, 0.0), FixedDocumentDefaults.TilingSize);

		public static readonly Size PageSize = PaperTypeConverter.ToSize(PaperTypes.Letter);

		public static readonly Rotation PageRotation = Rotation.Rotate0;

		public static readonly SimpleColor Color = RgbColors.Black;

		public static readonly SimpleColor EndColor = RgbColors.White;

		public static readonly FontBase Font = FontsRepository.Helvetica;

		public static readonly string TrueTypeFallbackFontName = "Arial";

		public static readonly string Password = "";

		public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;

		public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment VerticalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Top;

		public static readonly ColorBase ForegroundColor = FixedDocumentDefaults.Color;

		public static readonly ColorBase BackgroundColor = null;

		public static readonly ColorBase HighlightColor = null;

		public static readonly double SpacingAfter = 0.0;

		public static readonly double SpacingBefore = 0.0;

		public static readonly double BlockLineSpacing = 1.0;

		public static readonly double LineSpacing = 1.15;

		public static readonly HeightType LineSpacingType = HeightType.Auto;

		public static readonly double FirstLineIndent = 0.0;

		public static readonly double LeftIndent = 0.0;

		public static readonly double RightIndent = 0.0;

		public static readonly double BlockWidth = double.PositiveInfinity;

		public static readonly double BlockHeight = double.PositiveInfinity;

		public static readonly Size ListBoxChoicePadding = new Size(Unit.PointToDip(2.0), Unit.PointToDip(0.5));

		public static readonly double TextBoxTextPadding = Unit.PointToDip(1.0);

		public static readonly double ButtonPadding = Unit.PointToDip(1.0);

		public static readonly double ButtonWidgetBorderThickness = Unit.PointToDip(1.0);

		public static readonly RgbColor BeveledBorderLightColor = RgbColors.White;

		public static readonly RgbColor Border3dEffectDarkColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953));

		public static readonly RgbColor InsetBorderLightColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293));

		public static readonly RgbColor PushButtonBackground = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293));

		public static readonly RgbColor ListBoxSelectionColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.600006), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.756866), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.854904));

		public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment BaselineAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;

		public static readonly UnderlinePattern UnderlinePattern = UnderlinePattern.None;

		public static readonly TableLayoutType DefaultLayoutType = TableLayoutType.AutoFit;

		public static readonly BorderStyle DefaultBorderStyle = BorderStyle.Single;

		public static readonly Thickness DefaultCellPadding = new Thickness(0.0);

		public static readonly double DefaultBorderSpacing = 0.0;

		public static readonly BorderCollapse DefaultBorderCollapse = BorderCollapse.Collapse;

		public static readonly double DefaultIndentAfterBullet = 30.0;

		public static readonly double ListLevelIndentationStep = 24.0;

		public static readonly int DefaultListLevelsCount = 9;

		public static readonly int FirstListLevelIndex = 0;

		public static readonly int DefaultListLevelStartIndex = 1;

		public static readonly int DefaultListLevelRestartAfterLevel = -1;

		public static readonly ImageQuality ImageQuality = ImageQuality.High;

		public static readonly string[] DefaultImageFilters = new string[] { "DCTDecode" };

		public static readonly JpegColorSpace DefaultImageColorspace = JpegColorSpace.Rgb;

		public static readonly JpegColorSpace DefaultSMaskColorspace = JpegColorSpace.Grayscale;

		public static readonly ColorSpaceObject DefaultFormatProviderColorspace = new DeviceRgbColorSpaceObject();

		public static readonly byte DefaultAlphaComponent = byte.MaxValue;

		public static readonly int StandardEncryptRevision = 3;

		public static readonly int StandardEncryptionKeyLength = 128;
	}
}
