using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	class CryptoStream : OperationStream
	{
		public CryptoStream(Stream input, StreamOperationMode mode, ICryptoProvider cryptoProvider)
			: base(input, mode)
		{
			if ((!input.CanRead && mode == StreamOperationMode.Read) || (!input.CanWrite && mode == StreamOperationMode.Write))
			{
				throw new ArgumentOutOfRangeException("mode");
			}
			this.cryptoProvider = cryptoProvider;
			switch (base.Mode)
			{
			case StreamOperationMode.Read:
				base.Transform = this.cryptoProvider.CreateDecryptor();
				return;
			case StreamOperationMode.Write:
				base.Transform = this.cryptoProvider.CreateEncryptor();
				return;
			default:
				return;
			}
		}

		~CryptoStream()
		{
			this.Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			this.cryptoProvider = null;
			base.Dispose(disposing);
		}

		ICryptoProvider cryptoProvider;
	}
}
