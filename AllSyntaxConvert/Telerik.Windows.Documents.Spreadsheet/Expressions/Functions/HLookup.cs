using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class HLookup : LookupFunctionBase
	{
		public override string Name
		{
			get
			{
				return HLookup.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return HLookup.Info;
			}
		}

		static HLookup()
		{
			string description = "The HLOOKUP function looks for a value in the top row of a table or array of values and then returns a value in the same column from a row you specify.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_HLookup_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Lookup_value", "The value to be found in the first row of the table. It can be a value, a reference or a text string.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_LookupValue", "Spreadsheet_Functions_HLookup_LookupValue"),
				new ArgumentInfo("Table_array", "A table of text, numbers or logical values, in which the data is looked up. Table_array can be a reference to a range or a range name.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_TableArray", "Spreadsheet_Functions_VHLookup_TableArray"),
				new ArgumentInfo("Row_index_num", "The row number in table_array from which the matching value should be returned. The first row of values in the table is row 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_RowIndexNum", "Spreadsheet_Functions_HLookup_RowIndexNum")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Range_lookup", "A logical value: to find the closest match in the top row (sorted in ascending order) = TRUE or omitted; find an exact match = FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_RangeLookup", "Spreadsheet_Functions_HLookup_RangeLookup")
			};
			HLookup.Info = new FunctionInfo(HLookup.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		internal override ErrorExpression TryGetLookupVector(ArrayExpression lookupMatrix, out ArrayExpression lookupVector)
		{
			lookupVector = lookupMatrix.GetRowAsArrayExpression(0);
			return null;
		}

		internal override ErrorExpression TryGetResultVector(ArrayExpression lookupMatrix, FunctionEvaluationContext<object> context, out ArrayExpression resultVector)
		{
			resultVector = null;
			int num = (int)((double)context.Arguments[2]) - 1;
			if (num < 0)
			{
				return ErrorExpressions.ValueError;
			}
			if (num >= lookupMatrix.RowCount)
			{
				return ErrorExpressions.ReferenceError;
			}
			resultVector = lookupMatrix.GetRowAsArrayExpression(num);
			return null;
		}

		internal override bool GetSearchForApproximateMatch(FunctionEvaluationContext<object> context)
		{
			return context.Arguments.Length != 4 || (bool)context.Arguments[3];
		}

		public static readonly string FunctionName = "HLOOKUP";

		static readonly FunctionInfo Info;
	}
}
