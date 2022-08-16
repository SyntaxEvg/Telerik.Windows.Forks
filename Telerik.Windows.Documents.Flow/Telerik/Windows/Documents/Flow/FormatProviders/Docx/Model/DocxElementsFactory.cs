using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Comments;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.DefaultStyles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.StylePropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableCellPropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableRowPropertiesElements;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	static class DocxElementsFactory
	{
		static DocxElementsFactory()
		{
			DocxElementsFactory.RegisterFactoryMethod("body", (DocxPartsManager pm) => new BodyElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("p", (DocxPartsManager pm) => new ParagraphElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("r", (DocxPartsManager pm) => new RunElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("t", (DocxPartsManager pm) => new TextElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("instrText", (DocxPartsManager pm) => new InstructionTextElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tab", (DocxPartsManager pm) => new TabElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tbl", (DocxPartsManager pm) => new TableElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tr", (DocxPartsManager pm) => new TableRowElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tc", (DocxPartsManager pm) => new TableCellElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("fldChar", (DocxPartsManager pm) => new FieldCharacterElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("hyperlink", (DocxPartsManager pm) => new HyperlinkElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("fldSimple", (DocxPartsManager pm) => new SimpleFieldElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("bookmarkStart", (DocxPartsManager pm) => new BookmarkStartElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("bookmarkEnd", (DocxPartsManager pm) => new BookmarkEndElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("br", (DocxPartsManager pm) => new BreakElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("commentRangeStart", (DocxPartsManager pm) => new CommentRangeStartElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("commentRangeEnd", (DocxPartsManager pm) => new CommentRangeEndElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("commentReference", (DocxPartsManager pm) => new CommentReferenceElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("permStart", (DocxPartsManager pm) => new PermissionStartElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("permEnd", (DocxPartsManager pm) => new PermissionEndElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("comment", (DocxPartsManager pm) => new CommentElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("abstractNum", (DocxPartsManager pm) => new AbstractListElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("abstractNumId", (DocxPartsManager pm) => new AbstractListIdElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("num", (DocxPartsManager pm) => new NumberedListElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("lvl", (DocxPartsManager pm) => new ListLevelElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("lvlText", (DocxPartsManager pm) => new NumberTextFormatElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("multiLevelType", (DocxPartsManager pm) => new MultiLevelTypeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("numFmt", (DocxPartsManager pm) => new NumberingStyleElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("numStyleLink", (DocxPartsManager pm) => new NumStyleLinkElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("styleLink", (DocxPartsManager pm) => new StyleLinkElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("start", (DocxPartsManager pm) => new StartIndexElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("lvlRestart", (DocxPartsManager pm) => new RestartAfterLevelElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("isLgl", (DocxPartsManager pm) => new IsLegalElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("lvlJc", (DocxPartsManager pm) => new AlignmentElement(pm, "lvlJc"));
			DocxElementsFactory.RegisterFactoryMethod("startOverride", (DocxPartsManager pm) => new StartIndexOverrideElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("lvlOverride", (DocxPartsManager pm) => new ListLevelOverrideElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("docDefaults", (DocxPartsManager pm) => new DocumentDefaultStylesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pPrDefault", (DocxPartsManager pm) => new ParagraphDefaultPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("rPrDefault", (DocxPartsManager pm) => new RunDefaultPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("style", (DocxPartsManager pm) => new StyleElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("name", (DocxPartsManager pm) => new StyleNameElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("qFormat", (DocxPartsManager pm) => new IsPrimaryElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("next", (DocxPartsManager pm) => new NextStyleIdElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("link", (DocxPartsManager pm) => new LinkedStyleIdElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("uiPriority", (DocxPartsManager pm) => new UIPriorityElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("basedOn", (DocxPartsManager pm) => new BasedOnStyleIdElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("rPr", (DocxPartsManager pm) => new RunPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pPr", (DocxPartsManager pm) => new ParagraphPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("trPr", (DocxPartsManager pm) => new TableRowPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblPr", (DocxPartsManager pm) => new TablePropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tcPr", (DocxPartsManager pm) => new TableCellPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("sectPr", (DocxPartsManager pm) => new SectionPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("shd", (DocxPartsManager pm) => new ShadingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("jc", (DocxPartsManager pm) => new AlignmentElement(pm, "jc"));
			DocxElementsFactory.RegisterFactoryMethod("left", (DocxPartsManager pm) => new BorderElement(pm, "left"));
			DocxElementsFactory.RegisterFactoryMethod("top", (DocxPartsManager pm) => new BorderElement(pm, "top"));
			DocxElementsFactory.RegisterFactoryMethod("right", (DocxPartsManager pm) => new BorderElement(pm, "right"));
			DocxElementsFactory.RegisterFactoryMethod("bottom", (DocxPartsManager pm) => new BorderElement(pm, "bottom"));
			DocxElementsFactory.RegisterFactoryMethod("between", (DocxPartsManager pm) => new BorderElement(pm, "between"));
			DocxElementsFactory.RegisterFactoryMethod("insideH", (DocxPartsManager pm) => new BorderElement(pm, "insideH"));
			DocxElementsFactory.RegisterFactoryMethod("insideV", (DocxPartsManager pm) => new BorderElement(pm, "insideV"));
			DocxElementsFactory.RegisterFactoryMethod("tl2br", (DocxPartsManager pm) => new BorderElement(pm, "tl2br"));
			DocxElementsFactory.RegisterFactoryMethod("tr2bl", (DocxPartsManager pm) => new BorderElement(pm, "tr2bl"));
			DocxElementsFactory.RegisterFactoryMethod("vAlign", (DocxPartsManager pm) => new Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document.VerticalAlignmentElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("b", (DocxPartsManager pm) => new BoldElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("vertAlign", (DocxPartsManager pm) => new BaselineAlignmentElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("sz", (DocxPartsManager pm) => new FontSizeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("i", (DocxPartsManager pm) => new ItalicElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("rFonts", (DocxPartsManager pm) => new FontElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("color", (DocxPartsManager pm) => new ForegroundColorElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("highlight", (DocxPartsManager pm) => new HighlightElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("strike", (DocxPartsManager pm) => new StrikethroughElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("u", (DocxPartsManager pm) => new UnderlineElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("rtl", (DocxPartsManager pm) => new FlowDirectionElement(pm, "rtl"));
			DocxElementsFactory.RegisterFactoryMethod("rStyle", (DocxPartsManager pm) => new StyleIdElement(pm, "rStyle"));
			DocxElementsFactory.RegisterFactoryMethod("bidi", (DocxPartsManager pm) => new FlowDirectionElement(pm, "bidi"));
			DocxElementsFactory.RegisterFactoryMethod("keepLines", (DocxPartsManager pm) => new KeepOnOnePageElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("keepNext", (DocxPartsManager pm) => new KeepWithNextParagraphElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("kinsoku", (DocxPartsManager pm) => new ApplyEastAsianLineBreakingRulesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pageBreakBefore", (DocxPartsManager pm) => new PageBreakBeforeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("contextualSpacing", (DocxPartsManager pm) => new ContextualSpacingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("mirrorIndents", (DocxPartsManager pm) => new MirrorIndentsElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("ind", (DocxPartsManager pm) => new IndentationElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("overflowPunct", (DocxPartsManager pm) => new AllowOverflowPunctuationElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("numPr", (DocxPartsManager pm) => new NumberingPropertiesElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("numId", (DocxPartsManager pm) => new ListIdElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("ilvl", (DocxPartsManager pm) => new ParagraphListLevel(pm));
			DocxElementsFactory.RegisterFactoryMethod("spacing", (DocxPartsManager pm) => new SpacingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("outlineLvl", (DocxPartsManager pm) => new OutlineLevelElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pBdr", (DocxPartsManager pm) => new ParagraphBordersElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pStyle", (DocxPartsManager pm) => new StyleIdElement(pm, "pStyle"));
			DocxElementsFactory.RegisterFactoryMethod("tabs", (DocxPartsManager pm) => new TabStopsElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblBorders", (DocxPartsManager pm) => new TableBordersElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblCellSpacing", (DocxPartsManager pm) => new TableWidthElement(pm, "tblCellSpacing"));
			DocxElementsFactory.RegisterFactoryMethod("tblInd", (DocxPartsManager pm) => new TableWidthElement(pm, "tblInd"));
			DocxElementsFactory.RegisterFactoryMethod("tblCellMar", (DocxPartsManager pm) => new TableCellPaddingElement(pm, "tblCellMar"));
			DocxElementsFactory.RegisterFactoryMethod("tblStyleRowBandSize", (DocxPartsManager pm) => new RowBandingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblStyleColBandSize", (DocxPartsManager pm) => new ColumnBandingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblW", (DocxPartsManager pm) => new TableWidthElement(pm, "tblW"));
			DocxElementsFactory.RegisterFactoryMethod("bidiVisual", (DocxPartsManager pm) => new FlowDirectionElement(pm, "bidiVisual"));
			DocxElementsFactory.RegisterFactoryMethod("tblLayout", (DocxPartsManager pm) => new TableLayoutTypeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblLook", (DocxPartsManager pm) => new TableLooksElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblOverlap", (DocxPartsManager pm) => new OverlapElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblStyle", (DocxPartsManager pm) => new StyleIdElement(pm, "tblStyle"));
			DocxElementsFactory.RegisterFactoryMethod("headerReference", (DocxPartsManager pm) => new HeaderReferenceElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("footerReference", (DocxPartsManager pm) => new FooterReferenceElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("titlePg", (DocxPartsManager pm) => new TitlePageElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("type", (DocxPartsManager pm) => new SectionTypeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pgMar", (DocxPartsManager pm) => new PageMarginsElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pgSz", (DocxPartsManager pm) => new PageSizeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pgNumType", (DocxPartsManager pm) => new PageNumberingSettingsElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tblHeader", (DocxPartsManager pm) => new RepeatOnEveryPageElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("cantSplit", (DocxPartsManager pm) => new CanSplitElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("trHeight", (DocxPartsManager pm) => new TableRowHeightElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("gridSpan", (DocxPartsManager pm) => new GridSpanElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("vMerge", (DocxPartsManager pm) => new VerticalMergeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("hMerge", (DocxPartsManager pm) => new HorizontalMergeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tcBorders", (DocxPartsManager pm) => new TableCellBordersElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tcMar", (DocxPartsManager pm) => new TableCellPaddingElement(pm, "tcMar"));
			DocxElementsFactory.RegisterFactoryMethod("hideMark", (DocxPartsManager pm) => new IgnoreCellMarkerInRowHeightCalculationElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("noWrap", (DocxPartsManager pm) => new CanWrapContentElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("tcW", (DocxPartsManager pm) => new TableWidthElement(pm, "tcW"));
			DocxElementsFactory.RegisterFactoryMethod("textDirection", (DocxPartsManager pm) => new TextDirectionElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("extent", (DocxPartsManager pm) => new SizeElement(pm, "extent", OpenXmlNamespaces.WordprocessingDrawingMLNamespace));
			DocxElementsFactory.RegisterFactoryMethod("drawing", (DocxPartsManager pm) => new DrawingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("inline", (DocxPartsManager pm) => new InlineElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("anchor", (DocxPartsManager pm) => new AnchorElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("docPr", (DocxPartsManager pm) => new NonVisualDrawingPropertiesElement(pm, "docPr", OpenXmlNamespaces.WordprocessingDrawingMLNamespace));
			DocxElementsFactory.RegisterFactoryMethod("simplePos", (DocxPartsManager pm) => new OffsetElement(pm, "simplePos", OpenXmlNamespaces.WordprocessingDrawingMLNamespace));
			DocxElementsFactory.RegisterFactoryMethod("positionH", (DocxPartsManager pm) => new HorizontalPositionElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("positionV", (DocxPartsManager pm) => new VerticalPositionElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("posOffset", (DocxPartsManager pm) => new OneDimentionOffsetElement(pm, "posOffset", OpenXmlNamespaces.WordprocessingDrawingMLNamespace));
			DocxElementsFactory.RegisterFactoryMethod("wrapNone", (DocxPartsManager pm) => new WrapNoneElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("wrapSquare", (DocxPartsManager pm) => new WrapSquareElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("wrapTopAndBottom", (DocxPartsManager pm) => new WrapTopAndBottomElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("blipFill", (DocxPartsManager pm) => new PictureFillElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("effectExtent", (DocxPartsManager pm) => new EffectExtentElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("pict", (DocxPartsManager pm) => new VmlContainerElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("shape", (DocxPartsManager pm) => new ShapeElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("imagedata", (DocxPartsManager pm) => new ImageDataElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("fill", (DocxPartsManager pm) => new FillElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("textpath", (DocxPartsManager pm) => new TextpathElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("evenAndOddHeaders", (DocxPartsManager pm) => new EvenAndOddHeadersFootersElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("updateFields", (DocxPartsManager pm) => new UpdateFieldsElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("compat", (DocxPartsManager pm) => new CompatibilityElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("compatSetting", (DocxPartsManager pm) => new CompatibilitySettingElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("docVars", (DocxPartsManager pm) => new DocumentVariableCollectionElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("docVar", (DocxPartsManager pm) => new DocumentVariableElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("documentProtection", (DocxPartsManager pm) => new DocumentProtectionElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("defaultTabStop", (DocxPartsManager pm) => new DefaultTabStopWidthElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("sdt", (DocxPartsManager pm) => new StructuredDocumentTagElement(pm));
			DocxElementsFactory.RegisterFactoryMethod("sdtContent", (DocxPartsManager pm) => new StructuredDocumentTagContentElement(pm));
		}

		public static OpenXmlElementBase CreateInstance(string elementName, DocxPartsManager partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<DocxPartsManager>(partsManager, "partsManager");
			return DocxElementsFactory.elementNameToFactoryMethod[elementName](partsManager);
		}

		public static bool CanCreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return DocxElementsFactory.elementNameToFactoryMethod.ContainsKey(elementName);
		}

		static void RegisterFactoryMethod(string elementName, Func<DocxPartsManager, OpenXmlElementBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<Func<DocxPartsManager, OpenXmlElementBase>>(factoryMethod, "factoryMethod");
			DocxElementsFactory.elementNameToFactoryMethod.Add(elementName, factoryMethod);
		}

		static readonly Dictionary<string, Func<DocxPartsManager, OpenXmlElementBase>> elementNameToFactoryMethod = new Dictionary<string, Func<DocxPartsManager, OpenXmlElementBase>>();
	}
}
