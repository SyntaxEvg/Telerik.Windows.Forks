using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation
{
	class OutlinesOld : PdfObjectOld
	{
		public OutlinesOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.type = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "Type"
			});
			this.first = base.CreateLoadOnDemandProperty<OutlineItemOld>(new PdfPropertyDescriptor
			{
				Name = "First"
			});
			this.last = base.CreateLoadOnDemandProperty<OutlineItemOld>(new PdfPropertyDescriptor
			{
				Name = "Last"
			});
			this.count = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Count"
			});
		}

		public PdfNameOld Type
		{
			get
			{
				return this.type.GetValue();
			}
			set
			{
				this.type.SetValue(value);
			}
		}

		public OutlineItemOld First
		{
			get
			{
				return this.first.GetValue();
			}
			set
			{
				this.first.SetValue(value);
			}
		}

		public OutlineItemOld Last
		{
			get
			{
				return this.last.GetValue();
			}
			set
			{
				this.last.SetValue(value);
			}
		}

		public PdfIntOld Count
		{
			get
			{
				return this.count.GetValue();
			}
			set
			{
				this.count.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfNameOld> type;

		readonly LoadOnDemandProperty<OutlineItemOld> first;

		readonly LoadOnDemandProperty<OutlineItemOld> last;

		readonly InstantLoadProperty<PdfIntOld> count;
	}
}
