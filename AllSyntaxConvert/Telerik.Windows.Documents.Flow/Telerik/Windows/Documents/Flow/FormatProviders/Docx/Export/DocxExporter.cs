using System;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export
{
	class DocxExporter : OpenXmlExporter<DocxPartsManager, IDocxExportContext>
	{
		protected override DocxPartsManager CreatePartsManager()
		{
			return new DocxPartsManager();
		}

		protected override void InitializeParts(DocxPartsManager partsManager, IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<DocxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			base.InitializeParts(partsManager, context);
			ThemePart themePart = new ThemePart(partsManager, "/word/theme/theme1.xml");
			partsManager.RegisterPart(themePart);
			partsManager.CreateDocumentRelationship(themePart.Name, OpenXmlRelationshipTypes.ThemeRelationshipType, null);
			DocumentPart part = new DocumentPart(partsManager);
			DocxExporter.RegisterHeaderParts(partsManager, context);
			DocxExporter.RegisterFooterParts(partsManager, context);
			partsManager.RegisterPart(new DocumentSettingsPart(partsManager));
			partsManager.RegisterPart(new StylesPart(partsManager));
			if (context.CommentContext.HasCommentsToExport)
			{
				partsManager.RegisterPart(new CommentsPart(partsManager));
			}
			DocxExporter.RegisterListsPart(partsManager, context);
			partsManager.RegisterPart(part);
		}

		protected override string CreateResourceName(IResource resource)
		{
			return DocxHelper.CreateResourceName(resource);
		}

		static void RegisterHeaderParts(DocxPartsManager partsManager, IDocxExportContext context)
		{
			foreach (Header header in context.GetHeaders())
			{
				DocxExporter.RegisterHeaderPart(partsManager, context, header);
			}
		}

		static void RegisterFooterParts(DocxPartsManager partsManager, IDocxExportContext context)
		{
			foreach (Footer footer in context.GetFooters())
			{
				DocxExporter.RegisterFooterPart(partsManager, context, footer);
			}
		}

		static void RegisterHeaderPart(DocxPartsManager partsManager, IDocxExportContext context, Header header)
		{
			Guard.ThrowExceptionIfNull<DocxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Header>(header, "header");
			HeaderPart headerPart = new HeaderPart(partsManager, header, context.GetHeaderFooterPartNumberByHeaderFooter(header));
			context.RegisterHeaderFooter(headerPart.RelationshipId, header);
			partsManager.RegisterPart(headerPart);
		}

		static void RegisterFooterPart(DocxPartsManager partsManager, IDocxExportContext context, Footer footer)
		{
			Guard.ThrowExceptionIfNull<DocxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Footer>(footer, "footer");
			FooterPart footerPart = new FooterPart(partsManager, footer, context.GetHeaderFooterPartNumberByHeaderFooter(footer));
			context.RegisterHeaderFooter(footerPart.RelationshipId, footer);
			partsManager.RegisterPart(footerPart);
		}

		static void RegisterListsPart(DocxPartsManager partsManager, IDocxExportContext context)
		{
			if (context.Document.Lists.Count > 0)
			{
				partsManager.RegisterPart(new ListsPart(partsManager));
			}
		}
	}
}
