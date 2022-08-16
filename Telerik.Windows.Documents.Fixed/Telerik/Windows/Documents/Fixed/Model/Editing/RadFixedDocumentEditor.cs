using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class RadFixedDocumentEditor : IDisposable
	{
		public RadFixedDocumentEditor(RadFixedDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocument>(document, "document");
			this.document = document;
			this.section = new SectionInfo(this.document);
			this.paragraph = new Block();
			this.lists = new ListCollection();
			this.sectionProperties = new SectionProperties();
			this.paragraphProperties = new Telerik.Windows.Documents.Fixed.Model.Editing.Flow.ParagraphProperties();
			this.characterProperties = new CharacterProperties();
			this.isParagraphStarted = false;
		}

		internal event EventHandler<RadFixedPageCreatedEventArgs> PageCreated;

		public RadFixedDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public ListCollection Lists
		{
			get
			{
				return this.lists;
			}
		}

		public SectionProperties SectionProperties
		{
			get
			{
				return this.sectionProperties;
			}
		}

		public Telerik.Windows.Documents.Fixed.Model.Editing.Flow.ParagraphProperties ParagraphProperties
		{
			get
			{
				return this.paragraphProperties;
			}
		}

		public CharacterProperties CharacterProperties
		{
			get
			{
				return this.characterProperties;
			}
		}

		public void InsertParagraph()
		{
			this.AssureSectionExists();
			this.EndParagraph();
			this.StartParagraph();
		}

		public void InsertRun(string text)
		{
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertText(text);
		}

		public void InsertRun(FontFamily fontFamily, string text)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			this.CharacterProperties.TrySetFont(fontFamily);
			this.InsertRun(text);
		}

		public void InsertRun(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, string text)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			Guard.ThrowExceptionIfNull<FontStyle>(fontStyle, "fontStyle");
			Guard.ThrowExceptionIfNull<FontWeight>(fontWeight, "fontWeight");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			this.CharacterProperties.TrySetFont(fontFamily, fontStyle, fontWeight);
			this.InsertRun(text);
		}

		public void InsertLine(string text)
		{
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			this.InsertRun(text);
			this.InsertLineBreak();
		}

		public void InsertLineBreak()
		{
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertLineBreak();
		}

		public void InsertImageInline(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource, Size size)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(imageSource, "imageSource");
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertImage(imageSource, size);
		}

		public void InsertImageInline(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(imageSource, "imageSource");
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertImage(imageSource);
		}

		public void InsertFormInline(FormSource formSource)
		{
			Guard.ThrowExceptionIfNull<FormSource>(formSource, "formSource");
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertForm(formSource);
		}

		public void InsertFormInline(FormSource formSource, Size size)
		{
			Guard.ThrowExceptionIfNull<FormSource>(formSource, "formSource");
			this.AssureParagraphExists();
			this.CharacterProperties.CopyTo(this.paragraph);
			this.paragraph.InsertForm(formSource, size);
		}

		public void InsertSectionBreak()
		{
			this.StartNewPage(this.SectionProperties);
		}

		public void InsertPageBreak()
		{
			this.AssureSectionExists();
			this.StartNewPage();
		}

		public void InsertTable(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			this.InsertBlock(table);
		}

		public void InsertBlock(IBlockElement block)
		{
			Guard.ThrowExceptionIfNull<IBlockElement>(block, "block");
			this.AssureSectionExists();
			this.EndParagraph();
			if (!this.section.CanFit(block))
			{
				this.StartNewPage();
			}
			this.section.InsertBlockElement(block);
			while (block.HasPendingContent)
			{
				block = block.Split();
				this.StartNewPage();
				this.section.InsertBlockElement(block);
			}
		}

		void StartNewPage()
		{
			this.StartNewPage(this.section.Properties);
		}

		void StartNewPage(SectionProperties sectionProperties)
		{
			this.EndParagraph();
			this.section.StartNewPage(sectionProperties);
			this.OnPageCreated(this.section);
			this.section.BeginExportContent();
		}

		public void Dispose()
		{
			this.EndParagraph();
		}

		void AssureSectionExists()
		{
			if (!this.section.IsStarted)
			{
				this.InsertSectionBreak();
			}
		}

		void AssureParagraphExists()
		{
			this.AssureSectionExists();
			this.StartParagraph();
		}

		void StartParagraph()
		{
			if (!this.isParagraphStarted)
			{
				this.ParagraphProperties.CopyTo(this.paragraph);
				if (this.ParagraphProperties.ListId != null)
				{
					List list;
					if (!this.Lists.TryGetList(this.ParagraphProperties.ListId.Value, out list))
					{
						throw new ArgumentException(string.Format("There is no list with id {0}", this.ParagraphProperties.ListId.Value));
					}
					this.paragraph.SetBullet(list, this.ParagraphProperties.ListLevel);
				}
				this.isParagraphStarted = true;
			}
		}

		void EndParagraph()
		{
			if (this.isParagraphStarted)
			{
				this.isParagraphStarted = false;
				this.InsertBlock(this.paragraph);
				this.paragraph.Clear();
			}
		}

		void OnPageCreated(SectionInfo section)
		{
			if (this.PageCreated != null)
			{
				this.PageCreated(this, new RadFixedPageCreatedEventArgs(section));
			}
		}

		internal static readonly Size MinimalMeasureSize = new Size(0.0, 0.0);

		internal static readonly Size MinimalWidthMeasureSize = new Size(0.0, double.PositiveInfinity);

		readonly RadFixedDocument document;

		readonly ListCollection lists;

		readonly SectionInfo section;

		readonly Block paragraph;

		readonly SectionProperties sectionProperties;

		readonly Telerik.Windows.Documents.Fixed.Model.Editing.Flow.ParagraphProperties paragraphProperties;

		readonly CharacterProperties characterProperties;

		bool isParagraphStarted;
	}
}
