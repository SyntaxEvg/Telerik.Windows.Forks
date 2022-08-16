using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Lists
{
	class ListLevelInfo : RtfElementIteratorBase
	{
		static ListLevelInfo()
		{
			SpanStyleHandlers.InitializeSpanStyleHandlers(ListLevelInfo.tagHandlers);
			ParagraphStyleHandlers.InitializeParagraphStyleHandlers(ListLevelInfo.tagHandlers);
		}

		public int LevelStartIndex { get; set; }

		public string LevelTemplateId { get; set; }

		public NumberingStyle NumberingStyle { get; set; }

		public Alignment Alignment { get; set; }

		public bool IsLegal { get; set; }

		public bool RestartAfterLevel { get; set; }

		public int? StyleId { get; set; }

		public string LevelText
		{
			get
			{
				if (!this.isLevelTextValid && !string.IsNullOrEmpty(this.rtfLevelText))
				{
					this.ConstructText();
				}
				return this.levelText;
			}
		}

		public void ReadListLevelInfo(RtfGroup group, RtfImportContext context)
		{
			Util.EnsureGroupDestination(group, "listlevel");
			this.localContext = new RtfImportContext(context);
			base.VisitGroupChildren(group, false);
		}

		public ListLevel CreateListLevel(RtfImportContext context, int level)
		{
			ListLevel listLevel = new ListLevel(context.CurrentStyle.CurrentList)
			{
				StartIndex = this.LevelStartIndex,
				NumberingStyle = this.NumberingStyle,
				Alignment = this.Alignment,
				IsLegal = this.IsLegal
			};
			if (!string.IsNullOrEmpty(this.LevelText))
			{
				listLevel.NumberTextFormat = this.LevelText;
			}
			if (this.RestartAfterLevel)
			{
				listLevel.RestartAfterLevel = 0;
			}
			if (this.StyleId != null)
			{
				Style styleById = context.StylesTable.GetStyleById(this.StyleId.Value);
				if (styleById != null && styleById.StyleType == StyleType.Paragraph)
				{
					listLevel.StyleId = styleById.Id;
					if (!styleById.ParagraphProperties.ListLevel.HasLocalValue)
					{
						styleById.ParagraphProperties.ListLevel.LocalValue = new int?(level);
					}
					if (!styleById.ParagraphProperties.ListId.HasLocalValue)
					{
						styleById.ParagraphProperties.ListId.LocalValue = new int?(context.CurrentStyle.CurrentList.Id);
					}
					listLevel.CharacterProperties.CopyPropertiesFrom(styleById.CharacterProperties);
					listLevel.ParagraphProperties.CopyPropertiesFrom(styleById.ParagraphProperties);
				}
			}
			listLevel.CharacterProperties.CopyPropertiesFrom(this.localContext.CurrentStyle.CharacterStyle.Properties);
			listLevel.ParagraphProperties.CopyPropertiesFrom(this.localContext.CurrentStyle.ParagraphStyle.Properties);
			return listLevel;
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			switch (name = tag.Name)
			{
			case "levelnfc":
				if (!this.rtfNumberingFormatLocked)
				{
					this.NumberingStyle = (NumberingStyle)tag.ValueAsNumber;
				}
				break;
			case "levelnfcn":
				this.rtfNumberingFormatLocked = true;
				this.NumberingStyle = (NumberingStyle)tag.ValueAsNumber;
				break;
			case "levelstartat":
				this.LevelStartIndex = tag.ValueAsNumber;
				break;
			case "leveljcn":
			case "leveljc":
				this.Alignment = RtfHelper.ListLevelAlignmentMapper.GetToValue(tag.ValueAsNumber);
				break;
			case "s":
				this.StyleId = new int?(tag.ValueAsNumber);
				break;
			case "levellegal":
				this.IsLegal = tag.ValueAsNumber != 0;
				break;
			case "levelnorestart":
				this.RestartAfterLevel = true;
				break;
			}
			ControlTagHandler controlTagHandler;
			ListLevelInfo.tagHandlers.TryGetValue(tag.Name, out controlTagHandler);
			if (controlTagHandler != null)
			{
				controlTagHandler(tag, this.localContext);
			}
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (destination == "leveltext")
				{
					RtfTag rtfTag = group.Elements.FirstOrDefault((RtfElement el) => el is RtfTag && ((RtfTag)el).Name == "leveltemplateid") as RtfTag;
					this.LevelTemplateId = ((rtfTag != null) ? rtfTag.ValueAsText : null);
					string groupText = RtfHelper.GetGroupText(group, true);
					this.rtfLevelText = groupText.TrimEnd(new char[] { ';' });
					this.isLevelTextValid = false;
					return;
				}
				if (!(destination == "levelnumbers"))
				{
					return;
				}
				if (group.Elements.Count == 2)
				{
					RtfText rtfText = group.Elements[1] as RtfText;
					if (rtfText != null)
					{
						this.rtfLevelNumbersInfo = rtfText.Text.TrimEnd(new char[] { ';' });
					}
					this.isLevelTextValid = false;
				}
			}
		}

		void ConstructText()
		{
			List<int> list = new List<int>();
			if (!string.IsNullOrEmpty(this.rtfLevelNumbersInfo))
			{
				for (int i = 0; i < this.rtfLevelNumbersInfo.Length; i++)
				{
					list.Add((int)this.rtfLevelNumbersInfo[i]);
				}
			}
			list.Sort();
			list.Reverse();
			StringBuilder stringBuilder = new StringBuilder(this.rtfLevelText);
			foreach (int num in list)
			{
				int num2 = (int)this.rtfLevelText[num];
				stringBuilder.Remove(num, 1);
				stringBuilder.Insert(num, "%" + (num2 + 1).ToString());
			}
			this.isLevelTextValid = true;
			this.levelText = stringBuilder.ToString().Substring(1);
		}

		static readonly Dictionary<string, ControlTagHandler> tagHandlers = new Dictionary<string, ControlTagHandler>();

		string rtfLevelText;

		string rtfLevelNumbersInfo;

		bool rtfNumberingFormatLocked;

		bool isLevelTextValid;

		string levelText;

		RtfImportContext localContext;
	}
}
