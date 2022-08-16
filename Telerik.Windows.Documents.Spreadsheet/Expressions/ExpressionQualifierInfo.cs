using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ExpressionQualifierInfo
	{
		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public string Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		public Worksheet CurrentWorksheet
		{
			get
			{
				return this.currentWorksheet;
			}
		}

		public string EscapedQualifier
		{
			get
			{
				if (this.IsQualifierReferringWorkbook)
				{
					return TextHelper.EncodeWorksheetName(this.workbook.Name, false);
				}
				string referredWorksheetName = this.ReferredWorksheetName;
				if (!string.IsNullOrEmpty(referredWorksheetName))
				{
					return referredWorksheetName;
				}
				return this.qualifier;
			}
		}

		public string ReferredWorksheetName
		{
			get
			{
				Worksheet worksheetByName = SpreadsheetHelper.GetWorksheetByName(this.Workbook, this.decodedQualifier);
				if (worksheetByName != null)
				{
					return TextHelper.EncodeWorksheetName(worksheetByName.Name, this.qualifier[0] == '\'');
				}
				return null;
			}
		}

		public bool HasQualifier
		{
			get
			{
				return !string.IsNullOrEmpty(this.qualifier);
			}
		}

		public bool IsQualifierReferringWorkbook
		{
			get
			{
				return this.isQualifierReferringWorkbook;
			}
		}

		internal ExpressionQualifierInfo(Workbook workbook, Worksheet currentWorksheet, string qualifier)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			Guard.ThrowExceptionIfNull<Worksheet>(currentWorksheet, "currentWorksheet");
			this.workbook = workbook;
			this.currentWorksheet = currentWorksheet;
			this.qualifier = qualifier;
			this.decodedQualifier = TextHelper.DecodeWorksheetName(this.qualifier);
			this.isQualifierReferringWorkbook = this.workbook.Name.Equals(this.decodedQualifier, StringComparison.CurrentCultureIgnoreCase);
		}

		internal void UpdateQualifier(string newQualifier)
		{
			this.qualifier = newQualifier;
			this.decodedQualifier = TextHelper.DecodeWorksheetName(this.qualifier);
			this.isQualifierReferringWorkbook = this.workbook.Name.Equals(this.decodedQualifier, StringComparison.CurrentCultureIgnoreCase);
		}

		internal ExpressionQualifierInfo Translate(Worksheet worksheet)
		{
			return new ExpressionQualifierInfo(worksheet.Workbook, worksheet, this.qualifier);
		}

		internal ExpressionQualifierInfo TranslateOnWorkbookRenamed()
		{
			if (this.IsQualifierReferringWorkbook)
			{
				return new ExpressionQualifierInfo(this.workbook, this.currentWorksheet, this.workbook.Name);
			}
			return new ExpressionQualifierInfo(this.Workbook, this.CurrentWorksheet, this.Qualifier);
		}

		public override bool Equals(object obj)
		{
			ExpressionQualifierInfo expressionQualifierInfo = obj as ExpressionQualifierInfo;
			return expressionQualifierInfo != null && TelerikHelper.EqualsOfT<Workbook>(this.workbook, expressionQualifierInfo.workbook) && TelerikHelper.EqualsOfT<Worksheet>(this.currentWorksheet, expressionQualifierInfo.currentWorksheet) && TelerikHelper.EqualsOfT<string>(this.qualifier, expressionQualifierInfo.qualifier);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.workbook.GetHashCode(), this.currentWorksheet.GetHashCode(), this.qualifier.GetHashCodeOrZero());
		}

		readonly Workbook workbook;

		readonly Worksheet currentWorksheet;

		string qualifier;

		string decodedQualifier;

		bool isQualifierReferringWorkbook;
	}
}
