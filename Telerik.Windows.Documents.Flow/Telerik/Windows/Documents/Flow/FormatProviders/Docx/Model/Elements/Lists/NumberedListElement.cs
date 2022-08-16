using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class NumberedListElement : DocxElementBase
	{
		public NumberedListElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.listIdAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("numId", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.abstractListId = base.RegisterChildElement<AbstractListIdElement>("abstractNumId");
		}

		public override string ElementName
		{
			get
			{
				return "num";
			}
		}

		public void SetAssociatedElementInfo(NumberingListInfo numberingListInfo)
		{
			Guard.ThrowExceptionIfNull<NumberingListInfo>(numberingListInfo, "numberingListInfo");
			this.numberingListInfo = numberingListInfo;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			this.listIdAttribute.Value = this.numberingListInfo.Id;
			base.CreateElement(this.abstractListId);
			this.abstractListId.Element.Value = this.numberingListInfo.AbstractListId;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (this.listIdAttribute.HasValue)
			{
				this.numberingListInfo.Id = this.listIdAttribute.Value;
			}
			if (this.abstractListId.Element != null)
			{
				this.numberingListInfo.AbstractListId = this.abstractListId.Element.Value;
				base.ReleaseElement(this.abstractListId);
			}
			context.ListsImportContext.NumberingListInfoCollection.Add(this.numberingListInfo.Id, this.numberingListInfo);
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "element");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "lvlOverride"))
				{
					return;
				}
				ListLevelInfo listLevelInfo = new ListLevelInfo();
				this.numberingListInfo.ListLevelOverrideCollection.Add(listLevelInfo);
				((ListLevelOverrideElement)childElement).SetAssociatedElementInfo(listLevelInfo);
			}
		}

		readonly IntOpenXmlAttribute listIdAttribute;

		readonly OpenXmlChildElement<AbstractListIdElement> abstractListId;

		NumberingListInfo numberingListInfo;
	}
}
