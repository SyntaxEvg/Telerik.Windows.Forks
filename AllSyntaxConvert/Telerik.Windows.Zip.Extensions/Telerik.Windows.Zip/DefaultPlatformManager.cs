using System;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip
{
	class DefaultPlatformManager : IPlatformManager
	{
		public string AltDirectorySeparatorChar
		{
			get
			{
				return "/";
			}
		}

		public Encoding DefaultEncoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		public string DirectorySeparatorChar
		{
			get
			{
				return "\\";
			}
		}

		public Stream CreateTemporaryStream()
		{
			return new MemoryStream();
		}

		public void DeleteTemporaryStream(Stream stream)
		{
		}

		public ICryptoProvider GetCryptoProvider(EncryptionSettings settings)
		{
			string a;
			if ((a = settings.Algorithm.ToUpperInvariant()) != null && a == "DEFAULT")
			{
				ICryptoProvider cryptoProvider = new DefaultCryptoProvider();
				if (cryptoProvider != null)
				{
					cryptoProvider.Initialize(settings);
				}
				return cryptoProvider;
			}
			throw new NotSupportedException();
		}

		public bool IsEncodingSupported(Encoding encoding)
		{
			return encoding == null || (!encoding.Equals(Encoding.BigEndianUnicode) && !encoding.Equals(Encoding.Unicode));
		}
	}
}
