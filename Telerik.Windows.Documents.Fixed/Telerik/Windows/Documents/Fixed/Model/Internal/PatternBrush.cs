using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	abstract class PatternBrush : Brush, IPatternColor
	{
		public PatternBrush(Matrix transform)
		{
			this.transform = transform;
		}

		public Matrix Transform
		{
			get
			{
				return this.transform;
			}
		}

		Matrix IPatternColor.Position
		{
			get
			{
				return this.Transform.ToMatrix();
			}
		}

		readonly Matrix transform;
	}
}
