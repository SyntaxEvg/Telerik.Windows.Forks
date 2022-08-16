using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	static class FunctionHelper
	{
		public static ErrorExpression TryConvertArgument(RadExpression expression, ArgumentConversionRules conversionRules, ArgumentType argumentType, List<object> convertedArguments)
		{
			ErrorExpression result = null;
			switch (argumentType)
			{
			case ArgumentType.Any:
				result = FunctionHelper.TryConvertToAnyArgument(expression, convertedArguments);
				break;
			case ArgumentType.Logical:
				result = FunctionHelper.TryConvertArgument<bool>(expression, conversionRules, new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.TryConvertArgumentToBool), convertedArguments);
				break;
			case ArgumentType.Number:
				result = FunctionHelper.TryConvertArgument<double>(expression, conversionRules, new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.TryConvertArgumentToNumber), convertedArguments);
				break;
			case ArgumentType.Text:
				result = FunctionHelper.TryConvertArgument<string>(expression, conversionRules, new FunctionHelper.ConstantExpressionConverter<string>(FunctionHelper.TryConvertArgumentToString), convertedArguments);
				break;
			case ArgumentType.Reference:
				result = FunctionHelper.TryConvertToReferenceArgument(expression, convertedArguments);
				break;
			case ArgumentType.Array:
				result = FunctionHelper.TryConvertToArrayArgument(expression, convertedArguments);
				break;
			}
			return result;
		}

		internal static ErrorExpression TryConvertFirstArgument(RadExpression expression, ArgumentConversionRules convertionRules, ArgumentType argumentType, out object convertedArgument)
		{
			convertedArgument = null;
			List<object> list = new List<object>();
			ErrorExpression result = FunctionHelper.TryConvertArgument(expression, convertionRules, argumentType, list);
			if (list.Count > 0 && list[0] != null)
			{
				convertedArgument = list[0];
			}
			return result;
		}

		internal static ArrayExpression ConvertToArrayExpression(ConstantExpression constantExpression)
		{
			ArrayExpression result = constantExpression as ArrayExpression;
			NumberExpression numberExpression = constantExpression as NumberExpression;
			if (numberExpression != null)
			{
				RadExpression[,] array = new RadExpression[1, 1];
				array[0, 0] = numberExpression;
				result = new ArrayExpression(array);
			}
			return result;
		}

		internal static ErrorExpression TryGetArrayFromFunctionArgument(object argument, out ArrayExpression array, out Worksheet worksheet, out CellIndex topLeftCellIndex)
		{
			array = null;
			worksheet = null;
			topLeftCellIndex = new CellIndex(0, 0);
			ArrayExpression arrayExpression = FunctionHelper.ConvertToArrayExpression(argument as ConstantExpression);
			CellReferenceRangeExpression cellReferenceRangeExpression = argument as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression != null)
			{
				if (!cellReferenceRangeExpression.IsValid)
				{
					return ErrorExpressions.ReferenceError;
				}
				if (!cellReferenceRangeExpression.HasSingleRange)
				{
					return ErrorExpressions.NotAvailableError;
				}
				arrayExpression = cellReferenceRangeExpression.FirstRangeArray;
				worksheet = cellReferenceRangeExpression.Worksheet;
				CellReference fromCellReference = cellReferenceRangeExpression.CellReferenceRange.FromCellReference;
				topLeftCellIndex = new CellIndex(fromCellReference.ActualRowIndex, fromCellReference.ActualColumnIndex);
			}
			if (arrayExpression != null)
			{
				array = arrayExpression;
				return null;
			}
			return ErrorExpressions.ValueError;
		}

		internal static ErrorExpression TryGetCriteriaEvaluator(object argument, Worksheet criteriaWorksheet, ComparisonOperator comparisonOperator, out CriteriaEvaluator evaluator)
		{
			evaluator = null;
			ConstantExpression constantExpression;
			ErrorExpression errorExpression = FunctionHelper.TryGetNonArrayConstantExpression(argument, out constantExpression);
			if (errorExpression != null)
			{
				return errorExpression;
			}
			string str;
			switch (comparisonOperator)
			{
			case ComparisonOperator.EqualsTo:
				str = "=";
				goto IL_54;
			case ComparisonOperator.GreaterThanOrEqualsTo:
				str = ">=";
				goto IL_54;
			case ComparisonOperator.LessThanOrEqualsTo:
				str = "<=";
				goto IL_54;
			}
			throw new NotSupportedException("This type of comparison is not supported by the lookup and match functions.");
			IL_54:
			StringExpression criteriaExpression = new StringExpression(str + constantExpression.GetValueAsString());
			evaluator = new CriteriaEvaluator(criteriaExpression, criteriaWorksheet);
			return null;
		}

		static ErrorExpression TryGetNonArrayConstantExpression(object expression, out ConstantExpression constant)
		{
			constant = null;
			CellReferenceRangeExpression cellReferenceRangeExpression = expression as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression != null)
			{
				if (!cellReferenceRangeExpression.IsInRange)
				{
					return ErrorExpressions.ReferenceError;
				}
				if (!cellReferenceRangeExpression.HasSingleRange || cellReferenceRangeExpression.CellReferenceRange.ColumnsCount != 1 || cellReferenceRangeExpression.CellReferenceRange.RowsCount != 1)
				{
					return ErrorExpressions.ValueError;
				}
			}
			constant = ((RadExpression)expression).GetValueAsNonArrayConstantExpression(false);
			if (constant.GetCellValueType() == CellValueType.Empty)
			{
				return ErrorExpressions.NotAvailableError;
			}
			ErrorExpression errorExpression = constant as ErrorExpression;
			if (errorExpression != null)
			{
				return errorExpression;
			}
			return null;
		}

		static ErrorExpression TryConvertToArrayArgument(RadExpression expression, List<object> convertedArguments)
		{
			ConstantExpression valueAsConstant = expression.GetValueAsConstant();
			ArrayExpression arrayExpression = FunctionHelper.ConvertToArrayExpression(valueAsConstant);
			if (arrayExpression != null)
			{
				convertedArguments.Add(arrayExpression);
				return null;
			}
			ErrorExpression errorExpression = valueAsConstant as ErrorExpression;
			if (errorExpression != null)
			{
				return errorExpression;
			}
			return ErrorExpressions.ValueError;
		}

		static ErrorExpression TryConvertToReferenceArgument(RadExpression expression, List<object> convertedArgumnets)
		{
			ErrorExpression result = null;
			RadExpression value = expression.GetValue();
			if (expression is CellReferenceRangeExpression)
			{
				convertedArgumnets.Add(expression);
			}
			else if (value is CellReferenceRangeExpression)
			{
				convertedArgumnets.Add(value);
			}
			else if (value.Equals(ErrorExpressions.ReferenceError))
			{
				result = ErrorExpressions.ReferenceError;
			}
			else if (value.Equals(ErrorExpressions.NullError))
			{
				result = ErrorExpressions.NullError;
			}
			else
			{
				result = ErrorExpressions.ValueError;
			}
			return result;
		}

		static ErrorExpression TryConvertToAnyArgument(RadExpression expression, List<object> convertedArguments)
		{
			RadExpression valueAsConstantOrCellReference = expression.GetValueAsConstantOrCellReference();
			ErrorExpression errorExpression = valueAsConstantOrCellReference as ErrorExpression;
			if (errorExpression == null)
			{
				convertedArguments.Add(valueAsConstantOrCellReference);
			}
			return errorExpression;
		}

		static ErrorExpression TryConvertArgument<T>(RadExpression expression, ArgumentConversionRules convertionRules, FunctionHelper.ConstantExpressionConverter<T> argumentConverter, List<object> convertedArguments)
		{
			RadExpression valueAsConstant = expression.GetValueAsConstant();
			ArrayExpression arrayExpression = valueAsConstant as ArrayExpression;
			ErrorExpression result;
			if (arrayExpression != null)
			{
				result = FunctionHelper.TryConvertArrayArgument<T>(arrayExpression, convertionRules, argumentConverter, convertedArguments);
			}
			else
			{
				bool isDirect = FunctionHelper.IsDirectArgument(expression);
				result = FunctionHelper.TryConvertSingleArgument<T>(expression, convertionRules, argumentConverter, convertedArguments, isDirect);
			}
			return result;
		}

		static bool IsDirectArgument(RadExpression expression)
		{
			return !(expression is CellReferenceRangeExpression) && !(expression is ArrayExpression) && (expression is ConstantExpression || FunctionHelper.IsDirectArgument(expression.GetValueAsConstantOrCellReference()));
		}

		static ErrorExpression TryConvertSingleArgument<T>(RadExpression expression, ArgumentConversionRules convertionRules, FunctionHelper.ConstantExpressionConverter<T> argumentConverter, List<object> convertedArguments, bool isDirect)
		{
			ConstantExpression<T> constantExpression;
			ErrorExpression errorExpression = argumentConverter(expression, isDirect, convertionRules, out constantExpression);
			if (errorExpression == null && constantExpression != null)
			{
				convertedArguments.Add(constantExpression.Value);
			}
			return errorExpression;
		}

		static ErrorExpression TryConvertArrayArgument<T>(ArrayExpression arrayExpression, ArgumentConversionRules convertionRules, FunctionHelper.ConstantExpressionConverter<T> argumentConverter, List<object> convertedArguments)
		{
			ErrorExpression errorExpression = arrayExpression.GetContainingError();
			if (errorExpression != null)
			{
				return errorExpression;
			}
			if (convertionRules.ArrayArgument == ArrayArgumentInterpretation.UseAllElements)
			{
				using (IEnumerator<RadExpression> enumerator = arrayExpression.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RadExpression radExpression = enumerator.Current;
						CellReferenceRangeExpression cellReferenceRangeExpression = radExpression as CellReferenceRangeExpression;
						if (cellReferenceRangeExpression != null)
						{
							ArgumentConversionRules convertionRules2 = new ArgumentConversionRules(convertionRules, ArrayArgumentInterpretation.UseFirstElement);
							errorExpression = FunctionHelper.TryConvertArrayArgument<T>((ArrayExpression)cellReferenceRangeExpression.GetValue(), convertionRules2, argumentConverter, convertedArguments);
						}
						else
						{
							ArrayExpression arrayExpression2 = radExpression as ArrayExpression;
							if (arrayExpression2 != null)
							{
								errorExpression = FunctionHelper.TryConvertArrayArgument<T>(arrayExpression2, convertionRules, argumentConverter, convertedArguments);
							}
							else
							{
								errorExpression = FunctionHelper.TryConvertSingleArgument<T>(radExpression, convertionRules, argumentConverter, convertedArguments, false);
							}
						}
						if (errorExpression != null)
						{
							return errorExpression;
						}
					}
					return errorExpression;
				}
			}
			if (convertionRules.ArrayArgument == ArrayArgumentInterpretation.UseFirstElement)
			{
				RadExpression valueAsConstant = arrayExpression[0, 0].GetValueAsConstant();
				ArrayExpression arrayExpression3 = valueAsConstant as ArrayExpression;
				if (arrayExpression3 != null)
				{
					errorExpression = FunctionHelper.TryConvertArrayArgument<T>(arrayExpression3, convertionRules, argumentConverter, convertedArguments);
				}
				else
				{
					errorExpression = FunctionHelper.TryConvertSingleArgument<T>(arrayExpression[0, 0], convertionRules, argumentConverter, convertedArguments, false);
				}
			}
			return errorExpression;
		}

		static ErrorExpression TryConvertArgumentToNumber(RadExpression expression, bool isDirectArgument, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedArgument)
		{
			convertedArgument = null;
			RadExpression value = expression.GetValue();
			ErrorExpression errorExpression = value as ErrorExpression;
			if (errorExpression == null)
			{
				for (int i = 0; i < FunctionHelper.numberConverters.Length; i++)
				{
					errorExpression = FunctionHelper.numberConverters[i](value, isDirectArgument, convertionRules, out convertedArgument);
					if (errorExpression != null)
					{
						return errorExpression;
					}
					if (convertedArgument != null)
					{
						return null;
					}
				}
			}
			return errorExpression;
		}

		static ErrorExpression ToNumberExpression(double value, bool isDirect, ArgumentInterpretation directOption, ArgumentInterpretation indirectOption, out ConstantExpression<double> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			if ((isDirect && directOption == ArgumentInterpretation.TreatAsError) || (!isDirect && indirectOption == ArgumentInterpretation.TreatAsError))
			{
				result = ErrorExpressions.ValueError;
			}
			else if ((isDirect && directOption == ArgumentInterpretation.UseAsIs) || (!isDirect && indirectOption == ArgumentInterpretation.UseAsIs))
			{
				convertedExpression = new NumberExpression(value);
			}
			else if ((isDirect && directOption == ArgumentInterpretation.ConvertToDefault) || (!isDirect && indirectOption == ArgumentInterpretation.ConvertToDefault))
			{
				convertedExpression = NumberExpression.Zero;
			}
			return result;
		}

		static ErrorExpression EmptyToNumberExpression(RadExpression expressionValue, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedArgument)
		{
			ErrorExpression result = null;
			convertedArgument = null;
			if (expressionValue is EmptyExpression)
			{
				result = FunctionHelper.ToNumberExpression(0.0, isDirect, convertionRules.EmptyDirectArgument, convertionRules.EmptyIndirectArgument, out convertedArgument);
			}
			return result;
		}

		static ErrorExpression NumberToNumberExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			NumberExpression numberExpression = expression as NumberExpression;
			if (numberExpression != null)
			{
				result = FunctionHelper.ToNumberExpression(numberExpression.Value, isDirect, convertionRules.NumberDirectArgument, convertionRules.NumberIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression BoolToNumberExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			BooleanExpression booleanExpression = expression as BooleanExpression;
			if (booleanExpression != null)
			{
				double value = (double)(booleanExpression.Value ? 1 : 0);
				result = FunctionHelper.ToNumberExpression(value, isDirect, convertionRules.BoolDirectArgument, convertionRules.BoolIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression TextNumberToNumberExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			StringExpression stringExpression = expression as StringExpression;
			if (stringExpression != null)
			{
				bool flag = false;
				double value;
				if (convertionRules.ConvertDateTextToNumber)
				{
					if (!DateValue.TryParseToDate(stringExpression.Value, out value))
					{
						return ErrorExpressions.ValueError;
					}
					flag = true;
				}
				else if (FormatHelper.DefaultSpreadsheetCulture.TryParseDouble(stringExpression.Value, out value))
				{
					flag = true;
				}
				if (flag)
				{
					result = FunctionHelper.ToNumberExpression(value, isDirect, convertionRules.TextNumberDirectArgument, convertionRules.TextNumberIndirectArgument, out convertedExpression);
				}
			}
			return result;
		}

		static ErrorExpression NonTextNumbersToNumberExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<double> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			StringExpression stringExpression = expression as StringExpression;
			if (stringExpression != null)
			{
				double num;
				bool flag = !double.TryParse(stringExpression.Value, out num);
				if (flag)
				{
					result = FunctionHelper.ToNumberExpression(0.0, isDirect, convertionRules.NonTextNumberDirectArgument, convertionRules.NonTextNumberIndirectArgument, out convertedExpression);
				}
			}
			return result;
		}

		static ErrorExpression TryConvertArgumentToBool(RadExpression expression, bool isDirectArgument, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedArgument)
		{
			convertedArgument = null;
			RadExpression value = expression.GetValue();
			ErrorExpression errorExpression = value as ErrorExpression;
			if (errorExpression == null)
			{
				for (int i = 0; i < FunctionHelper.booleanConverters.Length; i++)
				{
					errorExpression = FunctionHelper.booleanConverters[i](value, isDirectArgument, convertionRules, out convertedArgument);
					if (errorExpression != null)
					{
						return errorExpression;
					}
					if (convertedArgument != null)
					{
						return null;
					}
				}
			}
			return errorExpression;
		}

		static ErrorExpression ToBoolExpression(bool value, bool isDirect, ArgumentInterpretation directOption, ArgumentInterpretation indirectOption, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			if ((isDirect && directOption == ArgumentInterpretation.TreatAsError) || (!isDirect && indirectOption == ArgumentInterpretation.TreatAsError))
			{
				result = ErrorExpressions.ValueError;
			}
			else if ((isDirect && directOption == ArgumentInterpretation.UseAsIs) || (!isDirect && indirectOption == ArgumentInterpretation.UseAsIs))
			{
				convertedExpression = value.ToBooleanExpression();
			}
			else if ((isDirect && directOption == ArgumentInterpretation.ConvertToDefault) || (!isDirect && indirectOption == ArgumentInterpretation.ConvertToDefault))
			{
				convertedExpression = BooleanExpression.False;
			}
			return result;
		}

		static ErrorExpression EmptyToBoolExpression(RadExpression expressionValue, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			if (expressionValue is EmptyExpression)
			{
				result = FunctionHelper.ToBoolExpression(false, isDirect, convertionRules.EmptyDirectArgument, convertionRules.EmptyIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression NumberToBoolExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			NumberExpression numberExpression = expression as NumberExpression;
			if (numberExpression != null)
			{
				bool value = numberExpression.Value != 0.0;
				result = FunctionHelper.ToBoolExpression(value, isDirect, convertionRules.NumberDirectArgument, convertionRules.NumberIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression BoolToBoolExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			BooleanExpression booleanExpression = expression as BooleanExpression;
			if (booleanExpression != null)
			{
				result = FunctionHelper.ToBoolExpression(booleanExpression.Value, isDirect, convertionRules.BoolDirectArgument, convertionRules.BoolIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression TextNumberToBoolExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			StringExpression stringExpression = expression as StringExpression;
			double num;
			if (stringExpression != null && double.TryParse(stringExpression.Value, out num))
			{
				bool value = num != 0.0;
				result = FunctionHelper.ToBoolExpression(value, isDirect, convertionRules.TextNumberDirectArgument, convertionRules.TextNumberIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression NonTextNumbersToBoolExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<bool> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			StringExpression stringExpression = expression as StringExpression;
			if (stringExpression != null)
			{
				double num;
				bool flag = !double.TryParse(stringExpression.Value, out num);
				if (flag)
				{
					bool value = num != 0.0;
					result = FunctionHelper.ToBoolExpression(value, isDirect, convertionRules.NonTextNumberDirectArgument, convertionRules.NonTextNumberIndirectArgument, out convertedExpression);
				}
			}
			return result;
		}

		static ErrorExpression TryConvertArgumentToString(RadExpression expression, bool isDirectArgument, ArgumentConversionRules convertionRules, out ConstantExpression<string> convertedArgument)
		{
			convertedArgument = null;
			RadExpression value = expression.GetValue();
			ErrorExpression errorExpression = value as ErrorExpression;
			if (errorExpression == null)
			{
				for (int i = 0; i < FunctionHelper.stringConverters.Length; i++)
				{
					errorExpression = FunctionHelper.stringConverters[i](value, isDirectArgument, convertionRules, out convertedArgument);
					if (errorExpression != null)
					{
						return errorExpression;
					}
					if (convertedArgument != null)
					{
						return null;
					}
				}
			}
			return errorExpression;
		}

		static ErrorExpression ToStringExpression(string value, bool isDirect, ArgumentInterpretation directOption, ArgumentInterpretation indirectOption, out ConstantExpression<string> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			if ((isDirect && directOption == ArgumentInterpretation.TreatAsError) || (!isDirect && indirectOption == ArgumentInterpretation.TreatAsError))
			{
				result = ErrorExpressions.ValueError;
			}
			else if ((isDirect && directOption == ArgumentInterpretation.UseAsIs) || (!isDirect && indirectOption == ArgumentInterpretation.UseAsIs))
			{
				convertedExpression = new StringExpression(value);
			}
			else if ((isDirect && directOption == ArgumentInterpretation.ConvertToDefault) || (!isDirect && indirectOption == ArgumentInterpretation.ConvertToDefault))
			{
				convertedExpression = new EmptyExpression();
			}
			return result;
		}

		static ErrorExpression BoolToStringExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<string> convertedExpression)
		{
			ErrorExpression result = null;
			convertedExpression = null;
			BooleanExpression booleanExpression = expression as BooleanExpression;
			if (booleanExpression != null)
			{
				string value = booleanExpression.ToString();
				result = FunctionHelper.ToStringExpression(value, isDirect, convertionRules.BoolDirectArgument, convertionRules.BoolIndirectArgument, out convertedExpression);
			}
			return result;
		}

		static ErrorExpression AllToStringExpression(RadExpression expression, bool isDirect, ArgumentConversionRules convertionRules, out ConstantExpression<string> convertedExpression)
		{
			convertedExpression = null;
			RadExpression value = expression.GetValue();
			ErrorExpression errorExpression = value as ErrorExpression;
			if (errorExpression != null)
			{
				return errorExpression;
			}
			convertedExpression = new StringExpression(value.GetValueAsString());
			return null;
		}

		static readonly FunctionHelper.ConstantExpressionConverter<double>[] numberConverters = new FunctionHelper.ConstantExpressionConverter<double>[]
		{
			new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.EmptyToNumberExpression),
			new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.NumberToNumberExpression),
			new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.BoolToNumberExpression),
			new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.TextNumberToNumberExpression),
			new FunctionHelper.ConstantExpressionConverter<double>(FunctionHelper.NonTextNumbersToNumberExpression)
		};

		static readonly FunctionHelper.ConstantExpressionConverter<bool>[] booleanConverters = new FunctionHelper.ConstantExpressionConverter<bool>[]
		{
			new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.EmptyToBoolExpression),
			new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.NumberToBoolExpression),
			new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.BoolToBoolExpression),
			new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.TextNumberToBoolExpression),
			new FunctionHelper.ConstantExpressionConverter<bool>(FunctionHelper.NonTextNumbersToBoolExpression)
		};

		static readonly FunctionHelper.ConstantExpressionConverter<string>[] stringConverters = new FunctionHelper.ConstantExpressionConverter<string>[]
		{
			new FunctionHelper.ConstantExpressionConverter<string>(FunctionHelper.BoolToStringExpression),
			new FunctionHelper.ConstantExpressionConverter<string>(FunctionHelper.AllToStringExpression)
		};

		public delegate ErrorExpression ConstantExpressionConverter<T>(RadExpression expression, bool isDirect, ArgumentConversionRules conversionRules, out ConstantExpression<T> convertedExpression);
	}
}
