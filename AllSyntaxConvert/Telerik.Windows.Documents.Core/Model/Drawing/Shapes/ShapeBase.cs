using System;
using System.Windows;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Model.Drawing.Shapes
{
	public abstract class ShapeBase
	{
		internal ShapeBase()
		{
			this.outline = new Outline();
		}

		protected ShapeBase(ShapeBase other)
			: this()
		{
			this.name = other.name;
			this.size = other.size;
			this.hasSize = other.hasSize;
			this.isVerticallyFlipped = other.isVerticallyFlipped;
			this.isHorizontallyFlipped = other.isHorizontallyFlipped;
			this.rotationAngle = other.rotationAngle;
			this.lockAspectRatio = other.LockAspectRatio;
			this.outline = other.outline.Clone();
			if (other.fill != null)
			{
				this.fill = other.fill.Clone();
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
				}
			}
		}

		public double Width
		{
			get
			{
				return this.Size.Width;
			}
			set
			{
				Guard.ThrowExceptionIfNaN(value, "value");
				Guard.ThrowExceptionIfPositiveInfinity(value, "value");
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				if (this.size.Width != value)
				{
					this.EnsureSizeIsNotEmpy();
					this.size.Width = value;
					this.hasSize = true;
				}
			}
		}

		public double Height
		{
			get
			{
				return this.Size.Height;
			}
			set
			{
				Guard.ThrowExceptionIfNaN(value, "value");
				Guard.ThrowExceptionIfPositiveInfinity(value, "value");
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				if (this.size.Height != value)
				{
					this.EnsureSizeIsNotEmpy();
					this.size.Height = value;
					this.hasSize = true;
				}
			}
		}

		public Size Size
		{
			get
			{
				if (!this.hasSize)
				{
					this.InitializeSize();
				}
				return this.size;
			}
			set
			{
				Guard.ThrowExceptionIfNaN(value.Width, "value.Width");
				Guard.ThrowExceptionIfNaN(value.Height, "value.Height");
				Guard.ThrowExceptionIfPositiveInfinity(value.Width, "value.Width");
				Guard.ThrowExceptionIfPositiveInfinity(value.Height, "value.Height");
				this.size = value;
				this.hasSize = true;
			}
		}

		internal Size SizeInternal
		{
			get
			{
				return this.size;
			}
		}

		protected virtual void InitializeSize()
		{
			this.Size = ShapeBase.DefaultSize;
		}

		public bool IsVerticallyFlipped
		{
			get
			{
				return this.isVerticallyFlipped;
			}
			set
			{
				if (this.isVerticallyFlipped != value)
				{
					this.isVerticallyFlipped = value;
				}
			}
		}

		public bool IsHorizontallyFlipped
		{
			get
			{
				return this.isHorizontallyFlipped;
			}
			set
			{
				if (this.isHorizontallyFlipped != value)
				{
					this.isHorizontallyFlipped = value;
				}
			}
		}

		public double RotationAngle
		{
			get
			{
				return this.rotationAngle;
			}
			set
			{
				if (this.rotationAngle != value)
				{
					this.rotationAngle = value;
				}
			}
		}

		public bool LockAspectRatio
		{
			get
			{
				return this.lockAspectRatio;
			}
			set
			{
				if (this.lockAspectRatio != value)
				{
					this.lockAspectRatio = value;
				}
			}
		}

		public Fill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				if (this.fill != value)
				{
					this.fill = value;
				}
			}
		}

		public Outline Outline
		{
			get
			{
				return this.outline;
			}
		}

		internal int Id { get; set; }

		public void SetWidth(bool respectLockAspectRatio, double width)
		{
			if (respectLockAspectRatio && this.hasSize)
			{
				this.ChangeHeightAccordingToAspectRatio(width);
			}
			this.Width = width;
		}

		public void SetHeight(bool respectLockAspectRatio, double height)
		{
			if (respectLockAspectRatio && this.hasSize)
			{
				this.ChangeWidthAccordingToAspectRatio(height);
			}
			this.Height = height;
		}

		void ChangeHeightAccordingToAspectRatio(double newWidth)
		{
			if (this.LockAspectRatio && this.Width != 0.0)
			{
				double num = this.Height / this.Width;
				this.size.Height = newWidth * num;
			}
		}

		void ChangeWidthAccordingToAspectRatio(double newHeight)
		{
			if (this.LockAspectRatio && this.Height != 0.0)
			{
				double num = this.Width / this.Height;
				this.size.Width = newHeight * num;
			}
		}

		void EnsureSizeIsNotEmpy()
		{
			if (this.size.IsEmpty)
			{
				this.Size = ShapeBase.DefaultSize;
			}
		}

		internal static readonly Size DefaultSize = new Size(1.0, 1.0);

		string name;

		Size size = Size.Empty;

		bool hasSize;

		bool isVerticallyFlipped;

		bool isHorizontallyFlipped;

		double rotationAngle;

		bool lockAspectRatio;

		readonly Outline outline;

		Fill fill;
	}
}
