using System;
using System.IO;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	interface IImageSizeDecoder
	{
		bool CanDecode(string imageTypeExtension);

		Size DecodeSize(Stream stream);

		Size DecodeSize(byte[] bytes);
	}
}
