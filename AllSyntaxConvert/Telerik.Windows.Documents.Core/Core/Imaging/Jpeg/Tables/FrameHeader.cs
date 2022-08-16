using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class FrameHeader : JpegTable
	{
		public FrameHeader()
		{
			this.MaxHSampleFactor = 1;
			this.MaxVSampleFactor = 1;
		}

		public int ImageComponents
		{
			get
			{
				return (int)this.imageComponents;
			}
		}

		public int Height
		{
			get
			{
				return (int)this.numberOfLines;
			}
			internal set
			{
				this.numberOfLines = (ushort)value;
			}
		}

		public override ushort Length
		{
			get
			{
				return (ushort)(8 + 3 * this.imageComponents);
			}
		}

		public byte SamplePrecision { get; internal set; }

		public int Width
		{
			get
			{
				return (int)this.samplesPerLine;
			}
			internal set
			{
				this.samplesPerLine = (ushort)value;
			}
		}

		public byte MaxHSampleFactor { get; set; }

		public byte MaxVSampleFactor { get; set; }

		public int BlocksPerLine
		{
			get
			{
				if (this.blocksPerLine == null)
				{
					this.blocksPerLine = new int?((int)Math.Ceiling((double)this.Width / 8.0 / (double)this.MaxHSampleFactor));
				}
				return this.blocksPerLine.Value;
			}
		}

		public int BlocksPerColumn
		{
			get
			{
				if (this.blocksPerColumn == null)
				{
					this.blocksPerColumn = new int?((int)Math.Ceiling((double)this.Height / 8.0 / (double)this.MaxVSampleFactor));
				}
				return this.blocksPerColumn.Value;
			}
		}

		public void AddComponent(JpegFrameComponent component)
		{
			if (this.components == null)
			{
				this.components = new Dictionary<int, JpegFrameComponent>();
			}
			this.components[(int)component.ComponentId] = component;
		}

		public JpegFrameComponent GetComponent(int componentId)
		{
			return this.components[componentId];
		}

		public override void Read(IJpegReader reader)
		{
			Guard.ThrowExceptionIfNull<IJpegReader>(reader, "reader");
			reader.Read16();
			this.SamplePrecision = reader.Read8();
			this.numberOfLines = reader.Read16();
			this.samplesPerLine = reader.Read16();
			this.imageComponents = reader.Read8();
			this.components = new Dictionary<int, JpegFrameComponent>((int)this.imageComponents);
			for (int i = 0; i < (int)this.imageComponents; i++)
			{
				byte b = reader.Read8();
				byte b2 = reader.Read4();
				byte b3 = reader.Read4();
				byte qTableId = reader.Read8();
				this.MaxHSampleFactor = Math.Max(this.MaxHSampleFactor, b2);
				this.MaxVSampleFactor = Math.Max(this.MaxVSampleFactor, b3);
				this.components[(int)b] = new JpegFrameComponent(b, b2, b3, qTableId);
			}
		}

		public override void Write(JpegWriter writer)
		{
			Guard.ThrowExceptionIfNull<JpegWriter>(writer, "writer");
			this.imageComponents = (byte)this.components.Count;
			writer.Write16(this.Length);
			writer.Write8(this.SamplePrecision);
			writer.Write16(this.numberOfLines);
			writer.Write16(this.samplesPerLine);
			writer.Write8(this.imageComponents);
			foreach (JpegFrameComponent jpegFrameComponent in this.components.Values)
			{
				writer.Write8(jpegFrameComponent.ComponentId);
				writer.Write4(jpegFrameComponent.HSampleFactor);
				writer.Write4(jpegFrameComponent.VSampleFactor);
				writer.Write8(jpegFrameComponent.QuantizationTableId);
			}
		}

		ushort numberOfLines;

		ushort samplesPerLine;

		Dictionary<int, JpegFrameComponent> components;

		byte imageComponents;

		int? blocksPerLine;

		int? blocksPerColumn;
	}
}
