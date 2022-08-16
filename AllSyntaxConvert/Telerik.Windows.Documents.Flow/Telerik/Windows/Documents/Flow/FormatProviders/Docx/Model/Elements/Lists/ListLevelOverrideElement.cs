using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class ListLevelOverrideElement : DocxElementBase
	{
		public ListLevelOverrideElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.listLevelIdAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("ilvl", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.startIndexOverride = base.RegisterChildElement<StartIndexOverrideElement>("startOverride");
		}

		public override string ElementName
		{
			get
			{
				return "lvlOverride";
			}
		}

		public void SetAssociatedElementInfo(ListLevelInfo listLevelInfo)
		{
			Guard.ThrowExceptionIfNull<ListLevelInfo>(listLevelInfo, "listLevelInfo");
			this.listLevelInfo = listLevelInfo;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			this.listLevelInfo.ListLevelId = this.listLevelIdAttribute.Value;
			if (this.startIndexOverride.Element != null && this.listLevelInfo.StartIndex == null)
			{
				this.listLevelInfo.StartIndex = new int?(this.startIndexOverride.Element.Value);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "element");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "lvl"))
				{
					return;
				}
				((ListLevelElement)childElement).SetAssociatedElementInfo(this.listLevelInfo);
			}
		}

		readonly IntOpenXmlAttribute listLevelIdAttribute;

		readonly OpenXmlChildElement<StartIndexOverrideElement> startIndexOverride;

		ListLevelInfo listLevelInfo;
	}
}
