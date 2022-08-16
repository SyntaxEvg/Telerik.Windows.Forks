using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions
{
	class GoToActionOld : ActionOld
	{
		public GoToActionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.destination = base.CreateInstantLoadProperty<DestinationOld>(new PdfPropertyDescriptor
			{
				Name = "D",
				IsRequired = true
			}, Converters.DestinationConverter);
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

		readonly InstantLoadProperty<DestinationOld> destination;
	}
}
