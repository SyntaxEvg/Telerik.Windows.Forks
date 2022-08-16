using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	public abstract class OperationStream : Stream
	{
		internal OperationStream(Stream baseStream, StreamOperationMode mode)
		{
			if ((!baseStream.CanRead && mode == StreamOperationMode.Read) || (!baseStream.CanWrite && mode == StreamOperationMode.Write))
			{
				throw new ArgumentOutOfRangeException("mode");
			}
			this.BaseStream = baseStream;
			this.streamMode = mode;
			this.IsDisposed = false;
		}

		~OperationStream()
		{
			this.Dispose(false);
		}

		public override bool CanRead
		{
			get
			{
				return this.baseStream != null && this.streamMode == StreamOperationMode.Read;
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
				return this.baseStream != null && this.streamMode == StreamOperationMode.Write;
			}
		}

		public bool HasFlushedFinalBlock
		{
			get
			{
				return this.finalBlockTransformed;
			}
		}

		public override long Length
		{
			get
			{
				if (this.BaseStream == null)
				{
					throw new NotSupportedException();
				}
				if (this.BaseStream.CanSeek)
				{
					if (this.Mode == StreamOperationMode.Read)
					{
						return this.length.Value;
					}
					return this.BaseStream.Length;
				}
				else
				{
					if (this.length != null)
					{
						return this.length.Value;
					}
					return long.MaxValue;
				}
			}
		}

		public override long Position
		{
			get
			{
				if (this.BaseStream == null)
				{
					throw new NotSupportedException();
				}
				if (!this.BaseStream.CanSeek)
				{
					return 0L;
				}
				if (this.Mode == StreamOperationMode.Read)
				{
					return this.BaseStream.Position - this.startPosition;
				}
				return this.BaseStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public long TotalPlainCount
		{
			get
			{
				return this.totalPlainCount;
			}
			set
			{
				this.totalPlainCount = value;
			}
		}

		public long TotalTransformedCount
		{
			get
			{
				return this.totalTransformedCount;
			}
			set
			{
				this.totalTransformedCount = value;
			}
		}

		internal Stream BaseStream
		{
			get
			{
				return this.baseStream;
			}
			set
			{
				this.baseStream = value;
				if (this.baseStream != null && this.Mode == StreamOperationMode.Read && this.baseStream.CanSeek)
				{
					this.startPosition = this.baseStream.Position;
					try
					{
						this.length = new long?(this.baseStream.Length);
					}
					catch
					{
						this.length = new long?(0L);
					}
				}
			}
		}

		internal StreamOperationMode Mode
		{
			get
			{
				return this.streamMode;
			}
		}

		internal bool IsDisposed { get; set; }

		internal IBlockTransform Transform
		{
			get
			{
				return this.blockTransform;
			}
			set
			{
				this.blockTransform = value;
				this.InitializeTransform();
				this.InitializeBuffers();
			}
		}

		public override void Flush()
		{
			this.FlushFinalBlock();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this.EnsureNotDisposed();
			OperationStream.ValidateBufferParameters(buffer, offset, count, true);
			if (!this.CanRead)
			{
				throw new NotSupportedException();
			}
			int i = count;
			int num = offset;
			if (this.outputBufferIndex != 0)
			{
				if (this.outputBufferIndex > count)
				{
					Buffer.BlockCopy(this.outputBuffer, 0, buffer, offset, count);
					Buffer.BlockCopy(this.outputBuffer, count, this.outputBuffer, 0, this.outputBufferIndex - count);
					this.outputBufferIndex -= count;
					return count;
				}
				Buffer.BlockCopy(this.outputBuffer, 0, buffer, offset, this.outputBufferIndex);
				i -= this.outputBufferIndex;
				num += this.outputBufferIndex;
				this.outputBufferIndex = 0;
			}
			if (this.finalBlockTransformed)
			{
				return count - i;
			}
			if (i > this.Transform.OutputBlockSize && this.Transform.CanTransformMultipleBlocks)
			{
				int num2 = i / this.Transform.OutputBlockSize * this.Transform.InputBlockSize;
				long val = this.Length - this.Position;
				num2 = (int)Math.Min((long)num2, val);
				byte[] array = new byte[num2];
				Buffer.BlockCopy(this.inputBuffer, 0, array, 0, this.inputBufferIndex);
				int num3 = this.inputBufferIndex + this.baseStream.Read(array, this.inputBufferIndex, num2 - this.inputBufferIndex);
				this.TotalTransformedCount += (long)(num3 - this.inputBufferIndex);
				this.inputBufferIndex = 0;
				if (num3 > this.Transform.InputBlockSize)
				{
					int num4 = num3 / this.Transform.InputBlockSize * this.Transform.InputBlockSize;
					int num5 = num3 - num4;
					if (num5 != 0)
					{
						this.inputBufferIndex = num5;
						Buffer.BlockCopy(array, num4, this.inputBuffer, 0, num5);
					}
					int num6 = num4 / this.Transform.InputBlockSize * this.Transform.OutputBlockSize;
					byte[] array2 = new byte[num6];
					int num7 = this.Transform.TransformBlock(array, 0, num4, array2, 0);
					this.TotalPlainCount += (long)num7;
					Buffer.BlockCopy(array2, 0, buffer, num, num7);
					Array.Clear(array, 0, array.Length);
					Array.Clear(array2, 0, array2.Length);
					i -= num7;
					num += num7;
				}
				else
				{
					this.inputBuffer = array;
					this.inputBufferIndex = num3;
				}
			}
			while (i > 0)
			{
				while (this.inputBufferIndex < this.Transform.InputBlockSize)
				{
					long val2 = this.Length - this.Position;
					int num8 = this.Transform.InputBlockSize - this.inputBufferIndex;
					num8 = (int)Math.Min((long)num8, val2);
					int num3 = this.baseStream.Read(this.inputBuffer, this.inputBufferIndex, num8);
					this.TotalTransformedCount += (long)num3;
					if (num3 == 0)
					{
						byte[] array3 = this.Transform.TransformFinalBlock(this.inputBuffer, 0, this.inputBufferIndex);
						this.TotalPlainCount += (long)array3.Length;
						this.outputBuffer = array3;
						this.outputBufferIndex = array3.Length;
						this.finalBlockTransformed = true;
						if (i >= this.outputBufferIndex)
						{
							Buffer.BlockCopy(this.outputBuffer, 0, buffer, num, this.outputBufferIndex);
							i -= this.outputBufferIndex;
							this.outputBufferIndex = 0;
							return count - i;
						}
						Buffer.BlockCopy(this.outputBuffer, 0, buffer, num, i);
						this.outputBufferIndex -= i;
						Buffer.BlockCopy(this.outputBuffer, i, this.outputBuffer, 0, this.outputBufferIndex);
						return count;
					}
					else
					{
						this.inputBufferIndex += num3;
					}
				}
				int num7 = this.Transform.TransformBlock(this.inputBuffer, 0, this.Transform.InputBlockSize, this.outputBuffer, 0);
				this.TotalPlainCount += (long)num7;
				this.inputBufferIndex = 0;
				if (i < num7)
				{
					Buffer.BlockCopy(this.outputBuffer, 0, buffer, num, i);
					this.outputBufferIndex = num7 - i;
					Buffer.BlockCopy(this.outputBuffer, i, this.outputBuffer, 0, this.outputBufferIndex);
					return count;
				}
				Buffer.BlockCopy(this.outputBuffer, 0, buffer, num, num7);
				num += num7;
				i -= num7;
			}
			return count;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			if (this.Mode == StreamOperationMode.Read)
			{
				this.length = new long?(value);
				return;
			}
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EnsureNotDisposed();
			OperationStream.ValidateBufferParameters(buffer, offset, count, true);
			if (!this.CanWrite)
			{
				throw new NotSupportedException();
			}
			this.TotalPlainCount += (long)count;
			int i = count;
			int num = offset;
			if (this.inputBufferIndex > 0)
			{
				if (count < this.Transform.InputBlockSize - this.inputBufferIndex)
				{
					Buffer.BlockCopy(buffer, offset, this.inputBuffer, this.inputBufferIndex, count);
					this.inputBufferIndex += count;
					return;
				}
				Buffer.BlockCopy(buffer, offset, this.inputBuffer, this.inputBufferIndex, this.Transform.InputBlockSize - this.inputBufferIndex);
				num += this.Transform.InputBlockSize - this.inputBufferIndex;
				i -= this.Transform.InputBlockSize - this.inputBufferIndex;
				this.inputBufferIndex = this.Transform.InputBlockSize;
			}
			if (this.outputBufferIndex > 0)
			{
				this.baseStream.Write(this.outputBuffer, 0, this.outputBufferIndex);
				this.outputBufferIndex = 0;
			}
			if (this.inputBufferIndex == this.Transform.InputBlockSize)
			{
				int num2 = this.Transform.TransformBlock(this.inputBuffer, 0, this.Transform.InputBlockSize, this.outputBuffer, 0);
				this.TotalTransformedCount += (long)num2;
				this.baseStream.Write(this.outputBuffer, 0, num2);
				this.inputBufferIndex = 0;
			}
			while (i > 0)
			{
				if (i < this.Transform.InputBlockSize)
				{
					Buffer.BlockCopy(buffer, num, this.inputBuffer, 0, i);
					this.inputBufferIndex += i;
					return;
				}
				if (!this.Transform.CanTransformMultipleBlocks)
				{
					int num2 = this.Transform.TransformBlock(buffer, num, this.Transform.InputBlockSize, this.outputBuffer, 0);
					this.TotalTransformedCount += (long)num2;
					this.baseStream.Write(this.outputBuffer, 0, num2);
					num += this.Transform.InputBlockSize;
					i -= this.Transform.InputBlockSize;
				}
				else
				{
					int num3 = i / this.Transform.InputBlockSize;
					int num4 = num3 * this.Transform.InputBlockSize;
					byte[] buffer2 = new byte[num3 * this.Transform.OutputBlockSize];
					int num2 = this.Transform.TransformBlock(buffer, num, num4, buffer2, 0);
					this.TotalTransformedCount += (long)num2;
					this.baseStream.Write(buffer2, 0, num2);
					num += num4;
					i -= num4;
				}
			}
		}

		internal static void ValidateBufferParameters(byte[] buffer, int offset, int count, bool allowZeroCount)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || (count == 0 && !allowZeroCount))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("InvalidArgumentOffsetCount");
			}
		}

		internal virtual void FlushFinalBlock()
		{
			if (this.finalBlockTransformed)
			{
				throw new NotSupportedException("Can't flush final block twice");
			}
			byte[] array = new byte[0];
			if (this.inputBuffer != null)
			{
				array = this.Transform.TransformFinalBlock(this.inputBuffer, 0, this.inputBufferIndex);
			}
			this.finalBlockTransformed = true;
			this.TotalTransformedCount += (long)array.Length;
			if (this.baseStream != null && this.CanWrite)
			{
				if (this.outputBufferIndex > 0)
				{
					this.baseStream.Write(this.outputBuffer, 0, this.outputBufferIndex);
					this.outputBufferIndex = 0;
				}
				this.baseStream.Write(array, 0, array.Length);
			}
			OperationStream operationStream = this.baseStream as OperationStream;
			if (operationStream == null)
			{
				try
				{
					if (this.baseStream.CanRead)
					{
						this.baseStream.Flush();
					}
					goto IL_DD;
				}
				catch (NotSupportedException)
				{
					goto IL_DD;
				}
			}
			if (!operationStream.HasFlushedFinalBlock && operationStream.CanWrite)
			{
				operationStream.FlushFinalBlock();
			}
			IL_DD:
			if (this.inputBuffer != null)
			{
				Array.Clear(this.inputBuffer, 0, this.inputBuffer.Length);
			}
			if (this.outputBuffer != null)
			{
				Array.Clear(this.outputBuffer, 0, this.outputBuffer.Length);
			}
		}

		protected void EnsureNotDisposed()
		{
			if (this.baseStream == null)
			{
				throw new ObjectDisposedException(null, "Stream closed");
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed)
			{
				try
				{
					if (disposing)
					{
						if (!this.finalBlockTransformed && this.Transform != null)
						{
							this.FlushFinalBlock();
						}
						OperationStream operationStream = this.baseStream as OperationStream;
						if (operationStream != null)
						{
							operationStream.Dispose();
						}
					}
				}
				finally
				{
					try
					{
						this.finalBlockTransformed = true;
						if (this.Transform != null)
						{
							this.Transform.Dispose();
						}
						if (this.inputBuffer != null)
						{
							Array.Clear(this.inputBuffer, 0, this.inputBuffer.Length);
						}
						if (this.outputBuffer != null)
						{
							Array.Clear(this.outputBuffer, 0, this.outputBuffer.Length);
						}
						this.inputBuffer = null;
						this.outputBuffer = null;
						this.blockTransform = null;
						this.baseStream = null;
					}
					finally
					{
						this.IsDisposed = true;
						base.Dispose(disposing);
					}
				}
			}
		}

		void InitializeBuffers()
		{
			if (this.blockTransform != null)
			{
				this.inputBuffer = new byte[this.blockTransform.InputBlockSize];
				this.outputBuffer = new byte[this.blockTransform.OutputBlockSize];
			}
		}

		void InitializeTransform()
		{
			switch (this.Mode)
			{
			case StreamOperationMode.Read:
				this.ReadTransformationHeader();
				return;
			case StreamOperationMode.Write:
				this.WriteTransformationHeader();
				return;
			default:
				return;
			}
		}

		void ReadTransformationHeader()
		{
			this.Transform.InitHeaderReading();
			while (this.Transform.Header.BytesToRead > 0)
			{
				int num = ((this.Transform.Header.Buffer != null) ? (this.Transform.Header.Buffer.Length + this.Transform.Header.BytesToRead) : this.Transform.Header.BytesToRead);
				byte[] array = new byte[num];
				int offset = 0;
				if (this.Transform.Header.Buffer != null)
				{
					Array.Copy(this.Transform.Header.Buffer, array, this.Transform.Header.Buffer.Length);
					offset = this.Transform.Header.Buffer.Length;
				}
				this.BaseStream.Read(array, offset, this.Transform.Header.BytesToRead);
				this.Transform.Header.Buffer = array;
				this.Transform.ProcessHeader();
			}
		}

		void WriteTransformationHeader()
		{
			this.Transform.CreateHeader();
			if (this.Transform.Header.Buffer != null)
			{
				this.BaseStream.Write(this.Transform.Header.Buffer, 0, this.Transform.Header.Buffer.Length);
			}
		}

		Stream baseStream;

		StreamOperationMode streamMode;

		IBlockTransform blockTransform;

		byte[] inputBuffer;

		int inputBufferIndex;

		byte[] outputBuffer;

		int outputBufferIndex;

		bool finalBlockTransformed;

		long totalPlainCount;

		long totalTransformedCount;

		long? length;

		long startPosition;
	}
}
