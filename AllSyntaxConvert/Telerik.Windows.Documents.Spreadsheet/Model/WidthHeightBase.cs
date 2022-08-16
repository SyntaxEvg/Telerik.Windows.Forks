using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class WidthHeightBase
	{
		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public bool IsCustom
		{
			get
			{
				return this.isCustom;
			}
		}

		protected WidthHeightBase(double value, bool isCustom)
		{
			this.value = value;
			this.isCustom = isCustom;
		}

		public override bool Equals(object obj)
		{
			WidthHeightBase widthHeightBase = obj as WidthHeightBase;
			return widthHeightBase != null && this.value == widthHeightBase.value && this.isCustom == widthHeightBase.isCustom;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.value.GetHashCode(), this.isCustom.GetHashCode());
		}

		public override string ToString()
		{
			return string.Format("[{0};{1}]", this.Value, this.IsCustom);
		}

		readonly double value;

		readonly bool isCustom;
	}
}
