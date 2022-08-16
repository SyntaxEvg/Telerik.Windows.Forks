using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists
{
	class RtfListOverrideTable : RtfElementIteratorBase
	{
		public RtfListOverrideTable()
		{
			this.lists = new Dictionary<int, List>();
		}

		public void ReadTable(RtfGroup group, RtfImportContext context)
		{
			this.context = context;
			if (!this.context.ListTable.IsLoaded)
			{
				return;
			}
			base.VisitGroupChildren(group, false);
		}

		public List GetList(int id)
		{
			List result = null;
			this.lists.TryGetValue(id, out result);
			return result;
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			if (group.Destination == "listoverride")
			{
				RtfListOverride rtfListOverride = new RtfListOverride();
				rtfListOverride.Context = this.context;
				rtfListOverride.ReadListOverride(group);
				List value = rtfListOverride.CreateListOverride(this.context);
				this.lists[rtfListOverride.ListOverrideId] = value;
			}
		}

		readonly Dictionary<int, List> lists;

		RtfImportContext context;
	}
}
