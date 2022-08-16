using System;
using System.Globalization;
using System.Text;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class FloatBlock
	{
		public FloatBlock()
		{
			this.data = new float[64];
		}

		public FloatBlock(Block intBlock)
		{
			this.data = new float[64];
			for (int i = 0; i < 64; i++)
			{
				this.data[i] = (float)intBlock[i];
			}
		}

		public float this[int index]
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

		public float this[int row, int column]
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

		public Block ToBlock()
		{
			Block block = new Block();
			for (int i = 0; i < 64; i++)
			{
				block[i] = (int)Math.Round((double)this.data[i]);
			}
			return block;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.data.Length; i++)
			{
				stringBuilder.Append(this.data[i].ToString(CultureInfo.InvariantCulture) + " ");
			}
			return stringBuilder.ToString();
		}

		readonly float[] data;
	}
}
