using System;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip
{
	public interface IPlatformManager
	{
		string AltDirectorySeparatorChar { get; }

		Encoding DefaultEncoding { get; }

		string DirectorySeparatorChar { get; }

		Stream CreateTemporaryStream();

		void DeleteTemporaryStream(Stream stream);

		ICryptoProvider GetCryptoProvider(EncryptionSettings settings);

		bool IsEncodingSupported(Encoding encoding);
	}
}
