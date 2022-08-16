using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Indirect : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Indirect.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Indirect.Info;
			}
		}

		static Indirect()
		{
			string description = "Returns the reference of the cell specified by a text string.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Indirect_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Ref_text", "A reference to a cell that contains an A1-style reference, a name defined as a reference, or a reference to a cell as a text string.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_RefText", "Spreadsheet_Functions_Indirect_RefText")
			};
			Indirect.Info = new FunctionInfo(Indirect.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string text = context.Arguments[0].ToString();
			if (string.IsNullOrEmpty(text))
			{
				return ErrorExpressions.ReferenceError;
			}
			List<CellReferenceRange> list = new List<CellReferenceRange>();
			CellReferenceRangeExpression cellReferenceRangeExpression;
			if (NameConverter.TryConvertNamesToCellReferenceRangeExpression(text, context.Worksheet, context.RowIndex, context.ColumnIndex, out cellReferenceRangeExpression))
			{
				list.AddRange(cellReferenceRangeExpression.CellReferenceRanges);
			}
			if (list.Count == 1)
			{
				CellReferenceRange cellReferenceRange = list.First<CellReferenceRange>();
				if (cellReferenceRange.Worksheet == context.Worksheet)
				{
					CellRange cellRange = cellReferenceRange.ToCellRange();
					if (cellRange.Contains(context.RowIndex, context.ColumnIndex))
					{
						return ErrorExpressions.CyclicReference;
					}
				}
			}
			if (list.Count == 0 || list.Count > 1)
			{
				return ErrorExpressions.ReferenceError;
			}
			return cellReferenceRangeExpression;
		}

		public static readonly string FunctionName = "INDIRECT";

		static readonly FunctionInfo Info;
	}
}
