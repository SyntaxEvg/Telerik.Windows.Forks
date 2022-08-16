using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists
{
	class RtfList : RtfElementIteratorBase
	{
		public RtfList()
		{
			this.ListLevelInfos = new List<ListLevelInfo>();
			this.listIdToStyleIds = new Dictionary<int, string>();
		}

		public int ListId { get; set; }

		public int? ListTemplateId { get; set; }

		public bool IsSingleLevel { get; set; }

		public bool ListHybrid { get; set; }

		public int? ListStyleId { get; set; }

		public string ListStyleName { get; set; }

		public List<ListLevelInfo> ListLevelInfos { get; set; }

		public List List
		{
			get
			{
				return this.list;
			}
		}

		public void ReadList(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "list");
			this.context = context;
			base.VisitGroupChildren(group, false);
			this.CreateList(context);
		}

		public List GetCreatedStyledList(RtfImportContext context)
		{
			Style style = null;
			if (!string.IsNullOrEmpty(this.list.StyleId))
			{
				style = context.Document.StyleRepository.GetStyle(this.list.StyleId);
				if (style == null)
				{
					style = new Style(this.list.StyleId, StyleType.Numbering);
					context.Document.StyleRepository.Add(style);
				}
				else if (style.StyleType != StyleType.Numbering)
				{
					this.list.StyleId = null;
					style = null;
				}
			}
			if (style != null)
			{
				style.ParagraphProperties.ListId.LocalValue = new int?(this.list.Id);
			}
			return this.list;
		}

		public void CreateList(RtfImportContext context)
		{
			this.list = context.Document.Lists.Add(ListTemplateType.NumberedDefault);
			this.list.MultilevelType = this.GetMultilevelType();
			context.CurrentStyle.CurrentList = this.list;
			if (this.ListLevelInfos.Count == 0)
			{
				return;
			}
			if (this.ListStyleName != null)
			{
				this.listIdToStyleIds.Add(this.ListId, this.ListStyleName);
				context.Document.Lists.Remove(this.list);
			}
			if (this.list.MultilevelType == MultilevelType.SingleLevel)
			{
				this.list.Levels[0] = this.ListLevelInfos[0].CreateListLevel(context, 0);
				return;
			}
			int num = 0;
			while (num < this.list.Levels.Count && num < this.ListLevelInfos.Count)
			{
				this.list.Levels[num] = this.ListLevelInfos[num].CreateListLevel(context, num);
				num++;
			}
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
				if (!(name == "listtemplateid"))
				{
					if (name == "listsimple")
					{
						this.IsSingleLevel = tag.ValueAsNumber != 0;
						return;
					}
					if (name == "listhybrid")
					{
						this.ListHybrid = true;
						return;
					}
					if (!(name == "liststyleid"))
					{
						return;
					}
					this.ListStyleId = new int?(tag.ValueAsNumber);
				}
				else if (tag.ValueAsNumber != -1)
				{
					this.ListTemplateId = new int?(tag.ValueAsNumber);
					return;
				}
			}
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			if (group.Destination == "listlevel")
			{
				ListLevelInfo listLevelInfo = new ListLevelInfo();
				listLevelInfo.ReadListLevelInfo(group, this.context);
				this.ListLevelInfos.Add(listLevelInfo);
			}
			if (group.Destination == "liststylename")
			{
				this.ListStyleName = RtfHelper.GetGroupText(group, true).Trim().TrimEnd(new char[] { ';' });
			}
		}

		MultilevelType GetMultilevelType()
		{
			if (this.IsSingleLevel)
			{
				return MultilevelType.SingleLevel;
			}
			if (this.ListHybrid)
			{
				return MultilevelType.HybridMultilevel;
			}
			return MultilevelType.Multilevel;
		}

		readonly Dictionary<int, string> listIdToStyleIds;

		List list;

		RtfImportContext context;
	}
}
