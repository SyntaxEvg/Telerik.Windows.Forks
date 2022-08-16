using System;
using System.Text;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Lists
{
	static class ListFactory
	{
		internal static string[] BulletSymbols
		{
			get
			{
				return ListFactory.bullets;
			}
		}

		internal static FontFamily[] BulletsFontFamily
		{
			get
			{
				return ListFactory.bulletsFontFamily;
			}
		}

		internal static List GetList(ListTemplateType listTemplate)
		{
			switch (listTemplate)
			{
			case ListTemplateType.BulletDefault:
				return ListFactory.CreateBulletedDefaultList();
			case ListTemplateType.NumberedDefault:
				return ListFactory.CreateNumberedDefault();
			case ListTemplateType.NumberedParentheses:
				return ListFactory.CreateNumberedParenthesesList();
			case ListTemplateType.NumberedHierarchical:
				return ListFactory.CreateNumberedHierarchicalList();
			default:
				return null;
			}
		}

		internal static List CreateBulletedDefaultList()
		{
			List list = new List();
			for (int i = 0; i < list.Levels.Count; i++)
			{
				list.Levels[i].StartIndex = 1;
				list.Levels[i].NumberingStyle = NumberingStyle.Bullet;
				list.Levels[i].NumberTextFormat = ListFactory.bullets[i % ListFactory.bullets.Length];
				list.Levels[i].CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ListFactory.bulletsFontFamily[i % ListFactory.bulletsFontFamily.Length]);
				ListFactory.SetIndentation(list.Levels[i].ParagraphProperties, i);
			}
			return list;
		}

		internal static List CreateNumberedDefault()
		{
			List list = new List();
			for (int i = 0; i < list.Levels.Count; i++)
			{
				list.Levels[i].StartIndex = 1;
				list.Levels[i].NumberingStyle = ListFactory.numberedListLevels[i % ListFactory.numberedListLevels.Length];
				list.Levels[i].NumberTextFormat = "%" + (i + 1) + ".";
				ListFactory.SetIndentation(list.Levels[i].ParagraphProperties, i);
			}
			return list;
		}

		internal static List CreateNumberedParenthesesList()
		{
			List list = new List();
			for (int i = 0; i < list.Levels.Count; i++)
			{
				list.Levels[i].StartIndex = 1;
				list.Levels[i].NumberingStyle = ListFactory.numberedListLevels[i % ListFactory.numberedListLevels.Length];
				ListLevel listLevel = list.Levels[i];
				object numberTextFormat = listLevel.NumberTextFormat;
				listLevel.NumberTextFormat = string.Concat(new object[]
				{
					numberTextFormat,
					"%",
					i + 1,
					")"
				});
				ListFactory.SetIndentation(list.Levels[i].ParagraphProperties, i);
			}
			return list;
		}

		internal static List CreateNumberedHierarchicalList()
		{
			List list = new List();
			for (int i = 0; i < list.Levels.Count; i++)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < i + 1; j++)
				{
					stringBuilder.Append("%" + (j + 1) + ".");
				}
				list.Levels[i].StartIndex = 1;
				list.Levels[i].NumberingStyle = NumberingStyle.Decimal;
				list.Levels[i].NumberTextFormat = stringBuilder.ToString();
				ListFactory.SetIndentation(list.Levels[i].ParagraphProperties, i);
			}
			return list;
		}

		static void SetIndentation(ParagraphProperties paragraphProperties, int listLevelIndex)
		{
			paragraphProperties.LeftIndent.LocalValue = new double?((double)(48 + listLevelIndex * 24));
			paragraphProperties.HangingIndent.LocalValue = new double?(24.0);
		}

		static readonly string[] bullets = new string[] { "\uf0b7", "o", "\uf0a7" };

		static readonly FontFamily[] bulletsFontFamily = new FontFamily[]
		{
			new FontFamily("Symbol"),
			new FontFamily("Courier New"),
			new FontFamily("Wingdings")
		};

		static readonly NumberingStyle[] numberedListLevels = new NumberingStyle[]
		{
			NumberingStyle.Decimal,
			NumberingStyle.LowerLetter,
			NumberingStyle.LowerRoman
		};
	}
}
