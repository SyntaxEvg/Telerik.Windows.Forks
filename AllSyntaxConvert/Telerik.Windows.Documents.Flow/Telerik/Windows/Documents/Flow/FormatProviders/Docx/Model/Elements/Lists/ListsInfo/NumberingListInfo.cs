using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists.ListsInfo
{
	class NumberingListInfo
	{
		public NumberingListInfo()
		{
			this.ListLevelOverrideCollection = new List<ListLevelInfo>();
		}

		public int Id { get; set; }

		public int AbstractListId { get; set; }

		public List<ListLevelInfo> ListLevelOverrideCollection { get; set; }
	}
}
