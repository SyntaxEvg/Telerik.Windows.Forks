using System;

namespace Telerik.Windows.Documents.Model.Drawing.Theming
{
	public class NoFill : Fill
	{
		internal override ShapeType ShapeFillType
		{
			get
			{
				return ShapeType.NoFill;
			}
		}

		public override Fill Clone()
		{
			return new NoFill();
		}
	}
}
