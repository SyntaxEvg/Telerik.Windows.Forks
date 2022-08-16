using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class UnsupportedAnnotation : AnnotationObject
	{
		public override bool IsSupported
		{
			get
			{
				return false;
			}
		}

		public override Annotation ToAnnotationOverride(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			throw new NotSupportedException("Import of this annotation type is not supported.");
		}
	}
}
