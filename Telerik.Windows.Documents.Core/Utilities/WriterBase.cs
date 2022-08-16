using System;
using System.Collections.Generic;
using System.IO;

namespace Telerik.Windows.Documents.Utilities
{
	abstract class WriterBase : IDisposable
	{
		public WriterBase()
		{
			this.data = new MemoryStream();
			this.beginWritingPositions = new Stack<long>();
		}

		public byte[] Data
		{
			get
			{
				return this.data.ToArray();
			}
		}

		public bool EndOfFile
		{
			get
			{
				return this.data == null || this.Position >= this.Length;
			}
		}

		public long Length
		{
			get
			{
				return this.data.Length;
			}
		}

		public virtual long Position
		{
			get
			{
				return this.data.Position;
			}
		}

		public virtual void BeginWritingBlock()
		{
			this.beginWritingPositions.Push(this.data.Position);
		}

		public void Dispose()
		{
			this.data.Dispose();
		}

		public virtual void EndWritingBlock()
		{
			if (this.beginWritingPositions.Count > 0)
			{
				this.data.Position = this.beginWritingPositions.Pop();
			}
		}

		public virtual void Write(byte byteToWrite)
		{
			this.data.WriteByte(byteToWrite);
		}

		public void Write(byte[] buffer, int count)
		{
			this.data.Write(buffer, 0, count);
		}

		public void WriteBE(byte[] buffer, int count)
		{
			for (int i = count - 1; i >= 0; i--)
			{
				this.Write(buffer[i]);
			}
		}

		public virtual void Seek(long offset, SeekOrigin origin)
		{
			this.data.Seek(offset, origin);
		}

		readonly MemoryStream data;

		readonly Stack<long> beginWritingPositions;
	}
}
