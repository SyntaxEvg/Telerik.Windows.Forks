using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Actions
{
	class GoToActionObject : ActionObject
	{
		public GoToActionObject()
			: base("GoTo")
		{
			this.destination = base.RegisterDirectProperty<DestinationObject>(new PdfPropertyDescriptor("D", true));
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

		public void CopyPropertiesFrom(IPdfExportContext context, GoToAction action)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<GoToAction>(action, "action");
			this.Destination = new DestinationObject();
			this.Destination.CopyPropertiesFrom(context, action.Destination);
		}

		public override Telerik.Windows.Documents.Fixed.Model.Actions.Action ToAction(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			GoToAction goToAction = new GoToAction();
			if (this.Destination != null)
			{
				goToAction.Destination = this.Destination.ToDestination(reader, context);
			}
			return goToAction;
		}

		readonly DirectProperty<DestinationObject> destination;
	}
}
