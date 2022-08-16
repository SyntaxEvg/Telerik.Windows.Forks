using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;
using VerticalAlignment = Telerik.Windows.Documents.Flow.Model.Styles.VerticalAlignment;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class RtfDocumentExporter
	{
		public RtfDocumentExporter(RadFlowDocument document, Stream output, RtfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<RtfExportSettings>(settings, "settings");
			this.Settings = settings;
			this.document = document;
			this.Writer = new RtfWriter(output);
			this.CommentExporter = new CommentExporter(this);
			this.ImageExporter = new ImageExporter(this);
			this.ParagraphExporter = new ParagraphExporter(this);
			this.InlineExporter = new InlineExporter(this);
		}

		public ExportContext Context { get; set; }

		public RtfWriter Writer { get; set; }

		public RtfExportSettings Settings { get; set; }

		public ImageExporter ImageExporter { get; set; }

		public CommentExporter CommentExporter { get; set; }

		public ParagraphExporter ParagraphExporter { get; set; }

		public InlineExporter InlineExporter { get; set; }

		public void Export()
		{
			this.Context = new ExportContext(this.document);
			this.Context.InitializeContext();
			this.ExportDocument();
		}

		public void ExportBlock(BlockBase block)
		{
			if (block is Paragraph)
			{
				this.ParagraphExporter.ExportParagraph((Paragraph)block);
				return;
			}
			if (block is Table)
			{
				this.ExportTable((Table)block);
			}
		}

		void ExportDocument()
		{
			using (this.Writer.WriteGroup("rtf", false))
			{
				HeaderExporter headerExporter = new HeaderExporter(this.Context, this.Writer);
				headerExporter.ExportHeader();
				this.ExportDocumentProperties(this.document);
				foreach (Section section in this.document.Sections)
				{
					this.ExportSection(section);
				}
			}
			this.Writer.Flush();
		}

		void ExportDocumentProperties(RadFlowDocument doc)
		{
			this.Writer.WriteTag("nouicompat");
			this.Writer.WriteTag("viewkind", RtfHelper.DocumentViewTypeMapper.GetFromValue(doc.ViewType));
			if (doc.HasDifferentEvenOddPageHeadersFooters)
			{
				this.Writer.WriteTag("facingp");
			}
			this.Writer.WriteTag("deftab", Unit.DipToTwipI(doc.DefaultTabStopWidth));
		}

		void ExportSection(Section section)
		{
			bool flag = section == section.Parent.Children.First<DocumentElementBase>();
			if (!flag)
			{
				this.Writer.WriteTag("sect");
			}
			this.ExportSectionProperties(section);
			if (flag)
			{
				this.ExportDocumentVariables();
			}
			foreach (BlockBase block in section.Blocks)
			{
				this.ExportBlock(block);
			}
		}

		void ExportSectionProperties(Section section)
		{
			this.Writer.WriteTag("sectd");
			if (section != section.Document.Sections.First<Section>())
			{
				string fromValue = RtfHelper.SectionTypeMapper.GetFromValue(section.SectionType);
				this.Writer.WriteTag(fromValue);
			}
			if (section.PageOrientation == PageOrientation.Landscape || section.PageOrientation == PageOrientation.Rotate270)
			{
				this.Writer.WriteTag("lndscpsxn");
			}
			Size pageSize = section.PageSize;
			this.Writer.WriteTag("pgwsxn", Unit.DipToTwipI(pageSize.Width));
			this.Writer.WriteTag("pghsxn", Unit.DipToTwipI(pageSize.Height));
			Padding pageMargins = section.PageMargins;
			this.Writer.WriteTag("marglsxn", Unit.DipToTwipI(pageMargins.Left));
			this.Writer.WriteTag("margrsxn", Unit.DipToTwipI(pageMargins.Right));
			this.Writer.WriteTag("margtsxn", Unit.DipToTwipI(pageMargins.Top));
			this.Writer.WriteTag("margbsxn", Unit.DipToTwipI(pageMargins.Bottom));
			if (section.HasDifferentFirstPageHeaderFooter)
			{
				this.Writer.WriteTag("titlepg");
			}
			switch (section.VerticalAlignment)
			{
			case VerticalAlignment.Top:
				this.Writer.WriteTag("vertalt");
				break;
			case VerticalAlignment.Bottom:
				this.Writer.WriteTag("vertalb");
				break;
			case VerticalAlignment.Center:
				this.Writer.WriteTag("vertalc");
				break;
			case VerticalAlignment.Justified:
				this.Writer.WriteTag("vertalj");
				break;
			}
			PageNumberingSettings pageNumberingSettings = section.PageNumberingSettings;
			if (pageNumberingSettings.StartingPageNumber != null)
			{
				this.Writer.WriteTag("pgnrestart");
				this.Writer.WriteTag("pgnstarts", section.PageNumberingSettings.StartingPageNumber.Value);
			}
			string tagName;
			if (pageNumberingSettings.PageNumberFormat != null && RtfHelper.NumberingStyleMapper.TryGetFromValue(pageNumberingSettings.PageNumberFormat.Value, out tagName))
			{
				this.Writer.WriteTag(tagName);
			}
			string tagName2;
			if (pageNumberingSettings.ChapterSeparatorCharacter != null && RtfHelper.ChapterSeparatorTypeMapper.TryGetFromValue(pageNumberingSettings.ChapterSeparatorCharacter.Value, out tagName2))
			{
				this.Writer.WriteTag(tagName2);
			}
			this.Writer.WriteTag("headery", Unit.DipToTwipI(section.HeaderTopMargin));
			this.Writer.WriteTag("footery", Unit.DipToTwipI(section.FooterBottomMargin));
			this.ExportHeaderFooterGroup("headerf", section.Headers.First);
			this.ExportHeaderFooterGroup("headerr", section.Headers.Default);
			this.ExportHeaderFooterGroup("headerl", section.Headers.Even);
			this.ExportHeaderFooterGroup("footerf", section.Footers.First);
			this.ExportHeaderFooterGroup("footerr", section.Footers.Default);
			this.ExportHeaderFooterGroup("footerl", section.Footers.Even);
		}

		void ExportHeaderFooterGroup(string tagName, HeaderFooterBase headerFooterBase)
		{
			if (headerFooterBase != null)
			{
				using (this.Writer.WriteGroup(tagName, false))
				{
					Header header = headerFooterBase as Header;
					if (header != null)
					{
						foreach (object obj in header.Watermarks)
						{
							Watermark watermark = (Watermark)obj;
							WatermarkExporter watermarkExporter = new WatermarkExporter(this, watermark);
							watermarkExporter.ExportWatermark();
						}
					}
					foreach (BlockBase block in headerFooterBase.Blocks)
					{
						this.ExportBlock(block);
					}
				}
			}
		}

		void ExportDocumentVariables()
		{
			if (this.document.DocumentVariables.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this.document.DocumentVariables)
			{
				using (this.Writer.WriteGroup("docvar", true))
				{
					using (this.Writer.WriteGroup())
					{
						this.Writer.WriteText(keyValuePair.Key);
					}
					using (this.Writer.WriteGroup())
					{
						this.Writer.WriteText(keyValuePair.Value);
					}
				}
			}
		}

		void ExportTable(Table table)
		{
			TableExporter tableExporter = new TableExporter(this, table);
			tableExporter.ExportTable();
		}

		readonly RadFlowDocument document;
	}
}
