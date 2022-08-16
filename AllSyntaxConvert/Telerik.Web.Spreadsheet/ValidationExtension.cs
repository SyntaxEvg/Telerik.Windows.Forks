using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace Telerik.Web.Spreadsheet
{
	public static class ValidationExtension
	{
		public static Validation ToCellValidation(this IDataValidationRule validationRule)
		{
			if (validationRule is ListDataValidationRule)
			{
				return ValidationExtension.FromListValidationRule((ListDataValidationRule)validationRule);
			}
			if (validationRule is CustomDataValidationRule)
			{
				return ValidationExtension.FromCustomValidationRule((CustomDataValidationRule)validationRule);
			}
			if (validationRule is NumberDataValidationRuleBase)
			{
				return ValidationExtension.FromNumberValidationRule((NumberDataValidationRuleBase)validationRule);
			}
			return null;
		}

		static Validation FromNumberValidationRule(NumberDataValidationRuleBase validationRule)
		{
			string dataType = "number";
			if (validationRule is DateDataValidationRule)
			{
				dataType = "date";
			}
			else if (validationRule is TextLengthDataValidationRule)
			{
				return null;
			}
			ComparisonOperator comparisonOperator = validationRule.ComparisonOperator;
			string comparerType = (ValidationExtension.ValidationComparisonOperators.ContainsKey(comparisonOperator) ? ValidationExtension.ValidationComparisonOperators[comparisonOperator] : "equalTo");
			return new Validation
			{
				DataType = dataType,
				From = validationRule.Argument1.ToFormula(),
				To = validationRule.Argument2.ToFormula(),
				Type = validationRule.ErrorStyle.ToType(),
				ComparerType = comparerType,
				AllowNulls = new bool?(validationRule.IgnoreBlank),
				MessageTemplate = validationRule.ErrorAlertContent,
				TitleTemplate = validationRule.ErrorAlertTitle
			};
		}

		static Validation FromListValidationRule(ListDataValidationRule validationRule)
		{
			return new Validation
			{
				DataType = "list",
				Type = validationRule.ErrorStyle.ToType(),
				From = validationRule.Argument1.ToFormula(),
				ComparerType = "list",
				AllowNulls = new bool?(validationRule.IgnoreBlank),
				ShowButton = new bool?(validationRule.InCellDropdown),
				MessageTemplate = validationRule.ErrorAlertContent,
				TitleTemplate = validationRule.ErrorAlertTitle
			};
		}

		static Validation FromCustomValidationRule(CustomDataValidationRule validationRule)
		{
			return new Validation
			{
				DataType = "custom",
				Type = validationRule.ErrorStyle.ToType(),
				From = validationRule.Argument1.ToFormula(),
				ComparerType = "custom",
				AllowNulls = new bool?(validationRule.IgnoreBlank),
				MessageTemplate = validationRule.ErrorAlertContent,
				TitleTemplate = validationRule.ErrorAlertTitle
			};
		}

		static string ToFormula(this ICellValue cellValue)
		{
			if (cellValue.ValueType == CellValueType.Text)
			{
				return string.Format("\"{0}\"", cellValue.RawValue);
			}
			return cellValue.RawValue;
		}

		static string ToType(this ErrorStyle errorStyle)
		{
			if (errorStyle == ErrorStyle.Stop)
			{
				return "reject";
			}
			return "warning";
		}

		static readonly Dictionary<ComparisonOperator, string> ValidationComparisonOperators = new Dictionary<ComparisonOperator, string>
		{
			{
				ComparisonOperator.EqualsTo,
				"equalTo"
			},
			{
				ComparisonOperator.Between,
				"between"
			},
			{
				ComparisonOperator.GreaterThan,
				"greaterThan"
			},
			{
				ComparisonOperator.GreaterThanOrEqualsTo,
				"greaterThanOrEqualTo"
			},
			{
				ComparisonOperator.LessThan,
				"lessThan"
			},
			{
				ComparisonOperator.LessThanOrEqualsTo,
				"lessThanOrEqualTo"
			},
			{
				ComparisonOperator.NotBetween,
				"notBetween"
			},
			{
				ComparisonOperator.NotEqualsTo,
				"notEqualTo"
			}
		};
	}
}
