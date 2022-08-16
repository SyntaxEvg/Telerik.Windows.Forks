using System;

namespace Telerik.Windows.Zip
{
	public class DefaultCryptoProvider : ICryptoProvider
	{
		public IBlockTransform CreateDecryptor()
		{
			DefaultDecryptor result = null;
			DefaultDecryptor defaultDecryptor = null;
			try
			{
				defaultDecryptor = new DefaultDecryptor();
				defaultDecryptor.InitKeys(this.cryptoSettings.Password);
				result = defaultDecryptor;
				defaultDecryptor = null;
			}
			finally
			{
				if (defaultDecryptor != null)
				{
					defaultDecryptor.Dispose();
				}
			}
			return result;
		}

		public IBlockTransform CreateEncryptor()
		{
			DefaultEncryptor result = null;
			DefaultEncryptor defaultEncryptor = null;
			try
			{
				defaultEncryptor = new DefaultEncryptor();
				defaultEncryptor.InitKeys(this.cryptoSettings.Password);
				if (this.cryptoSettings.FileTime > 0U)
				{
					byte[] initData = new byte[] { (byte)((this.cryptoSettings.FileTime >> 8) & 255U) };
					defaultEncryptor.Header.InitData = initData;
				}
				result = defaultEncryptor;
				defaultEncryptor = null;
			}
			finally
			{
				if (defaultEncryptor != null)
				{
					defaultEncryptor.Dispose();
				}
			}
			return result;
		}

		public void Initialize(EncryptionSettings settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			this.cryptoSettings = settings as DefaultEncryptionSettings;
			if (this.cryptoSettings == null)
			{
				throw new ArgumentException("Invalid argument type", "settings");
			}
		}

		DefaultEncryptionSettings cryptoSettings;
	}
}
