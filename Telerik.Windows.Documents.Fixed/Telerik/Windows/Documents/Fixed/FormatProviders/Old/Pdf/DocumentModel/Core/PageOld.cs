using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	[PdfClass]
	class PageOld : PageTreeNodeOld
	{
		public PageOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.contents = base.CreateLoadOnDemandProperty<ContentStreamOld>(new PdfPropertyDescriptor
			{
				Name = "Contents"
			}, Converters.ContentsConverter);
			this.annots = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Annots"
			});
		}

		public Rect Clip
		{
			get
			{
				return base.MediaBox.ToRect();
			}
		}

		public ContentStreamOld Contents
		{
			get
			{
				return this.contents.GetValue();
			}
			set
			{
				this.contents.SetValue(value);
			}
		}

		public PdfArrayOld Annots
		{
			get
			{
				return this.annots.GetValue();
			}
			set
			{
				this.annots.SetValue(value);
			}
		}

		public override string ToString()
		{
			if (base.Reference != null)
			{
				return base.Reference.ToString();
			}
			return base.ToString();
		}

		readonly LoadOnDemandProperty<ContentStreamOld> contents;

		readonly LoadOnDemandProperty<PdfArrayOld> annots;
	}
}
