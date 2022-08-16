using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Model.Drawing.Theming
{
	public class SolidFill : Fill
	{
		public ThemableColor Color
		{
			get
			{
				return this.color;
			}
		}

		internal override ShapeType ShapeFillType
		{
			get
			{
				return ShapeType.Solid;
			}
		}

		public SolidFill(ThemableColor color)
		{
			this.color = color;
		}

		public SolidFill(Color color)
		{
			this.color = new ThemableColor(color);
		}

		public override Fill Clone()
		{
			return new SolidFill(this.Color);
		}

		readonly ThemableColor color;
	}
}
