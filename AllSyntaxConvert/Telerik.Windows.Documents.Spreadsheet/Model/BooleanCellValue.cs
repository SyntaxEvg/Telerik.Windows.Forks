using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class BooleanCellValue : CellValueBase<bool>
	{
		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Boolean;
			}
		}

		internal BooleanCellValue(bool value)
			: base(value)
		{
		}

		protected override string GetValueAsStringOverride(CellValueFormat format)
		{
			return base.Value.ToString().ToUpper();
		}
	}
}
