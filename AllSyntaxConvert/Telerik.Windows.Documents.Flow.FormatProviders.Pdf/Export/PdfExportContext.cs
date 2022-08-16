using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	class PdfExportContext
	{
		public PdfExportContext(RadFlowDocument document, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
			this.document = document;
			this.settings = settings;
			this.defaultValues = new DefaultValues(document);
			this.imageSourceMapping = new Dictionary<Telerik.Windows.Documents.Media.ImageSource, Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>();
		}

		public PdfExportSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public DefaultValues DefaultValues
		{
			get
			{
				return this.defaultValues;
			}
		}

		public Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource GetImageSource(Telerik.Windows.Documents.Media.ImageSource imageSource)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Media.ImageSource>(imageSource, "imageSource");
			Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource2;
			if (!this.imageSourceMapping.TryGetValue(imageSource, out imageSource2))
			{
				using (MemoryStream memoryStream = new MemoryStream(imageSource.Data))
				{
					imageSource2 = new Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource(memoryStream);
					this.imageSourceMapping[imageSource] = imageSource2;
				}
			}
			return imageSource2;
		}

		readonly DefaultValues defaultValues;

		readonly RadFlowDocument document;

		readonly PdfExportSettings settings;

		readonly Dictionary<Telerik.Windows.Documents.Media.ImageSource, Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource> imageSourceMapping;
	}
}
