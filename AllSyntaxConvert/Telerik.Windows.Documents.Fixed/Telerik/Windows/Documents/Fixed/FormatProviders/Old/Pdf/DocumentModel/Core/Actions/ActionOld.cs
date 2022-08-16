using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions
{
	[PdfClass]
	class ActionOld : PdfObjectOld
	{
		public ActionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.type = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "S",
				IsRequired = true
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

		public static ActionOld CreateAction(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dict, "dict");
			PdfNameOld element = dict.GetElement<PdfNameOld>("S");
			string a;
			if (element != null && (a = element.ToString()) != null)
			{
				if (a == "URI")
				{
					ActionOld actionOld = new UriActionOld(contentManager);
					actionOld.Load(dict);
					return actionOld;
				}
				if (a == "GoTo")
				{
					ActionOld actionOld = new GoToActionOld(contentManager);
					actionOld.Load(dict);
					return actionOld;
				}
			}
			return null;
		}

		const string Uri = "URI";

		const string GoTo = "GoTo";

		readonly InstantLoadProperty<PdfNameOld> type;
	}
}
