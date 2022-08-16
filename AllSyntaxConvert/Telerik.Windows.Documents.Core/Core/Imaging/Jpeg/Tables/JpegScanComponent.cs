using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class JpegScanComponent
	{
		public byte ACTableId
		{
			get
			{
				return this.acTableId;
			}
		}

		public byte DCTableId
		{
			get
			{
				return this.dcTableId;
			}
		}

		public byte ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		public JpegScanComponent(byte componentId, byte dcTableId, byte acTableId)
		{
			this.componentId = componentId;
			this.dcTableId = dcTableId;
			this.acTableId = acTableId;
		}

		readonly byte componentId;

		readonly byte dcTableId;

		readonly byte acTableId;
	}
}
