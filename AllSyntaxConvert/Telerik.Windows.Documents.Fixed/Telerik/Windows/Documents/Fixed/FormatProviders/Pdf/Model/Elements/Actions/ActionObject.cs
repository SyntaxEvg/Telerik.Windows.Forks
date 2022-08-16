using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Actions
{
	abstract class ActionObject : PdfObject
	{
		public ActionObject()
		{
			this.name = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("S", true));
		}

		public ActionObject(string actionName)
			: this()
		{
			this.Name = new PdfName(actionName);
		}

		public PdfName Name
		{
			get
			{
				return this.name.GetValue();
			}
			set
			{
				this.name.SetValue(value);
			}
		}

		public static ActionObject CreateInstance(PdfName type)
		{
			Guard.ThrowExceptionIfNull<PdfName>(type, "type");
			string value;
			if ((value = type.Value) != null)
			{
				if (value == "GoTo")
				{
					return new GoToActionObject();
				}
				if (value == "URI")
				{
					return new UriActionObject();
				}
			}
			throw new NotSupportedException("Action type is not supported.");
		}

		public static ActionObject CreateAction(IPdfExportContext context, Telerik.Windows.Documents.Fixed.Model.Actions.Action action)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Actions.Action>(action, "action");
			switch (action.ActionType)
			{
			case ActionType.GoTo:
			{
				GoToAction action2 = action as GoToAction;
				GoToActionObject goToActionObject = new GoToActionObject();
				goToActionObject.CopyPropertiesFrom(context, action2);
				return goToActionObject;
			}
			case ActionType.Uri:
			{
				UriAction uriAction = action as UriAction;
				UriActionObject uriActionObject = new UriActionObject();
				uriActionObject.CopyPropertiesFrom(context, uriAction);
				return uriActionObject;
			}
			default:
				throw new NotSupportedException("The specified action is not supported.");
			}
		}

		public abstract Telerik.Windows.Documents.Fixed.Model.Actions.Action ToAction(PostScriptReader reader, IRadFixedDocumentImportContext context);

		readonly DirectProperty<PdfName> name;
	}
}
