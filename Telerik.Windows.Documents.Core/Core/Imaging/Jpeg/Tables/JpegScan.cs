using System;
using System.Linq;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class JpegScan
	{
		public JpegComponent[] Components
		{
			get
			{
				return this.components;
			}
		}

		public byte MaxVSampleFactor
		{
			get
			{
				return this.maxVSampleFactor;
			}
		}

		public byte MaxHSampleFactor
		{
			get
			{
				return this.maxHSampleFactor;
			}
		}

		public int MinHorizontalBlocksCount
		{
			get
			{
				return this.minHorizontalBlocksCount;
			}
		}

		public int MinVerticalBlocksCount
		{
			get
			{
				return this.minVerticalBlocksCount;
			}
		}

		public JpegScan(JpegComponent[] components)
		{
			this.components = components;
			this.maxHSampleFactor = this.components.Max((JpegComponent c) => c.HSampleFactor);
			this.maxVSampleFactor = this.components.Max((JpegComponent c) => c.VSampleFactor);
		}

		public void InitializeEncoding(JpegEncoder encoder)
		{
			foreach (JpegComponent jpegComponent in this.components)
			{
				jpegComponent.InitializeEncoding(this, encoder);
			}
			this.minHorizontalBlocksCount = this.components.Min((JpegComponent c) => c.HorizontalBlocksCount);
			this.minVerticalBlocksCount = this.components.Min((JpegComponent c) => c.VerticalBlocksCount);
		}

		public bool TryGetComponentById(int id, out JpegComponent jpegComponent)
		{
			jpegComponent = this.components.FirstOrDefault((JpegComponent component) => (int)component.Id == id);
			return jpegComponent != null;
		}

		readonly JpegComponent[] components;

		readonly byte maxHSampleFactor;

		readonly byte maxVSampleFactor;

		int minHorizontalBlocksCount;

		int minVerticalBlocksCount;
	}
}
