using System;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Data;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Shapes
{
	public class FloatingImage : FloatingShapeBase, ICopyable<FloatingShapeBase>
	{
		public override FloatingShapeType FloatingShapeType
		{
			get
			{
				return FloatingShapeType.Image;
			}
		}

		public ImageSource ImageSource
		{
			get
			{
				return this.image.ImageSource;
			}
			set
			{
				SetImageImageSourceCommandContext context = new SetImageImageSourceCommandContext(base.Worksheet, this, value);
				WorkbookCommands.SetImageImageSource.Execute(context);
			}
		}

		public bool PreferRelativeToOriginalResize
		{
			get
			{
				return this.image.PreferRelativeToOriginalResize;
			}
			set
			{
				if (this.image.PreferRelativeToOriginalResize != value)
				{
					SetImagePreferRelativeToOriginalResizeCommandContext context = new SetImagePreferRelativeToOriginalResizeCommandContext(base.Worksheet, this, value);
					WorkbookCommands.SetImagePreferRelativeToOriginalResizeCommand.Execute(context);
				}
			}
		}

		internal Image Image
		{
			get
			{
				return this.image;
			}
		}

		public FloatingImage(Worksheet worksheet, CellIndex cellIndex, double offsetX, double offsetY)
			: this(worksheet, cellIndex, offsetX, offsetY, new Image())
		{
		}

		internal FloatingImage(Worksheet worksheet, CellIndex cellIndex, double offsetX, double offsetY, Image image)
			: base(worksheet, image, cellIndex, offsetX, offsetY)
		{
			this.image = image;
			this.image.ImageSourceChanged += this.Image_ImageSourceChanged;
		}

		internal override FloatingShapeBase Copy(Worksheet worksheet, CellIndex cellIndex, double offsetX, double offsetY)
		{
			return new FloatingImage(worksheet, cellIndex, offsetX, offsetY, new Image(this.Image));
		}

		FloatingShapeBase ICopyable<FloatingShapeBase>.Copy(CopyContext context)
		{
			FloatingShapeBase sourceFloatingShape = context.SourceFloatingShape;
			FloatingImage floatingImage = new FloatingImage(context.TargetWorksheet, sourceFloatingShape.CellIndex, sourceFloatingShape.OffsetX, sourceFloatingShape.OffsetY, this.Image.Clone(true));
			context.TargetWorksheet.Workbook.Resources.RegisterResource(floatingImage.ImageSource);
			return floatingImage;
		}

		void Image_ImageSourceChanged(object sender, ResourceChangedEventArgs e)
		{
			if (e.OldValue != null)
			{
				base.Worksheet.Workbook.Resources.ReleaseResource(e.OldValue);
			}
			if (e.NewValue != null)
			{
				base.Worksheet.Workbook.Resources.RegisterResource(e.NewValue);
			}
		}

		readonly Image image;
	}
}
