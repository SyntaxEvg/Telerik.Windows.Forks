using System;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Data
{
	class StreamPart : Stream
	{
		public StreamPart(Stream baseStream, long offset, string endToken)
		{
			Guard.ThrowExceptionIfLessThan<long>(0L, offset, "offset");
			this.baseStream = baseStream;
			this.offset = offset;
			this.buffer = new byte[1024];
			this.length = this.CalculateStreamSize(endToken);
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

		long CalculateStreamSize(string token)
		{
			this.baseStream.Seek(this.offset, SeekOrigin.Begin);
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder(token);
			StringBuilder stringBuilder3 = new StringBuilder();
			long num = this.offset;
			bool flag = false;
			while (num < this.baseStream.Length)
			{
				int num2 = this.baseStream.Read(this.buffer, 0, 1024);
				int num3 = 0;
				while (num3 < num2 && (!flag || !Characters.IsDelimiter(this.buffer[num3])))
				{
					stringBuilder.Append((char)this.buffer[num3]);
					num += 1L;
					if (stringBuilder2.Length < stringBuilder.Length)
					{
						char value = stringBuilder[0];
						stringBuilder.Remove(0, 1);
						stringBuilder3.Append(value);
						if (stringBuilder3.Length > 2)
						{
							stringBuilder3.Remove(0, 1);
						}
					}
					flag = stringBuilder.Equals(stringBuilder2);
					num3++;
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				long num4 = num - this.offset - (long)stringBuilder.Length;
				if (stringBuilder3.Length > 0)
				{
					string source = stringBuilder3.ToString();
					if (source.First<char>() == '\r')
					{
						num4 -= 1L;
					}
					if (source.Last<char>() == '\n')
					{
						num4 -= 1L;
					}
				}
				return num4;
			}
			return -1L;
		}

		readonly Stream baseStream;

		readonly long offset;

		readonly long length;

		readonly byte[] buffer;
	}
}
