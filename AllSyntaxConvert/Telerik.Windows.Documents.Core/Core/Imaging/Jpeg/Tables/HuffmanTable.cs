using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class HuffmanTable : JpegTable
	{
		public HuffmanTable()
		{
		}

		public HuffmanTable(TableClass tableClass, byte tableIndex, byte[] lengths, byte[] values)
		{
			this.TableClass = tableClass;
			this.TableIndex = tableIndex;
			this.bits = new byte[lengths.Length];
			lengths.CopyTo(this.bits, 0);
			this.huffVals = new short[values.Length];
			values.CopyTo(this.huffVals, 0);
			this.GenerateSizeTable();
			this.GenerateCodeTable();
			this.GenerateEncoderTables();
		}

		public override ushort Length
		{
			get
			{
				return (ushort)(17 + this.bits.Sum((byte b) => (int)b));
			}
		}

		public byte TableIndex { get; set; }

		public TableClass TableClass { get; set; }

		public HuffmanTable Clone()
		{
			return new HuffmanTable
			{
				huffSize = (byte[])this.huffSize.Clone(),
				huffCode = (short[])this.huffCode.Clone(),
				bits = (byte[])this.bits.Clone(),
				huffVals = (short[])this.huffVals.Clone(),
				minCode = (short[])this.minCode.Clone(),
				maxCode = (short[])this.maxCode.Clone(),
				valPtr = (short[])this.valPtr.Clone()
			};
		}

		public int Decode(IJpegReader reader)
		{
			int num = 0;
			int num2 = reader.ReadBit();
			short num3;
			int num4;
			for (num3 = (short)num2; num3 > this.maxCode[num]; num3 = (short)num4)
			{
				num++;
				num3 = (short)(num3 << 1);
				num2 = reader.ReadBit();
				num4 = (int)(num3 | (short)num2);
			}
			int num5 = (int)this.huffVals[(int)(num3 + this.valPtr[num])];
			if (num5 < 0)
			{
				num5 = 256 + num5;
			}
			return num5;
		}

		public void Encode(JpegWriter writer, int nbits)
		{
			writer.WriteBits((int)this.encodingSize[nbits], (int)this.encodingCode[nbits]);
		}

		void GenerateSizeTable()
		{
			List<byte> list = new List<byte>();
			short num = 0;
			for (int i = 0; i < this.bits.Length; i++)
			{
				for (int j = 0; j < (int)this.bits[i]; j++)
				{
					list.Add((byte)(i + 1));
					num += 1;
				}
			}
			list.Add(0);
			this.huffSize = list.ToArray();
		}

		void GenerateCodeTable()
		{
			short num = 0;
			short num2 = 0;
			short num3 = (short)this.huffSize[0];
			List<short> list = new List<short>();
			int num4 = this.huffSize.Length - 1;
			short num5 = 0;
			while ((int)num5 < num4)
			{
				while ((short)this.huffSize[(int)num] == num3)
				{
					list.Add(num2);
					num2 += 1;
					num += 1;
				}
				num2 = (short)(num2 << 1);
				num3 += 1;
				num5 += 1;
			}
			this.huffCode = list.ToArray();
		}

		void GenerateEncoderTables()
		{
			this.encodingSize = new byte[256];
			this.encodingCode = new short[256];
			int num = this.huffSize.Length - 1;
			for (int i = 0; i < num; i++)
			{
				this.encodingCode[(int)this.huffVals[i]] = this.huffCode[i];
				this.encodingSize[(int)this.huffVals[i]] = this.huffSize[i];
			}
		}

		void GenerateDecoderTables()
		{
			this.minCode = new short[16];
			this.maxCode = new short[16];
			this.valPtr = new short[16];
			for (int i = 0; i < this.minCode.Length; i++)
			{
				this.minCode[i] = -1;
			}
			for (int j = 0; j < this.maxCode.Length; j++)
			{
				this.maxCode[j] = -1;
			}
			short num = 0;
			for (int k = 0; k < 16; k++)
			{
				if (this.bits[k] != 0)
				{
					this.valPtr[k] = num;
				}
				for (int l = 0; l < (int)this.bits[k]; l++)
				{
					if (this.huffCode[l + (int)num] < this.minCode[k] || this.minCode[k] == -1)
					{
						this.minCode[k] = this.huffCode[l + (int)num];
					}
					if (this.huffCode[l + (int)num] > this.maxCode[k])
					{
						this.maxCode[k] = this.huffCode[l + (int)num];
					}
				}
				if (this.minCode[k] != -1)
				{
					this.valPtr[k] = (short)(this.valPtr[k] - this.minCode[k]);
				}
				num += (short)this.bits[k];
			}
		}

		void InitializeHuffmanTables()
		{
			this.GenerateSizeTable();
			this.GenerateCodeTable();
			this.GenerateDecoderTables();
		}

		public override void Read(IJpegReader reader)
		{
			Guard.ThrowExceptionIfNull<IJpegReader>(reader, "reader");
			this.TableClass = (TableClass)reader.Read4();
			this.TableIndex = reader.Read4();
			this.bits = new byte[16];
			int num = 0;
			for (int i = 0; i < this.bits.Length; i++)
			{
				this.bits[i] = reader.Read8();
				num += (int)this.bits[i];
			}
			this.huffVals = new short[num];
			for (int j = 0; j < this.huffVals.Length; j++)
			{
				this.huffVals[j] = (short)reader.Read8();
			}
			this.InitializeHuffmanTables();
		}

		public override void Write(JpegWriter writer)
		{
			Guard.ThrowExceptionIfNull<JpegWriter>(writer, "writer");
			writer.Write4((byte)this.TableClass);
			writer.Write4(this.TableIndex);
			for (int i = 0; i < this.bits.Length; i++)
			{
				writer.Write8(this.bits[i]);
			}
			for (int j = 0; j < this.huffVals.Length; j++)
			{
				writer.Write8((byte)this.huffVals[j]);
			}
		}

		public const int HuffmanTableClasses = 2;

		const int HuffmanLengthsCount = 16;

		byte[] huffSize;

		short[] huffCode;

		byte[] bits;

		short[] huffVals;

		short[] minCode;

		short[] maxCode;

		short[] valPtr;

		byte[] encodingSize;

		short[] encodingCode;
	}
}
