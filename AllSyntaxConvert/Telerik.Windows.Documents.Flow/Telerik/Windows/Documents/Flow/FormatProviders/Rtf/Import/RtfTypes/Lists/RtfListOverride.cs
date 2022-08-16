using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists
{
	class RtfListOverride : RtfElementIteratorBase
	{
		public RtfImportContext Context { get; set; }

		public int ListOverrideId { get; set; }

		public int ListId { get; set; }

		public int ListLevelIndexToOverrideCount { get; set; }

		public int? ListOverrideStartAtIndex { get; set; }

		public int? ListOverrideFormat { get; set; }

		public void ReadListOverride(RtfGroup group)
		{
			Util.EnsureGroupDestination(group, "listoverride");
			this.listLevelInfos = new List<ListLevelInfo>();
			base.VisitGroupChildren(group, false);
		}

		public List CreateListOverride(RtfImportContext context)
		{
			RtfList rtfList = context.ListTable.GetRtfList(this.ListId);
			if (this.shouldCreateOverridenList)
			{
				rtfList.CreateList(context);
			}
			List createdStyledList = rtfList.GetCreatedStyledList(context);
			if (this.ListOverrideStartAtIndex != null && this.ListOverrideStartAtIndex != 0)
			{
				for (int i = 0; i < this.ListLevelIndexToOverrideCount; i++)
				{
					createdStyledList.Levels[i].StartIndex = this.ListOverrideStartAtIndex.Value;
				}
			}
			if (this.ListLevelIndexToOverrideCount <= this.listLevelInfos.Count)
			{
				for (int j = 0; j < this.ListLevelIndexToOverrideCount; j++)
				{
					ListLevel listLevel = createdStyledList.Levels[j];
					listLevel.Alignment = this.listLevelInfos[j].Alignment;
					listLevel.NumberingStyle = this.listLevelInfos[j].NumberingStyle;
					listLevel.NumberTextFormat = this.listLevelInfos[j].LevelText;
					listLevel.StartIndex = this.listLevelInfos[j].LevelStartIndex;
					listLevel.IsLegal = this.listLevelInfos[j].IsLegal;
					if (this.listLevelInfos[j].StyleId != null)
					{
						listLevel.StyleId = this.Context.StylesTable.GetStyleById(this.listLevelInfos[j].StyleId.Value).Id;
					}
				}
			}
			this.shouldCreateOverridenList = false;
			return createdStyledList;
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "listid")
				{
					this.ListId = tag.ValueAsNumber;
					return;
				}
				if (name == "ls")
				{
					this.ListOverrideId = tag.ValueAsNumber;
					return;
				}
				if (name == "listoverridecount")
				{
					this.ListLevelIndexToOverrideCount = tag.ValueAsNumber;
					return;
				}
				if (name == "levelstartat")
				{
					this.ListOverrideStartAtIndex = new int?(tag.ValueAsNumber);
					return;
				}
				if (!(name == "listoverrideformat"))
				{
					return;
				}
				this.ListOverrideFormat = new int?(tag.ValueAsNumber);
			}
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (destination == "lfolevel")
				{
					this.shouldCreateOverridenList = true;
					base.VisitGroupChildren(group, false);
					return;
				}
				if (!(destination == "listlevel"))
				{
					return;
				}
				ListLevelInfo listLevelInfo = new ListLevelInfo();
				listLevelInfo.ReadListLevelInfo(group, this.Context);
				this.listLevelInfos.Add(listLevelInfo);
			}
		}

		bool shouldCreateOverridenList;

		List<ListLevelInfo> listLevelInfos;
	}
}
