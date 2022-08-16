using System;

namespace Telerik.Windows.Zip
{
	public interface ICryptoProvider
	{
		IBlockTransform CreateDecryptor();

		IBlockTransform CreateEncryptor();

		void Initialize(EncryptionSettings settings);
	}
}
