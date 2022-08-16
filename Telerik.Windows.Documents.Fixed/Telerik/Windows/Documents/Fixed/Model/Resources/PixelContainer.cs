using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	abstract class PixelContainer
	{
		public abstract ImageDecodedDataType DataType { get; }

		public abstract int PixelCount { get; }

		public byte[] Array
		{
			get
			{
				return this.array;
			}
		}

		public PixelContainer(int byteCapacity)
			: this(new byte[byteCapacity])
		{
		}

		public PixelContainer(byte[] array)
		{
			this.array = array;
		}

		public abstract void SetColorToIndex(Color color, int index);

		public abstract Color GetColorFromIndex(int index);

		public abstract void Add(Color color);

		readonly byte[] array;
	}
}
