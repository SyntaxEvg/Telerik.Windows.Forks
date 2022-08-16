using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass(TypeName = "Annot", SubtypeProperty = "Subtype", SubtypeValue = "Link")]
	class LinkOld : AnnotationOld
	{
		public LinkOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.action = base.CreateLoadOnDemandProperty<ActionOld>(new PdfPropertyDescriptor
			{
				Name = "A"
			}, Converters.ActionConverter);
			this.destination = base.CreateLoadOnDemandProperty<DestinationOld>(new PdfPropertyDescriptor
			{
				Name = "Dest"
			}, Converters.DestinationConverter);
		}

		public override AnnotationType Type
		{
			get
			{
				return AnnotationType.Link;
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

		readonly LoadOnDemandProperty<ActionOld> action;

		readonly LoadOnDemandProperty<DestinationOld> destination;
	}
}
