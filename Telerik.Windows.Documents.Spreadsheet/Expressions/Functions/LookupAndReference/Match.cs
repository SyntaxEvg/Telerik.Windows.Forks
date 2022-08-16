using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.LookupAndReference
{
	public class Match : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Match.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Match.Info;
			}
		}

		static Match()
		{
			string description = "Returns the relative position of an item in an array that matches a specified value in a specified order.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Match_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Lookup_value", "is the value you use to find the value you want in the array, a number, text, or logical value, or a reference to one of these.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_LookupValue", "Spreadsheet_Functions_Match_LookupValueInfo"),
				new ArgumentInfo("Lookup_array", "is a contiguous range of cells containing possible lookup values, an array of values, or a reference to an array.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_LookupArray", "Spreadsheet_Functions_Match_LookupArrayInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Match_type", "is a number 1, 0, or -1 indicating which value to return.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_MatchType", "Spreadsheet_Functions_Match_MatchTypeInfo")
			};
			Match.Info = new FunctionInfo(Match.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			ArrayExpression lookupArray;
			Worksheet worksheet;
			CellIndex topLeftCellIndex;
			ErrorExpression errorExpression = Match.TryGetLookupArray(context, out lookupArray, out worksheet, out topLeftCellIndex);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			int matchType = Match.GetMatchType(context);
			CriteriaEvaluator criteriaEvaluator;
			errorExpression = Match.TryGetCriteriaEvaluator(context, matchType, out criteriaEvaluator);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			CriteriaEvaluator equalsEvaluator;
			errorExpression = Match.TryGetCriteriaEvaluator(context, 0, out equalsEvaluator);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			if (matchType > 0)
			{
				return Match.HandleMatchFunctionLookingForLessOrEqual(lookupArray, criteriaEvaluator, topLeftCellIndex);
			}
			if (matchType < 0)
			{
				return Match.HandleMatchFunctionLookingForGreaterOrEqual(lookupArray, criteriaEvaluator, equalsEvaluator, topLeftCellIndex);
			}
			return Match.HandleMatchFunctionLookingForEqual(lookupArray, equalsEvaluator, topLeftCellIndex);
		}

		static ErrorExpression TryGetLookupArray(FunctionEvaluationContext<object> context, out ArrayExpression lookUpArray, out Worksheet worksheet, out CellIndex topLeftCellIndex)
		{
			object obj = context.Arguments[1];
			lookUpArray = null;
			worksheet = null;
			topLeftCellIndex = null;
			if (obj is NumberExpression)
			{
				return ErrorExpressions.NotAvailableError;
			}
			return FunctionHelper.TryGetArrayFromFunctionArgument(obj, out lookUpArray, out worksheet, out topLeftCellIndex);
		}

		static ErrorExpression TryGetCriteriaEvaluator(FunctionEvaluationContext<object> context, int matchType, out CriteriaEvaluator evaluator)
		{
			evaluator = null;
			object argument = context.Arguments[0];
			Worksheet worksheet = context.Worksheet;
			ComparisonOperator comparisonOperatorFromMatchType = Match.GetComparisonOperatorFromMatchType(matchType);
			return FunctionHelper.TryGetCriteriaEvaluator(argument, worksheet, comparisonOperatorFromMatchType, out evaluator);
		}

		static int GetMatchType(FunctionEvaluationContext<object> context)
		{
			int result;
			if (context.Arguments.Length > 2)
			{
				double num = (double)context.Arguments[2];
				result = (int)num;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		static ComparisonOperator GetComparisonOperatorFromMatchType(int matchType)
		{
			if (matchType > 0)
			{
				return ComparisonOperator.LessThanOrEqualsTo;
			}
			if (matchType < 0)
			{
				return ComparisonOperator.GreaterThanOrEqualsTo;
			}
			return ComparisonOperator.EqualsTo;
		}

		static RadExpression HandleMatchFunctionLookingForLessOrEqual(ArrayExpression lookupArray, CriteriaEvaluator lessOrEqualEvaluator, CellIndex topLeftCellIndex)
		{
			bool flag;
			ErrorExpression errorExpression = Match.IsLookupArrayColumn(lookupArray, out flag);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			int rowCount = lookupArray.RowCount;
			int columnCount = lookupArray.ColumnCount;
			for (int i = rowCount - 1; i >= 0; i--)
			{
				int j = columnCount - 1;
				while (j >= 0)
				{
					RadExpression cellExpression = lookupArray[i, j];
					if (lessOrEqualEvaluator.Evaluate(cellExpression, topLeftCellIndex.RowIndex + i, topLeftCellIndex.ColumnIndex + j))
					{
						if (!flag)
						{
							return NumberExpression.CreateValidNumberOrErrorExpression((double)(j + 1));
						}
						return NumberExpression.CreateValidNumberOrErrorExpression((double)(i + 1));
					}
					else
					{
						j--;
					}
				}
			}
			return ErrorExpressions.NotAvailableError;
		}

		static RadExpression HandleMatchFunctionLookingForGreaterOrEqual(ArrayExpression lookupArray, CriteriaEvaluator greaterOrEqualEvaluator, CriteriaEvaluator equalsEvaluator, CellIndex topLeftCellIndex)
		{
			bool flag;
			ErrorExpression errorExpression = Match.IsLookupArrayColumn(lookupArray, out flag);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			int rowCount = lookupArray.RowCount;
			int columnCount = lookupArray.ColumnCount;
			for (int i = 0; i < rowCount; i++)
			{
				int j = 0;
				while (j < columnCount)
				{
					RadExpression cellExpression = lookupArray[i, j];
					if (equalsEvaluator.Evaluate(cellExpression, topLeftCellIndex.RowIndex + i, topLeftCellIndex.ColumnIndex + j))
					{
						if (!flag)
						{
							return NumberExpression.CreateValidNumberOrErrorExpression((double)(j + 1));
						}
						return NumberExpression.CreateValidNumberOrErrorExpression((double)(i + 1));
					}
					else if (!greaterOrEqualEvaluator.Evaluate(cellExpression, topLeftCellIndex.RowIndex + i, topLeftCellIndex.ColumnIndex + j))
					{
						if (!flag)
						{
							return Match.CreateValidRowOrColumnIndexOrErrorExpression(j);
						}
						return Match.CreateValidRowOrColumnIndexOrErrorExpression(i);
					}
					else
					{
						j++;
					}
				}
			}
			if (!flag)
			{
				return NumberExpression.CreateValidNumberOrErrorExpression((double)columnCount);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression((double)rowCount);
		}

		static RadExpression HandleMatchFunctionLookingForEqual(ArrayExpression lookupArray, CriteriaEvaluator equalsEvaluator, CellIndex topLeftCellIndex)
		{
			bool flag;
			ErrorExpression errorExpression = Match.IsLookupArrayColumn(lookupArray, out flag);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			int rowCount = lookupArray.RowCount;
			int columnCount = lookupArray.ColumnCount;
			for (int i = 0; i < rowCount; i++)
			{
				int j = 0;
				while (j < columnCount)
				{
					RadExpression cellExpression = lookupArray[i, j];
					if (equalsEvaluator.Evaluate(cellExpression, topLeftCellIndex.RowIndex + i, topLeftCellIndex.ColumnIndex + j))
					{
						if (!flag)
						{
							return NumberExpression.CreateValidNumberOrErrorExpression((double)(j + 1));
						}
						return NumberExpression.CreateValidNumberOrErrorExpression((double)(i + 1));
					}
					else
					{
						j++;
					}
				}
			}
			return ErrorExpressions.NotAvailableError;
		}

		static RadExpression CreateValidRowOrColumnIndexOrErrorExpression(int number)
		{
			if (number > 0)
			{
				return NumberExpression.CreateValidNumberOrErrorExpression((double)number);
			}
			return ErrorExpressions.NotAvailableError;
		}

		static ErrorExpression IsLookupArrayColumn(ArrayExpression lookupArray, out bool isColumn)
		{
			if (lookupArray.ColumnCount == 1)
			{
				isColumn = true;
				return null;
			}
			if (lookupArray.RowCount == 1)
			{
				isColumn = false;
				return null;
			}
			isColumn = false;
			return ErrorExpressions.NotAvailableError;
		}

		public static readonly string FunctionName = "MATCH";

		static readonly FunctionInfo Info;
	}
}
