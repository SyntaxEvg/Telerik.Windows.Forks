using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists
{
	class RtfListTable : RtfElementIteratorBase
	{
		public bool IsLoaded { get; set; }

		public void ReadTable(RtfGroup group, RtfImportContext context)
		{
			if (!this.IsLoaded)
			{
				this.context = context;
				Util.EnsureGroupDestination(group, "listtable");
				this.listDictionary = new Dictionary<int, RtfList>();
				base.VisitGroupChildren(group, false);
				this.LinkNumberingStyles();
				this.IsLoaded = true;
				this.context = null;
			}
		}

		public RtfList GetRtfList(int id)
		{
			RtfList result = null;
			this.listDictionary.TryGetValue(id, out result);
			return result;
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			if (group.Destination == "list")
			{
				RtfList rtfList = new RtfList();
				rtfList.ReadList(group, this.context);
				this.listDictionary[rtfList.ListId] = rtfList;
			}
		}

		void LinkNumberingStyles()
		{
			foreach (RtfList rtfList in this.listDictionary.Values)
			{
				RtfList rtfList2;
				if (!string.IsNullOrEmpty(rtfList.ListStyleName))
				{
					rtfList.List.StyleId = rtfList.ListStyleName;
				}
				else if (rtfList.ListStyleId != null && this.listDictionary.TryGetValue(rtfList.ListStyleId.Value, out rtfList2))
				{
					rtfList.List.StyleId = rtfList2.ListStyleName;
				}
			}
		}

		Dictionary<int, RtfList> listDictionary;

		RtfImportContext context;
	}
}
