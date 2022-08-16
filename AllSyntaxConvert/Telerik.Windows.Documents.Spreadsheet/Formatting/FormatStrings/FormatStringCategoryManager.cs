using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Infos;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public static class FormatStringCategoryManager
	{
		static FormatStringCategoryManager()
		{
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.General, new Predicate<string>(FormatStringCategoryManager.BelongsToGeneralCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Number, new Predicate<string>(FormatStringCategoryManager.BelongsToNumberCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Currency, new Predicate<string>(FormatStringCategoryManager.BelongsToCurrencyCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Accounting, new Predicate<string>(FormatStringCategoryManager.BelongsToAccountingCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Date, new Predicate<string>(FormatStringCategoryManager.BelongsToDateCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Time, new Predicate<string>(FormatStringCategoryManager.BelongsToTimeCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Percentage, new Predicate<string>(FormatStringCategoryManager.BelongsToPercentageCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Fraction, new Predicate<string>(FormatStringCategoryManager.BelongsToFractionCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Scientific, new Predicate<string>(FormatStringCategoryManager.BelongsToScientificCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Text, new Predicate<string>(FormatStringCategoryManager.BelongsToTextCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Special, new Predicate<string>(FormatStringCategoryManager.BelongsToSpecialCategory));
			FormatStringCategoryManager.RegisterFormatStringCategoryPredicate(FormatStringCategory.Custom, new Predicate<string>(FormatStringCategoryManager.BelongsToCustomCategory));
		}

		public static bool BelongsToFormatStringCategory(string formatString, FormatStringCategory category)
		{
			return FormatStringCategoryManager.formatStringCategoryPredicates[category](formatString);
		}

		public static FormatStringCategory GetCategoryFromFormatString(string localFormatString)
		{
			Guard.ThrowExceptionIfNull<string>(localFormatString, "localFormatString");
			foreach (KeyValuePair<FormatStringCategory, Predicate<string>> keyValuePair in FormatStringCategoryManager.formatStringCategoryPredicates)
			{
				if (keyValuePair.Value(localFormatString))
				{
					return keyValuePair.Key;
				}
			}
			throw new InvalidOperationException();
		}

		static void RegisterFormatStringCategoryPredicate(FormatStringCategory category, Predicate<string> predicate)
		{
			FormatStringCategoryManager.formatStringCategoryPredicates.Add(category, predicate);
		}

		static bool BelongsToGeneralCategory(string formatString)
		{
			return string.IsNullOrEmpty(formatString);
		}

		static bool BelongsToNumberCategory(string formatString)
		{
			NumberFormatStringInfo numberFormatStringInfo;
			return FormatStringCategoryManager.TryGetNumberFormatStringInfo(formatString, out numberFormatStringInfo);
		}

		static bool BelongsToCurrencyCategory(string formatString)
		{
			CurrencyFormatStringInfo currencyFormatStringInfo;
			return FormatStringCategoryManager.TryGetCurrencyFormatStringInfo(formatString, out currencyFormatStringInfo);
		}

		static bool BelongsToAccountingCategory(string formatString)
		{
			AccountingFormatStringInfo accountingFormatStringInfo;
			return FormatStringCategoryManager.TryGetAccountingFormatStringInfo(formatString, out accountingFormatStringInfo);
		}

		static bool BelongsToDateCategory(string formatString)
		{
			DateFormatStringInfo dateFormatStringInfo;
			return FormatStringCategoryManager.TryGetDateFormatStringInfo(formatString, out dateFormatStringInfo);
		}

		static bool BelongsToTimeCategory(string formatString)
		{
			TimeFormatStringInfo timeFormatStringInfo;
			return FormatStringCategoryManager.TryGetTimeFormatStringInfo(formatString, out timeFormatStringInfo);
		}

		static bool BelongsToPercentageCategory(string formatString)
		{
			PercentageFormatStringInfo percentageFormatStringInfo;
			return FormatStringCategoryManager.TryGetPercentageFormatStringInfo(formatString, out percentageFormatStringInfo);
		}

		static bool BelongsToFractionCategory(string formatString)
		{
			return FractionFormatCategoryManager.FractionTypeToFormatString.Values.Contains(formatString);
		}

		static bool BelongsToScientificCategory(string formatString)
		{
			ScientificFormatStringInfo scientificFormatStringInfo;
			return FormatStringCategoryManager.TryGetScientificFormatStringInfo(formatString, out scientificFormatStringInfo);
		}

		static bool BelongsToTextCategory(string formatString)
		{
			return formatString == FormatHelper.TextPlaceholder;
		}

		static bool BelongsToSpecialCategory(string formatString)
		{
			SpecialFormatStringInfo specialFormatStringInfo;
			return FormatStringCategoryManager.TryGetSpecialFormatStringInfo(formatString, out specialFormatStringInfo);
		}

		static bool BelongsToCustomCategory(string formatString)
		{
			return true;
		}

		public static bool TryGetNumberFormatStringInfo(string formatString, out NumberFormatStringInfo formatStringInfo)
		{
			return NumberFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetCurrencyFormatStringInfo(string formatString, out CurrencyFormatStringInfo formatStringInfo)
		{
			return CurrencyFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetAccountingFormatStringInfo(string formatString, out AccountingFormatStringInfo formatStringInfo)
		{
			return AccountingFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetDateFormatStringInfo(string formatString, out DateFormatStringInfo formatStringInfo)
		{
			return DateFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetTimeFormatStringInfo(string formatString, out TimeFormatStringInfo formatStringInfo)
		{
			return TimeFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetPercentageFormatStringInfo(string formatString, out PercentageFormatStringInfo formatStringInfo)
		{
			return PercentageFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetScientificFormatStringInfo(string formatString, out ScientificFormatStringInfo formatStringInfo)
		{
			return ScientificFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		public static bool TryGetSpecialFormatStringInfo(string formatString, out SpecialFormatStringInfo formatStringInfo)
		{
			return SpecialFormatStringInfo.TryCreate(formatString, out formatStringInfo);
		}

		static readonly QueueDictionary<FormatStringCategory, Predicate<string>> formatStringCategoryPredicates = new QueueDictionary<FormatStringCategory, Predicate<string>>();
	}
}
