using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	class RtfBinary : RtfElement
	{
		public RtfBinary(byte[] data)
		{
			this.Data = data;
		}

		public override RtfElementType Type
		{
			get
			{
				return RtfElementType.Binary;
			}
		}

		public byte[] Data { get; set; }

		public int Length
		{
			get
			{
				return this.Data.Length;
			}
		}
	}
}
