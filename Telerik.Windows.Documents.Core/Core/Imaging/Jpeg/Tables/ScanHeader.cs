using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class ScanHeader : JpegTable
	{
		public byte BitPositionHigh { get; internal set; }

		public byte BitPositionLow { get; internal set; }

		public JpegScanComponent[] Components
		{
			get
			{
				return this.components;
			}
		}

		public override ushort Length
		{
			get
			{
				return (ushort)(6 + 2 * this.NumberOfComponents);
			}
		}

		public int NumberOfComponents
		{
			get
			{
				return this.components.Length;
			}
			internal set
			{
				this.components = new JpegScanComponent[value];
			}
		}

		public byte SpectralSelectionEnd { get; internal set; }

		public byte SpectralSelectionStart { get; internal set; }

		public void AddComponent(int index, JpegScanComponent component)
		{
			this.components[index] = component;
		}

		public override void Read(IJpegReader reader)
		{
			Guard.ThrowExceptionIfNull<IJpegReader>(reader, "reader");
			reader.Read16();
			byte b = reader.Read8();
			this.components = new JpegScanComponent[(int)b];
			for (int i = 0; i < this.components.Length; i++)
			{
				byte componentId = reader.Read8();
				byte dcTableId = reader.Read4();
				byte acTableId = reader.Read4();
				this.components[i] = new JpegScanComponent(componentId, dcTableId, acTableId);
			}
			this.SpectralSelectionStart = reader.Read8();
			this.SpectralSelectionEnd = reader.Read8();
			this.BitPositionHigh = reader.Read4();
			this.BitPositionLow = reader.Read4();
		}

		public override void Write(JpegWriter writer)
		{
			Guard.ThrowExceptionIfNull<JpegWriter>(writer, "writer");
			writer.Write16(this.Length);
			writer.Write8((byte)this.NumberOfComponents);
			for (int i = 0; i < this.components.Length; i++)
			{
				JpegScanComponent jpegScanComponent = this.components[i];
				writer.Write8(jpegScanComponent.ComponentId);
				writer.Write4(jpegScanComponent.DCTableId);
				writer.Write4(jpegScanComponent.ACTableId);
			}
			writer.Write8(this.SpectralSelectionStart);
			writer.Write8(this.SpectralSelectionEnd);
			writer.Write4(this.BitPositionHigh);
			writer.Write4(this.BitPositionLow);
		}

		JpegScanComponent[] components;
	}
}
