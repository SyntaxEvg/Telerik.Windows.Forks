using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Actions
{
	class UriActionObject : ActionObject
	{
		public UriActionObject()
			: base("URI")
		{
			this.uri = base.RegisterDirectProperty<PdfLiteralString>(new PdfPropertyDescriptor("URI", true));
			this.isMap = base.RegisterDirectProperty<PdfBool>(new PdfPropertyDescriptor("IsMap"), false.ToPdfBool());
		}

		public PdfLiteralString Uri
		{
			get
			{
				return this.uri.GetValue();
			}
			set
			{
				this.uri.SetValue(value);
			}
		}

		public PdfBool IsMap
		{
			get
			{
				return this.isMap.GetValue();
			}
			set
			{
				this.isMap.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, UriAction uriAction)
		{
			this.Uri = uriAction.Uri.ToString().ToPdfLiteralString(StringType.ASCII);
			if (uriAction.IncludeMouseCoordinates != null)
			{
				this.IsMap = uriAction.IncludeMouseCoordinates.Value.ToPdfBool();
			}
		}

		public override Telerik.Windows.Documents.Fixed.Model.Actions.Action ToAction(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			UriAction uriAction = new UriAction();
			if (this.IsMap != null)
			{
				uriAction.IncludeMouseCoordinates = new bool?(this.IsMap.Value);
			}
			if (this.Uri != null)
			{
				string uriString = this.Uri.ToStringAs(StringType.ASCII);
				uriAction.Uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
			}
			return uriAction;
		}

		readonly DirectProperty<PdfLiteralString> uri;

		readonly DirectProperty<PdfBool> isMap;
	}
}
