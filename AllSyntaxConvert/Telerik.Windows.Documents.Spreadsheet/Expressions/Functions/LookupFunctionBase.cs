using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class LookupFunctionBase : FunctionWithArguments
	{
		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			ArrayExpression lookupMatrix;
			Worksheet worksheet;
			CellIndex topLeftCellIndex;
			ErrorExpression errorExpression = LookupFunctionBase.TryGetLookUpMatrix(context.Arguments[1], out lookupMatrix, out worksheet, out topLeftCellIndex);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			bool searchForApproximateMatch = this.GetSearchForApproximateMatch(context);
			CriteriaEvaluator criteriaEvaluator;
			errorExpression = LookupFunctionBase.TryGetCriteriaEvaluator(context, searchForApproximateMatch, out criteriaEvaluator);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			ArrayExpression lookUpVector;
			errorExpression = this.TryGetLookupVector(lookupMatrix, out lookUpVector);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			ArrayExpression resultVector;
			errorExpression = this.TryGetResultVector(lookupMatrix, context, out resultVector);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			return LookupFunctionBase.HandleLookupFunction(criteriaEvaluator, lookUpVector, resultVector, topLeftCellIndex, searchForApproximateMatch);
		}

		internal abstract ErrorExpression TryGetLookupVector(ArrayExpression lookupMatrix, out ArrayExpression lookupVector);

		internal abstract ErrorExpression TryGetResultVector(ArrayExpression lookupMatrix, FunctionEvaluationContext<object> context, out ArrayExpression resultVector);

		internal abstract bool GetSearchForApproximateMatch(FunctionEvaluationContext<object> context);

		static ErrorExpression TryGetCriteriaEvaluator(FunctionEvaluationContext<object> context, bool approximateMatch, out CriteriaEvaluator evaluator)
		{
			object argument = context.Arguments[0];
			Worksheet worksheet = context.Worksheet;
			ComparisonOperator comparisonOperator = (approximateMatch ? ComparisonOperator.LessThanOrEqualsTo : ComparisonOperator.EqualsTo);
			return FunctionHelper.TryGetCriteriaEvaluator(argument, worksheet, comparisonOperator, out evaluator);
		}

		static ErrorExpression TryGetLookUpMatrix(object argument, out ArrayExpression lookUpVector, out Worksheet worksheet, out CellIndex topLeftCellIndex)
		{
			return FunctionHelper.TryGetArrayFromFunctionArgument(argument, out lookUpVector, out worksheet, out topLeftCellIndex);
		}

		static RadExpression HandleLookupFunction(CriteriaEvaluator criteriaEvaluator, ArrayExpression lookUpVector, ArrayExpression resultVector, CellIndex topLeftCellIndex, bool approximateMatch)
		{
			Guard.ThrowExceptionIfNull<CriteriaEvaluator>(criteriaEvaluator, "criteriaEvaluator");
			Guard.ThrowExceptionIfNull<ArrayExpression>(lookUpVector, "lookUpVector");
			Guard.ThrowExceptionIfNull<ArrayExpression>(resultVector, "resultVector");
			Guard.ThrowExceptionIfNull<CellIndex>(topLeftCellIndex, "topLeftCellIndex");
			int rowCount = lookUpVector.RowCount;
			int columnCount = lookUpVector.ColumnCount;
			bool flag = !approximateMatch;
			int num = (flag ? 0 : (rowCount - 1));
			int num2 = (flag ? 0 : (columnCount - 1));
			int num3 = (flag ? 1 : (-1));
			int num4 = num;
			while (num4 >= 0 && num4 < rowCount)
			{
				int num5 = num2;
				while (num5 >= 0 && num5 < columnCount)
				{
					RadExpression cellExpression = lookUpVector[num4, num5];
					if (criteriaEvaluator.Evaluate(cellExpression, topLeftCellIndex.RowIndex + num4, topLeftCellIndex.ColumnIndex + num5))
					{
						return LookupFunctionBase.GetCorrespondingResultValue(resultVector, num4, num5);
					}
					num5 += num3;
				}
				num4 += num3;
			}
			return ErrorExpressions.NotAvailableError;
		}

		static RadExpression GetCorrespondingResultValue(ArrayExpression resultVector, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, rowIndex, "rowIndex");
			Guard.ThrowExceptionIfLessThan<int>(0, columnIndex, "columnIndex");
			int rowCount = resultVector.RowCount;
			int columnCount = resultVector.ColumnCount;
			int num = Math.Max(rowIndex, columnIndex);
			RadExpression radExpression = null;
			if (rowCount == 1)
			{
				if (num < columnCount)
				{
					radExpression = resultVector[0, num];
				}
			}
			else if (columnCount == 1 && num < rowCount)
			{
				radExpression = resultVector[num, 0];
			}
			return radExpression ?? ErrorExpressions.NotAvailableError;
		}
	}
}
