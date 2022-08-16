using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Lists
{
	public sealed class ListCollection : IEnumerable<List>, IEnumerable
	{
		internal ListCollection(RadFlowDocument parent)
		{
			this.document = parent;
		}

		public int Count
		{
			get
			{
				return this.listCollection.Count;
			}
		}

		internal RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public List GetList(int id)
		{
			return (from l in this.listCollection
				where l.Id == id
				select l).FirstOrDefault<List>();
		}

		public List Add(ListTemplateType listTemplate)
		{
			List list = ListFactory.GetList(listTemplate);
			this.Add(list);
			return list;
		}

		public void Add(List list)
		{
			if (list.Document != null)
			{
				throw new InvalidOperationException("The list is already added to the list collection of another document.");
			}
			list.Document = this.Document;
			list.Id = this.listStyleIdGenerator.GetNext();
			this.listCollection.Add(list);
		}

		public bool Remove(int listId)
		{
			List list = this.GetList(listId);
			return this.Remove(list);
		}

		public bool Remove(List list)
		{
			bool flag = this.listCollection.Remove(list);
			if (flag)
			{
				foreach (Paragraph paragraph in list.Document.EnumerateChildrenOfType<Paragraph>())
				{
					if (paragraph.ListId == list.Id)
					{
						paragraph.Properties.ListId.ClearValue();
						paragraph.Properties.ListLevel.ClearValue();
					}
				}
				foreach (Style style in list.Document.StyleRepository.Styles)
				{
					if (style.ParagraphProperties.ListId.LocalValue == list.Id)
					{
						style.ParagraphProperties.ListId.ClearValue();
						style.ParagraphProperties.ListLevel.ClearValue();
					}
				}
				list.Document = null;
			}
			return flag;
		}

		public bool Contains(List list)
		{
			return this.listCollection.Contains(list);
		}

		public void Clear()
		{
			foreach (List list in this.listCollection.ToList<List>())
			{
				this.Remove(list);
			}
		}

		public IEnumerator<List> GetEnumerator()
		{
			return this.listCollection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.listCollection.GetEnumerator();
		}

		internal void AddClonedListsFrom(ListCollection fromListCollection)
		{
			Guard.ThrowExceptionIfNull<ListCollection>(fromListCollection, "fromListCollection");
			foreach (List list in fromListCollection)
			{
				this.Add(list.Clone());
			}
		}

		internal void Merge(ListCollection withListCollection, CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<ListCollection>(withListCollection, "withListCollection");
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			ListCollectionMerger listCollectionMerger = new ListCollectionMerger(this, withListCollection);
			listCollectionMerger.Merge(cloneContext);
			foreach (KeyValuePair<int, int> keyValuePair in cloneContext.ReinitializedLists)
			{
				int key = keyValuePair.Key;
				if (cloneContext.OldListsToStyles.ContainsKey(key))
				{
					foreach (string styleId in cloneContext.OldListsToStyles[key])
					{
						Style style = this.Document.StyleRepository.GetStyle(styleId);
						int value = cloneContext.ReinitializedLists[key];
						style.ParagraphProperties.ListId.LocalValue = new int?(value);
					}
				}
			}
		}

		readonly List<List> listCollection = new List<List>();

		readonly RadFlowDocument document;

		readonly IdGenerator listStyleIdGenerator = new IdGenerator();
	}
}
