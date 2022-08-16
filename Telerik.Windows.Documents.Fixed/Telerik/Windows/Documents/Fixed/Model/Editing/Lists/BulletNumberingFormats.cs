using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	static class BulletNumberingFormats
	{
		public static string GetNumber(int number)
		{
			return number.ToString();
		}

		public static string GetLowerLetter(int number)
		{
			if (number <= 0)
			{
				return " ";
			}
			int num = number - 1;
			int num2 = 97;
			int num3 = 122;
			int num4 = num3 - num2 + 1;
			int count = 1 + num / num4;
			int num5 = num % num4;
			char c = (char)(num2 + num5);
			return new string(c, count);
		}

		public static string GetLowerRomanNumber(int number)
		{
			if (number <= 0)
			{
				return " ";
			}
			return MathUtilities.IntegerToRomanString(number);
		}

		public static string GetUpperRomanNumber(int number)
		{
			return BulletNumberingFormats.GetLowerRomanNumber(number).ToUpper();
		}

		public static string GetUpperLetter(int number)
		{
			return BulletNumberingFormats.GetLowerLetter(number).ToUpper();
		}

		public const string Whitespace = " ";

		public static readonly IBulletNumberingFormat EmptyNumbering = new TextBulletNumberingFormat((IListLevelsIndexer i) => null);
	}
}
