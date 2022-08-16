using System;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Text
{
	class CharInfo
	{
		internal CharInfo(string unicode, CharCode charCode)
		{
			Guard.ThrowExceptionIfNull<CharCode>(charCode, "charCode");
			this.unicode = unicode;
			this.charCode = charCode;
		}

		public CharCode CharCode
		{
			get
			{
				return this.charCode;
			}
		}

		public string Unicode
		{
			get
			{
				return this.unicode;
			}
		}

		public override int GetHashCode()
		{
			return this.CharCode.Code;
		}

		public override bool Equals(object obj)
		{
			CharInfo charInfo = obj as CharInfo;
			return charInfo != null && this.CharCode.Equals(charInfo.CharCode) && this.Unicode == charInfo.Unicode;
		}

		readonly string unicode;

		readonly CharCode charCode;
	}
}
