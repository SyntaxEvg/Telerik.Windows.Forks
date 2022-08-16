using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Product : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NaryIgnoreIndirectNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return Product.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Product.Info;
			}
		}

		static Product()
		{
			string description = "Multiplies all the numbers given as arguments and returns the product.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Product_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ... are 1 to 30 numbers that you want to multiply.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Product_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ... are 1 to 30 numbers that you want to multiply.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Product_NumberInfo")
			};
			Product.Info = new FunctionInfo(Product.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num;
			if (context.Arguments.Length > 0)
			{
				num = context.Arguments[0];
			}
			else
			{
				num = 0.0;
			}
			for (int i = 1; i < context.Arguments.Length; i++)
			{
				num *= context.Arguments[i];
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "PRODUCT";

		static readonly FunctionInfo Info;
	}
}
