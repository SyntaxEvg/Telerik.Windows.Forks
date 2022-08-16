using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;

namespace Telerik.Documents.SpreadsheetStreaming.Importers
{
	class XlsxCellImporter : ICellImporter
	{
		internal XlsxCellImporter(CellElement cellElement)
		{
			this.cellElement = cellElement;
			this.InitializeValue();
		}

		public int RowIndex
		{
			get
			{
				return this.cellElement.Reference.RowIndex;
			}
		}

		public int ColumnIndex
		{
			get
			{
				return this.cellElement.Reference.ColumnIndex;
			}
		}

		public SpreadCellFormat Format
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string Value { get; set; }

		void InitializeValue()
		{
			switch (CellTypesMapper.GetCellValueType(this.cellElement.CellDataType))
			{
			case CellValueType.Boolean:
			{
				string a = this.ReadValue();
				this.Value = (a == "1").ToString();
				return;
			}
			case CellValueType.Date:
			case CellValueType.Number:
			case CellValueType.Text:
			{
				string value = this.ReadValue();
				this.Value = value;
				return;
			}
			case CellValueType.Formula:
			{
				string value2 = this.ReadFormula();
				this.Value = value2;
				return;
			}
			}
			throw new NotImplementedException();
		}

		string ReadFormula()
		{
			string result = null;
			this.cellElement.ReadFormula(ref result);
			return result;
		}

		string ReadValue()
		{
			string result = null;
			this.cellElement.ReadValue(ref result);
			return result;
		}

		readonly CellElement cellElement;
	}
}
