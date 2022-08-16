using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Data
{
	class CharCode
	{
		public CharCode(byte code)
			: this((int)code, 1)
		{
		}

		public CharCode(ushort code)
			: this((int)code, 2)
		{
		}

		public CharCode(int code, int size)
		{
			this.Code = code;
			this.Size = size;
		}

		public CharCode(byte[] code)
		{
			this.Code = BytesHelper.GetInt(code);
			this.Size = code.Length;
		}

		public int Code { get; set; }

		public int Size { get; set; }

		public byte[] ToBytes()
		{
			byte[] result = new byte[this.Size];
			BytesHelper.GetBytesReverse(this.Code, result);
			return result;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Code, this.Size);
		}

		public override bool Equals(object obj)
		{
			CharCode charCode = obj as CharCode;
			return charCode != null && this.Code == charCode.Code && this.Size == charCode.Size;
		}

		public override string ToString()
		{
			return BytesHelper.ToHexString(this.ToBytes());
		}
	}
}
