using System;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	abstract class Encrypt : PdfObject
	{
		public Encrypt()
		{
			this.filter = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("Filter", true));
			this.algorithm = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("V"), new PdfInt(0));
			this.length = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Length"), new PdfInt(40));
			this.cryptFilters = base.RegisterDirectProperty<CryptFilters>(new PdfPropertyDescriptor("CF"));
			this.streamsFilter = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("StmF"), new PdfName("Identity"));
			this.stringsFilter = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("StrF"), new PdfName("Identity"));
		}

		public PdfName Filter
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

		public PdfName StreamsFilter
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

		public PdfName StringsFilter
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

		public PdfInt Algorithm
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

		public PdfInt Length
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

		public CryptFilters CryptFilters
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

		public static byte[] PadOrTruncatePassword(string password)
		{
			byte[] array = (string.IsNullOrEmpty(password) ? new byte[0] : PdfEncoding.Encoding.GetBytes(password));
			byte[] array2 = new byte[32];
			int num = ((array.Length > array2.Length) ? array2.Length : array.Length);
			Array.Copy(array, 0, array2, 0, num);
			if (array.Length < array2.Length)
			{
				Array.Copy(Encrypt.PasswordPadding, 0, array2, array.Length, array2.Length - array.Length);
			}
			return array2;
		}

		public static Encrypt CreateInstance(PdfName type)
		{
			string value;
			if ((value = type.Value) != null && value == "Standard")
			{
				return new StandardEncrypt();
			}
			throw new NotSupportedException("Encryption type is not supported.");
		}

		public abstract bool AuthenticateUserPassword(byte[] id, string password);

		public abstract bool AuthenticateOwnerPassword(byte[] id, string password);

		public abstract byte[] EncryptStream(int objectNo, int generationNo, byte[] data);

		public abstract byte[] DecryptStream(int objectNo, int generationNo, byte[] data);

		public abstract byte[] EncryptString(int objectNo, int generationNo, byte[] data);

		public abstract byte[] DecryptString(int objectNo, int generationNo, byte[] data);

		public void CopyPropertiesFrom(IPdfExportContext context, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
			this.CopyPropertiesFromOverride(context, settings);
		}

		protected virtual void CopyPropertiesFromOverride(IPdfExportContext context, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
		}

		internal static void ValidateEncryptionAlgorithmValue(int algorithmValue)
		{
			if (algorithmValue < 0 || algorithmValue == 3 || algorithmValue > 4)
			{
				throw new NotSupportedEncryptionException(algorithmValue);
			}
		}

		protected static readonly byte[] PasswordPadding = new byte[]
		{
			40, 191, 78, 94, 78, 117, 138, 65, 100, 0,
			78, 86, byte.MaxValue, 250, 1, 8, 46, 46, 0, 182,
			208, 104, 62, 128, 47, 12, 169, 254, 100, 83,
			105, 122
		};

		readonly DirectProperty<PdfInt> algorithm;

		readonly DirectProperty<PdfInt> length;

		readonly DirectProperty<CryptFilters> cryptFilters;

		readonly DirectProperty<PdfName> filter;

		readonly DirectProperty<PdfName> streamsFilter;

		readonly DirectProperty<PdfName> stringsFilter;
	}
}
