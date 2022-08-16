using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class ListsExportContext
	{
		public ListsExportContext(RadFlowDocument document)
		{
			this.document = document;
			this.InitializeCollections();
		}

		public int GetNumberedListIdForParagraphOrStyle(ParagraphProperties paragraphProperties)
		{
			if (paragraphProperties.OwnerDocumentElement == null)
			{
				return this.listAbstraction[paragraphProperties.ListId.LocalValue.Value].NumberedListIdForParagraph;
			}
			if (this.listAbstraction[paragraphProperties.ListId.LocalValue.Value].NumberedListIdForStyle != null)
			{
				return this.listAbstraction[paragraphProperties.ListId.LocalValue.Value].NumberedListIdForStyle.Value;
			}
			return this.listAbstraction[paragraphProperties.ListId.LocalValue.Value].NumberedListIdForParagraph;
		}

		public DocxListIds GetDocxListIds(int listId)
		{
			return this.listAbstraction[listId];
		}

		void InitializeCollections()
		{
			if (this.document.Lists.Count <= 0)
			{
				return;
			}
			foreach (List list in this.document.Lists)
			{
				if (!string.IsNullOrEmpty(list.StyleId))
				{
					this.ExtractComplexListIds(list);
				}
				else
				{
					this.ExtractSimpleListIds(list);
				}
			}
		}

		void ExtractComplexListIds(List list)
		{
			int next = this.numberedListIdGenerator.GetNext();
			int next2 = this.numberedListIdGenerator.GetNext();
			int next3 = this.abstractListIdGenerator.GetNext();
			int next4 = this.abstractListIdGenerator.GetNext();
			DocxListIds docxListIds = new DocxListIds();
			docxListIds.NumberedListIdForParagraph = next;
			docxListIds.NumberedListIdForStyle = new int?(next2);
			docxListIds.AbstractListId = next3;
			docxListIds.AbstractListIdForStyle = next4;
			this.listAbstraction.Add(list.Id, docxListIds);
		}

		void ExtractSimpleListIds(List list)
		{
			int next = this.numberedListIdGenerator.GetNext();
			int next2 = this.abstractListIdGenerator.GetNext();
			DocxListIds docxListIds = new DocxListIds();
			docxListIds.AbstractListId = next2;
			docxListIds.NumberedListIdForParagraph = next;
			this.listAbstraction.Add(list.Id, docxListIds);
		}

		readonly RadFlowDocument document;

		readonly IdGenerator numberedListIdGenerator = new IdGenerator(1);

		readonly IdGenerator abstractListIdGenerator = new IdGenerator(0);

		readonly Dictionary<int, DocxListIds> listAbstraction = new Dictionary<int, DocxListIds>();
	}
}
