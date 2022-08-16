using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	public class List
	{
		public List()
		{
			this.levels = new ListLevelCollection();
			this.indexer = new ListLevelsIndexer(this.levels);
			this.id = null;
		}

		public List(ListTemplateType listTemplateType)
			: this()
		{
			ListFactory.InitializeList(this, listTemplateType);
		}

		public int Id
		{
			get
			{
				if (this.id == null)
				{
					return -1;
				}
				return this.id.Value;
			}
			internal set
			{
				if (this.id != null)
				{
					throw new InvalidOperationException("Cannot add same List instance to ListCollection multiple times!");
				}
				this.id = new int?(value);
			}
		}

		public ListLevelCollection Levels
		{
			get
			{
				return this.levels;
			}
		}

		internal ListLevelsIndexer Indexer
		{
			get
			{
				return this.indexer;
			}
		}

		readonly ListLevelCollection levels;

		readonly ListLevelsIndexer indexer;

		int? id;
	}
}
