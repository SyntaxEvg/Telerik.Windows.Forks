using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	abstract class TransformParametersElementBase : PdfObject
	{
		internal abstract void CopyPropertiesTo(TransformParameters transformParameters);
	}
}
