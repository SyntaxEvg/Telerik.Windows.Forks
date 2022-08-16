using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class AppearancesConverter : Converter
	{
		public AppearancesConverter()
		{
			this.appearanceConverter = new AppearanceConverter();
		}

		protected override PdfPrimitive ConvertFromDictionary(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Appearances appearances = new Appearances();
			Appearance normalAppearance;
			if (this.TryImportAppearance("N", reader, context, dictionary, out normalAppearance))
			{
				appearances.NormalAppearance = normalAppearance;
			}
			Appearance rolloverAppearance;
			if (this.TryImportAppearance("R", reader, context, dictionary, out rolloverAppearance))
			{
				appearances.RolloverAppearance = rolloverAppearance;
			}
			Appearance downAppearance;
			if (this.TryImportAppearance("D", reader, context, dictionary, out downAppearance))
			{
				appearances.DownAppearance = downAppearance;
			}
			return appearances;
		}

		bool TryImportAppearance(string key, PostScriptReader reader, IPdfImportContext context, PdfDictionary appearancesDictionary, out Appearance appearance)
		{
			PdfPrimitive value;
			if (appearancesDictionary.TryGetElement(key, out value))
			{
				appearance = (Appearance)this.appearanceConverter.Convert(typeof(Appearance), reader, context, value);
				return true;
			}
			appearance = null;
			return false;
		}

		readonly AppearanceConverter appearanceConverter;
	}
}
