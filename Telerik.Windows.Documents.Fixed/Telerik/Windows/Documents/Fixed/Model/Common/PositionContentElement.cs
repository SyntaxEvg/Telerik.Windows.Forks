using System;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	public abstract class PositionContentElement : ContentElementBase
	{
		public PositionContentElement()
		{
			this.Position = new MatrixPosition();
		}

		public IPosition Position
		{
			get
			{
				return this.position;
			}
			set
			{
				Guard.ThrowExceptionIfNull<IPosition>(value, "Position");
				this.position = value;
			}
		}

		internal Marker Marker { get; set; }

		internal PositionContentElement Clone(RadFixedDocumentCloneContext cloneContext)
		{
			PositionContentElement positionContentElement = this.CreateClonedInstance();
			positionContentElement.Position = this.Position.Clone();
			positionContentElement.Clipping = cloneContext.GetClonedClipping(base.Clipping);
			return positionContentElement;
		}

		internal virtual PositionContentElement CreateClonedInstance()
		{
			throw new NotImplementedException();
		}

		IPosition position;
	}
}
