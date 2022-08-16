using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	[PdfClass(TypeName = "Catalog")]
	class DocumentCatalogOld : PdfObjectOld
	{
		public DocumentCatalogOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.pages = base.CreateLoadOnDemandProperty<PageTreeNodeOld>(new PdfPropertyDescriptor
			{
				Name = "Pages",
				IsRequired = true,
				State = PdfPropertyState.MustBeIndirectReference
			});
			this.names = base.CreateInstantLoadProperty<Names>(new PdfPropertyDescriptor
			{
				Name = "Names"
			});
			this.dests = base.CreateLoadOnDemandProperty<PdfDictionaryOld>(new PdfPropertyDescriptor
			{
				Name = "Dests",
				State = PdfPropertyState.MustBeIndirectReference
			});
			this.acroForm = base.CreateLoadOnDemandProperty<AcroFormOld>(new PdfPropertyDescriptor
			{
				Name = "AcroForm"
			});
			this.outlines = base.CreateLoadOnDemandProperty<OutlinesOld>(new PdfPropertyDescriptor
			{
				Name = "Outlines"
			});
			this.pageMode = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "PageMode"
			});
		}

		public PdfDictionaryOld Dests
		{
			get
			{
				return this.dests.GetValue();
			}
			set
			{
				this.dests.SetValue(value);
			}
		}

		public Names Names
		{
			get
			{
				return this.names.GetValue();
			}
			set
			{
				this.names.SetValue(value);
			}
		}

		public PageTreeNodeOld Pages
		{
			get
			{
				return this.pages.GetValue();
			}
			set
			{
				this.pages.SetValue(value);
			}
		}

		public AcroFormOld AcroForm
		{
			get
			{
				return this.acroForm.GetValue();
			}
			set
			{
				this.acroForm.SetValue(value);
			}
		}

		public DestinationOld GetDestination(PdfStringOld name)
		{
			if (this.Names != null && this.Names.Dests != null)
			{
				return this.Names.Dests.FindElement<DestinationOld>(name.ToString(), Converters.DestinationConverter);
			}
			return null;
		}

		public DestinationOld GetDestination(PdfNameOld name)
		{
			if (this.Dests != null)
			{
				return this.Dests.GetElement<DestinationOld>(name.Value, Converters.DestinationConverter);
			}
			return null;
		}

		public OutlinesOld Outlines
		{
			get
			{
				return this.outlines.GetValue();
			}
			set
			{
				this.outlines.SetValue(value);
			}
		}

		public PdfNameOld PageMode
		{
			get
			{
				return this.pageMode.GetValue();
			}
			set
			{
				this.pageMode.SetValue(value);
			}
		}

		readonly LoadOnDemandProperty<PageTreeNodeOld> pages;

		readonly LoadOnDemandProperty<PdfDictionaryOld> dests;

		readonly LoadOnDemandProperty<AcroFormOld> acroForm;

		readonly InstantLoadProperty<Names> names;

		readonly LoadOnDemandProperty<OutlinesOld> outlines;

		readonly InstantLoadProperty<PdfNameOld> pageMode;
	}
}
