using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class FixedContentEditor : FixedContentEditorBase
	{
		public FixedContentEditor(IContentRootElement root)
			: this(root, SimplePosition.Default)
		{
		}

		public FixedContentEditor(IContentRootElement root, IPosition initialPosition)
		{
			this.root = root;
			this.positionStack = new PositionStack(initialPosition);
			this.clippingStack = new Stack<Clipping>();
		}

		public IContentRootElement Root
		{
			get
			{
				return this.root;
			}
		}

		public IPosition Position
		{
			get
			{
				return this.positionStack.Position;
			}
			set
			{
				this.positionStack.Position = value;
			}
		}

		public Clipping Clipping
		{
			get
			{
				if (this.clippingStack.Count > 0)
				{
					return this.clippingStack.Peek();
				}
				return null;
			}
		}

		internal Marker CurrentMarker { get; set; }

		internal bool SupportsAnnotations
		{
			get
			{
				return this.Root.SupportsAnnotations;
			}
		}

		public IDisposable PushClipping(GeometryBase clip)
		{
			Clipping clipping = new Clipping();
			clipping.Clip = clip;
			clipping.Clipping = this.Clipping;
			this.clippingStack.Push(clipping);
			return new DisposableObject(delegate()
			{
				this.PopClipping();
			});
		}

		public IDisposable PushClipping(Rect clip)
		{
			return this.PushClipping(new RectangleGeometry
			{
				Rect = clip
			});
		}

		public Clipping PopClipping()
		{
			if (this.clippingStack.Count > 0)
			{
				return this.clippingStack.Pop();
			}
			return null;
		}

		public void DrawText(string text)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			Block block = new Block();
			block.TextProperties.CopyFrom(base.TextProperties);
			block.GraphicProperties.CopyFrom(base.GraphicProperties);
			block.InsertText(text);
			this.DrawBlock(block);
		}

		public void DrawText(string text, Size size)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			Block block = new Block();
			block.TextProperties.CopyFrom(base.TextProperties);
			block.GraphicProperties.CopyFrom(base.GraphicProperties);
			block.InsertText(text);
			this.DrawBlock(block, size);
		}

		public void DrawImage(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			base.ImageEditor.DrawImage(stream);
		}

		public void DrawImage(Stream stream, double width, double height)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			base.ImageEditor.DrawImage(stream, new double?(width), new double?(height));
		}

		public void DrawImage(Stream stream, Size size)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.DrawImage(stream, size.Width, size.Height);
		}

		public void DrawImage(ImageSource source)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			base.ImageEditor.DrawImage(source);
		}

		public void DrawImage(ImageSource source, Size size)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			this.DrawImage(source, size.Width, size.Height);
		}

		public void DrawImage(ImageSource source, double width, double height)
		{
			Guard.ThrowExceptionIfNull<ImageSource>(source, "source");
			base.ImageEditor.DrawImage(source, width, height);
		}

		public void DrawForm(FormSource source)
		{
			base.AppendForm(source);
		}

		public void DrawForm(FormSource source, double width, double height)
		{
			base.AppendForm(source, width, height);
		}

		public void DrawForm(FormSource source, Size size)
		{
			base.AppendForm(source, size);
		}

		public void DrawWidget<T>(FormField<T> parentField, Size annotationSize) where T : Widget
		{
			Guard.ThrowExceptionIfNull<FormField<T>>(parentField, "parentField");
			T t = parentField.Widgets.AddWidget();
			this.DrawWidgetInternal(t, annotationSize);
		}

		public void DrawWidget(RadioButtonField parentField, RadioOption option, Size annotationSize)
		{
			Guard.ThrowExceptionIfNull<RadioButtonField>(parentField, "parentField");
			RadioButtonWidget widget = parentField.Widgets.AddWidget(option);
			this.DrawWidgetInternal(widget, annotationSize);
		}

		public void DrawLine(Point point1, Point point2)
		{
			base.GraphicsEditor.DrawLine(point1, point2);
		}

		public void DrawRectangle(Rect rectangle)
		{
			base.GraphicsEditor.DrawRectangle(rectangle);
		}

		public void DrawEllipse(Point center, double radiusX, double radiusY)
		{
			base.GraphicsEditor.DrawEllipse(center, radiusX, radiusY);
		}

		public void DrawCircle(Point center, double radius)
		{
			this.DrawEllipse(center, radius, radius);
		}

		public void DrawPath(GeometryBase geometry)
		{
			base.GraphicsEditor.DrawPath(geometry);
		}

		public void DrawTable(Table table)
		{
			table.Draw(this, new Rect(0.0, 0.0, double.PositiveInfinity, double.PositiveInfinity));
		}

		public void DrawTable(Table table, double width)
		{
			table.Draw(this, new Rect(0.0, 0.0, width, double.PositiveInfinity));
		}

		public void DrawTable(Table table, Size size)
		{
			table.Draw(this, new Rect(0.0, 0.0, size.Width, size.Height));
		}

		public void DrawBlock(IBlockElement block)
		{
			Guard.ThrowExceptionIfNull<IBlockElement>(block, "block");
			this.DrawBlock(block, new Size(double.PositiveInfinity, double.PositiveInfinity));
		}

		public void DrawBlock(IBlockElement block, Size size)
		{
			Guard.ThrowExceptionIfNull<IBlockElement>(block, "block");
			block.Draw(this, new Rect(0.0, 0.0, size.Width, size.Height));
		}

		public void Draw(PositionContentElement element)
		{
			Guard.ThrowExceptionIfNull<PositionContentElement>(element, "element");
			this.Append(element);
		}

		public IDisposable SavePosition()
		{
			return this.positionStack.SavePosition();
		}

		public void RestorePosition()
		{
			this.positionStack.RestorePosition();
		}

		internal IDisposable PushTransformedClipping(Rect rect)
		{
			return this.PushClipping(this.Position.Matrix.TransformRectangle(rect));
		}

		internal void AddAnnotation(Annotation annotation)
		{
			Guard.ThrowExceptionIfNull<Annotation>(annotation, "annotation");
			if (this.SupportsAnnotations)
			{
				annotation.Rect = this.Position.Matrix.Transform(annotation.Rect);
				this.Root.Annotations.Add(annotation);
			}
		}

		internal override void Append(PositionContentElement element)
		{
			Guard.ThrowExceptionIfNull<PositionContentElement>(element, "element");
			element.Clipping = this.Clipping;
			element.Marker = this.CurrentMarker;
			element.Position = new MatrixPosition(element.Position.Matrix.MultiplyBy(this.Position.Matrix));
			this.Root.Content.Add(element);
		}

		void DrawWidgetInternal(Widget widget, Size annotationSize)
		{
			widget.Rect = new Rect(0.0, 0.0, annotationSize.Width, annotationSize.Height);
			widget.RecalculateContent();
			this.AddAnnotation(widget);
		}

		readonly IContentRootElement root;

		readonly PositionStack positionStack;

		readonly Stack<Clipping> clippingStack;
	}
}
