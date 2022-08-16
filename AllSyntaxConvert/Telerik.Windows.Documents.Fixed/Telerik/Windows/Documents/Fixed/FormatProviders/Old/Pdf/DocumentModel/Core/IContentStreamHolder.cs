using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	interface IContentStreamHolder : IResourcesHolder
	{
		Rect Clip { get; }
	}
}
