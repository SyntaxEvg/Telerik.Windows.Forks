using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class DestinationConverter : Converter
	{
		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			PdfPrimitive pdfPrimitive = array[0];
			IndirectReference indirectReference = pdfPrimitive as IndirectReference;
			PdfPrimitive pdfPrimitive2;
			if (indirectReference == null)
			{
				int value = ((PdfInt)pdfPrimitive).Value;
				pdfPrimitive2 = context.Pages[value];
			}
			else
			{
				context.TryGetIndirectObject(indirectReference, out pdfPrimitive2);
			}
			DestinationObject destinationObject = new DestinationObject();
			destinationObject.Page = (Page)pdfPrimitive2;
			destinationObject.DestinationType = (PdfName)array[1];
			string value2;
			if ((value2 = destinationObject.DestinationType.Value) != null)
			{
				if (!(value2 == "XYZ"))
				{
					if (!(value2 == "FitH"))
					{
						if (!(value2 == "FitV"))
						{
							if (!(value2 == "FitR"))
							{
								if (!(value2 == "FitBH"))
								{
									if (value2 == "FitBV")
									{
										destinationObject.Left = this.GetNumeric(array[2]).ToPdfReal();
									}
								}
								else
								{
									destinationObject.Top = this.GetNumeric(array[2]).ToPdfReal();
								}
							}
							else
							{
								destinationObject.Left = this.GetNumeric(array[2]).ToPdfReal();
								destinationObject.Bottom = this.GetNumeric(array[3]).ToPdfReal();
								destinationObject.Right = this.GetNumeric(array[4]).ToPdfReal();
								destinationObject.Top = this.GetNumeric(array[5]).ToPdfReal();
							}
						}
						else
						{
							destinationObject.Left = (PdfReal)array[2];
						}
					}
					else
					{
						destinationObject.Top = this.GetNumeric(array[2]).ToPdfReal();
					}
				}
				else
				{
					destinationObject.Left = this.GetNumeric(array[2]).ToPdfReal();
					destinationObject.Top = this.GetNumeric(array[3]).ToPdfReal();
					destinationObject.Zoom = this.GetNumeric(array[4]).ToPdfReal();
				}
			}
			return destinationObject;
		}

		double? GetNumeric(PdfPrimitive value)
		{
			if (value != null)
			{
				PdfInt pdfInt = value as PdfInt;
				if (pdfInt != null)
				{
					return new double?((double)pdfInt.Value);
				}
				PdfReal pdfReal = value as PdfReal;
				if (pdfReal != null)
				{
					return new double?(pdfReal.Value);
				}
			}
			return null;
		}
	}
}
