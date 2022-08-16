using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	class XlsxAppendWorkbookExporter : XlsxWorkbookExporterBase
	{
		public XlsxAppendWorkbookExporter(Stream source, Stream target)
			: base(target)
		{
			this.sourceArchive = new ZipArchive(source, ZipArchiveMode.Read, true, null);
			this.ReadContentTypesPart();
			this.ReadWorkbookRelationshipsPart();
			this.ReadStylesPart();
			this.ReadWorkbookPart();
			string[] source2 = new string[]
			{
				PartPaths.ContentTypesPath,
				PartPaths.StylesPartPath,
				PartPaths.WorkbookPartPath,
				PartPaths.WorkbookRelationshipsPartPath
			};
			foreach (ZipArchiveEntry zipArchiveEntry in this.sourceArchive.Entries)
			{
				if (!source2.Contains(zipArchiveEntry.FullName))
				{
					using (Stream stream = zipArchiveEntry.Open())
					{
						using (ZipArchiveEntry zipArchiveEntry2 = base.TargetArchive.CreateEntry(zipArchiveEntry.FullName))
						{
							using (Stream stream2 = zipArchiveEntry2.Open())
							{
								stream.CopyTo(stream2);
							}
						}
					}
				}
			}
		}

		public override bool IsAppender
		{
			get
			{
				return true;
			}
		}

		protected override StylesRepository InitializeStylesRepository()
		{
			this.importedStyles = new StylesRepository();
			return new StylesRepository();
		}

		protected override void WriteWorkbookPart()
		{
			using (Stream stream = new MemoryStream())
			{
				OpenXmlWriter openXmlWriter = new OpenXmlWriter(stream);
				ElementContext context = new ElementContext(openXmlWriter, null, base.Theme);
				IEnumerable<Relationship> relationships = base.WorkbookRelationships.GetRelationships(XlsxRelationshipTypes.WorksheetRelationshipType);
				IEnumerable<Relationship> relationshipsDifference = XlsxAppendWorkbookExporter.GetRelationshipsDifference(this.importedRelationships, relationships);
				WorkbookElementWriter workbookElementWriter = new WorkbookElementWriter();
				workbookElementWriter.SetContext(context);
				workbookElementWriter.Write(relationshipsDifference.ToList<Relationship>());
				openXmlWriter.Flush();
				stream.Seek(0L, SeekOrigin.Begin);
				XmlDocument targetDocument = this.workbookDocument;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(stream);
				XlsxAppendWorkbookExporter.MergeDocuments(targetDocument, xmlDocument, false);
			}
			using (ZipArchiveEntry zipArchiveEntry = base.TargetArchive.CreateEntry(PartPaths.WorkbookPartPath))
			{
				using (Stream stream2 = zipArchiveEntry.Open())
				{
					this.workbookDocument.Save(stream2);
				}
			}
		}

		protected override void WriteStylesPart()
		{
			using (Stream stream = new MemoryStream())
			{
				OpenXmlWriter openXmlWriter = new OpenXmlWriter(stream);
				ElementContext context = new ElementContext(openXmlWriter, null, base.Theme);
				StylesRepository stylesRepositoryDifference = XlsxAppendWorkbookExporter.GetStylesRepositoryDifference(this.importedStyles, base.StylesRepository);
				StyleSheetElement styleSheetElement = new StyleSheetElement();
				styleSheetElement.SetContext(context);
				styleSheetElement.Write(stylesRepositoryDifference);
				openXmlWriter.Flush();
				stream.Seek(0L, SeekOrigin.Begin);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(stream);
				XmlDocument targetDocument = this.stylesDocument;
				XlsxAppendWorkbookExporter.MergeDocuments(targetDocument, xmlDocument, false);
			}
			using (ZipArchiveEntry zipArchiveEntry = base.TargetArchive.CreateEntry(PartPaths.StylesPartPath))
			{
				using (Stream stream2 = zipArchiveEntry.Open())
				{
					this.stylesDocument.Save(stream2);
				}
			}
		}

		static void MergeDocuments(XmlDocument targetDocument, XmlDocument sourceDocument, bool shouldUpdateChildrenCount)
		{
			foreach (object obj in targetDocument.DocumentElement.ChildNodes)
			{
				XmlElement xmlElement = obj as XmlElement;
				if (xmlElement != null)
				{
					foreach (XmlElement xmlElement2 in sourceDocument.GetElementsByTagName(xmlElement.LocalName).Cast<XmlElement>())
					{
						foreach (XmlElement node in xmlElement2.ChildNodes.Cast<XmlElement>())
						{
							XmlNode xmlNode = targetDocument.ImportNode(node, true);
							xmlNode.Prefix = xmlElement.Prefix;
							xmlElement.AppendChild(xmlNode);
							if (shouldUpdateChildrenCount)
							{
								xmlElement.SetAttribute("count", xmlElement.ChildNodes.Count.ToString());
							}
						}
					}
				}
			}
		}

		static void CopyStyles(StylesRepository source, StylesRepository target)
		{
			foreach (SpreadCellBorders borders in source.Borders)
			{
				target.AddBorders(borders);
			}
			foreach (DiferentialFormat format in source.CellFormats)
			{
				target.AddCellFormat(format);
			}
			foreach (CellStyleInfo style in source.CellStyles)
			{
				target.AddCellStyle(style);
			}
			foreach (DiferentialFormat format2 in source.CellStyleFormats)
			{
				target.AddCellStyleFormat(format2);
			}
			foreach (ISpreadFill fill in source.Fills)
			{
				target.AddFill(fill);
			}
			foreach (FontProperties font in source.Fonts)
			{
				target.AddFont(font);
			}
			foreach (NumberFormat format3 in source.NumberFormats)
			{
				target.AddNumberFormat(format3);
			}
		}

		static StylesRepository GetStylesRepositoryDifference(StylesRepository initial, StylesRepository updated)
		{
			StylesRepository stylesRepository = new StylesRepository();
			foreach (SpreadCellBorders spreadCellBorders in updated.Borders)
			{
				if (!initial.Borders.Contains(spreadCellBorders))
				{
					stylesRepository.AddBorders(spreadCellBorders);
				}
			}
			foreach (DiferentialFormat diferentialFormat in updated.CellFormats)
			{
				if (!initial.CellFormats.Contains(diferentialFormat))
				{
					stylesRepository.AddCellFormat(diferentialFormat);
				}
			}
			foreach (CellStyleInfo cellStyleInfo in updated.CellStyles)
			{
				if (!initial.CellStyles.Contains(cellStyleInfo))
				{
					stylesRepository.AddCellStyle(cellStyleInfo);
				}
			}
			foreach (DiferentialFormat diferentialFormat2 in updated.CellStyleFormats)
			{
				if (!initial.CellStyleFormats.Contains(diferentialFormat2))
				{
					stylesRepository.AddCellStyleFormat(diferentialFormat2);
				}
			}
			foreach (ISpreadFill spreadFill in updated.Fills)
			{
				if (!initial.Fills.Contains(spreadFill))
				{
					stylesRepository.AddFill(spreadFill);
				}
			}
			foreach (FontProperties fontProperties in updated.Fonts)
			{
				if (!initial.Fonts.Contains(fontProperties))
				{
					stylesRepository.AddFont(fontProperties);
				}
			}
			foreach (NumberFormat numberFormat in updated.NumberFormats)
			{
				if (!initial.NumberFormats.Contains(numberFormat))
				{
					stylesRepository.AddNumberFormat(numberFormat);
				}
			}
			return stylesRepository;
		}

		static IEnumerable<Relationship> GetRelationshipsDifference(List<Relationship> initial, IEnumerable<Relationship> updated)
		{
			using (IEnumerator<Relationship> enumerator = updated.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Relationship item = enumerator.Current;
					if (!(from p in initial
						where p.RelationshipId == item.RelationshipId
						select p).Any<Relationship>())
					{
						yield return item;
					}
				}
			}
			yield break;
		}

		void ReadContentTypesPart()
		{
			using (ContentTypesPart contentTypesPart = new ContentTypesPart(new PartContext(this.sourceArchive)))
			{
				contentTypesPart.Read(base.ContentTypes);
			}
		}

		void ReadWorkbookRelationshipsPart()
		{
			using (WorkbookRelationshipsPart workbookRelationshipsPart = new WorkbookRelationshipsPart(new PartContext(this.sourceArchive)))
			{
				workbookRelationshipsPart.ReadRelationships(base.WorkbookRelationships);
			}
		}

		void ReadStylesPart()
		{
			using (StylesPart stylesPart = new StylesPart(new PartContext(this.sourceArchive, base.Theme)))
			{
				stylesPart.Read(base.StylesRepository);
			}
			XlsxAppendWorkbookExporter.CopyStyles(base.StylesRepository, this.importedStyles);
			this.stylesDocument = new XmlDocument();
			using (ZipArchiveEntry zipArchiveEntry = (from p in this.sourceArchive.Entries
				where p.FullName == PartPaths.StylesPartPath
				select p).FirstOrDefault<ZipArchiveEntry>())
			{
				using (Stream stream = zipArchiveEntry.Open())
				{
					this.stylesDocument.Load(stream);
				}
				foreach (object obj in this.stylesDocument.DocumentElement.ChildNodes)
				{
					XmlElement xmlElement = obj as XmlElement;
					if (xmlElement != null)
					{
						this.ReadStylesPartElement(xmlElement);
					}
				}
			}
		}

		void ReadStylesPartElement(XmlElement element)
		{
			int count = element.ChildNodes.Count;
			string localName;
			switch (localName = element.LocalName)
			{
			case "numFmts":
			{
				int num2 = count;
				foreach (XmlElement xmlElement in element.ChildNodes.Cast<XmlElement>())
				{
					string attribute = xmlElement.GetAttribute("numFmtId");
					if (!string.IsNullOrEmpty(attribute))
					{
						num2 = Math.Max(num2, int.Parse(attribute) + 1);
					}
				}
				base.StylesRepository.NumberFormatsStartIndex = num2;
				return;
			}
			case "fonts":
				base.StylesRepository.FontsStartIndex = count;
				return;
			case "fills":
				base.StylesRepository.FillsStartIndex = count;
				return;
			case "borders":
				base.StylesRepository.BordersStartIndex = count;
				return;
			case "cellStyles":
				base.StylesRepository.CellStylesStartIndex = count;
				return;
			case "cellStyleXfs":
				base.StylesRepository.CellStyleFormatsStartIndex = count;
				return;
			case "cellXfs":
				base.StylesRepository.CellFormatsStartIndex = count;
				break;

				return;
			}
		}

		void ReadWorkbookPart()
		{
			List<Relationship> list = new List<Relationship>();
			using (WorkbookPart workbookPart = new WorkbookPart(new PartContext(this.sourceArchive)))
			{
				workbookPart.Read(list);
			}
			foreach (Relationship relationship in list)
			{
				base.WorkbookRelationships.AddRelationship(relationship);
			}
			this.importedRelationships = list;
			this.workbookDocument = new XmlDocument();
			using (ZipArchiveEntry zipArchiveEntry = (from p in this.sourceArchive.Entries
				where p.FullName == PartPaths.WorkbookPartPath
				select p).FirstOrDefault<ZipArchiveEntry>())
			{
				using (Stream stream = zipArchiveEntry.Open())
				{
					this.workbookDocument.Load(stream);
				}
			}
		}

		readonly ZipArchive sourceArchive;

		XmlDocument stylesDocument;

		XmlDocument workbookDocument;

		StylesRepository importedStyles;

		List<Relationship> importedRelationships;
	}
}
