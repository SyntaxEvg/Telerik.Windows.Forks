using System;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	class UnsupportedAnnotation : Annotation
	{
		public override AnnotationType Type
		{
			get
			{
				return this.type;
			}
		}

		public UnsupportedAnnotation(AnnotationType type)
		{
			this.type = type;
		}

		readonly AnnotationType type;
	}
}
