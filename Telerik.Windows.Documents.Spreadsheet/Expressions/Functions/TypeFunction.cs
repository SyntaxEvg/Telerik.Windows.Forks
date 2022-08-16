using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class TypeFunction : FunctionBase
	{
		public override string Name
		{
			get
			{
				return TypeFunction.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return TypeFunction.Info;
			}
		}

		static TypeFunction()
		{
			string description = "Returns the type of value. Use TYPE when the behavior of another function depends on the type of value in a particular cell.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Type_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "can be any RadSpreadsheet value, such as a number, text, logical value, and so on.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Type_ValueInfo")
			};
			TypeFunction.Info = new FunctionInfo(TypeFunction.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			double number = 1.0;
			RadExpression valueAsConstantExpression = context.Arguments[0].GetValueAsConstantExpression();
			if (valueAsConstantExpression.Equals(ErrorExpressions.CyclicReference))
			{
				return ErrorExpressions.CyclicReference;
			}
			if (valueAsConstantExpression is StringExpression)
			{
				number = 2.0;
			}
			else if (valueAsConstantExpression is BooleanExpression)
			{
				number = 4.0;
			}
			else if (valueAsConstantExpression is ErrorExpression)
			{
				number = 16.0;
			}
			else if (valueAsConstantExpression is ArrayExpression)
			{
				number = 64.0;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TYPE";

		static readonly FunctionInfo Info;
	}
}
