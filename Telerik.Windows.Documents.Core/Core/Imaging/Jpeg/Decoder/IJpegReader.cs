using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	 interface IJpegReader : IReader
	{
		JpegMarker ReadNextJpegMarker();

		IEnumerable<T> ReadJpegTables<T>() where T : JpegTable, new();
	}
}
