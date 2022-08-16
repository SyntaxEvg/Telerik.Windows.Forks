using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class ListsElement : DocxPartRootElementBase
	{
		public ListsElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
		}

		public override string ElementName
		{
			get
			{
				return "numbering";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return context.Document.Lists.Count > 0;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			foreach (NumberingListInfo numberingListInfo in context.ListsImportContext.NumberingListInfoCollection.Values)
			{
				AbstractListInfo abstractListInfo = context.ListsImportContext.AbstractListInfoCollection[numberingListInfo.AbstractListId];
				if (string.IsNullOrEmpty(abstractListInfo.NumStyleLink))
				{
					if (numberingListInfo.ListLevelOverrideCollection.Count == 0)
					{
						List list = abstractListInfo.GetList();
						context.Document.Lists.Add(list);
						int id = abstractListInfo.Id;
						context.ListsImportContext.AddAbstractListToListMapping(abstractListInfo.Id, list.Id, out id);
						numberingListInfo.AbstractListId = id;
					}
					else
					{
						AbstractListInfo abstractListInfo2 = new AbstractListInfo(abstractListInfo);
						for (int i = 0; i < numberingListInfo.ListLevelOverrideCollection.Count; i++)
						{
							ListLevelInfo listLevelInfo = numberingListInfo.ListLevelOverrideCollection[i];
							abstractListInfo2.Levels[listLevelInfo.ListLevelId].CopyPropertiesFrom(listLevelInfo);
						}
						List list2 = abstractListInfo2.GetList();
						context.Document.Lists.Add(list2);
						int id2 = abstractListInfo.Id;
						context.ListsImportContext.AddAbstractListToListMapping(abstractListInfo2.Id, list2.Id, out id2);
						numberingListInfo.AbstractListId = id2;
					}
				}
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (List list in context.Document.Lists)
			{
				bool complexList = !string.IsNullOrEmpty(list.StyleId);
				AbstractListElement abstractListElement = base.CreateElement<AbstractListElement>("abstractNum");
				abstractListElement.SetAssociatedElementInfo(new AbstractListInfo(list, false));
				yield return abstractListElement;
				if (complexList)
				{
					AbstractListElement styleRelatedAbstractListElement = base.CreateElement<AbstractListElement>("abstractNum");
					styleRelatedAbstractListElement.SetAssociatedElementInfo(new AbstractListInfo(list, true));
					yield return styleRelatedAbstractListElement;
				}
			}
			foreach (List list2 in context.Document.Lists)
			{
				bool complexList2 = !string.IsNullOrEmpty(list2.StyleId);
				NumberingListInfo numberingListInfo = new NumberingListInfo();
				numberingListInfo.Id = context.ListsExportContext.GetDocxListIds(list2.Id).NumberedListIdForParagraph;
				numberingListInfo.AbstractListId = context.ListsExportContext.GetDocxListIds(list2.Id).AbstractListId;
				NumberedListElement numberedListElement = base.CreateElement<NumberedListElement>("num");
				numberedListElement.SetAssociatedElementInfo(numberingListInfo);
				yield return numberedListElement;
				if (complexList2)
				{
					NumberingListInfo styleNumberingListInfo = new NumberingListInfo();
					styleNumberingListInfo.Id = context.ListsExportContext.GetDocxListIds(list2.Id).NumberedListIdForStyle.Value;
					styleNumberingListInfo.AbstractListId = context.ListsExportContext.GetDocxListIds(list2.Id).AbstractListIdForStyle;
					NumberedListElement styleNumberedListElement = base.CreateElement<NumberedListElement>("num");
					styleNumberedListElement.SetAssociatedElementInfo(styleNumberingListInfo);
					yield return styleNumberedListElement;
				}
			}
			yield break;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "num")
				{
					(childElement as NumberedListElement).SetAssociatedElementInfo(new NumberingListInfo());
					return;
				}
				if (!(elementName == "abstractNum"))
				{
					return;
				}
				(childElement as AbstractListElement).SetAssociatedElementInfo(new AbstractListInfo());
			}
		}
	}
}
