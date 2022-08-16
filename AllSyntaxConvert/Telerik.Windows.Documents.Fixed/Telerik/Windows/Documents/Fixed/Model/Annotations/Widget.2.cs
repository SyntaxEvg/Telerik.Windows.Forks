using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public abstract class Widget<T> : Widget where T : DynamicAppearanceCharacteristics, new()
	{
		internal Widget(FormField field)
			: base(field)
		{
			this.AppearanceCharacteristics = Activator.CreateInstance<T>();
		}

		public T AppearanceCharacteristics
		{
			get
			{
				return this.appearanceCharacteristics;
			}
			set
			{
				Guard.ThrowExceptionIfNull<T>(value, "value");
				this.appearanceCharacteristics = value;
			}
		}

		internal double BorderThicknessIncludingStyleThickness
		{
			get
			{
				if (!this.HasBorderWidth)
				{
					return 0.0;
				}
				if (!Widget<T>.IsSimulated3dEffectStyle(base.Border.Style))
				{
					return base.Border.Width;
				}
				return base.Border.Width * 2.0;
			}
		}

		internal virtual Rect ButtonContentBox
		{
			get
			{
				double num = this.BorderThicknessIncludingStyleThickness + FixedDocumentDefaults.ButtonPadding;
				double num2 = base.Rect.Width - 2.0 * num;
				double num3 = base.Rect.Height - 2.0 * num;
				if (num2 > 0.0 && num3 > 0.0)
				{
					return new Rect(num, num, num2, num3);
				}
				return default(Rect);
			}
		}

		bool HasBorderWidth
		{
			get
			{
				return base.Border != null && base.Border.Style != AnnotationBorderStyle.None && base.Border.Width > 0.0;
			}
		}

		bool HasSimulatedBorder3dEffect
		{
			get
			{
				return this.HasBorderWidth && Widget<T>.IsSimulated3dEffectStyle(base.Border.Style);
			}
		}

		internal FormSource RecalculateAppearance(Action<FixedContentEditor> drawAppearanceContent)
		{
			if (base.Rect.Width == 0.0 || base.Rect.Height == 0.0)
			{
				return new FormSource();
			}
			Size size = new Size(base.Rect.Width, base.Rect.Height);
			FormSource formSource = new FormSource
			{
				Size = size
			};
			FixedContentEditor fixedContentEditor = new FixedContentEditor(formSource);
			using (fixedContentEditor.SaveGraphicProperties())
			{
				this.ApplyBackgroundProperties(fixedContentEditor);
				GeometryBase geometry = this.CalculateBackgroundGeometry(size);
				fixedContentEditor.DrawPath(geometry);
			}
			if (this.HasSimulatedBorder3dEffect)
			{
				this.DrawSimulatedBorder3dEffect(fixedContentEditor);
			}
			GeometryBase clip = this.CalculateContentClippingGeometry(size);
			using (fixedContentEditor.PushClipping(clip))
			{
				if (this.ContentMarkerName != null)
				{
					fixedContentEditor.CurrentMarker = new Marker(this.ContentMarkerName);
				}
				drawAppearanceContent(fixedContentEditor);
				fixedContentEditor.CurrentMarker = null;
			}
			return formSource;
		}

		internal virtual GeometryBase CalculateContentClippingGeometry(Size size)
		{
			double borderThicknessIncludingStyleThickness = this.BorderThicknessIncludingStyleThickness;
			Rect rect = new Rect(borderThicknessIncludingStyleThickness, borderThicknessIncludingStyleThickness, size.Width - 2.0 * borderThicknessIncludingStyleThickness, size.Height - 2.0 * borderThicknessIncludingStyleThickness);
			return new RectangleGeometry(rect);
		}

		internal virtual GeometryBase CalculateBackgroundGeometry(Size size)
		{
			double num = ((base.Border != null) ? base.Border.Width : 0.0);
			Rect rect = new Rect(num / 2.0, num / 2.0, size.Width - num, size.Height - num);
			return new RectangleGeometry(rect);
		}

		internal virtual void CalculateBorder3dEffectGeometries(Size size, out PathGeometry pathAbove, out PathGeometry pathBelow)
		{
			double width = base.Border.Width;
			double[] array = new double[]
			{
				width,
				2.0 * width,
				size.Width - 2.0 * width,
				size.Width - width
			};
			double[] array2 = new double[]
			{
				width,
				2.0 * width,
				size.Height - 2.0 * width,
				size.Height - width
			};
			Point startPoint = new Point(array[0], array2[0]);
			Point point = new Point(array[0], array2[3]);
			Point startPoint2 = new Point(array[3], array2[3]);
			Point point2 = new Point(array[3], array2[0]);
			Point point3 = new Point(array[1], array2[1]);
			Point point4 = new Point(array[1], array2[2]);
			Point point5 = new Point(array[2], array2[2]);
			Point point6 = new Point(array[2], array2[1]);
			pathAbove = new PathGeometry();
			PathFigure pathFigure = pathAbove.Figures.AddPathFigure();
			pathFigure.IsClosed = true;
			pathFigure.StartPoint = startPoint;
			pathFigure.Segments.AddLineSegment(point2);
			pathFigure.Segments.AddLineSegment(point6);
			pathFigure.Segments.AddLineSegment(point3);
			pathFigure.Segments.AddLineSegment(point4);
			pathFigure.Segments.AddLineSegment(point);
			pathBelow = new PathGeometry();
			PathFigure pathFigure2 = pathBelow.Figures.AddPathFigure();
			pathFigure2.IsClosed = true;
			pathFigure2.StartPoint = startPoint2;
			pathFigure2.Segments.AddLineSegment(point);
			pathFigure2.Segments.AddLineSegment(point4);
			pathFigure2.Segments.AddLineSegment(point5);
			pathFigure2.Segments.AddLineSegment(point6);
			pathFigure2.Segments.AddLineSegment(point2);
		}

		static bool IsSimulated3dEffectStyle(AnnotationBorderStyle style)
		{
			return style == AnnotationBorderStyle.Inset || style == AnnotationBorderStyle.Beveled;
		}

		void DrawSimulatedBorder3dEffect(FixedContentEditor editor)
		{
			PathGeometry geometry;
			PathGeometry geometry2;
			this.CalculateBorder3dEffectGeometries(editor.Root.Size, out geometry, out geometry2);
			using (editor.SaveGraphicProperties())
			{
				editor.GraphicProperties.IsFilled = true;
				editor.GraphicProperties.IsStroked = false;
				editor.GraphicProperties.StrokeThickness = 0.0;
				editor.GraphicProperties.FillColor = ((base.Border.Style == AnnotationBorderStyle.Inset) ? FixedDocumentDefaults.Border3dEffectDarkColor : FixedDocumentDefaults.BeveledBorderLightColor);
				editor.DrawPath(geometry);
				editor.GraphicProperties.FillColor = ((base.Border.Style == AnnotationBorderStyle.Inset) ? FixedDocumentDefaults.InsetBorderLightColor : FixedDocumentDefaults.Border3dEffectDarkColor);
				editor.DrawPath(geometry2);
			}
		}

		void ApplyBackgroundProperties(FixedContentEditor editor)
		{
			GraphicProperties graphicProperties = editor.GraphicProperties;
			T t = this.AppearanceCharacteristics;
			graphicProperties.IsFilled = t.Background != null;
			if (editor.GraphicProperties.IsFilled)
			{
				GraphicProperties graphicProperties2 = editor.GraphicProperties;
				T t2 = this.AppearanceCharacteristics;
				graphicProperties2.FillColor = t2.Background;
			}
			GraphicProperties graphicProperties3 = editor.GraphicProperties;
			bool isStroked;
			if (this.HasBorderWidth)
			{
				T t3 = this.AppearanceCharacteristics;
				isStroked = t3.BorderColor != null;
			}
			else
			{
				isStroked = false;
			}
			graphicProperties3.IsStroked = isStroked;
			if (editor.GraphicProperties.IsStroked)
			{
				GraphicProperties graphicProperties4 = editor.GraphicProperties;
				T t4 = this.AppearanceCharacteristics;
				graphicProperties4.StrokeColor = t4.BorderColor;
				editor.GraphicProperties.StrokeThickness = base.Border.Width;
				if (base.Border.Style == AnnotationBorderStyle.Dashed)
				{
					editor.GraphicProperties.StrokeDashArray = base.Border.DashArray;
				}
			}
		}

		T appearanceCharacteristics;
	}
}
