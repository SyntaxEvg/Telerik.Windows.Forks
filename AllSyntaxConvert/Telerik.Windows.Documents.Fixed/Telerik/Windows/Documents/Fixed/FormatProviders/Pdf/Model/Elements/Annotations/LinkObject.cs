using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Actions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class LinkObject : AnnotationObject
	{
		public LinkObject()
		{
			this.destination = base.RegisterDirectProperty<DestinationObject>(new PdfPropertyDescriptor("Dest"));
			this.action = base.RegisterDirectProperty<ActionObject>(new PdfPropertyDescriptor("A"));
		}

		public DestinationObject Destination
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

		public ActionObject Action
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

		public void CopyPropertiesFrom(IPdfExportContext context, Link annotation, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Link>(annotation, "annotation");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			base.CopyPropertiesFrom(context, annotation, fixedPage);
			if (annotation.Destination != null)
			{
				this.Destination = new DestinationObject();
				this.Destination.CopyPropertiesFrom(context, annotation.Destination);
			}
			if (annotation.Action != null)
			{
				this.Action = ActionObject.CreateAction(context, annotation.Action);
			}
		}

		public override Annotation ToAnnotationOverride(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			Link link = new Link();
			if (this.Action != null)
			{
				link.Action = this.Action.ToAction(reader, context);
			}
			else if (this.Destination != null)
			{
				link.Destination = this.Destination.ToDestination(reader, context);
			}
			return link;
		}

		readonly DirectProperty<DestinationObject> destination;

		readonly DirectProperty<ActionObject> action;
	}
}
