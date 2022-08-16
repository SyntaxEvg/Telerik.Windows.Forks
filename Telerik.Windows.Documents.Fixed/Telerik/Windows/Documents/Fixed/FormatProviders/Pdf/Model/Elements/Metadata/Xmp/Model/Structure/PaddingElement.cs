using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure
{
	class PaddingElement : XmpElementBase
	{
		public override string Name
		{
			get
			{
				return string.Empty;
			}
		}

		public override void Write(IXmpWriter writer)
		{
			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					writer.WriteRaw(" ");
				}
				writer.WriteRawLine();
			}
		}

		const int Rows = 24;

		const int RowSize = 100;

		const string EmptyCharacter = " ";
	}
}
