using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public abstract class PatternColor : ColorBase, IInstanceIdOwner, IPatternColor
	{
		protected PatternColor()
		{
			this.id = InstanceIdGenerator.GetNextId();
		}

		public abstract IPosition Position { get; set; }

		internal override ColorSpaceBase ColorSpace
		{
			get
			{
				return new Pattern();
			}
		}

		internal override bool IsTransparent
		{
			get
			{
				return false;
			}
		}

		internal abstract PatternType PatternType { get; }

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		Matrix IPatternColor.Position
		{
			get
			{
				return this.Position.Matrix;
			}
		}

		internal override Color ToColor()
		{
			throw new NotSupportedException("PatternColor cannot be converted to simple Color.");
		}

		readonly int id;
	}
}
