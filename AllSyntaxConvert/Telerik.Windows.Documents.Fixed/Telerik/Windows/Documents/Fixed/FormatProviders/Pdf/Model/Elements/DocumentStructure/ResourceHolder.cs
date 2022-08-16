using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class ResourceHolder : IResourceHolder
	{
		public ResourceHolder()
		{
			this.resourcesHolder = new PdfResource();
		}

		public PdfResource Resources
		{
			get
			{
				return this.resourcesHolder;
			}
		}

		readonly PdfResource resourcesHolder;
	}
}
