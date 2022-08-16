using System;
using System.IO;

namespace CsQuery.Implementation
{
	abstract class BaseStream : Stream
	{
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) != 1)
			{
				return -1;
			}
			return (int)array[0];
		}

		public override void WriteByte(byte value)
		{
			this.Write(new byte[] { value }, 0, 1);
		}
	}
}
