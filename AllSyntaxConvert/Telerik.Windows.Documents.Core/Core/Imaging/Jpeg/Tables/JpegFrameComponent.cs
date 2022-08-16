using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class JpegFrameComponent
	{
		public byte QuantizationTableId
		{
			get
			{
				return this.qTableId;
			}
		}

		public byte VSampleFactor
		{
			get
			{
				return this.vSampleFactor;
			}
		}

		public byte HSampleFactor
		{
			get
			{
				return this.hSampleFactor;
			}
		}

		public byte ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		public JpegFrameComponent(byte componentId, byte hSampleFactor, byte vSampleFactor, byte qTableId)
		{
			this.componentId = componentId;
			this.hSampleFactor = hSampleFactor;
			this.vSampleFactor = vSampleFactor;
			this.qTableId = qTableId;
		}

		readonly byte componentId;

		readonly byte hSampleFactor;

		readonly byte vSampleFactor;

		readonly byte qTableId;
	}
}
