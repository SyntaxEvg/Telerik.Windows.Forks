using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class ListLevelElement : DocxElementBase
	{
		public ListLevelElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.listLevelIdAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("ilvl", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.startIndexElement = base.RegisterChildElement<StartIndexElement>("start");
			this.numberingStyleElement = base.RegisterChildElement<NumberingStyleElement>("numFmt");
			this.restartAfterLevelElement = base.RegisterChildElement<RestartAfterLevelElement>("lvlRestart");
			this.styleIdElement = base.RegisterChildElement<StyleIdElement>("pStyle");
			this.isLegalElement = base.RegisterChildElement<IsLegalElement>("isLgl");
			this.numberTextFormatElement = base.RegisterChildElement<NumberTextFormatElement>("lvlText");
			this.alignmentElement = base.RegisterChildElement<AlignmentElement>("lvlJc");
			this.paragraphPropertiesChildElement = base.RegisterChildElement<ParagraphPropertiesElement>("pPr");
			this.runPropertiesChildElement = base.RegisterChildElement<RunPropertiesElement>("rPr");
		}

		public override string ElementName
		{
			get
			{
				return "lvl";
			}
		}

		public void SetAssociatedElementInfo(ListLevelInfo listLevelInfo)
		{
			Guard.ThrowExceptionIfNull<ListLevelInfo>(listLevelInfo, "listLevelInfo");
			this.listLevelInfo = listLevelInfo;
		}

		public void SetListLevelId(int listLevelId)
		{
			this.listLevelId = listLevelId;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			this.listLevelIdAttribute.Value = this.listLevelId;
			base.CreateElement(this.startIndexElement);
			this.startIndexElement.Element.Value = this.listLevelInfo.StartIndex.Value;
			base.CreateElement(this.numberingStyleElement);
			this.numberingStyleElement.Element.Value = this.listLevelInfo.NumberingStyle.Value;
			base.CreateElement(this.numberTextFormatElement);
			this.numberTextFormatElement.Element.Value = this.listLevelInfo.NumberTextFormat;
			if (this.listLevelInfo.RestartAfterLevel != null && this.listLevelInfo.RestartAfterLevel.Value != DocumentDefaultStyleSettings.RestartAfterLevel)
			{
				base.CreateElement(this.restartAfterLevelElement);
				this.restartAfterLevelElement.Element.Value = this.listLevelInfo.RestartAfterLevel.Value + 1;
			}
			if (this.listLevelInfo.Alignment != null && this.listLevelInfo.Alignment.Value != Alignment.Left)
			{
				base.CreateElement(this.alignmentElement);
				this.alignmentElement.Element.Value = this.listLevelInfo.Alignment.Value;
			}
			if (!string.IsNullOrEmpty(this.listLevelInfo.StyleId))
			{
				base.CreateElement(this.styleIdElement);
				this.styleIdElement.Element.Value = this.listLevelInfo.StyleId;
			}
			if (this.listLevelInfo.IsLegal != null)
			{
				base.CreateElement(this.isLegalElement);
				this.isLegalElement.Element.Value = this.listLevelInfo.IsLegal.Value;
			}
			if (this.listLevelInfo.ParagraphProperties.HasLocalValues())
			{
				base.CreateElement(this.paragraphPropertiesChildElement);
				this.paragraphPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.listLevelInfo.ParagraphProperties);
			}
			if (this.listLevelInfo.CharacterProperties.HasLocalValues())
			{
				base.CreateElement(this.runPropertiesChildElement);
				this.runPropertiesChildElement.Element.SetAssociatedFlowModelElement(this.listLevelInfo.CharacterProperties);
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			this.listLevelInfo.ListLevelId = this.listLevelIdAttribute.Value;
			if (this.startIndexElement.Element != null)
			{
				this.listLevelInfo.StartIndex = new int?(this.startIndexElement.Element.Value);
				base.ReleaseElement(this.startIndexElement);
			}
			if (this.numberingStyleElement.Element != null)
			{
				this.listLevelInfo.NumberingStyle = new NumberingStyle?(this.numberingStyleElement.Element.Value);
				base.ReleaseElement(this.numberingStyleElement);
			}
			if (this.numberTextFormatElement.Element != null)
			{
				this.listLevelInfo.NumberTextFormat = this.numberTextFormatElement.Element.Value;
				base.ReleaseElement(this.numberTextFormatElement);
			}
			if (this.restartAfterLevelElement.Element != null)
			{
				this.listLevelInfo.RestartAfterLevel = new int?(this.restartAfterLevelElement.Element.Value - 1);
				base.ReleaseElement(this.restartAfterLevelElement);
			}
			if (this.alignmentElement.Element != null)
			{
				this.listLevelInfo.Alignment = new Alignment?(this.alignmentElement.Element.Value);
				base.ReleaseElement(this.alignmentElement);
			}
			if (this.styleIdElement.Element != null)
			{
				this.listLevelInfo.StyleId = this.styleIdElement.Element.Value;
				base.ReleaseElement(this.styleIdElement);
			}
			if (this.isLegalElement.Element != null)
			{
				this.listLevelInfo.IsLegal = new bool?(this.isLegalElement.Element.Value);
				base.ReleaseElement(this.isLegalElement);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "pPr")
				{
					(childElement as ParagraphPropertiesElement).SetAssociatedFlowModelElement(this.listLevelInfo.ParagraphProperties);
					return;
				}
				if (!(elementName == "rPr"))
				{
					return;
				}
				(childElement as RunPropertiesElement).SetAssociatedFlowModelElement(this.listLevelInfo.CharacterProperties);
			}
		}

		readonly IntOpenXmlAttribute listLevelIdAttribute;

		readonly OpenXmlChildElement<StartIndexElement> startIndexElement;

		readonly OpenXmlChildElement<NumberingStyleElement> numberingStyleElement;

		readonly OpenXmlChildElement<NumberTextFormatElement> numberTextFormatElement;

		readonly OpenXmlChildElement<RestartAfterLevelElement> restartAfterLevelElement;

		readonly OpenXmlChildElement<AlignmentElement> alignmentElement;

		readonly OpenXmlChildElement<StyleIdElement> styleIdElement;

		readonly OpenXmlChildElement<IsLegalElement> isLegalElement;

		readonly OpenXmlChildElement<ParagraphPropertiesElement> paragraphPropertiesChildElement;

		readonly OpenXmlChildElement<RunPropertiesElement> runPropertiesChildElement;

		ListLevelInfo listLevelInfo;

		int listLevelId;
	}
}
