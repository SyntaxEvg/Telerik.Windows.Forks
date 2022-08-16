using System;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	abstract class AnnotationAppearances
	{
		public abstract AnnotationAppearancesType AppearancesType { get; }

		public abstract AnnotationAppearances Clone(RadFixedDocumentCloneContext cloneContext);
	}
}
