using System;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Objects
{
	public class Form : PositionContentElement
	{
		public FormSource FormSource { get; set; }

		public double Width
		{
			get
			{
				if (this.width != null)
				{
					return this.width.Value;
				}
				if (this.FormSource != null)
				{
					return this.FormSource.Size.Width;
				}
				return 0.0;
			}
			set
			{
				this.width = new double?(value);
			}
		}

		public double Height
		{
			get
			{
				if (this.height != null)
				{
					return this.height.Value;
				}
				if (this.FormSource != null)
				{
					return this.FormSource.Size.Height;
				}
				return 0.0;
			}
			set
			{
				this.height = new double?(value);
			}
		}

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Form;
			}
		}

		internal override PositionContentElement CreateClonedInstance()
		{
			return new Form
			{
				width = this.width,
				height = this.height,
				FormSource = this.FormSource
			};
		}

		double? width;

		double? height;
	}
}
