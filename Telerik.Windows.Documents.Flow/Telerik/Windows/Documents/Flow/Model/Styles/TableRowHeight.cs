using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class TableRowHeight
	{
		public TableRowHeight(HeightType type)
			: this(type, 0.0)
		{
		}

		public TableRowHeight(HeightType type, double value)
		{
			this.type = type;
			this.value = value;
		}

		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public HeightType Type
		{
			get
			{
				return this.type;
			}
		}

		public static bool operator ==(TableRowHeight a, TableRowHeight b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		public static bool operator !=(TableRowHeight a, TableRowHeight b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			TableRowHeight tableRowHeight = obj as TableRowHeight;
			return !(tableRowHeight == null) && this.Value.Equals(tableRowHeight.Value) && this.Type.Equals(tableRowHeight.Type);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Value.GetHashCode(), this.Type.GetHashCode());
		}

		public override string ToString()
		{
			if (this.Type == HeightType.Auto)
			{
				return this.Type.ToString();
			}
			return string.Format("{0}, {1}", this.Type.ToString(), this.Value.ToString());
		}

		readonly double value;

		readonly HeightType type;
	}
}
