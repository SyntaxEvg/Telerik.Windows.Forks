using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class NameFormat1 : Name
	{
		public NameFormat1(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public override string FontFamily
		{
			get
			{
				return "";
			}
		}

		internal override string ReadName(ushort languageID, ushort nameID)
		{
			return "";
		}

		public override void Read(OpenTypeFontReader reader)
		{
		}
	}
}
