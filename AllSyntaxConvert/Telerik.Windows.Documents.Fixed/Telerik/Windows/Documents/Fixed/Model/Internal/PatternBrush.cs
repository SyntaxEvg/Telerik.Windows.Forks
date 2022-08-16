using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	abstract class PatternBrush : global::Telerik.Windows.Documents.Fixed.Model.Internal.Brush, global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.IPatternColor
	{
		public PatternBrush(global::Telerik.Windows.Documents.Core.Data.Matrix transform)
		{
			this.transform = transform;
		}

		public global::Telerik.Windows.Documents.Core.Data.Matrix Transform
		{
			get
			{
				return this.transform;
			}
		}

		global::System.Windows.Media.Matrix global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.IPatternColor.Position
		{
			get
			{
				return this.Transform.ToMatrix();
			}
		}

		private readonly global::Telerik.Windows.Documents.Core.Data.Matrix transform;
	}
}
