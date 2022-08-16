using System;
using System.Windows;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Shapes
{
	public abstract class FloatingShapeBase
	{
		public abstract FloatingShapeType FloatingShapeType { get; }

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public CellIndex CellIndex
		{
			get
			{
				return this.cellIndex;
			}
			set
			{
				if (this.cellIndex != value)
				{
					SetShapePositionCommandContext context = new SetShapePositionCommandContext(this.worksheet, this, value, this.OffsetX, this.OffsetY);
					WorkbookCommands.SetShapePosition.Execute(context);
				}
			}
		}

		public double OffsetX
		{
			get
			{
				return this.offsetX;
			}
			set
			{
				if (this.offsetX != value)
				{
					SetShapePositionCommandContext context = new SetShapePositionCommandContext(this.worksheet, this, this.CellIndex, value, this.OffsetY);
					WorkbookCommands.SetShapePosition.Execute(context);
				}
			}
		}

		public double OffsetY
		{
			get
			{
				return this.offsetY;
			}
			set
			{
				if (this.offsetY != value)
				{
					SetShapePositionCommandContext context = new SetShapePositionCommandContext(this.worksheet, this, this.CellIndex, this.OffsetX, value);
					WorkbookCommands.SetShapePosition.Execute(context);
				}
			}
		}

		public int Id
		{
			get
			{
				return this.shape.Id;
			}
		}

		public string Name
		{
			get
			{
				return this.shape.Name;
			}
			set
			{
				if (this.shape.Name != value)
				{
					SetShapeNameCommandContext context = new SetShapeNameCommandContext(this.worksheet, this, value);
					WorkbookCommands.SetShapeName.Execute(context);
					this.OnShapeChanged();
				}
			}
		}

		public double Width
		{
			get
			{
				return this.shape.Width;
			}
			set
			{
				this.SetWidth(false, value, false);
			}
		}

		public double Height
		{
			get
			{
				return this.shape.Height;
			}
			set
			{
				this.SetHeight(false, value, false);
			}
		}

		public bool IsHorizontallyFlipped
		{
			get
			{
				return this.shape.IsHorizontallyFlipped;
			}
			set
			{
				if (this.shape.IsHorizontallyFlipped != value)
				{
					SetShapeFlipCommandContext context = new SetShapeFlipCommandContext(this.worksheet, this, value, this.IsVerticallyFlipped);
					WorkbookCommands.SetShapeFlip.Execute(context);
					this.OnShapeChanged();
				}
			}
		}

		public bool IsVerticallyFlipped
		{
			get
			{
				return this.shape.IsVerticallyFlipped;
			}
			set
			{
				if (this.shape.IsVerticallyFlipped != value)
				{
					SetShapeFlipCommandContext context = new SetShapeFlipCommandContext(this.worksheet, this, this.IsHorizontallyFlipped, value);
					WorkbookCommands.SetShapeFlip.Execute(context);
					this.OnShapeChanged();
				}
			}
		}

		public double RotationAngle
		{
			get
			{
				return this.shape.RotationAngle;
			}
			set
			{
				this.SetRotationAngle(value, false);
			}
		}

		public bool LockAspectRatio
		{
			get
			{
				return this.shape.LockAspectRatio;
			}
			set
			{
				if (this.shape.LockAspectRatio != value)
				{
					SetShapeLockAspectRatioCommandContext context = new SetShapeLockAspectRatioCommandContext(this.worksheet, this, value);
					WorkbookCommands.SetShapeLockAspectRatio.Execute(context);
				}
			}
		}

		public Fill Fill
		{
			get
			{
				return this.shape.Fill;
			}
			set
			{
				this.shape.Fill = value;
			}
		}

		public Outline Outline
		{
			get
			{
				return this.shape.Outline;
			}
		}

		internal ShapeBase Shape
		{
			get
			{
				return this.shape;
			}
		}

		internal CellIndex CellIndexInternal
		{
			get
			{
				return this.cellIndex;
			}
			set
			{
				if (this.cellIndex != value)
				{
					this.cellIndex = value;
					this.OnShapeChanged();
				}
			}
		}

		internal double OffsetXInternal
		{
			get
			{
				return this.offsetX;
			}
			set
			{
				if (this.offsetX != value)
				{
					this.offsetX = value;
					this.OnShapeChanged();
				}
			}
		}

		internal double OffsetYInternal
		{
			get
			{
				return this.offsetY;
			}
			set
			{
				if (this.offsetY != value)
				{
					this.offsetY = value;
					this.OnShapeChanged();
				}
			}
		}

		internal FloatingShapeBase(Worksheet worksheet, ShapeBase shape, CellIndex cellIndex, double offsetX, double offsetY)
		{
			this.worksheet = worksheet;
			this.CellIndexInternal = cellIndex;
			this.OffsetXInternal = offsetX;
			this.OffsetYInternal = offsetY;
			this.shape = shape;
			this.cellIndexAdjustedForRotation = this.DoesRotationAngleRequireCellIndexChange();
		}

		public void SetWidth(bool respectLockAspectRatio, double width, bool adjustCellIndex = false)
		{
			SetShapeSizeCommandContext context = new SetShapeSizeCommandContext(this.worksheet, this, width, respectLockAspectRatio, adjustCellIndex);
			WorkbookCommands.SetShapeWidth.Execute(context);
		}

		internal void SetWidthInternal(bool respectLockAspectRatio, double width, bool adjustCellIndex)
		{
			if (!this.Width.EqualsDouble(width))
			{
				if (adjustCellIndex)
				{
					double width2 = this.Width;
					double height = this.Height;
					RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
					Point shapeTopLeftConsideringAdjustmentForRotation = ShapesResizeHelper.GetShapeTopLeftConsideringAdjustmentForRotation(this, worksheetLayout);
					this.Shape.SetWidth(respectLockAspectRatio, width);
					double height2 = this.Height;
					this.AdjustCellIndexAndOffsetForSize(worksheetLayout, width2, height, width, height2, shapeTopLeftConsideringAdjustmentForRotation);
				}
				else
				{
					this.Shape.SetWidth(respectLockAspectRatio, width);
				}
				this.OnShapeChanged();
			}
		}

		public void SetHeight(bool respectLockAspectRatio, double height, bool adjustCellIndex = false)
		{
			SetShapeSizeCommandContext context = new SetShapeSizeCommandContext(this.worksheet, this, height, respectLockAspectRatio, adjustCellIndex);
			WorkbookCommands.SetShapeHeight.Execute(context);
		}

		internal void SetHeightInternal(bool respectLockAspectRatio, double height, bool adjustCellIndex)
		{
			if (!this.Height.EqualsDouble(height))
			{
				if (adjustCellIndex)
				{
					double height2 = this.Height;
					double width = this.Width;
					RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
					Point shapeTopLeftConsideringAdjustmentForRotation = ShapesResizeHelper.GetShapeTopLeftConsideringAdjustmentForRotation(this, worksheetLayout);
					this.Shape.SetHeight(respectLockAspectRatio, height);
					double width2 = this.Width;
					this.AdjustCellIndexAndOffsetForSize(worksheetLayout, width, height2, width2, height, shapeTopLeftConsideringAdjustmentForRotation);
				}
				else
				{
					this.Shape.SetHeight(respectLockAspectRatio, height);
				}
				this.OnShapeChanged();
			}
		}

		public void SetRotationAngle(double rotationAngle, bool adjustCellIndex = false)
		{
			SetShapeRotationAngleCommandContext context = new SetShapeRotationAngleCommandContext(this.worksheet, this, rotationAngle, adjustCellIndex);
			WorkbookCommands.SetShapeRotationAngle.Execute(context);
		}

		internal void SetRotationAngleInternal(double rotationAngle, bool adjustCellIndex)
		{
			if (this.shape.RotationAngle != rotationAngle)
			{
				this.Shape.RotationAngle = rotationAngle;
				this.OnShapeChanged();
				if (adjustCellIndex)
				{
					this.AdjustCellIndexAndOffsetForRotation();
					return;
				}
				this.cellIndexAdjustedForRotation = this.DoesRotationAngleRequireCellIndexChange();
			}
		}

		internal abstract FloatingShapeBase Copy(Worksheet worksheet, CellIndex cellIndex, double offsetX, double offsetY);

		void AdjustCellIndexAndOffsetForRotation()
		{
			bool flag = this.DoesRotationAngleRequireCellIndexChange();
			if ((flag && !this.cellIndexAdjustedForRotation) || (!flag && this.cellIndexAdjustedForRotation))
			{
				RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
				Point pointFromCellIndexAndOffset = worksheetLayout.GetPointFromCellIndexAndOffset(this.CellIndex, this.OffsetX, this.OffsetY);
				double width = this.shape.Width;
				double height = this.shape.Height;
				Point point;
				if (flag)
				{
					point = ShapesResizeHelper.AdjustTopLeftForRotation(pointFromCellIndexAndOffset, width, height);
					this.cellIndexAdjustedForRotation = true;
				}
				else
				{
					point = ShapesResizeHelper.ReverseAdjustmentForRotation(pointFromCellIndexAndOffset, width, height);
					this.cellIndexAdjustedForRotation = false;
				}
				point.X = Math.Max(point.X, 0.0);
				point.Y = Math.Max(point.Y, 0.0);
				double offsetXInternal;
				double offsetYInternal;
				CellIndex cellIndexAndOffsetFromPoint = worksheetLayout.GetCellIndexAndOffsetFromPoint(point, out offsetXInternal, out offsetYInternal);
				this.CellIndexInternal = cellIndexAndOffsetFromPoint;
				this.OffsetXInternal = offsetXInternal;
				this.OffsetYInternal = offsetYInternal;
			}
		}

		void AdjustCellIndexAndOffsetForSize(RadWorksheetLayout worksheetLayout, double oldWidth, double oldHeight, double newWidth, double newHeight, Point oldTopleftPoint)
		{
			Rect shapeCurrentRectangle = new Rect(oldTopleftPoint.X, oldTopleftPoint.Y, oldWidth, oldHeight);
			Point point = ShapesResizeHelper.DetermineNewTopLeftWhenResized(shapeCurrentRectangle, this.RotationAngle, newWidth, newHeight, false, false);
			if (this.cellIndexAdjustedForRotation)
			{
				point = ShapesResizeHelper.AdjustTopLeftForRotation(point, newWidth, newHeight);
			}
			point = ShapesResizeHelper.AdjustBoundingRectangleWhenOutOfSpreadsheet(worksheetLayout, new Rect(point, new Size(newWidth, newHeight)), point);
			point = ShapesResizeHelper.AdjustPointWhenOutOfSpreadsheet(worksheetLayout, point);
			double offsetXInternal;
			double offsetYInternal;
			CellIndex cellIndexAndOffsetFromPoint = worksheetLayout.GetCellIndexAndOffsetFromPoint(point, out offsetXInternal, out offsetYInternal);
			this.CellIndexInternal = cellIndexAndOffsetFromPoint;
			this.OffsetXInternal = offsetXInternal;
			this.OffsetYInternal = offsetYInternal;
		}

		internal bool DoesRotationAngleRequireCellIndexChange()
		{
			if (this.FloatingShapeType == FloatingShapeType.Chart)
			{
				return false;
			}
			double num = SpreadsheetHelper.RestrictRotationAngle(this.RotationAngle);
			return (num >= 45.0 && num < 135.0) || (num >= 225.0 && num < 315.0);
		}

		public event EventHandler ShapeChanged;

		protected void OnShapeChanged()
		{
			if (this.ShapeChanged != null)
			{
				this.ShapeChanged(this, EventArgs.Empty);
			}
		}

		readonly ShapeBase shape;

		readonly Worksheet worksheet;

		CellIndex cellIndex;

		double offsetX;

		double offsetY;

		bool cellIndexAdjustedForRotation;
	}
}
