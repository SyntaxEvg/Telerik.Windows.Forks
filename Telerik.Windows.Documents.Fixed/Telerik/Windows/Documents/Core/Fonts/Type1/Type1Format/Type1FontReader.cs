using System;
using Telerik.Windows.Documents.Core.PostScript;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format
{
	class Type1FontReader : PostScriptReader
	{
		public Type1FontReader(byte[] data)
			: base(data)
		{
		}

		public static byte[] StripData(byte[] data)
		{
			if (Characters.IsValidHexCharacter((int)data[0]))
			{
				PfbFontReader pfbFontReader = new PfbFontReader();
				return pfbFontReader.ReadData(data);
			}
			return data;
		}
	}
}
