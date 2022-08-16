using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	 interface IHuffmanTablesOwner
	{
		 HuffmanTable GetHuffmanTable(TableClass tableClass, int tableIndex);
	}
}
