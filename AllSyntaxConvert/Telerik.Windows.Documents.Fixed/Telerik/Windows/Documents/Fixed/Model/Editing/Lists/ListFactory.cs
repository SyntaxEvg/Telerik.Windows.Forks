using System;
using System.Text;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	static class ListFactory
	{
		public static void InitializeList(List list, ListTemplateType listTemplateType)
		{
			switch (listTemplateType)
			{
			case ListTemplateType.BulletDefault:
				ListFactory.InitializeBulletDefaultList(list);
				return;
			case ListTemplateType.NumberedDefault:
				ListFactory.InitializeNumberedDefaultList(list);
				return;
			case ListTemplateType.NumberedParentheses:
				ListFactory.InitializeNumberedParenthesesList(list);
				return;
			case ListTemplateType.NumberedHierarchical:
				ListFactory.InitializeNumberedHierarchicalList(list);
				return;
			default:
				throw new NotSupportedException(string.Format("ListTemplateType {0} is not supported!", listTemplateType));
			}
		}

		public static void InitializeBulletDefaultList(List list)
		{
			ListFactory.InitializeDefaultIndentationList(list, delegate(ListLevel level, int levelIndex)
			{
				level.BulletNumberingFormat = new TextBulletNumberingFormat((IListLevelsIndexer indexer) => ListFactory.bullets[levelIndex % ListFactory.bullets.Length]);
				level.CharacterProperties.TrySetFont(ListFactory.bulletsFontFamily[levelIndex % ListFactory.bulletsFontFamily.Length]);
			});
		}

		public static void InitializeNumberedDefaultList(List list)
		{
			ListFactory.InitializeDefaultIndentationList(list, delegate(ListLevel level, int levelIndex)
			{
				level.BulletNumberingFormat = new TextBulletNumberingFormat((IListLevelsIndexer indexer) => string.Format("{0}.", ListFactory.numberingStyles[levelIndex % ListFactory.numberingStyles.Length](indexer.GetCurrentIndex(levelIndex))));
			});
		}

		public static void InitializeNumberedParenthesesList(List list)
		{
			ListFactory.InitializeDefaultIndentationList(list, delegate(ListLevel level, int levelIndex)
			{
				level.BulletNumberingFormat = new TextBulletNumberingFormat((IListLevelsIndexer indexer) => string.Format("{0})", ListFactory.numberingStyles[levelIndex % ListFactory.numberingStyles.Length](indexer.GetCurrentIndex(levelIndex))));
			});
		}

		public static void InitializeNumberedHierarchicalList(List list)
		{
			ListFactory.InitializeDefaultIndentationList(list, delegate(ListLevel level, int levelIndex)
			{
				level.BulletNumberingFormat = new TextBulletNumberingFormat(delegate(IListLevelsIndexer indexer)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i <= levelIndex; i++)
					{
						stringBuilder.AppendFormat("{0}.", BulletNumberingFormats.GetNumber(indexer.GetCurrentIndex(i)));
					}
					return stringBuilder.ToString();
				});
			});
		}

		static void InitializeDefaultIndentationList(List list, Action<ListLevel, int> onListLevelCreated)
		{
			for (int i = 0; i < FixedDocumentDefaults.DefaultListLevelsCount; i++)
			{
				ListLevel listLevel = list.Levels.AddListLevel();
				onListLevelCreated(listLevel, i);
				double listLevelIndentationStep = FixedDocumentDefaults.ListLevelIndentationStep;
				listLevel.ParagraphProperties.LeftIndent = listLevelIndentationStep * 2.0 + (double)i * listLevelIndentationStep;
				listLevel.ParagraphProperties.FirstLineIndent = -listLevelIndentationStep;
			}
		}

		static readonly string[] bullets = new string[] { "\uf0b7", "o", "\uf0a7" };

		static readonly FontFamily[] bulletsFontFamily = new FontFamily[]
		{
			new FontFamily("Symbol"),
			new FontFamily("Courier New"),
			new FontFamily("Wingdings")
		};

		static readonly Func<int, string>[] numberingStyles = new Func<int, string>[]
		{
			new Func<int, string>(BulletNumberingFormats.GetNumber),
			new Func<int, string>(BulletNumberingFormats.GetLowerLetter),
			new Func<int, string>(BulletNumberingFormats.GetLowerRomanNumber)
		};
	}
}
