using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class BitAnd : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return BitAnd.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return BitAnd.Info;
			}
		}

		static BitAnd()
		{
			string description = "Returns a bitwise 'AND' of two numbers.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_BitAnd_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number1", "Must be in decimal form and greater than or equal to 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number1", "Spreadsheet_Functions_BitAnd_NumberInfo"),
				new ArgumentInfo("Number2", "Must be in decimal form and greater than or equal to 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number2", "Spreadsheet_Functions_BitAnd_NumberInfo")
			};
			BitAnd.Info = new FunctionInfo(BitAnd.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			long num4 = EngineeringFunctions.BITAND(num, num2);
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num4);
		}

		public static readonly string FunctionName = "BITAND";

		static readonly FunctionInfo Info;
	}
}
