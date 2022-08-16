using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	class StandardEncrypt : Encrypt, IStandardCryptFilterMethodProvider
	{
		public StandardEncrypt()
		{
			this.revision = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("R", true));
			this.owner = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("O", true));
			this.user = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("U", true));
			this.permissions = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("P", true));
		}

		public PdfInt Revision
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

		public PdfString Owner
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

		public PdfString User
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

		public PdfInt Permissions
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

		public override bool AuthenticateUserPassword(byte[] id, string password)
		{
			this.encryptionKey = StandardEncrypt.CalculateEncryptionKey(password, id, base.Length.Value, this.Revision.Value, this.Owner.Value, this.Permissions.Value, false);
			byte[] array = StandardEncrypt.CalculateUserPassword(this.encryptionKey, id);
			bool result = true;
			for (int i = 0; i < 16; i++)
			{
				if (this.User.Value[i] != array[i])
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public override bool AuthenticateOwnerPassword(byte[] id, string password)
		{
			throw new NotImplementedException();
		}

		public override byte[] EncryptStream(int objectNo, int generationNo, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			string cryptFilter = ((base.Algorithm.Value == 4) ? base.StreamsFilter.Value : null);
			StandardEncryptionContext encryptionContext = StandardEncrypt.GetEncryptionContext(this, objectNo, generationNo, data, this.encryptionKey, cryptFilter);
			return StandardEncrypt.Encrypt(encryptionContext);
		}

		public override byte[] DecryptStream(int objectNo, int generationNo, byte[] data)
		{
			return this.EncryptStream(objectNo, generationNo, data);
		}

		public override byte[] EncryptString(int objectNo, int generationNo, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			string cryptFilter = ((base.Algorithm.Value == 4) ? base.StringsFilter.Value : null);
			StandardEncryptionContext encryptionContext = StandardEncrypt.GetEncryptionContext(this, objectNo, generationNo, data, this.encryptionKey, cryptFilter);
			return StandardEncrypt.Encrypt(encryptionContext);
		}

		public override byte[] DecryptString(int objectNo, int generationNo, byte[] data)
		{
			return this.EncryptString(objectNo, generationNo, data);
		}

		protected override void CopyPropertiesFromOverride(IPdfExportContext context, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
			int standardEncryptRevision = FixedDocumentDefaults.StandardEncryptRevision;
			int standardEncryptionKeyLength = FixedDocumentDefaults.StandardEncryptionKeyLength;
			int permissionBits = StandardEncrypt.GetPermissionBits();
			base.Filter = new PdfName("Standard");
			this.Revision = new PdfInt(standardEncryptRevision);
			base.Algorithm = new PdfInt(2);
			base.Length = new PdfInt(standardEncryptionKeyLength);
			this.Permissions = new PdfInt(permissionBits);
			byte[] array = StandardEncrypt.CalculateOwnerPassword(settings.OwnerPassword, settings.UserPassword, standardEncryptRevision, standardEncryptionKeyLength);
			this.encryptionKey = StandardEncrypt.CalculateEncryptionKey(settings.UserPassword, context.DocumentId, standardEncryptionKeyLength, standardEncryptRevision, array, permissionBits, false);
			byte[] initialValue = StandardEncrypt.CalculateUserPassword(this.encryptionKey, context.DocumentId);
			this.Owner = new PdfHexString(array);
			this.User = new PdfHexString(initialValue);
			base.CopyPropertiesFromOverride(context, settings);
		}

		internal static byte[] CalculateEncryptionKey(string password, byte[] id, int keyBitLength, int revision, byte[] oValue, int permissions, bool encryptMetadata)
		{
			List<byte> list = new List<byte>();
			byte[] collection = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption.Encrypt.PadOrTruncatePassword(password);
			list.AddRange(collection);
			list.AddRange(oValue);
			byte[] bytes = BitConverter.GetBytes(permissions);
			list.AddRange(bytes);
			if (id != null)
			{
				list.AddRange(id);
			}
			if (revision >= 4 && encryptMetadata)
			{
				byte[] array = new byte[4];
				for (int i = 0; i < 4; i++)
				{
					array[i] = byte.MaxValue;
				}
				list.AddRange(bytes);
			}
			byte[] hash = MD5Core.GetHash(list.ToArray());
			int keyLength = StandardEncrypt.GetKeyLength(revision, keyBitLength);
			byte[] array2 = new byte[keyLength];
			Array.Copy(hash, array2, array2.Length);
			if (revision >= 3)
			{
				for (int j = 0; j < 50; j++)
				{
					hash = MD5Core.GetHash(array2);
					Array.Copy(hash, array2, array2.Length);
				}
			}
			return array2;
		}

		internal static byte[] CalculateOwnerPassword(string ownerPassword, string userPassword, int revision, int keyBitLength)
		{
			byte[] input = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption.Encrypt.PadOrTruncatePassword(string.IsNullOrEmpty(ownerPassword) ? userPassword : ownerPassword);
			byte[] hash = MD5Core.GetHash(input);
			if (revision >= 3)
			{
				for (int i = 0; i < 50; i++)
				{
					hash = MD5Core.GetHash(hash);
				}
			}
			int keyLength = StandardEncrypt.GetKeyLength(revision, keyBitLength);
			byte[] array = new byte[keyLength];
			Array.Copy(hash, array, keyLength);
			RC4 rc = new RC4();
			rc.PrepareKey(array);
			byte[] array2 = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption.Encrypt.PadOrTruncatePassword(userPassword);
			byte[] array3 = new byte[array2.Length];
			rc.Encrypt(array2, array3);
			if (revision >= 3)
			{
				byte[] array4 = new byte[array.Length];
				byte[] array5 = new byte[array3.Length];
				for (int j = 0; j < 19; j++)
				{
					StandardEncrypt.CreateRC4Key(array, j + 1, array4);
					rc = new RC4();
					rc.PrepareKey(array4);
					rc.Encrypt(array3, array5);
					Array.Copy(array5, array3, array5.Length);
				}
			}
			return array3;
		}

		internal static byte[] CalculateUserPassword(byte[] key, byte[] id)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption.Encrypt.PasswordPadding);
			if (id != null)
			{
				list.AddRange(id);
			}
			byte[] hash = MD5Core.GetHash(list.ToArray());
			RC4 rc = new RC4();
			rc.PrepareKey(key);
			byte[] array = new byte[hash.Length];
			rc.Encrypt(hash, array);
			byte[] array2 = new byte[key.Length];
			byte[] array3 = new byte[array.Length];
			for (int i = 0; i < 19; i++)
			{
				StandardEncrypt.CreateRC4Key(key, i + 1, array2);
				rc = new RC4();
				rc.PrepareKey(array2);
				rc.Encrypt(array, array3);
				Array.Copy(array3, array, array3.Length);
			}
			byte[] array4 = new byte[32];
			Array.Copy(array, array4, array.Length);
			return array4;
		}

		internal static byte[] Encrypt(StandardEncryptionContext context)
		{
			Guard.ThrowExceptionIfNull<byte[]>(context.InputData, "context.InputData");
			if (context.IsIdentityEncryption)
			{
				return context.InputData;
			}
			byte[] array = StandardEncrypt.GetEmptyInitialArray(context);
			Array.Copy(context.EncryptionKey, array, context.EncryptionKey.Length);
			byte[] array2 = new byte[3];
			BytesHelper.GetBytes(context.ObjectNumber, array2);
			byte[] array3 = new byte[2];
			BytesHelper.GetBytes(context.GenerationNumber, array3);
			Array.Copy(array2, 0, array, context.EncryptionKey.Length, array2.Length);
			Array.Copy(array3, 0, array, context.EncryptionKey.Length + array2.Length, array3.Length);
			if (context.CryptFilterMethod == CryptFilterMethod.AESV2)
			{
				Array.Copy(StandardEncrypt.aesSAlTbytes, 0, array, array.Length - StandardEncrypt.aesSAlTbytes.Length, StandardEncrypt.aesSAlTbytes.Length);
			}
			array = MD5Core.GetHash(array);
			int num = System.Math.Min(16, context.EncryptionKey.Length + 5);
			byte[] array4 = new byte[num];
			Array.Copy(array, array4, num);
			byte[] result;
			if (context.CryptFilterMethod == CryptFilterMethod.AESV2)
			{
				result = StandardEncrypt.CalculateAesResult(context.InputData, array4);
			}
			else
			{
				result = StandardEncrypt.CalculateRC4Result(context.InputData, array4);
			}
			return result;
		}

		internal static StandardEncryptionContext GetEncryptionContext(IStandardCryptFilterMethodProvider stdCF, int objectNo, int generationNo, byte[] data, byte[] encryptionKey, string cryptFilter)
		{
			StandardEncryptionContext result = new StandardEncryptionContext
			{
				ObjectNumber = objectNo,
				GenerationNumber = generationNo,
				InputData = data,
				EncryptionKey = encryptionKey,
				IsIdentityEncryption = (cryptFilter != null && cryptFilter == "Identity")
			};
			if (cryptFilter == null)
			{
				result.CryptFilterMethod = CryptFilterMethod.V2;
			}
			else if (cryptFilter == "StdCF")
			{
				string standardCryptFilterMethod = stdCF.GetStandardCryptFilterMethod();
				CryptFilterMethod cryptFilterMethod;
				if (!Enum.TryParse<CryptFilterMethod>(standardCryptFilterMethod, false, out cryptFilterMethod))
				{
					throw new NotSupportedException(string.Format("Not supported crypt filter method: {0}", standardCryptFilterMethod));
				}
				result.CryptFilterMethod = cryptFilterMethod;
			}
			else if (!result.IsIdentityEncryption)
			{
				throw new NotSupportedException(string.Format("Only {0} and {1} crypt filter names are supported", "Identity", "StdCF"));
			}
			return result;
		}

		static byte[] CalculateAesResult(byte[] cipherTextCombined, byte[] key)
		{
			byte[] result;
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				byte[] array = new byte[aes.BlockSize / 8];
				byte[] array2 = new byte[cipherTextCombined.Length - array.Length];
				Array.Copy(cipherTextCombined, array, array.Length);
				Array.Copy(cipherTextCombined, array.Length, array2, 0, array2.Length);
				aes.IV = array;
				aes.Mode = CipherMode.CBC;
				ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
				using (MemoryStream memoryStream = new MemoryStream(array2))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
					{
						byte[] array3 = cryptoStream.ReadAllBytes();
						result = array3;
					}
				}
			}
			return result;
		}

		static byte[] CalculateRC4Result(byte[] inputData, byte[] key)
		{
			RC4 rc = new RC4();
			rc.PrepareKey(key);
			byte[] array = new byte[inputData.Length];
			rc.Encrypt(inputData, array);
			return array;
		}

		static byte[] GetEmptyInitialArray(StandardEncryptionContext context)
		{
			switch (context.CryptFilterMethod)
			{
			case CryptFilterMethod.V2:
				return new byte[context.EncryptionKey.Length + 5];
			case CryptFilterMethod.AESV2:
				return new byte[context.EncryptionKey.Length + 9];
			default:
				throw new NotSupportedException(string.Format("Not supported crypt filter method: {0}", context.CryptFilterMethod));
			}
		}

		static int GetPermissionBits()
		{
			IEnumerable<int> reservedBitThatAreOnes = StandardEncrypt.GetReservedBitThatAreOnes();
			FlagWriter<PermissionFlag> flagWriter = new FlagWriter<PermissionFlag>(reservedBitThatAreOnes);
			return flagWriter.ResultFlags;
		}

		static IEnumerable<int> GetReservedBitThatAreOnes()
		{
			yield return 7;
			yield return 8;
			for (int number = 13; number <= 32; number++)
			{
				yield return number;
			}
			yield break;
		}

		static void CreateRC4Key(byte[] key, int step, byte[] result)
		{
			for (int i = 0; i < key.Length; i++)
			{
				result[i] = key[i] ^ (byte)step;
			}
		}

		static int GetKeyLength(int revision, int keyBitLength)
		{
			if (revision != 2)
			{
				return keyBitLength / 8;
			}
			return 5;
		}

		string IStandardCryptFilterMethodProvider.GetStandardCryptFilterMethod()
		{
			Guard.ThrowExceptionIfNull<CryptFilters>(base.CryptFilters, "CryptFilters");
			Guard.ThrowExceptionIfNull<CryptFilter>(base.CryptFilters.StandardCryptFilter, "CryptFilters.StandardCryptFilter");
			return base.CryptFilters.StandardCryptFilter.CryptFilterMethod.Value;
		}

		static readonly byte[] aesSAlTbytes = new byte[] { 115, 65, 108, 84 };

		readonly DirectProperty<PdfInt> revision;

		readonly DirectProperty<PdfString> owner;

		readonly DirectProperty<PdfString> user;

		readonly DirectProperty<PdfInt> permissions;

		byte[] encryptionKey;
	}
}
