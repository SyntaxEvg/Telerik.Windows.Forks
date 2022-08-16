using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	static class PatternsRenderHelper
	{
		static PatternsRenderHelper()
		{
			Dictionary<PatternType, int[,]> dictionary = PatternsRenderHelper.patterns;
			PatternType key = PatternType.Gray6Percent;
			int[,] array = new int[4, 8];
			array[0, 4] = 1;
			array[2, 0] = 1;
			dictionary.Add(key, array);
			Dictionary<PatternType, int[,]> dictionary2 = PatternsRenderHelper.patterns;
			PatternType key2 = PatternType.Gray12Percent;
			int[,] array2 = new int[4, 4];
			array2[0, 2] = 1;
			array2[2, 0] = 1;
			dictionary2.Add(key2, array2);
			Dictionary<PatternType, int[,]> dictionary3 = PatternsRenderHelper.patterns;
			PatternType key3 = PatternType.Gray25Percent;
			int[,] array3 = new int[2, 4];
			array3[0, 2] = 1;
			array3[1, 0] = 1;
			dictionary3.Add(key3, array3);
			Dictionary<PatternType, int[,]> dictionary4 = PatternsRenderHelper.patterns;
			PatternType key4 = PatternType.Gray50Percent;
			int[,] array4 = new int[2, 2];
			array4[0, 0] = 1;
			array4[1, 1] = 1;
			dictionary4.Add(key4, array4);
			PatternsRenderHelper.patterns.Add(PatternType.Gray75Percent, new int[,]
			{
				{ 1, 1, 1, 0 },
				{ 1, 0, 1, 1 }
			});
			Dictionary<PatternType, int[,]> dictionary5 = PatternsRenderHelper.patterns;
			PatternType key5 = PatternType.HorizontalStripe;
			int[,] array5 = new int[4, 1];
			array5[0, 0] = 1;
			array5[3, 0] = 1;
			dictionary5.Add(key5, array5);
			Dictionary<PatternType, int[,]> dictionary6 = PatternsRenderHelper.patterns;
			PatternType key6 = PatternType.VerticalStripe;
			int[,] array6 = new int[1, 4];
			array6[0, 0] = 1;
			array6[0, 1] = 1;
			dictionary6.Add(key6, array6);
			PatternsRenderHelper.patterns.Add(PatternType.ReverseDiagonalStripe, new int[,]
			{
				{ 0, 0, 1, 1 },
				{ 1, 0, 0, 1 },
				{ 1, 1, 0, 0 },
				{ 0, 1, 1, 0 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.DiagonalStripe, new int[,]
			{
				{ 0, 0, 1, 1 },
				{ 0, 1, 1, 0 },
				{ 1, 1, 0, 0 },
				{ 1, 0, 0, 1 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.DiagonalCrosshatch, new int[,]
			{
				{ 0, 0, 1, 1 },
				{ 0, 0, 1, 1 },
				{ 1, 1, 0, 0 },
				{ 1, 1, 0, 0 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.ThickDiagonalCrosshatch, new int[,]
			{
				{ 0, 0, 1, 1 },
				{ 1, 1, 1, 1 },
				{ 1, 1, 0, 0 },
				{ 1, 1, 1, 1 }
			});
			Dictionary<PatternType, int[,]> dictionary7 = PatternsRenderHelper.patterns;
			PatternType key7 = PatternType.ThinHorizontalStripe;
			int[,] array7 = new int[4, 1];
			array7[3, 0] = 1;
			dictionary7.Add(key7, array7);
			Dictionary<PatternType, int[,]> dictionary8 = PatternsRenderHelper.patterns;
			PatternType key8 = PatternType.ThinVerticalStripe;
			int[,] array8 = new int[1, 4];
			array8[0, 0] = 1;
			dictionary8.Add(key8, array8);
			PatternsRenderHelper.patterns.Add(PatternType.ThinReverseDiagonalStripe, new int[,]
			{
				{ 0, 0, 1, 0 },
				{ 0, 0, 0, 1 },
				{ 1, 0, 0, 0 },
				{ 0, 1, 0, 0 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.ThinDiagonalStripe, new int[,]
			{
				{ 0, 0, 0, 1 },
				{ 0, 0, 1, 0 },
				{ 0, 1, 0, 0 },
				{ 1, 0, 0, 0 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.ThinHorizontalCrossHatch, new int[,]
			{
				{ 0, 1, 0, 0 },
				{ 0, 1, 0, 0 },
				{ 0, 1, 0, 0 },
				{ 1, 1, 1, 1 }
			});
			PatternsRenderHelper.patterns.Add(PatternType.ThinDiagonalCrosshatch, new int[,]
			{
				{ 1, 0, 1, 0 },
				{ 0, 0, 0, 1 },
				{ 1, 0, 1, 0 },
				{ 0, 1, 0, 0 }
			});
		}

		public static int[,] GetPattern(PatternType patternType)
		{
			if (patternType == PatternType.Solid)
			{
				throw new ArgumentException("Pattern type cannot be Solid!");
			}
			return PatternsRenderHelper.patterns[patternType];
		}

		static readonly Dictionary<PatternType, int[,]> patterns = new Dictionary<PatternType, int[,]>();
	}
}
