using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class StreamPart : Stream
	{
		public StreamPart(Stream baseStream, long offset, string endToken)
		{
			Guard.ThrowExceptionIfLessThan<long>(0L, offset, "offset");
			this.baseStream = baseStream;
			this.offset = offset;
			this.buffer = new byte[1024];
			this.length = this.FindTokenOffset(endToken);
			this.baseStream.Seek(this.offset, SeekOrigin.Begin);
		}

		public StreamPart(Stream baseStream, long offset, int length)
		{
			if (offset < 0L || offset + (long)length > baseStream.Length)
			{
				throw new ArgumentException();
			}
			this.baseStream = baseStream;
			this.offset = offset;
			this.length = (long)length;
			this.baseStream.Seek(this.offset, SeekOrigin.Begin);
		}

		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.baseStream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		public override long Position
		{
			get
			{
				return this.baseStream.Position - this.offset;
			}
			set
			{
				this.baseStream.Position = value + this.offset;
			}
		}

		public override void Flush()
		{
			this.baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = (int)Math.Min((long)count, this.Length - this.Position);
			if (num == 0)
			{
				return 0;
			}
			return this.baseStream.Read(buffer, offset, num);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
			{
				Guard.ThrowExceptionIfOutOfRange<long>(0L, this.offset + this.length, offset, "offset");
				return this.baseStream.Seek(this.offset + offset, SeekOrigin.Begin) - this.offset;
			}
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		long FindTokenOffset(string token)
		{
			this.baseStream.Seek(this.offset, SeekOrigin.Begin);
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder(token);
			long num = this.offset;
			bool flag = false;
			while (num < this.baseStream.Length)
			{
				int num2 = this.baseStream.Read(this.buffer, 0, 1024);
				for (int i = 0; i < num2; i++)
				{
					if (flag && Characters.IsDelimiter((int)this.buffer[i]))
					{
						return num - this.offset - (long)stringBuilder.Length;
					}
					stringBuilder.Append((char)this.buffer[i]);
					num += 1L;
					if (stringBuilder2.Length < stringBuilder.Length)
					{
						stringBuilder.Remove(0, 1);
					}
					flag = stringBuilder.Equals(stringBuilder2);
				}
			}
			return -1L;
		}

		const int BufferSize = 1024;

		readonly Stream baseStream;

		readonly long offset;

		readonly long length;

		readonly byte[] buffer;
	}
}
