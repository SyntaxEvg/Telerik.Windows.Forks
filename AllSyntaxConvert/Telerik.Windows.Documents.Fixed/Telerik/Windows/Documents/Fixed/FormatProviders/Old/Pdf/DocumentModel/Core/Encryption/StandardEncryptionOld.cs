using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption
{
	class StandardEncryptionOld : EncryptOld, IStandardCryptFilterMethodProvider
	{
		public StandardEncryptionOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.revision = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor
			{
				Name = "R",
				IsRequired = true
			}, Converters.PdfRealConverter);
			this.owner = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "O",
				IsRequired = true
			});
			this.user = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor
			{
				Name = "U",
				IsRequired = true
			});
			this.permissions = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "P",
				IsRequired = true
			});
		}

		public PdfRealOld Revision
		{
			get
			{
				return this.revision.GetValue();
			}
			set
			{
				this.revision.SetValue(value);
			}
		}

		public PdfStringOld Owner
		{
			get
			{
				return this.owner.GetValue();
			}
			set
			{
				this.owner.SetValue(value);
			}
		}

		public PdfStringOld User
		{
			get
			{
				return this.user.GetValue();
			}
			set
			{
				this.user.SetValue(value);
			}
		}

		public PdfIntOld Permissions
		{
			get
			{
				return this.permissions.GetValue();
			}
			set
			{
				this.permissions.SetValue(value);
			}
		}

		public override void Initialize(PdfArrayOld id, string password)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(id, "id");
			if (this.encryptionKey == null)
			{
				byte[] id2 = ((id == null) ? null : id.GetElement<PdfStringOld>(0).Value);
				this.encryptionKey = StandardEncrypt.CalculateEncryptionKey(password, id2, base.Length.Value, (int)this.Revision.Value, this.Owner.Value, this.Permissions.Value, false);
			}
		}

		public override byte[] DecryptStream(int objectNo, int generationNo, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			string cryptFilter = ((base.Algorithm.Value == 4.0) ? base.StreamsFilter.Value : null);
			StandardEncryptionContext encryptionContext = StandardEncrypt.GetEncryptionContext(this, objectNo, generationNo, data, this.encryptionKey, cryptFilter);
			return StandardEncrypt.Encrypt(encryptionContext);
		}

		public override byte[] DecryptString(int objectNo, int generationNo, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			string cryptFilter = ((base.Algorithm.Value == 4.0) ? base.StringsFilter.Value : null);
			StandardEncryptionContext encryptionContext = StandardEncrypt.GetEncryptionContext(this, objectNo, generationNo, data, this.encryptionKey, cryptFilter);
			return StandardEncrypt.Encrypt(encryptionContext);
		}

		string IStandardCryptFilterMethodProvider.GetStandardCryptFilterMethod()
		{
			Guard.ThrowExceptionIfNull<CryptFiltersOld>(base.CryptFilters, "CryptFilters");
			Guard.ThrowExceptionIfNull<CryptFilterOld>(base.CryptFilters.StandardCryptFilter, "CryptFilters.StandardCryptFilter");
			return base.CryptFilters.StandardCryptFilter.CryptFilterMethod.Value;
		}

		readonly InstantLoadProperty<PdfRealOld> revision;

		readonly InstantLoadProperty<PdfStringOld> owner;

		readonly InstantLoadProperty<PdfStringOld> user;

		readonly InstantLoadProperty<PdfIntOld> permissions;

		byte[] encryptionKey;
	}
}
