using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions
{
	class UriActionOld : ActionOld
	{
		public UriActionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.uri = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "URI",
				IsRequired = true
			});
			this.isMap = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "IsMap",
				IsRequired = false
			});
		}

		public PdfStringOld Uri
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

		public PdfBoolOld IsMap
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

		readonly InstantLoadProperty<PdfStringOld> uri;

		readonly InstantLoadProperty<PdfBoolOld> isMap;
	}
}
