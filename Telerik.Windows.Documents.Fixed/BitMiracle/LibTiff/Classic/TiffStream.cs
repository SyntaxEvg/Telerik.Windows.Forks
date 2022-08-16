using System;
using System.IO;

namespace BitMiracle.LibTiff.Classic
{
	class TiffStream
	{
		public virtual int Read(object clientData, byte[] buffer, int offset, int count)
		{
			Stream stream = clientData as Stream;
			if (stream == null)
			{
				throw new ArgumentException("Can't get underlying stream to read from");
			}
			return stream.Read(buffer, offset, count);
		}

		public virtual void Write(object clientData, byte[] buffer, int offset, int count)
		{
			Stream stream = clientData as Stream;
			if (stream == null)
			{
				throw new ArgumentException("Can't get underlying stream to write to");
			}
			stream.Write(buffer, offset, count);
		}

		public virtual long Seek(object clientData, long offset, SeekOrigin origin)
		{
			if (offset == -1L)
			{
				return -1L;
			}
			Stream stream = clientData as Stream;
			if (stream == null)
			{
				throw new ArgumentException("Can't get underlying stream to seek in");
			}
			return stream.Seek(offset, origin);
		}

		public virtual void Close(object clientData)
		{
			Stream stream = clientData as Stream;
			if (stream == null)
			{
				throw new ArgumentException("Can't get underlying stream to close");
			}
			stream.Close();
		}

		public virtual long Size(object clientData)
		{
			Stream stream = clientData as Stream;
			if (stream == null)
			{
				throw new ArgumentException("Can't get underlying stream to retrieve size from");
			}
			return stream.Length;
		}
	}
}
