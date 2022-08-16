using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser
{
	class Reader
	{
		public Reader(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			this.input = input;
			this.beginReadingPositions = new Stack<long>();
			this.waitHandles = new Stack<ManualResetEvent>();
			this.stringBuilder = new StringBuilder();
			this.syncRoot = new object();
		}

		internal Stream Stream
		{
			get
			{
				return this.input;
			}
		}

		public bool EndOfFile
		{
			get
			{
				return this.Position >= this.Length;
			}
		}

		public long Length
		{
			get
			{
				if (this.cachedLength != -1L)
				{
					return this.cachedLength;
				}
				return this.input.Length;
			}
		}

		public virtual long Position
		{
			get
			{
				return this.input.Position;
			}
		}

		public virtual IDisposable BeginReadingBlock()
		{
			if (this.lockedThread == null || Thread.CurrentThread.ManagedThreadId != this.lockedThread.Value)
			{
				lock (this.syncRoot)
				{
					if (this.lockedThread != null && Thread.CurrentThread.ManagedThreadId != this.lockedThread.Value)
					{
						ManualResetEvent manualResetEvent = new ManualResetEvent(false);
						this.waitHandles.Push(manualResetEvent);
						manualResetEvent.WaitOne();
					}
					this.lockedThread = new int?(Thread.CurrentThread.ManagedThreadId);
				}
			}
			this.beginReadingPositions.Push(this.Position);
			return new DisposableObject(new Action(this.EndReadingBlock));
		}

		public virtual void EndReadingBlock()
		{
			if (this.beginReadingPositions.Count > 0)
			{
				this.Seek(this.beginReadingPositions.Pop(), SeekOrigin.Begin);
			}
			if (this.beginReadingPositions.Count == 0)
			{
				this.lockedThread = null;
				if (this.waitHandles.Count > 0)
				{
					this.waitHandles.Pop().Set();
				}
			}
		}

		public byte Peek()
		{
			byte result = this.Read();
			this.Seek(-1L, SeekOrigin.Current);
			return result;
		}

		public virtual byte Read()
		{
			int num = this.input.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return (byte)num;
		}

		public byte[] Read(int count)
		{
			byte[] array = new byte[count];
			this.input.Read(array, 0, count);
			return array;
		}

		public byte[] ReadBE(int count)
		{
			byte[] array = this.Read(count);
			int num = array.Length / 2;
			for (int i = 0; i < num; i++)
			{
				byte b = array[i];
				array[i] = array[count - i];
				array[count - i] = b;
			}
			return array;
		}

		public string ReadLine()
		{
			this.CacheLength();
			this.stringBuilder.Clear();
			byte b;
			while (!this.EndOfFile && !Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Characters.IsLineFeed(b = this.Read()))
			{
				if (!Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Characters.IsCarriageReturn(b))
				{
					this.stringBuilder.Append((char)b);
				}
				else if (!this.EndOfFile && !Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Characters.IsLineFeed(this.Peek()))
				{
					break;
				}
			}
			this.ClearCachedLength();
			return this.stringBuilder.ToString();
		}

		public string[] ReadSplitLine()
		{
			char[] separators = PostScriptConstants.Separators;
			return this.ReadLine().Split(separators, StringSplitOptions.RemoveEmptyEntries);
		}

		public void Seek(long offset, SeekOrigin origin)
		{
			this.input.Seek(offset, origin);
		}

		public long IndexOf(byte[] byteSequence)
		{
			int num = 0;
			while (!this.EndOfFile && num != byteSequence.Length)
			{
				if (this.Read().Equals(byteSequence[num]))
				{
					num++;
				}
				else if (num != 0)
				{
					this.Seek((long)(-(long)num), SeekOrigin.Current);
					num = 0;
				}
			}
			if (num == byteSequence.Length)
			{
				return this.Position - (long)num;
			}
			return -1L;
		}

		public void CacheLength()
		{
			if (this.input is MemoryStream || this.input is FileStream)
			{
				this.cachedLength = this.input.Length;
			}
		}

		public void ClearCachedLength()
		{
			this.cachedLength = -1L;
		}

		readonly Stream input;

		readonly Stack<long> beginReadingPositions;

		readonly Stack<ManualResetEvent> waitHandles;

		readonly StringBuilder stringBuilder;

		readonly object syncRoot;

		long cachedLength = -1L;

		int? lockedThread;
	}
}
