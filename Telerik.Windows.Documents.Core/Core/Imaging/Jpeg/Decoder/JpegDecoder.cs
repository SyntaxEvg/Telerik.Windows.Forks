using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class JpegDecoder : JpegDecoderBase
	{
		public JpegFrame Frame { get; set; }

		bool ShouldScanByHorizontalBlocks
		{
			get
			{
				return base.ScanHeader.NumberOfComponents == 1 && base.FrameHeader.ImageComponents > 1;
			}
		}

		public JpegDecoder(byte[] data)
			: base(data)
		{
		}

		public JpegImage Decode()
		{
			if (!base.TryReadJpegStartOfImageMarker())
			{
				throw new InvalidOperationException("JPEG is not valid.");
			}
			JpegMarker jpegMarker;
			while ((jpegMarker = base.Reader.ReadNextJpegMarker()).MarkerType != JpegMarkerType.SOF)
			{
				jpegMarker.InterpretMarker(this);
			}
			this.DecodeFrame((StartOfFrameMarker)jpegMarker);
			return this.Rasterize(this.Frame);
		}

		void DecodeFrame(StartOfFrameMarker startOfFrameMarker)
		{
			startOfFrameMarker.InterpretMarker(this);
			base.ComponentDecoder = startOfFrameMarker.ScanDecoder;
			this.Frame = new JpegFrame(base.FrameHeader.Width, base.FrameHeader.Height);
			JpegMarker jpegMarker;
			while ((jpegMarker = base.Reader.ReadNextJpegMarker()).MarkerType != JpegMarkerType.EOI)
			{
				if (jpegMarker.MarkerType == JpegMarkerType.SOS)
				{
					this.DecodeScan((StartOfScanMarker)jpegMarker);
				}
				else
				{
					if (jpegMarker.MarkerType == JpegMarkerType.SOF)
					{
						throw new NotSupportedException("Jpeg images with more than one frame are not supported!");
					}
					jpegMarker.InterpretMarker(this);
				}
			}
		}

		void DecodeScan(StartOfScanMarker startOfScan)
		{
			startOfScan.InterpretMarker(this);
			if (this.Frame.Scan == null)
			{
				JpegComponent[] array = new JpegComponent[base.FrameHeader.ImageComponents];
				for (int i = 0; i < base.FrameHeader.ImageComponents; i++)
				{
					array[i] = this.CreateJpegComponent(base.ScanHeader.Components[i]);
				}
				this.Frame.Scan = new JpegScan(array);
			}
			JpegScan scan = this.Frame.Scan;
			RestartMarker.RestartDecoder(this);
			RestartMarker.RestartScan(scan);
			this.ApplyHuffmanTablesToComponents();
			int num = this.CalculateExpectedMcuCount();
			this.CalculateRestartInterval(num);
			JpegMarker jpegMarker = null;
			int num2 = 0;
			for (;;)
			{
				if (num2 >= num)
				{
					if ((jpegMarker != null && jpegMarker.MarkerType != JpegMarkerType.RST) || num2 >= num)
					{
						break;
					}
				}
				else
				{
					int num3 = 0;
					while (num3 < base.RestartInterval && num2 < num)
					{
						this.DecodeBlock(scan, num2);
						num2++;
						num3++;
					}
					this.TryPeekMarkerWhileScaning(out jpegMarker);
					if (jpegMarker != null && jpegMarker.MarkerType == JpegMarkerType.RST)
					{
						jpegMarker.InterpretMarker(this);
					}
					else
					{
						RestartMarker.RestartDecoder(this);
					}
					RestartMarker.RestartScan(scan);
				}
			}
		}

		int CalculateExpectedMcuCount()
		{
			if (base.ScanHeader.NumberOfComponents != 1)
			{
				return base.FrameHeader.BlocksPerLine * base.FrameHeader.BlocksPerColumn;
			}
			JpegComponent jpegComponent;
			if (this.Frame.Scan.TryGetComponentById((int)base.ScanHeader.Components[0].ComponentId, out jpegComponent))
			{
				return jpegComponent.HorizontalBlocksCount * jpegComponent.VerticalBlocksCount;
			}
			throw new InvalidOperationException("Frame scan does not have any components!");
		}

		void CalculateRestartInterval(int expectedMcuCount)
		{
			if (!base.IsRestartIntervalDefined)
			{
				base.RestartInterval = expectedMcuCount;
			}
			Guard.ThrowExceptionIfLessThanOrEqual<int>(0, base.RestartInterval, "RestartInterval");
		}

		void ApplyHuffmanTablesToComponents()
		{
			for (int i = 0; i < base.ScanHeader.Components.Length; i++)
			{
				JpegScanComponent jpegScanComponent = base.ScanHeader.Components[i];
				JpegComponent jpegComponent;
				if (this.Frame.Scan.TryGetComponentById((int)jpegScanComponent.ComponentId, out jpegComponent))
				{
					jpegComponent.ACTableId = (int)jpegScanComponent.ACTableId;
					jpegComponent.DCTableId = (int)jpegScanComponent.DCTableId;
				}
			}
		}

		bool TryPeekMarkerWhileScaning(out JpegMarker marker)
		{
			marker = null;
			byte b = base.Reader.Peek(0);
			byte b2 = base.Reader.Peek(1);
			bool flag = JpegMarker.IsJpegMarker(b, b2);
			if (flag)
			{
				bool flag2;
				do
				{
					flag2 = b2 == byte.MaxValue;
					if (flag2)
					{
						base.Reader.Read();
						b2 = base.Reader.Peek(1);
					}
				}
				while (flag2);
				marker = JpegMarker.GetMarker(b2);
				return true;
			}
			return false;
		}

		void DecodeBlock(JpegScan scan, int mcuIndex)
		{
			foreach (JpegScanComponent jpegScanComponent in base.ScanHeader.Components)
			{
				JpegComponent jpegComponent;
				if (scan.TryGetComponentById((int)jpegScanComponent.ComponentId, out jpegComponent))
				{
					bool flag = jpegComponent.HSampleFactor > 1 || jpegComponent.VSampleFactor > 1;
					if (this.ShouldScanByHorizontalBlocks && flag)
					{
						int num = mcuIndex / jpegComponent.HorizontalBlocksCount;
						int num2 = mcuIndex % jpegComponent.HorizontalBlocksCount;
						int num3 = num / (int)jpegComponent.VSampleFactor;
						int num4 = num2 / (int)jpegComponent.HSampleFactor;
						int rowIndex = num % (int)jpegComponent.VSampleFactor;
						int columnIndex = num2 % (int)jpegComponent.HSampleFactor;
						int mcuIndex2 = num3 * jpegComponent.McuPerRow + num4;
						Block block = jpegComponent.GetBlock(mcuIndex2, rowIndex, columnIndex);
						base.ComponentDecoder.DecodeBlock(this, jpegComponent, block);
					}
					else if (!flag || base.FrameHeader.ImageComponents == 1)
					{
						Block block2 = jpegComponent.GetBlock(mcuIndex);
						base.ComponentDecoder.DecodeBlock(this, jpegComponent, block2);
					}
					else
					{
						for (int j = 0; j < (int)jpegComponent.VSampleFactor; j++)
						{
							for (int k = 0; k < (int)jpegComponent.HSampleFactor; k++)
							{
								Block block3 = jpegComponent.GetBlock(mcuIndex, j, k);
								base.ComponentDecoder.DecodeBlock(this, jpegComponent, block3);
							}
						}
					}
				}
			}
		}

		JpegImage Rasterize(JpegFrame frame)
		{
			byte[][,] raster = new byte[frame.Scan.Components.Length][,];
			Action[] array = new Action[frame.Scan.Components.Length];
			for (int i = 0; i < frame.Scan.Components.Length; i++)
			{
				int index = i;
				Action action = delegate()
				{
					JpegComponent jpegComponent = frame.Scan.Components[index];
					QuantizationTable quantisationTable = this.GetQuantisationTable(jpegComponent.QuantizationTableId);
					jpegComponent.Dequantize(quantisationTable);
					jpegComponent.IDCT();
					if (frame.Scan.Components.Length == 1)
					{
						raster[index] = jpegComponent.RasterizeNonInterleaved(frame.Scan, frame.Width, frame.Height);
						return;
					}
					raster[index] = jpegComponent.RasterizeInterleaved(frame.Scan, frame.Width, frame.Height);
				};
				array[i] = action;
			}
			TasksHelper.DoAsync(array);
			return new JpegImage(frame.Width, frame.Height, base.ColorTransform, raster);
		}

		JpegComponent CreateJpegComponent(JpegScanComponent scanComponent)
		{
			JpegFrameComponent component = base.FrameHeader.GetComponent((int)scanComponent.ComponentId);
			JpegComponent jpegComponent = new JpegComponent(scanComponent.ComponentId, component.HSampleFactor, component.VSampleFactor, component.QuantizationTableId, this);
			jpegComponent.InitializeDecoding(this);
			return jpegComponent;
		}
	}
}
