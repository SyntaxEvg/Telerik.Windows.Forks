using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class EmptyCellValue : CellValueBase<string>
	{
		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Empty;
			}
		}

		EmptyCellValue()
			: base(string.Empty)
		{
		}

		protected override string GetValueAsStringOverride(CellValueFormat format)
		{
			return string.Empty;
		}

		public override string GetResultValueAsString(CellValueFormat format)
		{
			return base.GetValueAsString(format);
		}

		public static readonly EmptyCellValue EmptyValue = new EmptyCellValue();
	}
}
