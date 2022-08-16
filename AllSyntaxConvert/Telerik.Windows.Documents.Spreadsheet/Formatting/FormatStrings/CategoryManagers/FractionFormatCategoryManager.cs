using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers
{
	public static class FractionFormatCategoryManager
	{
		public static Dictionary<FractionDenominatorType, string> FractionTypeToFormatString
		{
			get
			{
				return FractionFormatCategoryManager.fractionTypeToFormatString;
			}
		}

		static FractionFormatCategoryManager()
		{
			FractionFormatCategoryManager.InitFractionDenominatorTypes();
		}

		static void InitFractionDenominatorTypes()
		{
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.UpToOneDigit, "# ?/?");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.UpToTwoDigits, "# ??/??");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.UpToThreeDigits, "# ???/???");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsHalves, "# ?/2");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsQuarters, "# ?/4");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsEighths, "# ?/8");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsSixteenths, "# ??/16");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsTenths, "# ?/10");
			FractionFormatCategoryManager.AddFractionType(FractionDenominatorType.AsHundredths, "# ??/100");
		}

		static void AddFractionType(FractionDenominatorType fractionType, string formatString)
		{
			FractionFormatCategoryManager.fractionTypeToFormatString.Add(fractionType, formatString);
		}

		static readonly Dictionary<FractionDenominatorType, string> fractionTypeToFormatString = new Dictionary<FractionDenominatorType, string>();
	}
}
