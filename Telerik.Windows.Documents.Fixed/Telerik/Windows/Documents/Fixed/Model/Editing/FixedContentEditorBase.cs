using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public abstract class FixedContentEditorBase
	{
		internal FixedContentEditorBase()
		{
			this.textProperties = new TextProperties();
			this.graphicProperties = new GraphicProperties();
		}

		public GraphicProperties GraphicProperties
		{
			get
			{
				return this.graphicProperties;
			}
		}

		public TextProperties TextProperties
		{
			get
			{
				return this.textProperties;
			}
		}

		internal ImageEditor ImageEditor
		{
			get
			{
				if (this.imageEditor == null)
				{
					this.imageEditor = new ImageEditor(this);
				}
				return this.imageEditor;
			}
		}

		internal GraphicsEditor GraphicsEditor
		{
			get
			{
				if (this.graphicsEditor == null)
				{
					this.graphicsEditor = new GraphicsEditor(this);
				}
				return this.graphicsEditor;
			}
		}

		public IDisposable SaveGraphicProperties()
		{
			this.GraphicProperties.Save();
			return new DisposableObject(new Action(this.RestoreGraphicProperties));
		}

		public void RestoreGraphicProperties()
		{
			this.GraphicProperties.Restore();
		}

		public IDisposable SaveTextProperties()
		{
			this.TextProperties.Save();
			return new DisposableObject(new Action(this.RestoreTextProperties));
		}

		public void RestoreTextProperties()
		{
			this.TextProperties.Restore();
		}

		public IDisposable SaveProperties()
		{
			this.textProperties.Save();
			this.graphicProperties.Save();
			return new DisposableObject(new Action(this.RestoreProperties));
		}

		public void RestoreProperties()
		{
			this.textProperties.Restore();
			this.graphicProperties.Restore();
		}

		internal void AppendForm(FormSource source)
		{
			Guard.ThrowExceptionIfNull<FormSource>(source, "source");
			this.Append(new Form
			{
				FormSource = source
			});
		}

		internal void AppendForm(FormSource source, double width, double height)
		{
			Guard.ThrowExceptionIfNull<FormSource>(source, "source");
			this.Append(new Form
			{
				FormSource = source,
				Width = width,
				Height = height
			});
		}

		internal void AppendForm(FormSource source, Size size)
		{
			Guard.ThrowExceptionIfNull<FormSource>(source, "source");
			this.Append(new Form
			{
				FormSource = source,
				Width = size.Width,
				Height = size.Height
			});
		}

		internal abstract void Append(PositionContentElement element);

		readonly TextProperties textProperties;

		readonly GraphicProperties graphicProperties;

		ImageEditor imageEditor;

		GraphicsEditor graphicsEditor;
	}
}
