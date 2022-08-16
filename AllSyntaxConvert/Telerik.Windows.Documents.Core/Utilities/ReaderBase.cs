using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Documents.Utilities
{
	abstract class ReaderBase
	{
		public ReaderBase(byte[] data)
			: this()
		{
			this.data = data;
		}

		internal ReaderBase()
		{
			this.position = 0L;
			this.beginReadingPositions = new Stack<long>();
		}

		public virtual bool EndOfFile
		{
			get
			{
				return this.data == null || this.Position >= this.Length;
			}
		}

		public virtual long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		public virtual void BeginReadingBlock()
		{
			this.beginReadingPositions.Push(this.position);
		}

		public virtual void EndReadingBlock()
		{
			if (this.beginReadingPositions.Count > 0)
			{
				this.position = this.beginReadingPositions.Pop();
			}
		}

		public virtual byte Read()
		{
			if (this.EndOfFile)
			{
				Guard.ThrowPositionOutOfRangeException();
			}
			byte[] array = this.data;
			long num;
			this.position = (num = this.position) + 1L;
			return array[(int)(checked((IntPtr)num))];
		}

		public virtual byte Peek(int skip = 0)
		{
			if (this.position + (long)skip >= (long)this.data.Length)
			{
				Guard.ThrowPositionOutOfRangeException();
			}
			return this.data[(int)(checked((IntPtr)(unchecked(this.position + (long)skip))))];
		}

		public virtual long Position
		{
			get
			{
				return this.position;
			}
		}

		public int Read(byte[] buffer, int count)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (this.Position >= this.Length)
				{
					return num;
				}
				buffer[i] = this.Read();
				num++;
			}
			return num;
		}

		public int ReadBE(byte[] buffer, int count)
		{
			int num = 0;
			for (int i = count - 1; i >= 0; i--)
			{
				if (this.Position >= this.Length)
				{
					return num;
				}
				buffer[i] = this.Read();
				num++;
			}
			return num;
		}

		public virtual void Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.position = offset;
				return;
			case SeekOrigin.Current:
				this.position += offset;
				return;
			case SeekOrigin.End:
				this.position = this.Length - offset;
				return;
			default:
				return;
			}
		}

		readonly byte[] data;

		readonly Stack<long> beginReadingPositions;

		long position;
	}
}
