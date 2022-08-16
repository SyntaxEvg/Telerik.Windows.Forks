using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ErrorType : FunctionBase
	{
		public override string Name
		{
			get
			{
				return ErrorType.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ErrorType.Info;
			}
		}

		static ErrorType()
		{
			string description = "Returns a number corresponding to one of the error values in Microsoft Excel or returns the #N/A error if no error exists. You can use ERROR.TYPE in an IF function to test for an error value and return a text string, such as a message, instead of the error value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ErrorType_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "is the error value whose identifying number you want to find. Although error_val can be the actual error value, it will usually be a reference to a cell containing a formula that you want to test.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_ErrorType_ValueInfo")
			};
			ErrorType.Info = new FunctionInfo(ErrorType.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			double num = 0.0;
			ErrorExpression errorExpression = context.Arguments[0].GetValueAsConstantExpression() as ErrorExpression;
			if (errorExpression != null)
			{
				if (errorExpression == ErrorExpressions.CyclicReference)
				{
					return ErrorExpressions.CyclicReference;
				}
				if (errorExpression == ErrorExpressions.NullError)
				{
					num = 1.0;
				}
				else if (errorExpression == ErrorExpressions.DivisionByZero)
				{
					num = 2.0;
				}
				else if (errorExpression == ErrorExpressions.ValueError)
				{
					num = 3.0;
				}
				else if (errorExpression == ErrorExpressions.ReferenceError)
				{
					num = 4.0;
				}
				else if (errorExpression == ErrorExpressions.NameError)
				{
					num = 5.0;
				}
				else if (errorExpression == ErrorExpressions.NumberError)
				{
					num = 6.0;
				}
				else if (errorExpression == ErrorExpressions.NotAvailableError)
				{
					num = 7.0;
				}
			}
			if (num == 0.0)
			{
				return ErrorExpressions.NotAvailableError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "ERROR.TYPE";

		static readonly FunctionInfo Info;
	}
}
