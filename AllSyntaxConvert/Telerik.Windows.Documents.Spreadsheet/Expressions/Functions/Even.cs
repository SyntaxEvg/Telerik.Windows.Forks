using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Even : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Even.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Even.Info;
			}
		}

		static Even()
		{
			string description = "Returns number rounded up to the nearest even integer. You can use this function for processing items that come in twos. For example, a packing crate accepts rows of one or two items. The crate is full when the number of items, rounded up to the nearest two, matches the crate's capacity.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Even_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Even_NumberInfo")
			};
			Even.Info = new FunctionInfo(Even.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 0.0)
			{
				num = Math.Floor(num);
				if (num % 2.0 != 0.0)
				{
					num -= 1.0;
				}
			}
			else
			{
				num = Math.Ceiling(num);
				if (num % 2.0 != 0.0)
				{
					num += 1.0;
				}
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "EVEN";

		static readonly FunctionInfo Info;
	}
}
