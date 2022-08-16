using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class PageTypeAwareConverter : TypeAwareConverter<PdfName>
	{
		public PageTypeAwareConverter(Func<PdfName, object> factoryFunction, string typeProperty = "Type")
			: base(factoryFunction, typeProperty)
		{
		}

		protected override PdfPrimitive ConvertFromIndirectReference(Type type, PostScriptReader reader, IPdfImportContext context, IndirectReference reference)
		{
			IndirectObject indirectObject = context.ReadIndirectObject(reference);
			PdfPrimitive content = indirectObject.Content;
			bool flag = context.ImportSettings.ReadingMode == ReadingMode.OnDemand;
			if (flag)
			{
				PdfDictionary pdfDictionary = content as PdfDictionary;
				PageTreeNode pageTreeNode = this.CreateInstance(type, reader, context, pdfDictionary) as PageTreeNode;
				PdfPrimitive value;
				IPdfProperty pdfProperty;
				if (pdfDictionary.TryGetElement("Parent", out value) && pageTreeNode.Properties.TryGetValue("Parent", out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, value);
				}
				if (pdfDictionary.TryGetElement("Kids", out value) && pageTreeNode.Properties.TryGetValue("Kids", out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, value);
				}
				if (pdfDictionary.TryGetElement("MediaBox", out value) && pageTreeNode.Properties.TryGetValue("MediaBox", out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, value);
				}
				if (pdfDictionary.TryGetElement("CropBox", out value) && pageTreeNode.Properties.TryGetValue("CropBox", out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, value);
				}
				if (pdfDictionary.TryGetElement("Rotate", out value) && pageTreeNode.Properties.TryGetValue("Rotate", out pdfProperty))
				{
					pdfProperty.SetValue(reader, context, value);
				}
				context.MapPageToPdfDictionary(pageTreeNode, pdfDictionary);
				return pageTreeNode;
			}
			return base.Convert(type, reader, context, content);
		}

		const string Kids = "Kids";

		const string MediaBox = "MediaBox";

		const string CropBox = "CropBox";

		const string Parent = "Parent";

		const string Rotate = "Rotate";
	}
}
