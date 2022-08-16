using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class BitOr : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return BitOr.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return BitOr.Info;
			}
		}

		static BitOr()
		{
			string description = "Returns a bitwise 'OR' of two numbers.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_BitOr_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number1", "Must be in decimal form and greater than or equal to 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number1", "Spreadsheet_Functions_BitOr_NumberInfo"),
				new ArgumentInfo("Number2", "Must be in decimal form and greater than or equal to 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number2", "Spreadsheet_Functions_BitOr_NumberInfo")
			};
			BitOr.Info = new FunctionInfo(BitOr.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			long num;
			bool flag = long.TryParse(context.Arguments[0].ToString(), out num);
			long num2;
			if (!(flag & long.TryParse(context.Arguments[1].ToString(), out num2)))
			{
				return ErrorExpressions.NumberError;
			}
			long num3 = 281474976710655L;
			if (0L > num || num > num3 || 0L > num2 || num2 > num3)
			{
				return ErrorExpressions.NumberError;
			}
			long num4 = EngineeringFunctions.BITOR(num, num2);
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num4);
		}

		public static readonly string FunctionName = "BITOR";

		static readonly FunctionInfo Info;
	}
}
