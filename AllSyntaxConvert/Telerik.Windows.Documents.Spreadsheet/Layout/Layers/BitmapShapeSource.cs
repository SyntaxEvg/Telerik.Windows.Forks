using System;
using System.Windows.Media.Imaging;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class BitmapShapeSource : IShapeSource
	{
		public BitmapShapeSource(BitmapSource bitmap)
		{
			this.bitmap = bitmap;
		}

		public BitmapSource Bitmap
		{
			get
			{
				return this.bitmap;
			}
		}

		bool IShapeSource.IsLocked { get; set; }

		readonly BitmapSource bitmap;
	}
}
