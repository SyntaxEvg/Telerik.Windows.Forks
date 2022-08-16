using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation
{
	class OutlineItemOld : PdfObjectOld
	{
		public OutlineItemOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.title = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "Title"
			});
			this.parent = base.CreateLoadOnDemandProperty<OutlineItemOld>(new PdfPropertyDescriptor
			{
				Name = "Parent"
			});
			this.prev = base.CreateLoadOnDemandProperty<OutlineItemOld>(new PdfPropertyDescriptor
			{
				Name = "Prev"
			});
			this.next = base.CreateLoadOnDemandProperty<OutlineItemOld>(new PdfPropertyDescriptor
			{
				Name = "Next"
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
			this.destination = base.CreateLoadOnDemandProperty<DestinationOld>(new PdfPropertyDescriptor
			{
				Name = "Dest"
			}, Converters.DestinationConverter);
			this.action = base.CreateLoadOnDemandProperty<ActionOld>(new PdfPropertyDescriptor
			{
				Name = "A"
			}, Converters.ActionConverter);
			this.flag = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "F"
			});
		}

		public PdfStringOld Title
		{
			get
			{
				return this.title.GetValue();
			}
			set
			{
				this.title.SetValue(value);
			}
		}

		public OutlineItemOld Parent
		{
			get
			{
				return this.parent.GetValue();
			}
			set
			{
				this.parent.SetValue(value);
			}
		}

		public OutlineItemOld Prev
		{
			get
			{
				return this.prev.GetValue();
			}
			set
			{
				this.prev.SetValue(value);
			}
		}

		public OutlineItemOld Next
		{
			get
			{
				return this.next.GetValue();
			}
			set
			{
				this.next.SetValue(value);
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

		public DestinationOld Destination
		{
			get
			{
				return this.destination.GetValue();
			}
			set
			{
				this.destination.SetValue(value);
			}
		}

		public ActionOld Action
		{
			get
			{
				return this.action.GetValue();
			}
			set
			{
				this.action.SetValue(value);
			}
		}

		public PdfIntOld Flag
		{
			get
			{
				return this.flag.GetValue();
			}
			set
			{
				this.flag.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfStringOld> title;

		readonly LoadOnDemandProperty<OutlineItemOld> parent;

		readonly LoadOnDemandProperty<OutlineItemOld> prev;

		readonly LoadOnDemandProperty<OutlineItemOld> next;

		readonly LoadOnDemandProperty<OutlineItemOld> first;

		readonly LoadOnDemandProperty<OutlineItemOld> last;

		readonly InstantLoadProperty<PdfIntOld> count;

		readonly LoadOnDemandProperty<DestinationOld> destination;

		readonly LoadOnDemandProperty<ActionOld> action;

		readonly InstantLoadProperty<PdfIntOld> flag;
	}
}
