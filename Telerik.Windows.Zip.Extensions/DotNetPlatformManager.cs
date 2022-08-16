using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip.Extensions
{
	public class DotNetPlatformManager : IPlatformManager
	{
		public DotNetPlatformManager()
		{
			this.TemporaryStreamType = TemporaryStreamType.Memory;
		}

		public string AltDirectorySeparatorChar
		{
			get
			{
				return DotNetPlatformManager.altDirectorySeparatorChar;
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
				return DotNetPlatformManager.directorySeparatorChar;
			}
		}

		public TemporaryStreamType TemporaryStreamType { get; set; }

		public Stream CreateTemporaryStream()
		{
			if (this.TemporaryStreamType == TemporaryStreamType.Memory)
			{
				return new MemoryStream();
			}
			string tempFileName = Path.GetTempFileName();
			FileStream fileStream = File.Open(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			this.temporaryStreams.Add(fileStream, tempFileName);
			return fileStream;
		}

		public void DeleteTemporaryStream(Stream stream)
		{
			if (this.TemporaryStreamType == TemporaryStreamType.File)
			{
				string path = null;
				if (this.temporaryStreams.TryGetValue(stream, out path))
				{
					File.Delete(path);
					this.temporaryStreams.Remove(stream);
				}
			}
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

		static readonly string altDirectorySeparatorChar = new string(Path.AltDirectorySeparatorChar, 1);

		static readonly string directorySeparatorChar = new string(Path.DirectorySeparatorChar, 1);

		Dictionary<Stream, string> temporaryStreams = new Dictionary<Stream, string>();
	}
}
