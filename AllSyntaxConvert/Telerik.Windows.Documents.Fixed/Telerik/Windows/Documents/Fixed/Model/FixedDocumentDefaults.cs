// Decompiled with JetBrains decompiler
// Type: Telerik.Windows.Documents.Fixed.Model.FixedDocumentDefaults
// Assembly: Telerik.Windows.Documents.Fixed, Version=2019.2.503.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// MVID: 6454440F-C021-4D95-99F0-3BFB2485AF25
// Assembly location: C:\Users\user\Downloads\FromFileToFileCore\FromFileToFileCore\Export_Word_Excel_PDF_CSV_HTML-master\ExportDemo1\ConsoleApp1\bin\x64\Debug\Telerik.Windows.Documents.Fixed.dll
// Compiler-generated code is shown

using System.Collections.Generic;
using System.Windows;
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
    internal static class FixedDocumentDefaults
    {
        public static readonly double FontSize;
        public static readonly double StrokeThickness;
        public static readonly bool Extend;
        public static readonly bool IsFilled;
        public static readonly bool IsStroked;
        public static readonly LineCap StrokeLineCap;
        public static readonly LineJoin StrokeLineJoin;
        public static readonly IEnumerable<double> StrokeDashArray;
        public static readonly double StrokeDashOffset;
        public static readonly double? MiterLimit;
        public static readonly Point StartPoint;
        public static readonly Point EndPoint;
        public static readonly RenderingMode TextRenderingMode;
        public static readonly FillRule FillRule;
        public static readonly Padding PageMargins;
        public static readonly Size TilingSize;
        public static readonly Rect TilingBoundingBox;
        public static readonly Size PageSize;
        public static readonly Rotation PageRotation;
        public static readonly SimpleColor Color;
        public static readonly SimpleColor EndColor;
        public static readonly FontBase Font;
        public static readonly string TrueTypeFallbackFontName;
        public static readonly string Password;
        public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment HorizontalAlignment;
        public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment VerticalAlignment;
        public static readonly ColorBase ForegroundColor;
        public static readonly ColorBase BackgroundColor;
        public static readonly ColorBase HighlightColor;
        public static readonly double SpacingAfter;
        public static readonly double SpacingBefore;
        public static readonly double BlockLineSpacing;
        public static readonly double LineSpacing;
        public static readonly HeightType LineSpacingType;
        public static readonly double FirstLineIndent;
        public static readonly double LeftIndent;
        public static readonly double RightIndent;
        public static readonly double BlockWidth;
        public static readonly double BlockHeight;
        public static readonly Size ListBoxChoicePadding;
        public static readonly double TextBoxTextPadding;
        public static readonly double ButtonPadding;
        public static readonly double ButtonWidgetBorderThickness;
        public static readonly RgbColor BeveledBorderLightColor;
        public static readonly RgbColor Border3dEffectDarkColor;
        public static readonly RgbColor InsetBorderLightColor;
        public static readonly RgbColor PushButtonBackground;
        public static readonly RgbColor ListBoxSelectionColor;
        public static readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment BaselineAlignment;
        public static readonly UnderlinePattern UnderlinePattern;
        public static readonly TableLayoutType DefaultLayoutType;
        public static readonly BorderStyle DefaultBorderStyle;
        public static readonly Thickness DefaultCellPadding;
        public static readonly double DefaultBorderSpacing;
        public static readonly BorderCollapse DefaultBorderCollapse;
        public static readonly double DefaultIndentAfterBullet;
        public static readonly double ListLevelIndentationStep;
        public static readonly int DefaultListLevelsCount;
        public static readonly int FirstListLevelIndex;
        public static readonly int DefaultListLevelStartIndex;
        public static readonly int DefaultListLevelRestartAfterLevel;
        public static readonly ImageQuality ImageQuality;
        public static readonly string[] DefaultImageFilters;
        public static readonly JpegColorSpace DefaultImageColorspace;
        public static readonly JpegColorSpace DefaultSMaskColorspace;
        public static readonly ColorSpaceObject DefaultFormatProviderColorspace;
        public static readonly byte DefaultAlphaComponent;
        public static readonly int StandardEncryptRevision;
        public static readonly int StandardEncryptionKeyLength;

        static FixedDocumentDefaults()
        {
            FontSize = Unit.PointToDip(10.0);
            StrokeThickness = 1.0;
            Extend = true;
            IsFilled = true;
            IsStroked = true;
            StrokeLineCap = LineCap.Flat;
            StrokeLineJoin = LineJoin.Miter;
            StrokeDashArray = (IEnumerable<double>)null;
            StrokeDashOffset = 0.0;
            MiterLimit = new double?();
            StartPoint = new Point(0.0, 0.0);
            EndPoint = new Point(1.0, 0.0);
            TextRenderingMode = RenderingMode.Fill;
            FillRule = FillRule.Nonzero;
            PageMargins = new Padding(96.0);
            TilingSize = new Size(10.0, 10.0);
            TilingBoundingBox = new Rect(new Point(0.0, 0.0), FixedDocumentDefaults.TilingSize);
            PageSize = PaperTypeConverter.ToSize(PaperTypes.Letter);
            PageRotation = Rotation.Rotate0;
            Color = RgbColors.Black;

            EndColor = RgbColors.White;
            Font = FontsRepository.Helvetica;
            TrueTypeFallbackFontName = "Arial";
            Password = "";
            HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
            VerticalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Top;
            ForegroundColor = (ColorBase)FixedDocumentDefaults.Color;
            BackgroundColor = (ColorBase)null;
            HighlightColor = (ColorBase)null;
            SpacingAfter = 0.0;
            SpacingBefore = 0.0;
            BlockLineSpacing = 1.0;
            LineSpacing = 1.15;
            LineSpacingType = HeightType.Auto;
            FirstLineIndent = 0.0;
            LeftIndent = 0.0;
            RightIndent = 0.0;
            BlockWidth = double.PositiveInfinity;
            BlockHeight = double.PositiveInfinity;
            ListBoxChoicePadding = new Size(Unit.PointToDip(2.0), Unit.PointToDip(0.5));
            TextBoxTextPadding = Unit.PointToDip(1.0);
            ButtonPadding = Unit.PointToDip(1.0);
            ButtonWidgetBorderThickness = Unit.PointToDip(1.0);
            BeveledBorderLightColor = RgbColors.White;
            Border3dEffectDarkColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.501953));
            InsetBorderLightColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293));
            PushButtonBackground = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.75293));
            ListBoxSelectionColor = new RgbColor(Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.600006), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.756866), Telerik.Windows.Documents.Core.Imaging.Color.ConvertColorComponentToByte(0.854904));
            BaselineAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
            UnderlinePattern = UnderlinePattern.None;
            DefaultLayoutType = TableLayoutType.AutoFit;
            DefaultBorderStyle = BorderStyle.Single;
            DefaultCellPadding = new Thickness(0.0);
            DefaultBorderSpacing = 0.0;
            DefaultBorderCollapse = BorderCollapse.Collapse;
            DefaultIndentAfterBullet = 30.0;
            ListLevelIndentationStep = 24.0;
            DefaultListLevelsCount = 9;
            FirstListLevelIndex = 0;
            DefaultListLevelStartIndex = 1;
            DefaultListLevelRestartAfterLevel = -1;
            ImageQuality = ImageQuality.High;
            DefaultImageFilters = new string[1]
            {
        "DCTDecode"
            };
           DefaultImageColorspace = JpegColorSpace.Rgb;
           DefaultSMaskColorspace = JpegColorSpace.Grayscale;
           DefaultFormatProviderColorspace = (ColorSpaceObject)new DeviceRgbColorSpaceObject();
           DefaultAlphaComponent = 255;
           StandardEncryptRevision = 3;
           StandardEncryptionKeyLength = 128;
        }
    }
}
