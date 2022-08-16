using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class AbstractListElement : DocxElementBase
	{
		public AbstractListElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.abstractListIdAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("abstractNumId", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.multiLevelTypeElement = base.RegisterChildElement<MultiLevelTypeElement>("multiLevelType");
			this.numStyleLinkElement = base.RegisterChildElement<NumStyleLinkElement>("numStyleLink");
			this.styleLinkElement = base.RegisterChildElement<StyleLinkElement>("styleLink");
		}

		public override string ElementName
		{
			get
			{
				return "abstractNum";
			}
		}

		public void SetAssociatedElementInfo(AbstractListInfo abstractListInfo)
		{
			Guard.ThrowExceptionIfNull<AbstractListInfo>(abstractListInfo, "abstractListInfo");
			this.abstractListInfo = abstractListInfo;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			if (this.abstractListInfo.IsAbstractListForStyle)
			{
				this.abstractListIdAttribute.Value = context.ListsExportContext.GetDocxListIds(this.abstractListInfo.Id).AbstractListIdForStyle;
				base.CreateElement(this.numStyleLinkElement);
				this.numStyleLinkElement.Element.Value = this.abstractListInfo.NumStyleLink;
			}
			else
			{
				this.abstractListIdAttribute.Value = context.ListsExportContext.GetDocxListIds(this.abstractListInfo.Id).AbstractListId;
				if (!string.IsNullOrEmpty(this.abstractListInfo.StyleLink))
				{
					base.CreateElement(this.styleLinkElement);
					this.styleLinkElement.Element.Value = this.abstractListInfo.StyleLink;
				}
			}
			base.CreateElement(this.multiLevelTypeElement);
			this.multiLevelTypeElement.Element.Value = this.abstractListInfo.MultilevelType.Value;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			this.abstractListInfo.Id = this.abstractListIdAttribute.Value;
			if (this.numStyleLinkElement.Element != null)
			{
				this.abstractListInfo.NumStyleLink = this.numStyleLinkElement.Element.Value;
				base.ReleaseElement(this.numStyleLinkElement);
			}
			if (this.styleLinkElement.Element != null)
			{
				this.abstractListInfo.StyleLink = this.styleLinkElement.Element.Value;
				base.ReleaseElement(this.styleLinkElement);
			}
			if (this.multiLevelTypeElement.Element != null)
			{
				this.abstractListInfo.MultilevelType = new MultilevelType?(this.multiLevelTypeElement.Element.Value);
				base.ReleaseElement(this.multiLevelTypeElement);
			}
			context.ListsImportContext.AbstractListInfoCollection.Add(this.abstractListInfo.Id, this.abstractListInfo);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			if (!this.abstractListInfo.IsAbstractListForStyle)
			{
				for (int i = 0; i < this.abstractListInfo.Levels.Count; i++)
				{
					ListLevelElement listLevelElement = base.CreateElement<ListLevelElement>("lvl");
					listLevelElement.SetAssociatedElementInfo(this.abstractListInfo.Levels[i]);
					listLevelElement.SetListLevelId(i);
					yield return listLevelElement;
				}
			}
			yield break;
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
				ListLevelInfo listLevelInfo = new ListLevelInfo();
				this.abstractListInfo.Levels.Add(listLevelInfo);
				((ListLevelElement)childElement).SetAssociatedElementInfo(listLevelInfo);
			}
		}

		readonly IntOpenXmlAttribute abstractListIdAttribute;

		readonly OpenXmlChildElement<NumStyleLinkElement> numStyleLinkElement;

		readonly OpenXmlChildElement<StyleLinkElement> styleLinkElement;

		readonly OpenXmlChildElement<MultiLevelTypeElement> multiLevelTypeElement;

		AbstractListInfo abstractListInfo;
	}
}
