using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	public interface IFixedPage
	{
		Rect MediaBox { get; }

		Rect CropBox { get; }

		Rotation Rotation { get; }
	}
}
