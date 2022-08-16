using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class BitLShift : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return BitLShift.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return BitLShift.Info;
			}
		}

		static BitLShift()
		{
			string description = "Returns a number shifted left by the specified number of bits.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_BitLShift_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number must be an integer greater than or equal to 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_BitShift_NumberInfo"),
				new ArgumentInfo("Shift_amount", "Shift_amount must be an integer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_ShiftAmount", "Spreadsheet_Functions_BitShift_ShiftAmountInfo")
			};
			BitLShift.Info = new FunctionInfo(BitLShift.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			long num;
			bool flag = long.TryParse(context.Arguments[0].ToString(), out num);
			int num2;
			if (!(flag & int.TryParse(context.Arguments[1].ToString(), out num2)))
			{
				return ErrorExpressions.NumberError;
			}
			long num3 = 281474976710655L;
			if (0L > num || num > num3 || Math.Abs(num2) > 53)
			{
				return ErrorExpressions.NumberError;
			}
			bool flag2 = num != 0L && MathUtility.IndexOfBiggestBit(num) + num2 > 47;
			if (flag2)
			{
				return ErrorExpressions.NumberError;
			}
			long num4 = EngineeringFunctions.BITLSHIFT(num, num2);
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num4);
		}

		const int BiggestBitIndex = 47;

		const int MaxShiftAmount = 53;

		public static readonly string FunctionName = "BITLSHIFT";

		static readonly FunctionInfo Info;
	}
}
