using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class SingleStateAppearanceFieldPropertiesEditor : FormFieldPropertiesEditor
	{
		public SingleStateAppearanceFieldPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
			: base(context, field)
		{
		}

		public void SetAppearance(IEnumerable<FormSource> normalAppearances)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FormSource>>(normalAppearances, "normalAppearances");
			int num = 0;
			foreach (FormSource formSource in normalAppearances)
			{
				FormXObject formXObject = new FormXObject();
				formXObject.CopyPropertiesFrom(base.Context, formSource);
				Appearances appearances = new Appearances();
				appearances.NormalAppearance = new Appearance(formXObject);
				PdfDictionary clonedWidgetDictionary = base.GetClonedWidgetDictionary(num);
				clonedWidgetDictionary["AP"] = appearances;
				num++;
			}
		}
	}
}
