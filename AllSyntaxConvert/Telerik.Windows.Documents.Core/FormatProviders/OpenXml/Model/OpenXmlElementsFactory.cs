using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.ContentTypes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Relationships;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	static class OpenXmlElementsFactory
	{
		static OpenXmlElementsFactory()
		{
			OpenXmlElementsFactory.RegisterFactoryMethod("Default", (OpenXmlPartsManager pm) => new DefaultElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("Override", (OpenXmlPartsManager pm) => new OverrideElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("Relationship", (OpenXmlPartsManager pm) => new RelationshipElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("themeElements", (OpenXmlPartsManager pm) => new ThemeElementsElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("clrScheme", (OpenXmlPartsManager pm) => new ColorSchemeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("srgbClr", (OpenXmlPartsManager pm) => new RgbHexColorModelElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent1", (OpenXmlPartsManager pm) => new Accent1Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent2", (OpenXmlPartsManager pm) => new Accent2Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent3", (OpenXmlPartsManager pm) => new Accent3Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent4", (OpenXmlPartsManager pm) => new Accent4Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent5", (OpenXmlPartsManager pm) => new Accent5Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("accent6", (OpenXmlPartsManager pm) => new Accent6Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("dk1", (OpenXmlPartsManager pm) => new Dark1Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("dk2", (OpenXmlPartsManager pm) => new Dark2Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("folHlink", (OpenXmlPartsManager pm) => new FollowedHyperlinkElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("hlink", (OpenXmlPartsManager pm) => new HlinkElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lt1", (OpenXmlPartsManager pm) => new Light1Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lt2", (OpenXmlPartsManager pm) => new Light2Element(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("fontScheme", (OpenXmlPartsManager pm) => new FontSchemeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("majorFont", (OpenXmlPartsManager pm) => new MajorFontElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("minorFont", (OpenXmlPartsManager pm) => new MinorFontElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("latin", (OpenXmlPartsManager pm) => new LatinFontElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("ea", (OpenXmlPartsManager pm) => new EastAsianFontElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("cs", (OpenXmlPartsManager pm) => new ComplexScriptFontElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("fmtScheme", (OpenXmlPartsManager pm) => new FormatSchemeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("bgFillStyleLst", (OpenXmlPartsManager pm) => new BackgroundFillStyleListElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("fillStyleLst", (OpenXmlPartsManager pm) => new FillStyleListElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("noFill", (OpenXmlPartsManager pm) => new NoFillElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("effectStyleLst", (OpenXmlPartsManager pm) => new EffectStyleListElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("effectStyle", (OpenXmlPartsManager pm) => new EffectStyleElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("effectLst", (OpenXmlPartsManager pm) => new EffectContainerElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lnStyleLst", (OpenXmlPartsManager pm) => new LineStyleListElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("ln", (OpenXmlPartsManager pm) => new OutlineElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("sysClr", (OpenXmlPartsManager pm) => new SystemColorElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("pic", (OpenXmlPartsManager pm) => new PictureElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("nvPicPr", (OpenXmlPartsManager pm) => new NonVisualPicturePropertiesElement(pm, OpenXmlNamespaces.PictureDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("cNvPicPr", (OpenXmlPartsManager pm) => new NonVisualPictureDrawingPropertiesElement(pm, OpenXmlNamespaces.PictureDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("blip", (OpenXmlPartsManager pm) => new BlipElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("xdr:spPr", (OpenXmlPartsManager pm) => new ShapePropertiesElement(pm, OpenXmlNamespaces.PictureDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("graphic", (OpenXmlPartsManager pm) => new GraphicElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("graphicData", (OpenXmlPartsManager pm) => new GraphicDataElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("graphicFrame", (OpenXmlPartsManager pm) => new GraphicFrameElement(pm, OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("nvGraphicFramePr", (OpenXmlPartsManager pm) => new NonVisualGraphicFramePropertiesElement(pm, OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("cNvGraphicFramePr", (OpenXmlPartsManager pm) => new NonVisualGraphicFrameDrawingPropertiesElement(pm, OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("cNvPr", (OpenXmlPartsManager pm) => new NonVisualDrawingPropertiesElement(pm, "cNvPr", OpenXmlNamespaces.PictureDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("a:xfrm", (OpenXmlPartsManager pm) => new TransformElement(pm, OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("xdr:xfrm", (OpenXmlPartsManager pm) => new TransformElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("ext", (OpenXmlPartsManager pm) => new Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing.SizeElement(pm, "ext", OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("off", (OpenXmlPartsManager pm) => new OffsetElement(pm, "off", OpenXmlNamespaces.DrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("prstGeom", (OpenXmlPartsManager pm) => new PresetGeometryElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("fillRect", (OpenXmlPartsManager pm) => new FillRectElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("stretch", (OpenXmlPartsManager pm) => new StretchElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("picLocks", (OpenXmlPartsManager pm) => new PictureLockingElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("solidFill", (OpenXmlPartsManager pm) => new SolidFillElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("schemeClr", (OpenXmlPartsManager pm) => new SchemeColorElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("bodyPr", (OpenXmlPartsManager pm) => new BodyPropertiesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("p", (OpenXmlPartsManager pm) => new ParagraphElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("r", (OpenXmlPartsManager pm) => new RichTextRunElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("t", (OpenXmlPartsManager pm) => new TextElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("a:p", (OpenXmlPartsManager pm) => new ParagraphElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("a:r", (OpenXmlPartsManager pm) => new RichTextRunElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("a:t", (OpenXmlPartsManager pm) => new TextElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lumMod", (OpenXmlPartsManager pm) => new LuminanceModulationElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lumOff", (OpenXmlPartsManager pm) => new LuminanceOffsetElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("tint", (OpenXmlPartsManager pm) => new TintElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("chart", (OpenXmlPartsManager pm) => new ChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("plotArea", (OpenXmlPartsManager pm) => new PlotAreaElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("barChart", (OpenXmlPartsManager pm) => new BarChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lineChart", (OpenXmlPartsManager pm) => new LineChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("pieChart", (OpenXmlPartsManager pm) => new PieChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("doughnutChart", (OpenXmlPartsManager pm) => new DoughnutChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("areaChart", (OpenXmlPartsManager pm) => new AreaChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("barDir", (OpenXmlPartsManager pm) => new BarDirectionElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("axId", (OpenXmlPartsManager pm) => new AxisIdElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("ser", (OpenXmlPartsManager pm) => new SeriesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("cat", (OpenXmlPartsManager pm) => new SeriesCategoriesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("val", (OpenXmlPartsManager pm) => new SeriesValuesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("xVal", (OpenXmlPartsManager pm) => new SeriesXValuesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("yVal", (OpenXmlPartsManager pm) => new SeriesYValuesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("bubbleSize", (OpenXmlPartsManager pm) => new SeriesBubbleSizeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("smooth", (OpenXmlPartsManager pm) => new SmoothElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("strRef", (OpenXmlPartsManager pm) => new StringReferenceElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("numRef", (OpenXmlPartsManager pm) => new NumberReferenceElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("strLit", (OpenXmlPartsManager pm) => new StringLiteralsElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("numLit", (OpenXmlPartsManager pm) => new NumberLiteralsElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("order", (OpenXmlPartsManager pm) => new SeriesOrderElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("idx", (OpenXmlPartsManager pm) => new SeriesIndexElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("v", (OpenXmlPartsManager pm) => new ValueElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("c:v", (OpenXmlPartsManager pm) => new ValueElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("pt", (OpenXmlPartsManager pm) => new DataPointElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("catAx", (OpenXmlPartsManager pm) => new CategoryAxisElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("valAx", (OpenXmlPartsManager pm) => new ValueAxisElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("dateAx", (OpenXmlPartsManager pm) => new DateAxisElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("scaling", (OpenXmlPartsManager pm) => new ScalingElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("axPos", (OpenXmlPartsManager pm) => new AxisPositionElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("crossAx", (OpenXmlPartsManager pm) => new CrossesAxisElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("delete", (OpenXmlPartsManager pm) => new DeleteElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("barGrouping", (OpenXmlPartsManager pm) => new BarGroupingElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("lineAreaGrouping", (OpenXmlPartsManager pm) => new GroupingElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("overlap", (OpenXmlPartsManager pm) => new OverlapElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("roundedCorners", (OpenXmlPartsManager pm) => new RoundedCornersElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("charttx", (OpenXmlPartsManager pm) => new ChartTextElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("seriestx", (OpenXmlPartsManager pm) => new SeriesTextElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("title", (OpenXmlPartsManager pm) => new TitleElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("rich", (OpenXmlPartsManager pm) => new RichTextElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("legend", (OpenXmlPartsManager pm) => new LegendElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("legendPos", (OpenXmlPartsManager pm) => new LegendPositionElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("holeSize", (OpenXmlPartsManager pm) => new HoleSizeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("f", (OpenXmlPartsManager pm) => new FormulaElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("c:f", (OpenXmlPartsManager pm) => new FormulaElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("c:chart", (OpenXmlPartsManager pm) => new ChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("chartReference", (OpenXmlPartsManager pm) => new ChartReferenceElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("numFmt", (OpenXmlPartsManager pm) => new NumberFormatElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("c:numFmt", (OpenXmlPartsManager pm) => new NumberFormatElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("overlay", (OpenXmlPartsManager pm) => new OverlayElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("min", (OpenXmlPartsManager pm) => new MinAxisValueElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("max", (OpenXmlPartsManager pm) => new MaxAxisValueElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("majorGridlines", (OpenXmlPartsManager pm) => new MajorGridlinesElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("c:spPr", (OpenXmlPartsManager pm) => new ShapePropertiesElement(pm, OpenXmlNamespaces.ChartDrawingMLNamespace));
			OpenXmlElementsFactory.RegisterFactoryMethod("scatterChart", (OpenXmlPartsManager pm) => new ScatterChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("bubbleChart", (OpenXmlPartsManager pm) => new BubbleChartElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("marker", (OpenXmlPartsManager pm) => new MarkerElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("size", (OpenXmlPartsManager pm) => new Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart.SizeElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("symbol", (OpenXmlPartsManager pm) => new SymbolElement(pm));
			OpenXmlElementsFactory.RegisterFactoryMethod("scatterStyle", (OpenXmlPartsManager pm) => new ScatterStyleElement(pm));
		}

		public static OpenXmlElementBase CreateInstance(string elementName, OpenXmlPartsManager partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlPartsManager>(partsManager, "partsManager");
			return OpenXmlElementsFactory.elementNameToFactoryMethod[elementName](partsManager);
		}

		public static bool CanCreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return OpenXmlElementsFactory.elementNameToFactoryMethod.ContainsKey(elementName);
		}

		static void RegisterFactoryMethod(string elementName, Func<OpenXmlPartsManager, OpenXmlElementBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<Func<OpenXmlPartsManager, OpenXmlElementBase>>(factoryMethod, "factoryMethod");
			OpenXmlElementsFactory.elementNameToFactoryMethod.Add(elementName, factoryMethod);
		}

		static readonly Dictionary<string, Func<OpenXmlPartsManager, OpenXmlElementBase>> elementNameToFactoryMethod = new Dictionary<string, Func<OpenXmlPartsManager, OpenXmlElementBase>>();
	}
}
