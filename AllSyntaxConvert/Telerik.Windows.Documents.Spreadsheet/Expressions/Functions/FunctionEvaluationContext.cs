using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class FunctionEvaluationContext<T>
	{
		public T[] Arguments { get; set; }

		public Worksheet Worksheet { get; set; }

		public int RowIndex { get; set; }

		public int ColumnIndex { get; set; }

		public FunctionEvaluationContext(T[] arguments, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			this.Arguments = arguments;
			this.Worksheet = worksheet;
			this.RowIndex = rowIndex;
			this.ColumnIndex = columnIndex;
		}
	}
}
