using System;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class SpreadsheetStrings
	{
		public static readonly string GeneralErrorMessage = "We found a problem with this formula. Try clicking Insert Function on the Formulas tab to fix it.\\n\\nNot trying to type a formula? When the first character is an equal (=) or minus (-) sign, RadSpreadsheet thinks it is a formula. For example, when you type =1+1 the cell shows 2.";

		public static readonly string CyclicReferenceErrorMessage = "Cyclic Reference";

		public static readonly string CyclicReferenceMessage = "Careful, we found one or more circular references in your workbook that might cause your formulas to calculate incorrectly.\\n\\nFYI: A circular reference can be a formula that refers to its own cell value, or refers to a cell dependent on its own cell value.";

		public static readonly string NotAvailableErrorMessage = "Value Not Available Error";

		public static readonly string NumberErrorMessage = "Number Error";

		public static readonly string NameErrorMessage = "Invalid Name Error";

		public static readonly string ValueErrorMessage = "Error in Value";

		public static readonly string DivideByZeroErrorMessage = "Divide by Zero Error";

		public static readonly string NullErrorMessage = "Null Error";

		public static readonly string ReferenceErrorMessage = "Invalid Cell Reference Error";

		public static readonly string MissingOperandErrorMessage = "Your formula is incomplete. You must include an operand following each operator. For example, =A1+A2+ is missing an operand following the second plus sign. Try one of the following: \\n\\n• Add the missing operand to the formula, or delete the extra operator.\\n• If you are not trying to enter a formula, avoid using an equal sign (=) or a minus sign (-).";

		public static readonly string InvalidSheetNameMessage = "Sheet name is not valid. A valid sheet name must meet the following criteria:\r\n- The name cannot be empty\r\n- The name cannot exceed 31 characters\r\n- The name cannot start or end with a single quote (')\r\n- The name cannot contain any of the following characters: \\ / ? * [ ] :";

		public static readonly string ExistingSheetName = "Sheet with Name={0} already exists.";

		public static readonly string SheetAlreadyAdded = "The sheet has already been added to another workbook.";
	}
}
