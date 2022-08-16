using System;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetImageImageSourceCommandContext : WorksheetCommandContextBase
	{
		public FloatingImage Image
		{
			get
			{
				return this.image;
			}
		}

		public ImageSource NewImageSource
		{
			get
			{
				return this.newImageSource;
			}
		}

		public ImageSource OldImageSource
		{
			get
			{
				return this.oldImageSource;
			}
			set
			{
				this.oldImageSource = value;
			}
		}

		public SetImageImageSourceCommandContext(Worksheet worksheet, FloatingImage image, ImageSource newSource)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingImage>(image, "image");
			Guard.ThrowExceptionIfNull<ImageSource>(newSource, "newSource");
			this.image = image;
			this.newImageSource = newSource;
		}

		readonly FloatingImage image;

		readonly ImageSource newImageSource;

		ImageSource oldImageSource;
	}
}
