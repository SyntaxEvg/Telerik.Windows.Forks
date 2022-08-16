using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	class SectionInfo
	{
		public SectionInfo(RadFixedDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocument>(document, "document");
			this.document = document;
			this.properties = new SectionProperties();
			this.spacingCalculator = new SpacingsCalculator(this);
		}

		public SectionProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public Size RemainingSize
		{
			get
			{
				return new Size(Math.Max(0.0, this.actualWidth), Math.Max(0.0, this.actualHeight - this.offsetY));
			}
		}

		public bool IsStarted
		{
			get
			{
				return this.editor != null;
			}
		}

		public void StartNewPage(SectionProperties properties)
		{
			Guard.ThrowExceptionIfNull<SectionProperties>(properties, "properties");
			this.properties.CopyPropertiesFrom(properties);
			this.StartNewPage();
		}

		public void StartNewPage()
		{
			RadFixedPage radFixedPage = this.document.Pages.AddPage();
			radFixedPage.Size = this.Properties.PageSize;
			radFixedPage.Rotation = this.Properties.PageRotation;
			this.editor = new FixedContentEditor(radFixedPage, SimplePosition.Default);
			this.UpdateAvailableSize();
		}

		public void UpdateAvailableSize()
		{
			this.actualWidth = this.properties.PageSize.Width - this.properties.PageMargins.Left - this.properties.PageMargins.Right;
			this.actualHeight = this.properties.PageSize.Height - this.properties.PageMargins.Bottom;
			this.offsetX = this.properties.PageMargins.Left;
			this.offsetY = this.properties.PageMargins.Top;
			this.spacingCalculator.Clear();
		}

		public void BeginExportContent()
		{
			this.UpdateAvailableSize();
			this.spacingCalculator.BeginExportContent();
		}

		public bool CanFit(IBlockElement blockElement)
		{
			return blockElement.Measure(RadFixedDocumentEditor.MinimalMeasureSize).Height <= this.RemainingSize.Height;
		}

		public void InsertBlockElement(IBlockElement blockElement)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(this.editor, "editor");
			Guard.ThrowExceptionIfNull<IBlockElement>(blockElement, "blockElement");
			Block block = blockElement as Block;
			double spacingBefore = 0.0;
			if (block != null)
			{
				spacingBefore = block.SpacingBefore;
				block.SpacingBefore = this.spacingCalculator.GetSpacingBefore(block);
				if (this.spacingCalculator.ShouldExpandBackground(block))
				{
					Path expandedBackground = this.spacingCalculator.GetExpandedBackground(this.offsetX, this.offsetY);
					this.editor.Draw(expandedBackground);
				}
			}
			blockElement.Draw(this.editor, new Rect(this.offsetX, this.offsetY, this.RemainingSize.Width, this.RemainingSize.Height));
			this.offsetY += blockElement.DesiredSize.Height;
			this.spacingCalculator.SetPreviousBlock(blockElement);
			if (block != null)
			{
				block.SpacingBefore = spacingBefore;
			}
		}

		public void InsertWatermark(TextWatermarkSettings settings)
		{
			Block block = new Block();
			block.TextProperties.Font = settings.Font;
			block.GraphicProperties.FillColor = settings.ForegroundColor;
			block.InsertText(settings.Text);
			block.Measure();
			this.DrawWatermark(settings, block, false);
		}

		public void InsertWatermark(ImageWatermarkSettings settings)
		{
			Block block = new Block();
			block.InsertImage(settings.ImageSource, new Size(settings.Width, settings.Height));
			this.DrawWatermark(settings, block, true);
		}

		public bool IsFirstPage()
		{
			return this.document.Pages.Count == 1;
		}

		void DrawWatermark(WatermarkSettingsBase settings, Block block, bool hasCorrectSize = false)
		{
			using (this.editor.SavePosition())
			{
				double val = (this.actualWidth - settings.Width) / 2.0 + this.properties.PageMargins.Left;
				double val2 = (this.actualHeight - this.properties.PageMargins.Top - settings.Height) / 2.0 + this.properties.PageMargins.Top;
				this.editor.Position.Translate(Math.Max(0.0, val), Math.Max(0.0, val2));
				this.editor.Position.RotateAt(settings.Angle, settings.Width / 2.0, settings.Height / 2.0);
				if (!hasCorrectSize)
				{
					this.editor.Position.Scale(settings.Width / block.DesiredSize.Width, settings.Height / block.DesiredSize.Height);
				}
				this.editor.DrawBlock(block);
			}
		}

		readonly RadFixedDocument document;

		readonly SectionProperties properties;

		readonly SpacingsCalculator spacingCalculator;

		FixedContentEditor editor;

		double actualWidth;

		double actualHeight;

		double offsetX;

		double offsetY;
	}
}
