using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	class UnsupportedAnnotationOld : AnnotationOld
	{
		public UnsupportedAnnotationOld(PdfContentManager contentManager, AnnotationType type)
			: base(contentManager)
		{
			this.type = type;
		}

		public override AnnotationType Type
		{
			get
			{
				return this.type;
			}
		}

		readonly AnnotationType type;
	}
}
