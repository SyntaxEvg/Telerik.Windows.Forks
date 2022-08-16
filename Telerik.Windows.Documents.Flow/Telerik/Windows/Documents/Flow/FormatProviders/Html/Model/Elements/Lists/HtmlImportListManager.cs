using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class HtmlImportListManager : HtmlListManager
	{
		internal List List { get; set; }

		internal int? LevelIndex { get; set; }

		internal int LevelBulletSerialIndex
		{
			get
			{
				return base.ListIndexes.TotalListItemsPerLevel[this.LevelIndex.Value].BulletSerialIndex;
			}
			set
			{
				base.ListIndexes.TotalListItemsPerLevel[this.LevelIndex.Value].BulletSerialIndex = value;
			}
		}

		internal ListLevel BeginLevel(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			if (this.List == null)
			{
				this.List = document.Lists.Add(ListTemplateType.NumberedDefault);
				for (int i = 0; i < this.List.Levels.Count; i++)
				{
					this.List.Levels[i].NumberingStyle = NumberingStyle.Decimal;
				}
				base.ListIndexes = new ListIndexes(this.List.Id);
			}
			if (this.LevelIndex == null)
			{
				this.LevelIndex = new int?(0);
			}
			else if (this.LevelIndex.Value < 8)
			{
				this.LevelIndex++;
			}
			return this.List.Levels[this.LevelIndex.Value];
		}

		internal ListLevel GetLevel()
		{
			return this.List.Levels[this.LevelIndex.Value];
		}

		internal void EndLevel()
		{
			if (this.LevelIndex.Value >= 0)
			{
				this.LevelIndex--;
			}
			if (this.LevelIndex.Value == -1)
			{
				this.List = null;
			}
		}

		internal void ResertAllSubsequentLevels()
		{
			for (int i = this.LevelIndex.Value + 1; i < this.List.Levels.Count; i++)
			{
				if (base.ListIndexes.TotalListItemsPerLevel.ContainsKey(i))
				{
					base.ListIndexes.TotalListItemsPerLevel[i].BulletSerialIndex = 0;
				}
			}
		}

		internal void ClearListLevel()
		{
			ListLevel level = this.GetLevel();
			level.NumberingStyle = NumberingStyle.Decimal;
			level.NumberTextFormat = "%" + ((this.LevelIndex + 1) ?? 1) + ".";
			level.CharacterProperties.FontFamily.ClearValue();
		}
	}
}
