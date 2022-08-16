using System;
using System.Text;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class QuantizationTable : JpegTable
	{
		public QuantizationTable()
		{
		}

		public QuantizationTable(byte tableIndex, byte[] byteData)
		{
			this.TableIndex = tableIndex;
			this.data = new ushort[byteData.Length];
			byteData.CopyTo(this.data, 0);
			this.divisor = new double[byteData.Length];
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					this.divisor[num] = 1.0 / ((double)this.data[num] * QuantizationTable.CosineScaleFactor[i] * QuantizationTable.CosineScaleFactor[j] * 8.0);
					num++;
				}
			}
		}

		public double[] Divisor
		{
			get
			{
				return this.divisor;
			}
		}

		public override ushort Length
		{
			get
			{
				return (ushort)(64 * (1 + this.Precision) + 1);
			}
		}

		public byte Precision { get; internal set; }

		public byte TableIndex { get; internal set; }

		public ushort this[int index]
		{
			get
			{
				return this.data[index];
			}
		}

		public override void Read(IJpegReader reader)
		{
			Guard.ThrowExceptionIfNull<IJpegReader>(reader, "reader");
			this.Precision = reader.Read4();
			this.TableIndex = reader.Read4();
			this.data = new ushort[64];
			for (int i = 0; i < this.data.Length; i++)
			{
				switch (this.Precision)
				{
				case 0:
					this.data[i] = (ushort)reader.Read8();
					break;
				case 1:
					this.data[i] = reader.Read16();
					break;
				}
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					stringBuilder.AppendFormat("{0} ", this.data[num++]);
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		public override void Write(JpegWriter writer)
		{
			Guard.ThrowExceptionIfNull<JpegWriter>(writer, "writer");
			writer.Write4(this.Precision);
			writer.Write4(this.TableIndex);
			for (int i = 0; i < this.data.Length; i++)
			{
				switch (this.Precision)
				{
				case 0:
					writer.Write8((byte)this.data[ZigZagScan.ZigZagMap[i]]);
					break;
				case 1:
					writer.Write16(this.data[ZigZagScan.ZigZagMap[i]]);
					break;
				}
			}
		}

		public void DequantizeAndInverse(Block block)
		{
			for (int i = 0; i < 64; i++)
			{
				block[i] *= (int)this[i];
			}
			for (int j = 0; j < 8; j++)
			{
				int num = 8 * j;
				if (block[1 + num] == 0 && block[2 + num] == 0 && block[3 + num] == 0 && block[4 + num] == 0 && block[5 + num] == 0 && block[6 + num] == 0 && block[7 + num] == 0)
				{
					int num2 = 5793 * block[num] + 512 >> 10;
					block[num] = num2;
					block[1 + num] = num2;
					block[2 + num] = num2;
					block[3 + num] = num2;
					block[4 + num] = num2;
					block[5 + num] = num2;
					block[6 + num] = num2;
					block[7 + num] = num2;
				}
				else
				{
					int num3 = 5793 * block[num] + 128 >> 8;
					int num4 = 5793 * block[4 + num] + 128 >> 8;
					int num5 = block[2 + num];
					int num6 = block[6 + num];
					int num7 = 2896 * (block[1 + num] - block[7 + num]) + 128 >> 8;
					int num8 = 2896 * (block[1 + num] + block[7 + num]) + 128 >> 8;
					int num9 = block[3 + num] << 4;
					int num10 = block[5 + num] << 4;
					int num2 = num3 - num4 + 1 >> 1;
					num3 = num3 + num4 + 1 >> 1;
					num4 = num2;
					num2 = num5 * 3784 + num6 * 1567 + 128 >> 8;
					num5 = num5 * 1567 - num6 * 3784 + 128 >> 8;
					num6 = num2;
					num2 = num7 - num10 + 1 >> 1;
					num7 = num7 + num10 + 1 >> 1;
					num10 = num2;
					num2 = num8 + num9 + 1 >> 1;
					num9 = num8 - num9 + 1 >> 1;
					num8 = num2;
					num2 = num3 - num6 + 1 >> 1;
					num3 = num3 + num6 + 1 >> 1;
					num6 = num2;
					num2 = num4 - num5 + 1 >> 1;
					num4 = num4 + num5 + 1 >> 1;
					num5 = num2;
					num2 = num7 * 2276 + num8 * 3406 + 2048 >> 12;
					num7 = num7 * 3406 - num8 * 2276 + 2048 >> 12;
					num8 = num2;
					num2 = num9 * 799 + num10 * 4017 + 2048 >> 12;
					num9 = num9 * 4017 - num10 * 799 + 2048 >> 12;
					num10 = num2;
					block[num] = num3 + num8;
					block[7 + num] = num3 - num8;
					block[1 + num] = num4 + num10;
					block[6 + num] = num4 - num10;
					block[2 + num] = num5 + num9;
					block[5 + num] = num5 - num9;
					block[3 + num] = num6 + num7;
					block[4 + num] = num6 - num7;
				}
			}
			for (int k = 0; k < 8; k++)
			{
				int num11 = k;
				if (block[8 + num11] == 0 && block[16 + num11] == 0 && block[24 + num11] == 0 && block[32 + num11] == 0 && block[40 + num11] == 0 && block[48 + num11] == 0 && block[56 + num11] == 0)
				{
					int num2 = 5793 * block[k] + 8192 >> 14;
					block[num11] = num2;
					block[8 + num11] = num2;
					block[16 + num11] = num2;
					block[24 + num11] = num2;
					block[32 + num11] = num2;
					block[40 + num11] = num2;
					block[48 + num11] = num2;
					block[56 + num11] = num2;
				}
				else
				{
					int num3 = 5793 * block[num11] + 2048 >> 12;
					int num4 = 5793 * block[32 + num11] + 2048 >> 12;
					int num5 = block[16 + num11];
					int num6 = block[48 + num11];
					int num7 = 2896 * (block[8 + num11] - block[56 + num11]) + 2048 >> 12;
					int num8 = 2896 * (block[8 + num11] + block[56 + num11]) + 2048 >> 12;
					int num9 = block[24 + num11];
					int num10 = block[40 + num11];
					int num2 = num3 - num4 + 1 >> 1;
					num3 = num3 + num4 + 1 >> 1;
					num4 = num2;
					num2 = num5 * 3784 + num6 * 1567 + 2048 >> 12;
					num5 = num5 * 1567 - num6 * 3784 + 2048 >> 12;
					num6 = num2;
					num2 = num7 - num10 + 1 >> 1;
					num7 = num7 + num10 + 1 >> 1;
					num10 = num2;
					num2 = num8 + num9 + 1 >> 1;
					num9 = num8 - num9 + 1 >> 1;
					num8 = num2;
					num2 = num3 - num6 + 1 >> 1;
					num3 = num3 + num6 + 1 >> 1;
					num6 = num2;
					num2 = num4 - num5 + 1 >> 1;
					num4 = num4 + num5 + 1 >> 1;
					num5 = num2;
					num2 = num7 * 2276 + num8 * 3406 + 2048 >> 12;
					num7 = num7 * 3406 - num8 * 2276 + 2048 >> 12;
					num8 = num2;
					num2 = num9 * 799 + num10 * 4017 + 2048 >> 12;
					num9 = num9 * 4017 - num10 * 799 + 2048 >> 12;
					num10 = num2;
					block[num11] = num3 + num8;
					block[56 + num11] = num3 - num8;
					block[8 + num11] = num4 + num10;
					block[48 + num11] = num4 - num10;
					block[16 + num11] = num5 + num9;
					block[40 + num11] = num5 - num9;
					block[24 + num11] = num6 + num7;
					block[32 + num11] = num6 - num7;
				}
			}
			for (int l = 0; l < 64; l++)
			{
				int num12 = block[l];
				num12 = ((num12 <= -2056) ? 0 : ((num12 >= 2024) ? 255 : (num12 + 2056 >> 4)));
				block[l] = num12;
			}
		}

		const int dctCos1 = 4017;

		const int dctSin1 = 799;

		const int dctCos3 = 3406;

		const int dctSin3 = 2276;

		const int dctCos6 = 1567;

		const int dctSin6 = 3784;

		const int dctSqrt2 = 5793;

		const int dctSqrt1d2 = 2896;

		static readonly double[] CosineScaleFactor = new double[] { 1.0, 1.387039845, 1.306562965, 1.175875602, 1.0, 0.785694958, 0.5411961, 0.275899379 };

		ushort[] data;

		double[] divisor;
	}
}
