using System;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns
{
	abstract class Shading : PdfObject
	{
		public Shading()
		{
			this.colorSpace = base.RegisterReferenceProperty<ColorSpaceObject>(new PdfPropertyDescriptor("ColorSpace"));
			this.background = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Background"));
		}

		public ColorSpaceObject ColorSpace
		{
			get
			{
				return this.colorSpace.GetValue();
			}
			set
			{
				this.colorSpace.SetValue(value);
			}
		}

		public PdfArray Background
		{
			get
			{
				return this.background.GetValue();
			}
			set
			{
				this.background.SetValue(value);
			}
		}

		public static Shading CreateInstance(PdfInt type)
		{
			Guard.ThrowExceptionIfNull<PdfInt>(type, "type");
			return Shading.CreateInstance(type.Value);
		}

		public static Shading CreateInstance(int type)
		{
			switch (type)
			{
			case 2:
				return new AxialShading();
			case 3:
				return new RadialShading();
			default:
				throw new NotSupportedShadingTypeException(type);
			}
		}

		public void CopyPropertiesFrom(IPdfContentExportContext context, Gradient gradient)
		{
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Gradient>(gradient, "gradient");
			Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop gradientStop = gradient.GradientStops.FirstOrDefault<Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop>();
			if (gradientStop != null)
			{
				this.ColorSpace = ColorSpaceManager.CreateColorSpaceObject(gradientStop.Color.ColorSpace);
			}
			else
			{
				this.ColorSpace = new DeviceRgbColorSpaceObject();
			}
			this.CopyPropertiesFromOverride(context, gradient);
		}

		public abstract ColorBase ToColor(PostScriptReader reader, IPdfContentImportContext context, Matrix matrix);

		protected abstract void CopyPropertiesFromOverride(IPdfContentExportContext context, Gradient gradient);

		protected ColorBase GetBackgroundColor(PostScriptReader reader, IPdfContentImportContext context)
		{
			if (this.Background == null)
			{
				return null;
			}
			return this.ColorSpace.GetColor(context, reader, this.Background).ToColor(reader, context);
		}

		readonly ReferenceProperty<ColorSpaceObject> colorSpace;

		readonly DirectProperty<PdfArray> background;
	}
}
