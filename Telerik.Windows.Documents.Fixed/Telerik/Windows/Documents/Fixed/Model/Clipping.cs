using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model
{
	public class Clipping : ContentElementBase, IContextClonable<Clipping>
	{
		public GeometryBase Clip { get; set; }

		internal Matrix Transform { get; set; }

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Clipping;
			}
		}

		Clipping IContextClonable<Clipping>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			return new Clipping
			{
				Clip = this.Clip,
				Transform = this.Transform,
				Clipping = cloneContext.GetClonedClipping(base.Clipping)
			};
		}
	}
}
