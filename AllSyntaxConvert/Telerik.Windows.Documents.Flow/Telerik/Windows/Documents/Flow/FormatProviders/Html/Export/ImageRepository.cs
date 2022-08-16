using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class ImageRepository
	{
		public ImageRepository()
		{
			this.idGenerator = new IdGenerator();
			this.registeredImages = new Dictionary<int, string>();
		}

		public bool TryGetImageName(int sourceId, out string imageName)
		{
			return this.registeredImages.TryGetValue(sourceId, out imageName);
		}

		public string RegisterImage(int sourceId)
		{
			string text = "image" + this.idGenerator.GetNext().ToString(CultureInfo.InvariantCulture);
			this.registeredImages[sourceId] = text;
			return text;
		}

		const string ImageNamePrefix = "image";

		readonly IdGenerator idGenerator;

		readonly Dictionary<int, string> registeredImages;
	}
}
