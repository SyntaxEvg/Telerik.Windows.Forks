using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class Name : TrueTypeTableBase
	{
		internal static Name ReadNameTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			Name name;
			switch (reader.ReadUShort())
			{
			case 0:
				name = new NameFormat0(fontSource);
				break;
			case 1:
				name = new NameFormat1(fontSource);
				break;
			default:
				return null;
			}
			name.Read(reader);
			return name;
		}

		public Name(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.NAME_TABLE;
			}
		}

		public abstract string FontFamily { get; }

		internal abstract string ReadName(ushort languageID, ushort nameID);
	}
}
