using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class EffectExtentElement : DrawingElementBase
	{
		public EffectExtentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.left = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("l", Converters.EmuToDipConverter, false));
			this.right = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("r", Converters.EmuToDipConverter, false));
			this.top = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("t", Converters.EmuToDipConverter, false));
			this.bottom = base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("b", Converters.EmuToDipConverter, false));
		}

		public override string ElementName
		{
			get
			{
				return "effectExtent";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public double Left
		{
			get
			{
				return this.left.Value;
			}
			set
			{
				this.left.Value = value;
			}
		}

		public double Right
		{
			get
			{
				return this.right.Value;
			}
			set
			{
				this.right.Value = value;
			}
		}

		public double Top
		{
			get
			{
				return this.top.Value;
			}
			set
			{
				this.top.Value = value;
			}
		}

		public double Bottom
		{
			get
			{
				return this.bottom.Value;
			}
			set
			{
				this.bottom.Value = value;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeAnchorBase anchor)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(anchor, "anchor");
			this.CopyPropertiesFrom(context, anchor.Shape);
			this.Left = 0.0;
			this.Right = 0.0;
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeInlineBase inline)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeInlineBase>(inline, "inline");
			this.CopyPropertiesFrom(context, inline.Shape);
			this.Right -= this.Left;
			this.Left = 0.0;
		}

		static double Min(params double[] args)
		{
			double num = args[0];
			for (int i = 1; i < args.Length; i++)
			{
				if (num > args[i])
				{
					num = args[i];
				}
			}
			return num;
		}

		static double Max(params double[] args)
		{
			double num = args[0];
			for (int i = 1; i < args.Length; i++)
			{
				if (num < args[i])
				{
					num = args[i];
				}
			}
			return num;
		}

		void CopyPropertiesFrom(IDocxExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			Rect rect = new Rect(0.0, 0.0, shape.Width, shape.Height);
			Matrix matrix = default(Matrix);
			matrix.RotateAt(shape.RotationAngle, rect.Left + rect.Width / 2.0, rect.Top + rect.Height / 2.0);
			Point point = matrix.Transform(rect.TopLeft);
			Point point2 = matrix.Transform(rect.TopRight);
			Point point3 = matrix.Transform(rect.BottomLeft);
			Point point4 = matrix.Transform(rect.BottomRight);
			Rect rect2 = new Rect(new Point(EffectExtentElement.Min(new double[] { point.X, point2.X, point3.X, point4.X }), EffectExtentElement.Min(new double[] { point.Y, point2.Y, point3.Y, point4.Y })), new Point(EffectExtentElement.Max(new double[] { point.X, point2.X, point3.X, point4.X }), EffectExtentElement.Max(new double[] { point.Y, point2.Y, point3.Y, point4.Y })));
			this.Left = Math.Abs(rect.Left - rect2.Left);
			this.Right = Math.Abs(rect.Right - rect2.Right);
			this.Top = Math.Abs(rect.Top - rect2.Top);
			this.Bottom = Math.Abs(rect.Bottom - rect2.Bottom);
		}

		readonly ConvertedOpenXmlAttribute<double> left;

		readonly ConvertedOpenXmlAttribute<double> right;

		readonly ConvertedOpenXmlAttribute<double> top;

		readonly ConvertedOpenXmlAttribute<double> bottom;
	}
}
