using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class ListImportContext
	{
		public ListImportContext()
		{
			this.listLevelContentImporters = new Stack<ListLevelContentImporter>();
		}

		public ListLevelContentImporter CurrentListLevelContentImporter
		{
			get
			{
				return this.listLevelContentImporters.Peek();
			}
		}

		public bool IsInListLevel
		{
			get
			{
				return this.listLevelContentImporters.Count > 0;
			}
		}

		public void BeginListLevel()
		{
			this.listLevelContentImporters.Push(new ListLevelContentImporter());
		}

		public void EndListLevel()
		{
			this.listLevelContentImporters.Pop();
		}

		readonly Stack<ListLevelContentImporter> listLevelContentImporters;
	}
}
