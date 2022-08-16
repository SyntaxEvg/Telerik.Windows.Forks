using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class JpegComponent
	{
		public JpegComponent(byte id, byte hSampleFactor, byte vSampleFactor, byte quantizationTableId, IHuffmanTablesOwner huffmanTablesOwner)
		{
			this.id = id;
			this.hSampleFactor = hSampleFactor;
			this.vSampleFactor = vSampleFactor;
			this.quantizationTableId = quantizationTableId;
			this.huffmanTablesOwner = huffmanTablesOwner;
			this.lastDCCoefficient = 0;
			this.blocks = new List<Block>();
		}

		public int ACTableId { get; set; }

		public int DCTableId { get; set; }

		public HuffmanTable AcTable
		{
			get
			{
				return this.huffmanTablesOwner.GetHuffmanTable(TableClass.ACTable, this.ACTableId);
			}
		}

		public HuffmanTable DcTable
		{
			get
			{
				return this.huffmanTablesOwner.GetHuffmanTable(TableClass.DCTable, this.DCTableId);
			}
		}

		public int HorizontalBlocksCount
		{
			get
			{
				return this.horizontalBlocksCount;
			}
		}

		public byte HSampleFactor
		{
			get
			{
				return this.hSampleFactor;
			}
		}

		public byte Id
		{
			get
			{
				return this.id;
			}
		}

		public int LastDCCoefficient
		{
			get
			{
				return this.lastDCCoefficient;
			}
		}

		public int QuantizationTableId
		{
			get
			{
				return (int)this.quantizationTableId;
			}
		}

		public int VerticalBlocksCount
		{
			get
			{
				return this.verticalBlocksCount;
			}
		}

		public byte VSampleFactor
		{
			get
			{
				return this.vSampleFactor;
			}
		}

		public int McuPerRow
		{
			get
			{
				return this.mcuPerRow;
			}
		}

		public static List<FloatBlock> FDCT(List<Block> blocksToProcess)
		{
			List<FloatBlock> list = new List<FloatBlock>();
			for (int i = 0; i < blocksToProcess.Count; i++)
			{
				FloatBlock item = DiscreteCosineTransform.ForwardDCT(blocksToProcess[i]);
				list.Add(item);
			}
			return list;
		}

		public static List<Block> Quantize(List<FloatBlock> encodingBlocks, QuantizationTable table)
		{
			List<Block> list = new List<Block>();
			for (int i = 0; i < encodingBlocks.Count; i++)
			{
				FloatBlock floatBlock = encodingBlocks[i];
				for (int j = 0; j < 64; j++)
				{
					FloatBlock floatBlock2;
					int index;
					(floatBlock2 = floatBlock)[index = j] = floatBlock2[index] * (float)table.Divisor[j];
				}
				list.Add(ZigZagScan.ZigZag(floatBlock).ToBlock());
			}
			return list;
		}

		public Block GetBlock(int mcuIndex, int rowIndex, int columnIndex)
		{
			int index = mcuIndex * (int)this.VSampleFactor * (int)this.HSampleFactor + rowIndex * (int)this.HSampleFactor + columnIndex;
			return this.GetBlock(index);
		}

		public Block GetBlock(int index)
		{
			while (index >= this.blocks.Count)
			{
				this.blocks.Add(new Block());
			}
			return this.blocks[index];
		}

		public void UpdateLastDCCoefficient(Block block)
		{
			this.lastDCCoefficient = block[0];
		}

		public void DequantizeAndInverse(QuantizationTable quantizationTable)
		{
			for (int i = 0; i < this.blocks.Count; i++)
			{
				quantizationTable.DequantizeAndInverse(this.blocks[i]);
			}
		}

		public void Dequantize(QuantizationTable table)
		{
			foreach (Block block in this.blocks)
			{
				for (int i = 0; i < 64; i++)
				{
					int num = ZigZagScan.ZigZagMap[i];
					Block block2;
					int index;
					(block2 = block)[index = num] = block2[index] * (int)table[i];
				}
			}
		}

		public void EncodeNextBlocks(JpegEncoder encoder, List<Block> quantizedBlocks)
		{
			int num = 0;
			for (int i = 0; i < (int)this.VSampleFactor; i++)
			{
				for (int j = 0; j < (int)this.HSampleFactor; j++)
				{
					encoder.ComponentEncoder.EncodeBlock(encoder, this, quantizedBlocks[num]);
					this.lastDCCoefficient = quantizedBlocks[num][0];
					num++;
				}
			}
		}

		public void IDCT()
		{
			for (int i = 0; i < this.blocks.Count; i++)
			{
				this.blocks[i] = DiscreteCosineTransform.InverseDCT(this.blocks[i]);
			}
		}

		public void InitializeDecoding(JpegDecoder decoder)
		{
			this.horizontalBlocksCount = (int)Math.Ceiling(Math.Ceiling((double)decoder.FrameHeader.Width / 8.0) * (double)this.HSampleFactor / (double)decoder.FrameHeader.MaxHSampleFactor);
			this.verticalBlocksCount = (int)Math.Ceiling(Math.Ceiling((double)decoder.FrameHeader.Height / 8.0) * (double)this.VSampleFactor / (double)decoder.FrameHeader.MaxVSampleFactor);
			this.mcuPerRow = this.CalculateMcuPerRow();
		}

		public void InitializeEncoding(JpegScan scan, JpegEncoder encoder)
		{
			int num = (int)(scan.MaxHSampleFactor / this.HSampleFactor);
			int num2 = (int)(scan.MaxVSampleFactor / this.VSampleFactor);
			this.completedWidth = ((encoder.Width % 8 != 0) ? ((int)Math.Ceiling((double)encoder.Width / 8.0) * 8) : encoder.Width) * num;
			this.horizontalBlocksCount = (int)Math.Ceiling((double)this.completedWidth / 8.0);
			this.completedHeight = ((encoder.Height % 8 != 0) ? ((int)Math.Ceiling((double)encoder.Height / 8.0) * 8) : encoder.Height) * num2;
			this.verticalBlocksCount = (int)Math.Ceiling((double)this.completedHeight / 8.0);
			this.mcuPerRow = this.CalculateMcuPerRow();
		}

		public List<Block> PrepareBlocks(byte[][,] raster, int blockStartYPos, int blockStartXPos)
		{
			List<Block> list = new List<Block>();
			for (int i = 0; i < (int)this.VSampleFactor; i++)
			{
				for (int j = 0; j < (int)this.HSampleFactor; j++)
				{
					Block item = new Block(raster[(int)(this.Id - 1)], blockStartXPos, blockStartYPos, j, i);
					list.Add(item);
				}
			}
			return list;
		}

		public byte[,] RasterizeInterleaved(JpegScan scan, int width, int height)
		{
			byte[,] array = new byte[height, width];
			int i = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = (int)(scan.MaxHSampleFactor / this.HSampleFactor);
			int num6 = (int)(scan.MaxVSampleFactor / this.VSampleFactor);
			int num7 = 8 * num5;
			int num8 = 8 * num6;
			while (i < this.blocks.Count)
			{
				int num9 = 0;
				int num10 = 0;
				if (num2 >= width)
				{
					num2 = 0;
					num3 += num;
				}
				for (int j = 0; j < (int)this.VSampleFactor; j++)
				{
					num9 = 0;
					for (int k = 0; k < (int)this.HSampleFactor; k++)
					{
						Block block = this.blocks[i++];
						JpegComponent.RasterizeBlockInterleaved(array, block, width, height, num2, num3, num7, num8, num5, num6);
						num9 += num7;
						num2 += num7;
						num10 = num8;
					}
					num3 += num10;
					num2 -= num9;
					num4 += num10;
				}
				num3 -= num4;
				num = num4;
				num4 = 0;
				num2 += num9;
			}
			return array;
		}

		public byte[,] RasterizeNonInterleaved(JpegScan scan, int width, int height)
		{
			byte[,] array = new byte[height, width];
			int i = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = (int)(scan.MaxHSampleFactor / this.HSampleFactor);
			int num6 = (int)(scan.MaxVSampleFactor / this.VSampleFactor);
			int num7 = 8 * num5;
			int num8 = 8 * num6;
			while (i < this.blocks.Count)
			{
				if (num2 >= width)
				{
					num2 = 0;
					num3 += num;
				}
				int num9 = 0;
				if (i < this.blocks.Count)
				{
					Block block = this.blocks[i++];
					JpegComponent.RasterizeBlockNonInterleaved(array, block, width, height, num2, num3, num7, num8);
					num9 += num7;
					num2 += num7;
					int num10 = num8;
					num3 += num10;
					num2 -= num9;
					num4 += num10;
					num3 -= num4;
					num = num4;
					num4 = 0;
					num2 += num9;
				}
			}
			return array;
		}

		public void Restart()
		{
			this.lastDCCoefficient = 0;
		}

		int CalculateMcuPerRow()
		{
			return (int)Math.Ceiling((double)this.HorizontalBlocksCount / (double)this.HSampleFactor);
		}

		static void RasterizeBlockInterleaved(byte[,] raster, Block block, int width, int height, int x, int y, int blockWidth, int blockHeight, int hFactor, int vFactor)
		{
			int num = blockHeight;
			if (y + num > height)
			{
				num = height - y;
			}
			int num2 = blockWidth;
			if (x + num2 > width)
			{
				num2 = width - x;
			}
			if (vFactor == 1 && hFactor == 1)
			{
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						raster[i + y, j + x] = (byte)block[i, j];
					}
				}
				return;
			}
			if (hFactor == 2 && vFactor == 2 && num2 == blockWidth && num == blockHeight)
			{
				for (int k = 0; k < 8; k++)
				{
					int num3 = k * 2 + y;
					for (int l = 0; l < 8; l++)
					{
						byte b = (byte)block[k, l];
						int num4 = l * 2 + x;
						raster[num3, num4] = b;
						raster[num3, num4 + 1] = b;
						raster[num3 + 1, num4] = b;
						raster[num3 + 1, num4 + 1] = b;
					}
				}
				return;
			}
			for (int m = 0; m < num; m++)
			{
				int row = m / vFactor;
				for (int n = 0; n < num2; n++)
				{
					int column = n / hFactor;
					raster[m + y, n + x] = (byte)block[row, column];
				}
			}
		}

		static void RasterizeBlockNonInterleaved(byte[,] raster, Block block, int width, int height, int x, int y, int blockWidth, int blockHeight)
		{
			int num = blockHeight;
			if (y + num > height)
			{
				num = height - y;
			}
			int num2 = blockWidth;
			if (x + num2 > width)
			{
				num2 = width - x;
			}
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					raster[i + y, j + x] = (byte)block[i, j];
				}
			}
		}

		readonly byte id;

		readonly byte quantizationTableId;

		readonly List<Block> blocks;

		readonly byte hSampleFactor;

		readonly byte vSampleFactor;

		readonly IHuffmanTablesOwner huffmanTablesOwner;

		int lastDCCoefficient;

		int completedHeight;

		int completedWidth;

		int verticalBlocksCount;

		int horizontalBlocksCount;

		int mcuPerRow;
	}
}
