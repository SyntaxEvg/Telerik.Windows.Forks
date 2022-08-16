using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model.Charts;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Shapes
{
	public class ShapeCollection : IEnumerable<FloatingShapeBase>, IEnumerable
	{
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public IEnumerable<FloatingImage> Images
		{
			get
			{
				foreach (FloatingShapeBase s in this.innerList)
				{
					if (s.FloatingShapeType == FloatingShapeType.Image)
					{
						yield return s as FloatingImage;
					}
				}
				yield break;
			}
		}

		public IEnumerable<FloatingChartShape> Charts
		{
			get
			{
				foreach (FloatingShapeBase s in this.innerList)
				{
					if (s.FloatingShapeType == FloatingShapeType.Chart)
					{
						yield return s as FloatingChartShape;
					}
				}
				yield break;
			}
		}

		internal BeginEndCounter ShapeChangeCounter
		{
			get
			{
				return this.shapeChangeCounter;
			}
		}

		internal CellRange UsedCellRange
		{
			get
			{
				if (this.usedCellRangeInvalidated)
				{
					this.RecalculateUsedCellRange();
					this.usedCellRangeInvalidated = false;
				}
				return this.usedCellRange;
			}
		}

		internal Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public ShapeCollection(Worksheet worksheet)
		{
			this.innerList = new List<FloatingShapeBase>();
			this.worksheet = worksheet;
			this.usedCellRangeInvalidated = true;
			this.collectionChangeCounter = new BeginEndCounter(new Action(this.OnChanged));
			this.shapeChangeCounter = new BeginEndCounter(new Action(this.OnShapeChanged));
		}

		public void Add(FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			if (shape.Worksheet != this.Worksheet)
			{
				throw new InvalidOperationException("The worksheet of the shape and of the collection it belongs to should be the same.");
			}
			AddRemoveShapeCommandContext context = new AddRemoveShapeCommandContext(this.Worksheet, shape);
			this.Worksheet.ExecuteCommand<AddRemoveShapeCommandContext>(WorkbookCommands.AddShape, context);
		}

		public void Add(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			Guard.ThrowExceptionIfContainsNull<FloatingShapeBase>(shapes, "shapes");
			if (shapes.Any((FloatingShapeBase shape) => shape.Worksheet != this.Worksheet))
			{
				throw new InvalidOperationException("The worksheet of the shape and of the collection it belongs to should be the same.");
			}
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.collectionChangeCounter.BeginUpdate();
				this.Worksheet.BeginUndoGroup();
			}, delegate()
			{
				this.Worksheet.EndUndoGroup();
				this.collectionChangeCounter.EndUpdate();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase shape2 in shapes)
				{
					AddRemoveShapeCommandContext context = new AddRemoveShapeCommandContext(this.Worksheet, shape2);
					this.Worksheet.ExecuteCommand<AddRemoveShapeCommandContext>(WorkbookCommands.AddShape, context);
				}
			}
		}

		public FloatingChartShape AddChart(CellIndex cellIndex, CellRange chartDataRange, params ChartType[] chartTypes)
		{
			return this.AddChart(cellIndex, chartDataRange, SeriesRangesOrientation.Automatic, chartTypes);
		}

		public FloatingChartShape AddChart(CellIndex cellIndex, CellRange chartDataRange, SeriesRangesOrientation seriesRangesOrientation, params ChartType[] chartTypes)
		{
			FloatingChartShape floatingChartShape = new FloatingChartShape(this.Worksheet, cellIndex, chartDataRange, seriesRangesOrientation, chartTypes);
			floatingChartShape.Width = SpreadsheetDefaultValues.ChartDefaultSize.Width;
			floatingChartShape.Height = SpreadsheetDefaultValues.ChartDefaultSize.Height;
			this.Add(floatingChartShape);
			return floatingChartShape;
		}

		public bool Remove(FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			AddRemoveShapeCommandContext context = new AddRemoveShapeCommandContext(this.Worksheet, shape);
			return this.Worksheet.ExecuteCommand<AddRemoveShapeCommandContext>(WorkbookCommands.RemoveShape, context);
		}

		public void Remove(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			Guard.ThrowExceptionIfContainsNull<FloatingShapeBase>(shapes, "shapes");
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.collectionChangeCounter.BeginUpdate();
				this.Worksheet.BeginUndoGroup();
			}, delegate()
			{
				this.Worksheet.EndUndoGroup();
				this.collectionChangeCounter.EndUpdate();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase shape in shapes)
				{
					AddRemoveShapeCommandContext context = new AddRemoveShapeCommandContext(this.Worksheet, shape);
					this.Worksheet.ExecuteCommand<AddRemoveShapeCommandContext>(WorkbookCommands.RemoveShape, context);
				}
			}
		}

		public void Clear()
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.collectionChangeCounter.BeginUpdate();
				this.Worksheet.BeginUndoGroup();
			}, delegate()
			{
				this.Worksheet.EndUndoGroup();
				this.collectionChangeCounter.EndUpdate();
			});
			using (updateScope)
			{
				List<FloatingShapeBase> list = new List<FloatingShapeBase>(this.innerList);
				foreach (FloatingShapeBase shape in list)
				{
					AddRemoveShapeCommandContext context = new AddRemoveShapeCommandContext(this.Worksheet, shape);
					this.Worksheet.ExecuteCommand<AddRemoveShapeCommandContext>(WorkbookCommands.RemoveShape, context);
				}
			}
		}

		public FloatingShapeBase this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<int>(0, this.innerList.Count - 1, index, "index");
				FloatingShapeBase shape = this.innerList[index];
				UpdateScope updateScope = new UpdateScope(delegate()
				{
					this.collectionChangeCounter.BeginUpdate();
					this.Worksheet.BeginUndoGroup();
				}, delegate()
				{
					this.Worksheet.EndUndoGroup();
					this.collectionChangeCounter.EndUpdate();
				});
				using (updateScope)
				{
					this.Remove(shape);
					this.Add(value);
					this.MoveShapesToIndices(new Dictionary<FloatingShapeBase, int> { { value, index } });
				}
			}
		}

		internal void AddInternal(FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.OnChanging();
			this.SetUniqueIdAndNameToShape(shape);
			this.innerList.Add(shape);
			shape.ShapeChanged += this.Shape_ShapeChanged;
			this.DoOnChanged();
		}

		internal void RemoveInternal(FloatingShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<FloatingShapeBase>(shape, "shape");
			this.OnChanging();
			this.innerList.Remove(shape);
			shape.ShapeChanged -= this.Shape_ShapeChanged;
			this.DoOnChanged();
		}

		internal void BringForward(IEnumerable<FloatingShapeBase> shapes)
		{
			MoveShapeInDepthCommandContext context = new MoveShapeInDepthCommandContext(this.Worksheet, shapes);
			this.Worksheet.ExecuteCommand<MoveShapeInDepthCommandContext>(WorkbookCommands.BringShapeForward, context);
		}

		internal void BringForwardInternal(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			this.OnChanging();
			IOrderedEnumerable<FloatingShapeBase> orderedEnumerable = from x in shapes
				orderby this.GetZIndex(x) descending
				select x;
			foreach (FloatingShapeBase item in orderedEnumerable)
			{
				int num = this.innerList.IndexOf(item);
				if (num != this.innerList.Count - 1)
				{
					this.innerList.Remove(item);
					this.innerList.Insert(num + 1, item);
				}
			}
			this.DoOnChanged();
		}

		internal void BringToFront(IEnumerable<FloatingShapeBase> shapes)
		{
			MoveShapeInDepthCommandContext context = new MoveShapeInDepthCommandContext(this.Worksheet, shapes);
			this.Worksheet.ExecuteCommand<MoveShapeInDepthCommandContext>(WorkbookCommands.BringShapeToFront, context);
		}

		internal void BringToFrontInternal(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			this.OnChanging();
			IOrderedEnumerable<FloatingShapeBase> orderedEnumerable = from x in shapes
				orderby this.GetZIndex(x)
				select x;
			foreach (FloatingShapeBase item in orderedEnumerable)
			{
				int num = this.innerList.IndexOf(item);
				if (num == this.innerList.Count - 1)
				{
					return;
				}
				this.innerList.Remove(item);
				this.innerList.Insert(this.innerList.Count, item);
			}
			this.DoOnChanged();
		}

		internal void SendBackward(IEnumerable<FloatingShapeBase> shapes)
		{
			MoveShapeInDepthCommandContext context = new MoveShapeInDepthCommandContext(this.Worksheet, shapes);
			this.Worksheet.ExecuteCommand<MoveShapeInDepthCommandContext>(WorkbookCommands.SendShapeBackward, context);
		}

		internal void SendBackwardInternal(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			this.OnChanging();
			IOrderedEnumerable<FloatingShapeBase> orderedEnumerable = from x in shapes
				orderby this.GetZIndex(x)
				select x;
			foreach (FloatingShapeBase item in orderedEnumerable)
			{
				int num = this.innerList.IndexOf(item);
				if (num != 0)
				{
					this.innerList.Remove(item);
					this.innerList.Insert(num - 1, item);
				}
			}
			this.DoOnChanged();
		}

		internal void SendToBack(IEnumerable<FloatingShapeBase> shapes)
		{
			MoveShapeInDepthCommandContext context = new MoveShapeInDepthCommandContext(this.Worksheet, shapes);
			this.Worksheet.ExecuteCommand<MoveShapeInDepthCommandContext>(WorkbookCommands.SendShapeToBack, context);
		}

		internal void SendToBackInternal(IEnumerable<FloatingShapeBase> shapes)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(shapes, "shapes");
			this.OnChanging();
			IOrderedEnumerable<FloatingShapeBase> orderedEnumerable = from x in shapes
				orderby this.GetZIndex(x) descending
				select x;
			foreach (FloatingShapeBase item in orderedEnumerable)
			{
				if (this.innerList.IndexOf(item) == 0)
				{
					return;
				}
				this.innerList.Remove(item);
				this.innerList.Insert(0, item);
			}
			this.DoOnChanged();
		}

		internal void MoveShapesToIndices(Dictionary<FloatingShapeBase, int> shapeToIndex)
		{
			Guard.ThrowExceptionIfNull<Dictionary<FloatingShapeBase, int>>(shapeToIndex, "shapeToIndex");
			this.OnChanging();
			foreach (FloatingShapeBase item in shapeToIndex.Keys)
			{
				if (!this.innerList.Contains(item))
				{
					throw new ArgumentException("One or more of the shapes is not present in the collection.");
				}
				this.innerList.Remove(item);
			}
			IOrderedEnumerable<KeyValuePair<FloatingShapeBase, int>> orderedEnumerable = from x in shapeToIndex
				orderby x.Value
				select x;
			foreach (KeyValuePair<FloatingShapeBase, int> keyValuePair in orderedEnumerable)
			{
				FloatingShapeBase key = keyValuePair.Key;
				int value = keyValuePair.Value;
				this.innerList.Insert(value, key);
			}
			this.DoOnChanged();
		}

		internal void Rotate(IEnumerable<FloatingShapeBase> shapes, double degree, bool adjustCellIndex)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					double rotationAngle = SpreadsheetHelper.RestrictRotationAngle(floatingShapeBase.RotationAngle + degree);
					floatingShapeBase.SetRotationAngle(rotationAngle, adjustCellIndex);
				}
			}
		}

		internal void SetRotation(IEnumerable<FloatingShapeBase> shapes, double degree, bool adjustCellIndex)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					floatingShapeBase.SetRotationAngle(degree, adjustCellIndex);
				}
			}
		}

		internal void FlipHorizontal(IEnumerable<FloatingShapeBase> shapes)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					floatingShapeBase.IsHorizontallyFlipped = !floatingShapeBase.IsHorizontallyFlipped;
				}
			}
		}

		internal void FlipVertical(IEnumerable<FloatingShapeBase> shapes)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					floatingShapeBase.IsVerticallyFlipped = !floatingShapeBase.IsVerticallyFlipped;
				}
			}
		}

		internal void SetWidth(IEnumerable<FloatingShapeBase> shapes, double width, bool respectLockAspectRatio, bool adjustCellIndex)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					floatingShapeBase.SetWidth(respectLockAspectRatio, width, adjustCellIndex);
				}
			}
		}

		internal void SetHeight(IEnumerable<FloatingShapeBase> shapes, double height, bool respectLockAspectRatio, bool adjustCellIndex)
		{
			UpdateScope updateScope = new UpdateScope(delegate()
			{
				this.worksheet.BeginUndoGroup();
				this.shapeChangeCounter.BeginUpdate();
			}, delegate()
			{
				this.shapeChangeCounter.EndUpdate();
				this.worksheet.EndUndoGroup();
			});
			using (updateScope)
			{
				foreach (FloatingShapeBase floatingShapeBase in shapes)
				{
					floatingShapeBase.SetHeight(respectLockAspectRatio, height, adjustCellIndex);
				}
			}
		}

		public IEnumerator<FloatingShapeBase> GetEnumerator()
		{
			return ((IEnumerable<FloatingShapeBase>)this.innerList).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		public IEnumerable<FloatingShapeBase> ReverseEnumerate()
		{
			for (int i = this.innerList.Count<FloatingShapeBase>() - 1; i >= 0; i--)
			{
				FloatingShapeBase current = this.innerList.ElementAt(i);
				yield return current;
			}
			yield break;
		}

		public int GetZIndex(FloatingShapeBase shape)
		{
			return this.innerList.IndexOf(shape);
		}

		public int GetZIndexById(int shapeId)
		{
			FloatingShapeBase floatingShapeBase = this.innerList.FirstOrDefault((FloatingShapeBase x) => x.Id == shapeId);
			if (floatingShapeBase == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(floatingShapeBase);
		}

		internal void CopyFrom(ShapeCollection fromShapeCollection, CopyContext context)
		{
			foreach (FloatingShapeBase floatingShapeBase in fromShapeCollection.innerList)
			{
				context.SourceFloatingShape = floatingShapeBase;
				this.innerList.Add(((ICopyable<FloatingShapeBase>)floatingShapeBase).Copy(context));
			}
			context.SourceFloatingShape = null;
		}

		void SetUniqueIdAndNameToShape(FloatingShapeBase shape)
		{
			shape.Shape.Id = this.shapeIdCounter;
			this.shapeIdCounter++;
			if (shape.Name == null)
			{
				switch (shape.FloatingShapeType)
				{
				case FloatingShapeType.Image:
					shape.Name = string.Format("Picture {0}", shape.Id);
					return;
				case FloatingShapeType.Chart:
					shape.Name = string.Format("Chart {0}", shape.Id);
					break;
				default:
					return;
				}
			}
		}

		void RecalculateUsedCellRange()
		{
			CellIndex topLeftCellIndex = this.GetTopLeftCellIndex();
			CellIndex bottomRightCellIndex = this.GetBottomRightCellIndex();
			this.usedCellRange = new CellRange(topLeftCellIndex, bottomRightCellIndex);
		}

		CellIndex GetTopLeftCellIndex()
		{
			Point topLeftPoint = this.GetTopLeftPoint();
			topLeftPoint.X = Math.Max(topLeftPoint.X, 0.0);
			topLeftPoint.Y = Math.Max(topLeftPoint.Y, 0.0);
			RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
			return worksheetLayout.GetCellIndexFromPoint(topLeftPoint.X, topLeftPoint.Y, false, false, false, false);
		}

		CellIndex GetBottomRightCellIndex()
		{
			RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
			Point bottomRightPoint = this.GetBottomRightPoint();
			bottomRightPoint.X = System.Math.Min(bottomRightPoint.X, worksheetLayout.Width);
			bottomRightPoint.Y = System.Math.Min(bottomRightPoint.Y, worksheetLayout.Height);
			return worksheetLayout.GetCellIndexFromPoint(bottomRightPoint.X, bottomRightPoint.Y, true, true, false, false);
		}

		Point GetTopLeftPoint()
		{
			RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			foreach (FloatingShapeBase floatingShapeBase in this.innerList)
			{
				Rect boundingRectangle = worksheetLayout.GetShapeLayoutBox(floatingShapeBase.Id).BoundingRectangle;
				double left = boundingRectangle.Left;
				double top = boundingRectangle.Top;
				if (left < num)
				{
					num = left;
				}
				if (top < num2)
				{
					num2 = top;
				}
			}
			if (num == 1.7976931348623157E+308)
			{
				num = 0.0;
			}
			if (num2 == 1.7976931348623157E+308)
			{
				num2 = 0.0;
			}
			return new Point(num, num2);
		}

		Point GetBottomRightPoint()
		{
			RadWorksheetLayout worksheetLayout = this.Worksheet.Workbook.GetWorksheetLayout(this.worksheet, false);
			double num = 0.0;
			double num2 = 0.0;
			foreach (FloatingShapeBase floatingShapeBase in this.innerList)
			{
				Rect boundingRectangle = worksheetLayout.GetShapeLayoutBox(floatingShapeBase.Id).BoundingRectangle;
				double right = boundingRectangle.Right;
				double bottom = boundingRectangle.Bottom;
				if (right > num)
				{
					num = right;
				}
				if (bottom > num2)
				{
					num2 = bottom;
				}
			}
			return new Point(num, num2);
		}

		void DoOnChanged()
		{
			this.usedCellRangeInvalidated = true;
			if (!this.collectionChangeCounter.IsUpdateInProgress)
			{
				this.OnChanged();
			}
		}

		void Shape_ShapeChanged(object sender, EventArgs e)
		{
			this.usedCellRangeInvalidated = true;
			if (!this.shapeChangeCounter.IsUpdateInProgress)
			{
				this.OnShapeChanged();
			}
		}

		internal void OnWorksheetCellPropertyChanged(Worksheet source, CellPropertyChangedEventArgs e)
		{
			foreach (FloatingChartShape floatingChartShape in this.Charts)
			{
				floatingChartShape.OnWorksheetCellPropertyChanged(source, e);
			}
		}

		internal event EventHandler Changing;

		void OnChanging()
		{
			if (this.Changing != null)
			{
				this.Changing(this, EventArgs.Empty);
			}
		}

		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		internal event EventHandler ShapeChanged;

		void OnShapeChanged()
		{
			if (this.ShapeChanged != null)
			{
				this.ShapeChanged(this, EventArgs.Empty);
			}
		}

		const string ImagesName = "Picture {0}";

		const string ChartsName = "Chart {0}";

		public static readonly double InsertDistance = 16.0;

		int shapeIdCounter;

		readonly List<FloatingShapeBase> innerList;

		readonly Worksheet worksheet;

		readonly BeginEndCounter collectionChangeCounter;

		readonly BeginEndCounter shapeChangeCounter;

		bool usedCellRangeInvalidated;

		CellRange usedCellRange;
	}
}
