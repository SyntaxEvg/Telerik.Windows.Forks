using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	class CriteriaEvaluator
	{
		static CriteriaEvaluator()
		{
			CriteriaEvaluator.numberComparer[">="] = (double x, double y) => x >= y;
			CriteriaEvaluator.numberComparer["<="] = (double x, double y) => x <= y;
			CriteriaEvaluator.numberComparer["<>"] = (double x, double y) => x != y;
			CriteriaEvaluator.numberComparer[">"] = (double x, double y) => x > y;
			CriteriaEvaluator.numberComparer["<"] = (double x, double y) => x < y;
			CriteriaEvaluator.numberComparer["="] = (double x, double y) => x == y;
		}

		public CriteriaEvaluator(RadExpression criteriaExpression, Worksheet worksheet = null)
		{
			this.worksheet = worksheet ?? new Workbook().Worksheets.Add();
			this.criteria = criteriaExpression.GetValueAsNonArrayConstantExpression(false);
			if (this.criteria is NumberExpression)
			{
				this.evaluate = new Func<RadExpression, int, int, bool>(this.EvaluateNumberCriteria);
				return;
			}
			if (this.criteria is BooleanExpression)
			{
				this.evaluate = new Func<RadExpression, int, int, bool>(this.EvaluateBooleanCriteria);
				return;
			}
			if (this.criteria is StringExpression)
			{
				this.evaluate = new Func<RadExpression, int, int, bool>(this.EvaluateStringCriteria);
				this.CacheStringParsingValues((this.criteria as StringExpression).Value);
				return;
			}
			if (this.criteria is ErrorExpression)
			{
				this.evaluate = new Func<RadExpression, int, int, bool>(this.EvaluateErrorCriteria);
				return;
			}
			this.evaluate = new Func<RadExpression, int, int, bool>(this.EvaluateFalse);
		}

		void CacheStringParsingValues(string criteriaValue)
		{
			this.comparissonOperator = this.GetComparissonOperator(criteriaValue);
			string text = CriteriaEvaluator.TrimFromComparissonOperator(criteriaValue, this.comparissonOperator);
			double value;
			if (this.TryGetNumberCellValueFromString(0, 0, text, out value))
			{
				this.parsedNumber = new double?(value);
			}
			bool value2;
			if (bool.TryParse(text, out value2))
			{
				this.parsedBoolean = new bool?(value2);
			}
			this.criteriaTrimmedFromComparissonOperator = text;
			this.criteriaAsRegex = CriteriaEvaluator.CreateRegexFrom(this.criteriaTrimmedFromComparissonOperator);
			if (this.comparissonOperator != "=")
			{
				int num = this.IndexOfFirstWildcardCharacter();
				if (num >= 0)
				{
					this.containsWildCharacter = true;
					this.criteriaTrimmedToFirstWildCharacter = this.criteriaTrimmedFromComparissonOperator.Substring(0, num);
				}
			}
		}

		int IndexOfFirstWildcardCharacter()
		{
			if (this.criteriaTrimmedFromComparissonOperator[0].IsNonTildeWildcardCharacter())
			{
				return 0;
			}
			for (int i = 1; i < this.criteriaTrimmedFromComparissonOperator.Length; i++)
			{
				if (this.criteriaTrimmedFromComparissonOperator[i].IsNonTildeWildcardCharacter() && this.criteriaTrimmedFromComparissonOperator[i - 1] != "~"[0])
				{
					return i;
				}
			}
			return -1;
		}

		static string CreateRegexFrom(string excelWildcardString)
		{
			return string.Format("^{0}$", excelWildcardString.FromWildcardStringToRegex());
		}

		bool Compare(string cellValue, string stringComparissonOperator)
		{
			if (stringComparissonOperator == "=")
			{
				return this.GetEqualToComparissonResult(cellValue);
			}
			if (stringComparissonOperator == "<>")
			{
				return !this.GetEqualToComparissonResult(cellValue);
			}
			return this.GetInequalityComparissonResult(cellValue, stringComparissonOperator);
		}

		bool GetInequalityComparissonResult(string cellValue, string stringComparissonOperator)
		{
			if (this.containsWildCharacter && cellValue.Length > this.criteriaTrimmedToFirstWildCharacter.Length && cellValue.StartsWith(this.criteriaTrimmedToFirstWildCharacter))
			{
				return stringComparissonOperator.StartsWith(">");
			}
			string stringComparisson = string.Format("=\"{0}\"{1}\"{2}\"", CriteriaEvaluator.EscapeQuotations(cellValue), stringComparissonOperator, CriteriaEvaluator.EscapeQuotations(this.criteriaTrimmedFromComparissonOperator));
			return this.CalculateStringComparissonAsCellValue(stringComparisson);
		}

		static string EscapeQuotations(string stringCellValue)
		{
			return stringCellValue.Replace("\"", "\"\"");
		}

		bool CalculateStringComparissonAsCellValue(string stringComparisson)
		{
			bool flag;
			return this.TryGetBooleanCellValueFromString(0, 0, stringComparisson, out flag) && flag;
		}

		bool GetEqualToComparissonResult(string cellValue)
		{
			return Regex.Match(cellValue, this.criteriaAsRegex).Success;
		}

		static string TrimFromComparissonOperator(string criteriaToTrim, string operatorToTrim)
		{
			if (criteriaToTrim.StartsWith(operatorToTrim))
			{
				return criteriaToTrim.Substring(operatorToTrim.Length);
			}
			return criteriaToTrim;
		}

		string GetComparissonOperator(string criteria)
		{
			for (int i = 0; i < CriteriaEvaluator.comperissonOperators.Length; i++)
			{
				if (criteria.StartsWith(CriteriaEvaluator.comperissonOperators[i]))
				{
					return CriteriaEvaluator.comperissonOperators[i];
				}
			}
			return "=";
		}

		public bool Evaluate(RadExpression cellExpression, int rowIndex, int columnIndex)
		{
			return this.evaluate(cellExpression, rowIndex, columnIndex);
		}

		bool EvaluateNumberCriteria(RadExpression cellExpression, int rowIndex, int columnIndex)
		{
			NumberExpression numberExpression = this.criteria as NumberExpression;
			NumberExpression numberExpression2 = cellExpression as NumberExpression;
			if (numberExpression2 != null)
			{
				return numberExpression2.Value == numberExpression.Value;
			}
			StringExpression stringExpression = cellExpression as StringExpression;
			double num;
			return stringExpression != null && this.TryGetNumberCellValueFromString(rowIndex, columnIndex, stringExpression.Value, out num) && numberExpression.Value == num;
		}

		bool TryGetNumberCellValueFromString(int rowIndex, int columnIndex, string stringValue, out double number)
		{
			ICellValue cellValue = stringValue.ToCellValue(this.worksheet, rowIndex, columnIndex);
			number = 0.0;
			return double.TryParse(cellValue.RawValue, out number);
		}

		bool TryGetBooleanCellValueFromString(int rowIndex, int columnIndex, string stringValue, out bool boolResult)
		{
			ICellValue cellValue = stringValue.ToCellValue(this.worksheet, rowIndex, columnIndex);
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				cellValue = formulaCellValue.GetResultValueAsCellValue();
			}
			boolResult = false;
			return bool.TryParse(cellValue.RawValue, out boolResult);
		}

		bool EvaluateBooleanCriteria(RadExpression cellExpression, int rowIndex, int columnIndex)
		{
			BooleanExpression booleanExpression = this.criteria as BooleanExpression;
			BooleanExpression booleanExpression2 = cellExpression as BooleanExpression;
			return booleanExpression2 != null && booleanExpression.Value == booleanExpression2.Value;
		}

		bool EvaluateStringCriteria(RadExpression cellExpression, int rowIndex, int columnIndex)
		{
			NumberExpression numberExpression = cellExpression as NumberExpression;
			if (numberExpression != null)
			{
				return this.EvaluateNumberWithStringCriteria(numberExpression.Value);
			}
			BooleanExpression booleanExpression = cellExpression as BooleanExpression;
			if (booleanExpression != null)
			{
				return this.EvaluateBooleanWithStringCriteria(booleanExpression.Value);
			}
			StringExpression stringExpression = cellExpression as StringExpression;
			if (stringExpression != null)
			{
				return this.EvaluateStringWithStringCriteria(stringExpression.Value, rowIndex, columnIndex);
			}
			if (cellExpression is EmptyExpression)
			{
				return this.EvaluateEmptyWithStringCriteria();
			}
			return this.StartsWithNotEqualComparissonOperator();
		}

		bool EvaluateNumberWithStringCriteria(double cellValue)
		{
			if (this.parsedNumber != null)
			{
				return CriteriaEvaluator.numberComparer[this.comparissonOperator](cellValue, this.parsedNumber.Value);
			}
			return this.StartsWithNotEqualComparissonOperator();
		}

		bool EvaluateBooleanWithStringCriteria(bool cellValue)
		{
			if (this.parsedBoolean != null)
			{
				int num = ((this.parsedBoolean == true) ? 1 : 0);
				int num2 = (cellValue ? 1 : 0);
				return CriteriaEvaluator.numberComparer[this.comparissonOperator]((double)num2, (double)num);
			}
			return this.StartsWithNotEqualComparissonOperator();
		}

		bool EvaluateStringWithStringCriteria(string cellValue, int rowIndex, int columnIndex)
		{
			if (this.parsedNumber != null)
			{
				double arg;
				if ((this.comparissonOperator == "=" || this.comparissonOperator == "<>") && this.TryGetNumberCellValueFromString(rowIndex, columnIndex, cellValue, out arg))
				{
					return CriteriaEvaluator.numberComparer[this.comparissonOperator](arg, this.parsedNumber.Value);
				}
				return this.StartsWithNotEqualComparissonOperator();
			}
			else
			{
				if (this.parsedBoolean != null && this.parsedBoolean.ToString().Length == this.criteriaTrimmedFromComparissonOperator.Length)
				{
					return this.StartsWithNotEqualComparissonOperator();
				}
				return this.Compare(cellValue, this.comparissonOperator);
			}
		}

		bool EvaluateEmptyWithStringCriteria()
		{
			if (this.comparissonOperator == "=")
			{
				return string.IsNullOrEmpty(this.criteriaTrimmedFromComparissonOperator);
			}
			return this.StartsWithNotEqualComparissonOperator();
		}

		bool EvaluateErrorCriteria(RadExpression cellValue, int rowIndex, int columnIndex)
		{
			return cellValue == this.criteria;
		}

		bool EvaluateFalse(RadExpression cellExpression, int rowIndex, int columnIndex)
		{
			return this.StartsWithNotEqualComparissonOperator();
		}

		bool StartsWithNotEqualComparissonOperator()
		{
			return this.comparissonOperator == "<>";
		}

		readonly Worksheet worksheet;

		readonly RadExpression criteria;

		readonly Func<RadExpression, int, int, bool> evaluate;

		static readonly string[] comperissonOperators = new string[] { ">=", "<=", "<>", ">", "<", "=" };

		static readonly Dictionary<string, Func<double, double, bool>> numberComparer = new Dictionary<string, Func<double, double, bool>>();

		string comparissonOperator = "=";

		double? parsedNumber = null;

		bool? parsedBoolean = null;

		string criteriaTrimmedFromComparissonOperator;

		bool containsWildCharacter;

		string criteriaTrimmedToFirstWildCharacter;

		string criteriaAsRegex;
	}
}
