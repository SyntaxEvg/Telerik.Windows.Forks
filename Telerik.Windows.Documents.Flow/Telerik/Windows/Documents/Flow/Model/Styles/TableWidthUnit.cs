using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TableWidthUnit
	{
		public TableWidthUnit(TableWidthUnitType type)
			: this(type, double.NaN)
		{
		}

		public TableWidthUnit(double value)
			: this(TableWidthUnitType.Fixed, value)
		{
		}

		public TableWidthUnit(TableWidthUnitType type, double value)
		{
			this.type = type;
			this.value = value;
		}

		public TableWidthUnitType Type
		{
			get
			{
				return this.type;
			}
		}

		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public static bool operator ==(TableWidthUnit a, TableWidthUnit b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(TableWidthUnit a, TableWidthUnit b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			TableWidthUnit tableWidthUnit = obj as TableWidthUnit;
			return !(tableWidthUnit == null) && this.Type.Equals(tableWidthUnit.Type) && this.Value.Equals(tableWidthUnit.Value);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Type.GetHashCode(), this.Value.GetHashCode());
		}

		public override string ToString()
		{
			if (this.Type == TableWidthUnitType.Auto)
			{
				return this.Type.ToString();
			}
			return string.Format("{0}, {1}", this.Type.ToString(), this.Value.ToString());
		}

		readonly TableWidthUnitType type;

		readonly double value;
	}
}
