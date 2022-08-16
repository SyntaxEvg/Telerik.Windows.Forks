using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	public class FormSource : IContentRootElement, IContainerElement, IFixedDocumentElement, IInstanceIdOwner
	{
		public FormSource()
		{
			this.id = InstanceIdGenerator.GetNextId();
			this.content = new ContentElementCollection(this);
			this.Matrix = Matrix.Identity;
			this.BoundingBox = default(Rect);
		}

		public ContentElementCollection Content
		{
			get
			{
				return this.content;
			}
		}

		public Size Size
		{
			get
			{
				return new Size(this.BoundingBox.Width, this.BoundingBox.Height);
			}
			set
			{
				this.BoundingBox = new Rect(this.BoundingBox.X, this.BoundingBox.Y, value.Width, value.Height);
			}
		}

		public bool SupportsAnnotations
		{
			get
			{
				return false;
			}
		}

		public AnnotationCollection Annotations
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public IFixedDocumentElement Parent
		{
			get
			{
				return null;
			}
		}

		internal Matrix Matrix { get; set; }

		internal Rect BoundingBox { get; set; }

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		readonly int id;

		readonly ContentElementCollection content;
	}
}
