using System;
using System.Linq;
using System.Text;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class Block
	{
		public Block()
		{
			this.data = new int[64];
		}

		public Block(byte[] data)
		{
			this.data = new int[64];
			for (int i = 0; i < 64; i++)
			{
				this.data[i] = (int)data[i];
			}
		}

		public Block(byte[,] data)
		{
			this.data = new int[64];
			int length = data.GetLength(0);
			int length2 = data.GetLength(1);
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					this.data[num++] = (int)data[i, j];
				}
			}
		}

		public Block(byte[,] raster, int blockStartX, int blockStartY, int horizontalFactor, int verticalFactor)
		{
			this.data = new int[64];
			int length = raster.GetLength(0);
			int length2 = raster.GetLength(1);
			int num = horizontalFactor * 8;
			int num2 = verticalFactor * 8;
			for (int i = 0; i < 8; i++)
			{
				int num3 = blockStartY + num2 + i;
				if (num3 >= length)
				{
					return;
				}
				for (int j = 0; j < 8; j++)
				{
					int num4 = blockStartX + num + j;
					if (num4 >= length2)
					{
						break;
					}
					this[i, j] = (int)raster[num3, num4];
				}
			}
		}

		public int this[int index]
		{
			get
			{
				return this.data[index];
			}
			set
			{
				this.data[index] = value;
			}
		}

		public int this[int row, int column]
		{
			get
			{
				return this.data[Block.GetIndex(row, column)];
			}
			set
			{
				this.data[Block.GetIndex(row, column)] = value;
			}
		}

		public static int GetIndex(int row, int column)
		{
			return row * 8 + column;
		}

		public float[] ToArray()
		{
			return (from d in this.data
				select (float)d).ToArray<float>();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.data.Length; i++)
			{
				stringBuilder.Append(this.data[i] + " ");
			}
			return stringBuilder.ToString();
		}

		readonly int[] data;
	}
}
