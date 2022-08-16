using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class WorksheetReference
	{
		public bool IsWorksheetAbsolute
		{
			get
			{
				return this.isWorksheetAbsolute;
			}
		}

		public string WorksheetName
		{
			get
			{
				return this.worksheetName;
			}
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		internal WorksheetReference(Workbook workbook, string worksheetName, bool isWorksheetAbsolute)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			this.workbook = workbook;
			this.worksheetName = worksheetName;
			this.isWorksheetAbsolute = isWorksheetAbsolute;
			this.worksheet = this.GetWorksheet();
		}

		Worksheet GetWorksheet()
		{
			string text = TextHelper.DecodeWorksheetName(this.worksheetName);
			return SpreadsheetHelper.GetWorksheetByName(this.workbook, text);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.isWorksheetAbsolute)
			{
				stringBuilder.Append(this.worksheetName);
				stringBuilder.Append("!");
			}
			return stringBuilder.ToString();
		}

		public WorksheetReference CloneAndTranslate(Worksheet worksheet)
		{
			string text = (this.IsWorksheetAbsolute ? this.worksheetName : worksheet.Name);
			return new WorksheetReference(worksheet.Workbook, text, this.isWorksheetAbsolute);
		}

		internal void Translate()
		{
			if (this.Worksheet != null)
			{
				this.worksheetName = this.Worksheet.Name;
			}
		}

		public override bool Equals(object obj)
		{
			WorksheetReference worksheetReference = obj as WorksheetReference;
			return worksheetReference != null && (TelerikHelper.EqualsOfT<Workbook>(this.workbook, worksheetReference.workbook) && TelerikHelper.EqualsOfT<Worksheet>(this.worksheet, worksheetReference.worksheet)) && TelerikHelper.EqualsOfT<bool>(this.isWorksheetAbsolute, worksheetReference.isWorksheetAbsolute);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.workbook.GetHashCode(), this.worksheet.GetHashCode(), this.isWorksheetAbsolute.GetHashCodeOrZero());
		}

		readonly Workbook workbook;

		readonly bool isWorksheetAbsolute;

		readonly Worksheet worksheet;

		string worksheetName;
	}
}
