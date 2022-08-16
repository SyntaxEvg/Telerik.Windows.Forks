using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata
{
	class DocumentInfo : MetadataStream
	{
		public override IEnumerable<DescriptionElement> Descriptions
		{
			get
			{
				return this.descriptions;
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			List<DescriptionElement> list = new List<DescriptionElement>();
			DocumentInfo.AddPdfComplianceLevelDescriptionElement(context, list);
			DocumentInfo.AddPdfDocumentInfoDescriptionElement(context, list);
			string value = DateTime.Now.ToXmpDate();
			list.Add(new DescriptionElement(new XmpDataElement[]
			{
				new XmpDataElement(XmpNamespaces.Xmp, "CreateDate", value),
				new XmpDataElement(XmpNamespaces.Xmp, "ModifyDate", value),
				new XmpDataElement(XmpNamespaces.Xmp, "CreatorTool", "Telerik PdfProcessing")
			}));
			this.descriptions = list;
		}

		static void AddPdfComplianceLevelDescriptionElement(IPdfExportContext context, List<DescriptionElement> descriptions)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<List<DescriptionElement>>(descriptions, "descriptions");
			int num;
			string value;
			switch (context.Settings.ComplianceLevel)
			{
			case PdfComplianceLevel.PdfA1B:
				num = 1;
				value = "B";
				break;
			case PdfComplianceLevel.PdfA2B:
				num = 2;
				value = "B";
				break;
			case PdfComplianceLevel.PdfA2U:
				num = 2;
				value = "U";
				break;
			case PdfComplianceLevel.PdfA3B:
				num = 3;
				value = "B";
				break;
			case PdfComplianceLevel.PdfA3U:
				num = 3;
				value = "U";
				break;
			default:
				return;
			}
			descriptions.Add(new DescriptionElement(new XmpDataElement[]
			{
				new XmpDataElement(XmpNamespaces.PdfAId, "part", num),
				new XmpDataElement(XmpNamespaces.PdfAId, "conformance", value)
			}));
		}

		static void AddPdfDocumentInfoDescriptionElement(IPdfExportContext context, List<DescriptionElement> descriptions)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<List<DescriptionElement>>(descriptions, "descriptions");
			List<XmpDataElement> list = new List<XmpDataElement>();
			if (!string.IsNullOrEmpty(context.DocumentInfo.Title))
			{
				list.Add(new XmpDataElement(XmpNamespaces.Dc, "title", new LanguageAlternativeElement(context.DocumentInfo.Title)));
			}
			if (!string.IsNullOrEmpty(context.DocumentInfo.Author))
			{
				list.Add(new XmpDataElement(XmpNamespaces.Dc, "creator", new SequenceElement(new string[] { context.DocumentInfo.Author })));
			}
			if (!string.IsNullOrEmpty(context.DocumentInfo.Description))
			{
				list.Add(new XmpDataElement(XmpNamespaces.Dc, "description", new LanguageAlternativeElement(context.DocumentInfo.Description)));
			}
			if (list.Count > 0)
			{
				descriptions.Add(new DescriptionElement(list.ToArray()));
			}
		}

		IEnumerable<DescriptionElement> descriptions;
	}
}
