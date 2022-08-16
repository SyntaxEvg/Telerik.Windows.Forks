using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	class JpegEncoder : IDisposable, IHuffmanTablesOwner
	{
		public JpegEncoder(JpegImage jpegImage, JpegEncoderParameters encoderParameters)
			: this(encoderParameters)
		{
			this.jpegImage = jpegImage;
		}

		JpegEncoder(JpegEncoderParameters encoderParameters)
		{
			this.encoderParameters = encoderParameters;
			this.writer = new JpegWriter();
			this.InitializeTables();
		}

		public ScanEncoder ComponentEncoder { get; set; }

		public JpegFrame Frame { get; set; }

		public FrameHeader FrameHeader { get; set; }

		public int Height
		{
			get
			{
				return this.jpegImage.Height;
			}
		}

		public IEnumerable<HuffmanTable> HuffmanTables
		{
			get
			{
				HuffmanTable[,] array = this.huffmanTables;
				int upperBound = array.GetUpperBound(0);
				int upperBound2 = array.GetUpperBound(1);
				for (int i = array.GetLowerBound(0); i <= upperBound; i++)
				{
					for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
					{
						HuffmanTable table = array[i, j];
						if (table != null)
						{
							yield return table;
						}
					}
				}
				yield break;
			}
		}

		public JpegEncoderParameters Parameters
		{
			get
			{
				return this.encoderParameters;
			}
		}

		public IEnumerable<QuantizationTable> QuantizationTables
		{
			get
			{
				foreach (QuantizationTable table in this.quantizationTables)
				{
					if (table != null)
					{
						yield return table;
					}
				}
				yield break;
			}
		}

		public ushort RestartInterval { get; set; }

		public ScanHeader ScanHeader { get; set; }

		public int Width
		{
			get
			{
				return this.jpegImage.Width;
			}
		}

		public JpegWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		public void AddQuantizationTable(QuantizationTable table)
		{
			Guard.ThrowExceptionIfNull<QuantizationTable>(table, "table");
			this.quantizationTables[(int)table.TableIndex] = table;
		}

		public void AddHuffmanTable(HuffmanTable table)
		{
			Guard.ThrowExceptionIfNull<HuffmanTable>(table, "table");
			this.huffmanTables[(int)table.TableClass, (int)table.TableIndex] = table;
		}

		public void Dispose()
		{
			this.writer.Dispose();
		}

		public byte[] Encode()
		{
			JpegMarker marker = JpegMarker.GetMarker(JpegMarkerType.SOI);
			marker.WriteMarker(this);
			marker = JpegMarker.GetMarker(JpegMarkerType.APP0);
			marker.WriteMarker(this);
			StartOfFrameMarker sofmarkerForEncoding = JpegMarker.GetSOFMarkerForEncoding(this.Parameters.EncodingType);
			this.ComponentEncoder = sofmarkerForEncoding.ScanEncoder;
			sofmarkerForEncoding.ScanEncoder.PrepareQuantizationTables(this);
			marker = JpegMarker.GetMarker(JpegMarkerType.DQT);
			marker.WriteMarker(this);
			this.FrameHeader = new FrameHeader
			{
				Height = this.Height,
				SamplePrecision = (byte)this.encoderParameters.SamplePrecision,
				Width = this.Width
			};
			for (int i = 0; i < this.jpegImage.NumberOfComponents; i++)
			{
				JpegFrameComponent component = new JpegFrameComponent(JpegEncoder.ComponentId[i], JpegEncoder.HSamplingFactor[i], JpegEncoder.VSamplingFactor[i], JpegEncoder.QTableNumber[i]);
				this.FrameHeader.AddComponent(component);
			}
			sofmarkerForEncoding.WriteMarker(this);
			sofmarkerForEncoding.ScanEncoder.PrepareHuffmanTables(this);
			marker = JpegMarker.GetMarker(JpegMarkerType.DHT);
			marker.WriteMarker(this);
			this.ScanHeader = new ScanHeader
			{
				BitPositionHigh = 0,
				BitPositionLow = 0,
				NumberOfComponents = this.jpegImage.NumberOfComponents,
				SpectralSelectionStart = 0,
				SpectralSelectionEnd = 63
			};
			for (int j = 0; j < this.jpegImage.NumberOfComponents; j++)
			{
				JpegScanComponent component2 = new JpegScanComponent(JpegEncoder.ComponentId[j], JpegEncoder.DCTableNumber[j], JpegEncoder.ACTableNumber[j]);
				this.ScanHeader.AddComponent(j, component2);
			}
			marker = JpegMarker.GetMarker(JpegMarkerType.SOS);
			marker.WriteMarker(this);
			this.EncodeScan();
			marker = JpegMarker.GetMarker(JpegMarkerType.EOI);
			marker.WriteMarker(this);
			return this.writer.Data;
		}

		void EncodeBlocks(JpegScan scan)
		{
			foreach (JpegComponent jpegComponent in scan.Components)
			{
				jpegComponent.Restart();
			}
			for (int j = 0; j < scan.MinVerticalBlocksCount; j++)
			{
				int blockStartYPos = j * 8;
				for (int k = 0; k < scan.MinHorizontalBlocksCount; k++)
				{
					int blockStartXPos = k * 8;
					for (int l = 0; l < scan.Components.Length; l++)
					{
						JpegComponent jpegComponent2 = scan.Components[l];
						List<Block> blocksToProcess = jpegComponent2.PrepareBlocks(this.jpegImage.Raster, blockStartYPos, blockStartXPos);
						List<FloatBlock> encodingBlocks = JpegComponent.FDCT(blocksToProcess);
						QuantizationTable table = this.quantizationTables[jpegComponent2.QuantizationTableId];
						List<Block> quantizedBlocks = JpegComponent.Quantize(encodingBlocks, table);
						jpegComponent2.EncodeNextBlocks(this, quantizedBlocks);
					}
				}
			}
			this.Writer.Flush();
		}

		void EncodeScan()
		{
			JpegComponent[] array = new JpegComponent[this.FrameHeader.ImageComponents];
			for (int i = 0; i < this.FrameHeader.ImageComponents; i++)
			{
				array[i] = this.GetJpegComponent(this.ScanHeader.Components[i]);
			}
			JpegScan jpegScan = new JpegScan(array);
			jpegScan.InitializeEncoding(this);
			this.EncodeBlocks(jpegScan);
		}

		JpegComponent GetJpegComponent(JpegScanComponent scanComponent)
		{
			JpegFrameComponent component = this.FrameHeader.GetComponent((int)scanComponent.ComponentId);
			return new JpegComponent(scanComponent.ComponentId, component.HSampleFactor, component.VSampleFactor, component.QuantizationTableId, this)
			{
				ACTableId = (int)scanComponent.ACTableId,
				DCTableId = (int)scanComponent.DCTableId
			};
		}

		public HuffmanTable GetHuffmanTable(TableClass tableClass, int tableIndex)
		{
			return this.huffmanTables[(int)tableClass, tableIndex];
		}

		void InitializeTables()
		{
			this.quantizationTables = new QuantizationTable[4];
			this.huffmanTables = new HuffmanTable[2, 4];
		}

		internal static readonly byte[] DefaultChromianceQuantizationTable = new byte[]
		{
			17, 18, 24, 47, 99, 99, 99, 99, 18, 21,
			26, 66, 99, 99, 99, 99, 24, 26, 56, 99,
			99, 99, 99, 99, 47, 66, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99
		};

		internal static readonly byte[] DefaultLuminanceQuantizationTable = new byte[]
		{
			16, 11, 10, 16, 24, 40, 51, 61, 12, 12,
			14, 19, 26, 58, 60, 55, 14, 13, 16, 24,
			40, 57, 69, 56, 14, 17, 22, 29, 51, 87,
			80, 62, 18, 22, 37, 56, 68, 109, 103, 77,
			24, 35, 55, 64, 81, 104, 113, 92, 49, 64,
			78, 87, 103, 121, 120, 101, 72, 92, 95, 98,
			112, 100, 103, 99
		};

		internal static readonly byte[] StandardDCLuminanceLengths = new byte[]
		{
			0, 1, 5, 1, 1, 1, 1, 1, 1, 0,
			0, 0, 0, 0, 0, 0
		};

		internal static readonly byte[] StandardDCLuminanceValues = new byte[]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		};

		internal static readonly byte[] StandardDCChromianceLengths = new byte[]
		{
			0, 3, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 0, 0, 0, 0, 0
		};

		internal static readonly byte[] StandardDCChromianceValues = new byte[]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		};

		internal static readonly byte[] StandardACLuminanceLengths = new byte[]
		{
			0, 2, 1, 3, 3, 2, 4, 3, 5, 5,
			4, 4, 0, 0, 1, 125
		};

		internal static readonly byte[] StandardACLuminanceValues = new byte[]
		{
			1, 2, 3, 0, 4, 17, 5, 18, 33, 49,
			65, 6, 19, 81, 97, 7, 34, 113, 20, 50,
			129, 145, 161, 8, 35, 66, 177, 193, 21, 82,
			209, 240, 36, 51, 98, 114, 130, 9, 10, 22,
			23, 24, 25, 26, 37, 38, 39, 40, 41, 42,
			52, 53, 54, 55, 56, 57, 58, 67, 68, 69,
			70, 71, 72, 73, 74, 83, 84, 85, 86, 87,
			88, 89, 90, 99, 100, 101, 102, 103, 104, 105,
			106, 115, 116, 117, 118, 119, 120, 121, 122, 131,
			132, 133, 134, 135, 136, 137, 138, 146, 147, 148,
			149, 150, 151, 152, 153, 154, 162, 163, 164, 165,
			166, 167, 168, 169, 170, 178, 179, 180, 181, 182,
			183, 184, 185, 186, 194, 195, 196, 197, 198, 199,
			200, 201, 202, 210, 211, 212, 213, 214, 215, 216,
			217, 218, 225, 226, 227, 228, 229, 230, 231, 232,
			233, 234, 241, 242, 243, 244, 245, 246, 247, 248,
			249, 250
		};

		internal static readonly byte[] StandardACChromianceLengths = new byte[]
		{
			0, 2, 1, 2, 4, 4, 3, 4, 7, 5,
			4, 4, 0, 1, 2, 119
		};

		internal static readonly byte[] StandardACChromianceValues = new byte[]
		{
			0, 1, 2, 3, 17, 4, 5, 33, 49, 6,
			18, 65, 81, 7, 97, 113, 19, 34, 50, 129,
			8, 20, 66, 145, 161, 177, 193, 9, 35, 51,
			82, 240, 21, 98, 114, 209, 10, 22, 36, 52,
			225, 37, 241, 23, 24, 25, 26, 38, 39, 40,
			41, 42, 53, 54, 55, 56, 57, 58, 67, 68,
			69, 70, 71, 72, 73, 74, 83, 84, 85, 86,
			87, 88, 89, 90, 99, 100, 101, 102, 103, 104,
			105, 106, 115, 116, 117, 118, 119, 120, 121, 122,
			130, 131, 132, 133, 134, 135, 136, 137, 138, 146,
			147, 148, 149, 150, 151, 152, 153, 154, 162, 163,
			164, 165, 166, 167, 168, 169, 170, 178, 179, 180,
			181, 182, 183, 184, 185, 186, 194, 195, 196, 197,
			198, 199, 200, 201, 202, 210, 211, 212, 213, 214,
			215, 216, 217, 218, 226, 227, 228, 229, 230, 231,
			232, 233, 234, 242, 243, 244, 245, 246, 247, 248,
			249, 250
		};

		static readonly byte[] ComponentId = new byte[] { 1, 2, 3, 4 };

		static readonly byte[] HSamplingFactor = new byte[] { 1, 1, 1, 1 };

		static readonly byte[] VSamplingFactor = new byte[] { 1, 1, 1, 1 };

		static readonly byte[] QTableNumber = new byte[] { 0, 1, 1, 1 };

		static readonly byte[] DCTableNumber = new byte[] { 0, 1, 1, 1 };

		static readonly byte[] ACTableNumber = new byte[] { 0, 1, 1, 1 };

		readonly JpegImage jpegImage;

		readonly JpegWriter writer;

		readonly JpegEncoderParameters encoderParameters;

		QuantizationTable[] quantizationTables;

		HuffmanTable[,] huffmanTables;
	}
}
