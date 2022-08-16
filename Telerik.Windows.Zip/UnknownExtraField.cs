using System;

namespace Telerik.Windows.Zip
{
	class UnknownExtraField : ExtraFieldBase
	{
		public UnknownExtraField(ushort fieldHeaderId)
		{
			this.headerId = fieldHeaderId;
		}

		protected override ushort HeaderId
		{
			get
			{
				return this.headerId;
			}
		}

		byte[] ExtraFieldData { get; set; }

		protected override byte[] GetBlock()
		{
			return this.ExtraFieldData;
		}

		protected override void ParseBlock(byte[] extraData)
		{
			this.ExtraFieldData = extraData;
		}

		ushort headerId;
	}
}
