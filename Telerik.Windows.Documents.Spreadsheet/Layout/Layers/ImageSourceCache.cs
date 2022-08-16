using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class ImageSourceCache : IShapeSourceFactory<FloatingImage>
	{
		public ImageSourceCache()
		{
			this.imageSourceCache = new Dictionary<int, BitmapShapeSource>();
		}

		public IShapeSource GetShapeSource(FloatingImage shape)
		{
			int id = ((IResource)shape.ImageSource).Id;
			if (!this.imageSourceCache.ContainsKey(id))
			{
				MemoryStream stream = new MemoryStream(shape.Image.ImageSource.Data);
				BitmapSource imageSourceOrNoImagePlaceHolder = SpreadsheetHelper.GetImageSourceOrNoImagePlaceHolder(stream);
				this.imageSourceCache.Add(id, new BitmapShapeSource(imageSourceOrNoImagePlaceHolder));
			}
			return this.imageSourceCache[id];
		}

		readonly Dictionary<int, BitmapShapeSource> imageSourceCache;
	}
}
