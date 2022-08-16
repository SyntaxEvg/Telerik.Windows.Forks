using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class SupplementalEncoding
	{
		public void Read(CFFFontReader reader)
		{
			byte b = reader.ReadCard8();
			this.supplements = new Supplement[(int)b];
			for (int i = 0; i < (int)b; i++)
			{
				Supplement supplement = new Supplement();
				supplement.Read(reader);
				this.supplements[i] = supplement;
			}
		}

		Supplement[] supplements;
	}
}
