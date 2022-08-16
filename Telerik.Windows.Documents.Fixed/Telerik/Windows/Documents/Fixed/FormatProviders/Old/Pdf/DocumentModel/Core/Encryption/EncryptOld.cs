using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption
{
	abstract class EncryptOld : PdfObjectOld
	{
		public EncryptOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.filter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "Filter",
				IsRequired = true
			});
			this.subFilter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "SubFilter"
			});
			this.algorithm = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "V"
			}, new PdfRealOld(contentManager, 0.0), Converters.PdfRealConverter);
			this.length = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Length"
			}, new PdfIntOld(contentManager, 40));
			this.cryptFilters = base.CreateLoadOnDemandProperty<CryptFiltersOld>(new PdfPropertyDescriptor
			{
				Name = "CF"
			});
			this.streamsFilter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "StmF"
			}, new PdfNameOld(contentManager, "Identity"));
			this.stringsFilter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "StrF"
			}, new PdfNameOld(contentManager, "Identity"));
		}

		public PdfNameOld Filter
		{
			get
			{
				return this.filter.GetValue();
			}
			set
			{
				this.filter.SetValue(value);
			}
		}

		public PdfNameOld SubFilter
		{
			get
			{
				return this.subFilter.GetValue();
			}
			set
			{
				this.subFilter.SetValue(value);
			}
		}

		public PdfRealOld Algorithm
		{
			get
			{
				return this.algorithm.GetValue();
			}
			set
			{
				this.algorithm.SetValue(value);
			}
		}

		public PdfIntOld Length
		{
			get
			{
				return this.length.GetValue();
			}
			set
			{
				this.length.SetValue(value);
			}
		}

		public CryptFiltersOld CryptFilters
		{
			get
			{
				return this.cryptFilters.GetValue();
			}
			set
			{
				this.cryptFilters.SetValue(value);
			}
		}

		public PdfNameOld StreamsFilter
		{
			get
			{
				return this.streamsFilter.GetValue();
			}
			set
			{
				this.streamsFilter.SetValue(value);
			}
		}

		public PdfNameOld StringsFilter
		{
			get
			{
				return this.stringsFilter.GetValue();
			}
			set
			{
				this.stringsFilter.SetValue(value);
			}
		}

		public static EncryptOld CreateEncrypt(PdfContentManager contentManager, PdfDictionaryOld dict)
		{
			string value;
			if ((value = dict.GetElement<PdfNameOld>("Filter").Value) != null && value == "Standard")
			{
				StandardEncryptionOld standardEncryptionOld = new StandardEncryptionOld(contentManager);
				standardEncryptionOld.Load(dict);
				return standardEncryptionOld;
			}
			return null;
		}

		public abstract void Initialize(PdfArrayOld id, string password);

		public abstract byte[] DecryptStream(int objectNo, int generationNo, byte[] data);

		public abstract byte[] DecryptString(int objectNo, int generationNo, byte[] data);

		const string Standard = "Standard";

		readonly InstantLoadProperty<PdfNameOld> filter;

		readonly InstantLoadProperty<PdfNameOld> subFilter;

		readonly InstantLoadProperty<PdfRealOld> algorithm;

		readonly InstantLoadProperty<PdfIntOld> length;

		readonly LoadOnDemandProperty<CryptFiltersOld> cryptFilters;

		readonly InstantLoadProperty<PdfNameOld> streamsFilter;

		readonly InstantLoadProperty<PdfNameOld> stringsFilter;
	}
}
