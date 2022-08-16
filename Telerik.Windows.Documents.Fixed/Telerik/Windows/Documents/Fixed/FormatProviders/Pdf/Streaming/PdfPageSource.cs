using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	public class PdfPageSource : IFixedPage
	{
		internal PdfPageSource(PdfFileSource fileSource, IndirectReference reference, Page page)
		{
			this.fileSource = fileSource;
			this.reference = reference;
			this.page = page;
			this.mediaBox = page.ConvertToTopLeftBox(page.MediaBox, fileSource.Context.Reader, fileSource.Context);
			this.cropBox = page.ConvertToTopLeftBox(page.CropBox, fileSource.Context.Reader, fileSource.Context);
		}

		public Rotation Rotation
		{
			get
			{
				return this.page.Rotation;
			}
		}

		public Size Size
		{
			get
			{
				return new Size(this.MediaBox.Width, this.MediaBox.Height);
			}
		}

		public Rect MediaBox
		{
			get
			{
				return this.mediaBox;
			}
		}

		public Rect CropBox
		{
			get
			{
				return this.cropBox;
			}
		}

		internal PdfFileSource FileSource
		{
			get
			{
				return this.fileSource;
			}
		}

		internal Page Page
		{
			get
			{
				return this.page;
			}
		}

		internal IndirectReference PageReference
		{
			get
			{
				return this.reference;
			}
		}

		internal PdfDictionary PageDictionary
		{
			get
			{
				if (this.pageDictionary == null)
				{
					this.pageDictionary = this.ReadPageDictionary();
				}
				return this.pageDictionary;
			}
		}

		PdfDictionary ReadPageDictionary()
		{
			IndirectObject indirectObject = this.FileSource.Context.ReadIndirectObject(this.reference);
			return (PdfDictionary)indirectObject.Content;
		}

		readonly PdfFileSource fileSource;

		readonly IndirectReference reference;

		readonly Page page;

		readonly Rect mediaBox;

		readonly Rect cropBox;

		PdfDictionary pageDictionary;
	}
}
