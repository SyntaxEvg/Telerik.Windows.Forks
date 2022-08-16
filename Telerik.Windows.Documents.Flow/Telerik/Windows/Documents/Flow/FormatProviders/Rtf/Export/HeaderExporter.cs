using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class HeaderExporter
	{
		public HeaderExporter(ExportContext context, RtfWriter writer)
		{
			this.context = context;
			this.writer = writer;
		}

		public void ExportHeader()
		{
			this.writer.WriteTag("ansi");
			this.writer.WriteTag("ansicpg", CharSetHelper.AnsiCodePage);
			this.writer.WriteTag("uc", HeaderExporter.DefaultUnicodeSkipCount);
			this.writer.WriteTag("deff", HeaderExporter.DefaultFontID);
			this.writer.WriteTag("deflang", HeaderExporter.DefaultLanguage);
			this.WriteFontTable();
			this.WriteColorTable();
			this.WriteDefaultStyles();
			this.WriteStyleTable();
			this.WriteListTable();
			this.WriteListOverridesTable();
		}

		void WriteFontTable()
		{
			using (this.writer.WriteGroup("fonttbl", false))
			{
				foreach (FontFamily fontFamily in this.context.FontTable.Keys)
				{
					int parameter = this.context.FontTable[fontFamily];
					using (this.writer.WriteGroup("f", parameter, false))
					{
						this.writer.WriteText(fontFamily.Source);
						this.writer.WriteText(";");
					}
				}
			}
		}

		void WriteColorTable()
		{
			using (this.writer.WriteGroup("colortbl", false))
			{
				int i;
				for (i = 0; i < this.context.ColorTable.Count; i++)
				{
					Color key = this.context.ColorTable.Single((KeyValuePair<Color, int> pair) => pair.Value == i).Key;
					this.writer.WriteTag("red", (int)key.R);
					this.writer.WriteTag("green", (int)key.G);
					this.writer.WriteTag("blue", (int)key.B);
					this.writer.WriteText(";");
				}
			}
		}

		void WriteDefaultStyles()
		{
			using (this.writer.WriteGroup("defchp", true))
			{
				InlineExporter.ExportCharacterProperties(this.context.Document.DefaultStyle.CharacterProperties, this.context, this.writer);
			}
			using (this.writer.WriteGroup("defpap", true))
			{
				ParagraphExporter.ExportParagraphProperties(this.context.Document.DefaultStyle.ParagraphProperties, this.context, this.writer, true, false, false);
			}
		}

		void WriteStyleTable()
		{
			using (this.writer.WriteGroup("stylesheet", false))
			{
				foreach (Style style in this.context.Document.StyleRepository.GetSortedTopologicallyStyles())
				{
					switch (style.StyleType)
					{
					case StyleType.Character:
						this.ExportSpanStyle(style);
						break;
					case StyleType.Paragraph:
						this.ExportParagraphStyle(style);
						break;
					case StyleType.Table:
						this.ExportTableStyle(style);
						break;
					}
				}
			}
		}

		void WriteListTable()
		{
			if (this.context.Document.Lists == null || this.context.Document.Lists.Count == 0)
			{
				return;
			}
			using (this.writer.WriteGroup("listtable", true))
			{
				foreach (List list in this.context.Document.Lists)
				{
					this.WriteList(list, false);
				}
			}
		}

		void WriteList(List list, bool shouldAssignDifferentId = false)
		{
			bool flag = false;
			using (this.writer.WriteGroup("list", false))
			{
				this.writer.WriteTag("listtemplateid", -1);
				switch (list.MultilevelType)
				{
				case MultilevelType.SingleLevel:
					this.writer.WriteTag("listsimple");
					this.WriteListLevelInfo(list.Levels[0]);
					break;
				case MultilevelType.Multilevel:
					this.WriteListLevelsInfo(list);
					break;
				case MultilevelType.HybridMultilevel:
					this.writer.WriteTag("listhybrid");
					this.WriteListLevelsInfo(list);
					break;
				default:
					throw new NotSupportedException("Unsupported list mulitilevel type.");
				}
				using (this.writer.WriteGroup("listname", false))
				{
					this.writer.WriteText(";");
				}
				int num = list.Id;
				if (shouldAssignDifferentId)
				{
					num = list.GetHashCode();
					this.context.StyleNumberingListIdsToActualListIds.Add(list.Id, num);
				}
				this.writer.WriteTag("listid", num);
				if (!string.IsNullOrEmpty(list.StyleId))
				{
					Style style = this.context.Document.StyleRepository.GetStyle(list.StyleId);
					if (style.ParagraphProperties.ListId.HasLocalValue)
					{
						if (!this.context.ListNumberingStyleNamesToListIds.ContainsKey(list.StyleId))
						{
							this.context.ListNumberingStyleNamesToListIds.Add(list.StyleId, list.Id);
							using (this.writer.WriteGroup("liststylename", false))
							{
								this.writer.WriteText(list.StyleId);
								this.writer.WriteText(";");
							}
							flag = true;
						}
						else
						{
							this.writer.WriteTag("liststyleid", this.context.ListNumberingStyleNamesToListIds[list.StyleId]);
						}
					}
				}
			}
			if (flag)
			{
				this.WriteList(list, true);
			}
		}

		void WriteListLevelsInfo(List list)
		{
			for (int i = 0; i < list.Levels.Count; i++)
			{
				this.WriteListLevelInfo(list.Levels[i]);
			}
		}

		void WriteListLevelInfo(ListLevel listLevel)
		{
			using (this.writer.WriteGroup("listlevel", false))
			{
				int numberingStyle = (int)listLevel.NumberingStyle;
				this.writer.WriteTag("levelnfc", numberingStyle);
				this.writer.WriteTag("levelnfcn", numberingStyle);
				this.writer.WriteTag("leveljc", RtfHelper.ListLevelAlignmentMapper.GetFromValue(listLevel.Alignment));
				this.writer.WriteTag("leveljcn", RtfHelper.ListLevelAlignmentMapper.GetFromValue(listLevel.Alignment));
				string text = listLevel.NumberTextFormat;
				if (string.IsNullOrEmpty(text))
				{
					using (this.writer.WriteGroup("leveltext", false))
					{
						this.writer.WriteHex(0);
						goto IL_CF;
					}
				}
				if (listLevel.NumberingStyle != NumberingStyle.Bullet)
				{
					text = RtfHelper.ConvertNumberedListFormatToRtfLevelText(text);
				}
				this.WriteListLevelTextAndNumberingInfo(text);
				IL_CF:
				if (listLevel.IsLegal)
				{
					this.writer.WriteTag("levellegal", 1);
				}
				InlineExporter.ExportCharacterProperties(listLevel.CharacterProperties, this.context, this.writer);
				this.writer.WriteTag("levelfollow", 0);
				this.writer.WriteTag("levelstartat", listLevel.StartIndex);
				if (listLevel.RestartAfterLevel != DocumentDefaultStyleSettings.RestartAfterLevel)
				{
					this.writer.WriteTag("levelnorestart");
				}
				if (!string.IsNullOrEmpty(listLevel.StyleId))
				{
					int parameter = this.context.StyleTable[listLevel.StyleId];
					this.writer.WriteTag("s", parameter);
				}
				ParagraphExporter.ExportParagraphProperties(listLevel.ParagraphProperties, this.context, this.writer, false, true, false);
			}
		}

		void WriteListLevelTextAndNumberingInfo(string textTemplate)
		{
			string text = textTemplate.Replace('@', 'a');
			text = string.Format(text, new object[] { '@', '@', '@', '@', '@', '@', '@', '@', '@' });
			string text2 = string.Format(textTemplate, new object[] { '0', '1', '2', '3', '4', '5', '6', '7', '8' });
			List<int> list = new List<int>();
			using (this.writer.WriteGroup("leveltext", false))
			{
				int length = text.Length;
				this.writer.WriteHex(length);
				for (int i = 0; i < length; i++)
				{
					if (text[i] == '@')
					{
						list.Add(i);
						int number = int.Parse(text2.Substring(i, 1));
						this.writer.WriteHex(number);
					}
					else
					{
						this.writer.WriteChar(text2[i]);
					}
				}
				this.writer.WriteChar(';');
			}
			using (this.writer.WriteGroup("levelnumbers", false))
			{
				foreach (int num in list)
				{
					this.writer.WriteHex(num + 1);
				}
				this.writer.WriteChar(';');
			}
		}

		void WriteListOverridesTable()
		{
			if (this.context.Document.Lists == null || this.context.Document.Lists.Count == 0)
			{
				return;
			}
			using (this.writer.WriteGroup("listoverridetable", true))
			{
				foreach (List list in this.context.Document.Lists)
				{
					this.WriteListOverride(list.Id);
				}
				foreach (int listId in this.context.StyleNumberingListIdsToActualListIds.Values)
				{
					this.WriteListOverride(listId);
				}
			}
		}

		void WriteListOverride(int listId)
		{
			using (this.writer.WriteGroup("listoverride", false))
			{
				this.writer.WriteTag("listid", listId);
				this.writer.WriteTag("listoverridecount", 0);
				this.writer.WriteTag("ls", listId);
			}
		}

		void ExportSpanStyle(Style style)
		{
			using (this.writer.WriteGroup("cs", this.context.StyleTable[style.Id], true))
			{
				this.writer.WriteTag("additive");
				this.ExportStyleMetaProperties(style);
				InlineExporter.ExportCharacterProperties(style.CharacterProperties, this.context, this.writer);
				this.writer.WriteText(style.Name + ";");
			}
		}

		void ExportParagraphStyle(Style style)
		{
			using (this.writer.WriteGroup("s", this.context.StyleTable[style.Id], false))
			{
				this.ExportStyleMetaProperties(style);
				InlineExporter.ExportCharacterProperties(style.CharacterProperties, this.context, this.writer);
				ParagraphExporter.ExportParagraphProperties(style.ParagraphProperties, this.context, this.writer, true, true, true);
				this.writer.WriteText(style.Name + ";");
			}
		}

		void ExportTableStyle(Style style)
		{
			using (this.writer.WriteGroup("ts", this.context.StyleTable[style.Id], true))
			{
				this.writer.WriteTag("tsrowd");
				this.ExportStyleMetaProperties(style);
				TableExporter.ExportTableStyle(style, this.context, this.writer);
				InlineExporter.ExportCharacterProperties(style.CharacterProperties, this.context, this.writer);
				ParagraphExporter.ExportParagraphProperties(style.ParagraphProperties, this.context, this.writer, true, true, false);
				this.writer.WriteText(style.Name + ";");
			}
		}

		void ExportStyleMetaProperties(Style style)
		{
			int parameter;
			if (!string.IsNullOrEmpty(style.BasedOnStyleId) && this.context.StyleTable.TryGetValue(style.BasedOnStyleId, out parameter))
			{
				this.writer.WriteTag("sbasedon", parameter);
			}
			int parameter2;
			if (!string.IsNullOrEmpty(style.LinkedStyleId) && this.context.StyleTable.TryGetValue(style.LinkedStyleId, out parameter2))
			{
				this.writer.WriteTag("slink", parameter2);
			}
			int parameter3;
			if (!string.IsNullOrEmpty(style.NextStyleId) && this.context.StyleTable.TryGetValue(style.NextStyleId, out parameter3))
			{
				this.writer.WriteTag("snext", parameter3);
			}
			if (style.IsPrimary)
			{
				this.writer.WriteTag("sqformat");
			}
			if (style.UIPriority != DocumentDefaultStyleSettings.UIPriority)
			{
				this.writer.WriteTag("spriority", style.UIPriority);
			}
		}

		static readonly int DefaultLanguage = 1033;

		static readonly int DefaultUnicodeSkipCount = 1;

		static readonly int DefaultFontID = 0;

		readonly RtfWriter writer;

		readonly ExportContext context;
	}
}
