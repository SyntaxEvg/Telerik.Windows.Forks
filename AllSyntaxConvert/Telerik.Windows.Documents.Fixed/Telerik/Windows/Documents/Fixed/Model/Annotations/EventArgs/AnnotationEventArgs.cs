using System;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations.EventArgs
{
	public class AnnotationEventArgs : global::System.EventArgs
	{
		public RadFixedPage Page { get; set; }

		public Annotation Annotation { get; set; }

		public bool Handled { get; set; }
	}
}
