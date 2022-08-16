using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	class TextFormattedBullet : IBulletNumberingFormat
	{
		public TextFormattedBullet(ListHelper listHelper, NumberingStyle[] levelsNumberingStyles, string numberFormatString)
		{
			Guard.ThrowExceptionIfNull<ListHelper>(listHelper, "listHelper");
			Guard.ThrowExceptionIfOutOfRange<int>(TextFormattedBullet.levelsCount, TextFormattedBullet.levelsCount, levelsNumberingStyles.Length, "levelsNumberingStyles.Length");
			Guard.ThrowExceptionIfNull<string>(numberFormatString, "numberFormatString");
			this.listHelper = listHelper;
			this.formattedString = TextFormattedBullet.ConvertNumberFormatStringToFormattedString(numberFormatString);
			this.levelsNumberingStyles = levelsNumberingStyles;
		}

		public PositionContentElement GetBulletNumberingElement(IListLevelsIndexer listLevelsIndexer)
		{
			IEnumerable<string> formattedStringArguments = this.GetFormattedStringArguments(listLevelsIndexer);
			string text = string.Format(this.formattedString, formattedStringArguments.ToArray<string>());
			return string.IsNullOrEmpty(text) ? null : new TextFragment(text);
		}

		static string ConvertNumberFormatStringToFormattedString(string numberFormatString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = numberFormatString.Length;
			for (int i = 0; i < length; i++)
			{
				char c = numberFormatString[i];
				int num = i + 1;
				if (c == '{')
				{
					stringBuilder.AppendFormat("{0}{1}{2}", '{', TextFormattedBullet.leftBracketIndex, '}');
				}
				else if (c == '}')
				{
					stringBuilder.AppendFormat("{0}{1}{2}", '{', TextFormattedBullet.rightBracketIndex, '}');
				}
				else if (c == '%' && num < length && char.IsNumber(numberFormatString[num]))
				{
					int num2 = int.Parse(numberFormatString[num].ToString());
					i++;
					if (!TextFormattedBullet.IsValidLevelNumber(num2))
					{
						return string.Empty;
					}
					stringBuilder.AppendFormat("{0}{1}{2}", '{', num2 - 1, '}');
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		static bool IsValidLevelNumber(int number)
		{
			return number >= 1 && number <= TextFormattedBullet.levelsCount;
		}

		IEnumerable<string> GetFormattedStringArguments(IListLevelsIndexer listLevelsIndexer)
		{
			for (int i = 0; i < TextFormattedBullet.levelsCount; i++)
			{
				int index = listLevelsIndexer.GetCurrentIndex(i);
				NumberingStyle numberingStyle = this.levelsNumberingStyles[i];
				yield return this.listHelper.GetNumberText(numberingStyle, index);
			}
			yield return '{'.ToString();
			yield return '}'.ToString();
			yield break;
		}

		const char Percent = '%';

		const char LeftBracket = '{';

		const char RightBracket = '}';

		static readonly int leftBracketIndex = TextFormattedBullet.levelsCount;

		static readonly int rightBracketIndex = TextFormattedBullet.levelsCount + 1;

		static readonly int levelsCount = FixedDocumentDefaults.DefaultListLevelsCount;

		readonly string formattedString;

		readonly ListHelper listHelper;

		readonly NumberingStyle[] levelsNumberingStyles;
	}
}
