using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class DeviceNAttributes : PdfObjectOld
	{
		public DeviceNAttributes(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.subType = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor("Subtype"), new PdfNameOld(base.ContentManager, "DeviceN"));
			this.colorants = base.CreateInstantLoadProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("Colorants"));
			this.process = base.CreateInstantLoadProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("Process"));
			this.mixingHints = base.CreateInstantLoadProperty<PdfDictionaryOld>(new PdfPropertyDescriptor("MixingHints"));
		}

		public PdfNameOld SubType
		{
			get
			{
				return this.subType.GetValue();
			}
			set
			{
				this.subType.SetValue(value);
			}
		}

		public PdfDictionaryOld Colorants
		{
			get
			{
				return this.colorants.GetValue();
			}
			set
			{
				this.colorants.SetValue(value);
			}
		}

		public PdfDictionaryOld Process
		{
			get
			{
				return this.process.GetValue();
			}
			set
			{
				this.process.SetValue(value);
			}
		}

		public PdfDictionaryOld MixingHints
		{
			get
			{
				return this.mixingHints.GetValue();
			}
			set
			{
				this.mixingHints.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfNameOld> subType;

		readonly InstantLoadProperty<PdfDictionaryOld> colorants;

		readonly InstantLoadProperty<PdfDictionaryOld> process;

		readonly InstantLoadProperty<PdfDictionaryOld> mixingHints;
	}
}
