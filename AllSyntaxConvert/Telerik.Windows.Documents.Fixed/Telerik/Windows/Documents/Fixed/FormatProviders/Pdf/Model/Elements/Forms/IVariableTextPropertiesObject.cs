using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	interface IVariableTextPropertiesObject
	{
		PdfString GetDefaultAppearance(AcroFormObject form);

		void SetDefaultAppearance(PdfString value);

		PdfInt GetQuadding(AcroFormObject form);

		void SetQuadding(PdfInt value);
	}
}
