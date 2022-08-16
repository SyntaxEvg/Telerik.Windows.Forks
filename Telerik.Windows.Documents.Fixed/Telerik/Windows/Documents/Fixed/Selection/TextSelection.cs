using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Selection
{
	public class TextSelection
	{
		internal TextSelection(TextDocument document)
		{
			this.document = document;
			this.StartPosition = null;
			this.EndPosition = null;
		}

		public event EventHandler SelectionChanging;

		public event EventHandler SelectionChanged;

		public TextPosition StartPosition { get; set; }

		public TextPosition EndPosition { get; set; }

		public bool IsEmpty
		{
			get
			{
				return this.StartPosition == this.EndPosition;
			}
		}

		public PathGeometry GetSelectionGeometry(RadFixedPage page)
		{
			TextPage textPage = this.document.GetTextPage(page.PageNumber - 1);
			return this.GetSelectionGeometry(textPage);
		}

		public void SetSelectionStart(TextPosition startPosition)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(startPosition, "startPosition");
			TextPosition endPosition = ((this.EndPosition == null) ? startPosition : this.EndPosition);
			this.SetSelection(startPosition, endPosition);
		}

		public void SetSelectionEnd(TextPosition endPosition)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(endPosition, "endPosition");
			TextPosition startPosition = ((this.StartPosition == null) ? endPosition : this.StartPosition);
			this.SetSelection(startPosition, endPosition);
		}

		public void SetSelection(TextPosition startPosition, TextPosition endPosition)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(startPosition, "startPosition");
			Guard.ThrowExceptionIfNull<TextPosition>(endPosition, "endPosition");
			if (startPosition.Document != endPosition.Document || endPosition.Document != this.document)
			{
				throw new ArgumentException("The start and the end position should be from the same document as the selection.");
			}
			this.OnSelectionChanging();
			this.StartPosition = new TextPosition(startPosition);
			this.EndPosition = new TextPosition(endPosition);
			this.OnSelectionChanged();
		}

		public void SelectAll()
		{
			TextPosition textPosition = new TextPosition(this.document);
			textPosition.MoveToStartOfDocument();
			TextPosition textPosition2 = new TextPosition(this.document);
			textPosition2.MoveToEndOfDocument();
			this.SetSelection(textPosition, textPosition2);
		}

		public void Clear()
		{
			if (!this.IsEmpty)
			{
				this.OnSelectionChanging();
				this.StartPosition = null;
				this.EndPosition = null;
				this.OnSelectionChanged();
			}
		}

		public string GetSelectedText()
		{
			if (this.IsEmpty)
			{
				return string.Empty;
			}
			return TextDocument.GetText(this.StartPosition, this.EndPosition, Line.LineSeparator);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use GetSelectedTextAsync() method instead.")]
		public void GetSelectedTextAsync(Action<string> callback)
		{
			if (!this.isRunning)
			{
				this.isRunning = true;
				Thread thread = new Thread(new ParameterizedThreadStart(this.GetSelectedText));
				thread.Start(callback);
			}
		}

		public Task<string> GetSelectedTextAsync()
		{
			if (!this.isRunning)
			{
				this.isRunning = true;
				Task<string> task = Task.Factory.StartNew<string>(new Func<string>(this.GetSelectedText));
				return task.ContinueWith<string>(delegate(Task<string> t)
				{
					this.isRunning = false;
					return t.Result;
				});
			}
			return Task.Factory.StartNew<string>(() => string.Empty);
		}

		public override string ToString()
		{
			return this.GetSelectedText();
		}

		internal PathGeometry GetSelectionGeometry(TextPage page)
		{
			Guard.ThrowExceptionIfNull<TextPage>(page, "page");
			if (this.IsEmpty)
			{
				return new PathGeometry();
			}
			TextPosition textPosition;
			TextPosition textPosition2;
			if (this.StartPosition < this.EndPosition)
			{
				textPosition = this.StartPosition;
				textPosition2 = this.EndPosition;
			}
			else
			{
				textPosition = this.EndPosition;
				textPosition2 = this.StartPosition;
			}
			TextPosition textPosition3 = new TextPosition(page);
			TextPosition textPosition4 = new TextPosition(page, page.LastIndex);
			if (textPosition > textPosition4 || textPosition2 < textPosition3)
			{
				return new PathGeometry();
			}
			if (textPosition3 < textPosition)
			{
				textPosition3 = new TextPosition(textPosition);
			}
			if (textPosition4 > textPosition2)
			{
				textPosition4 = textPosition2;
			}
			textPosition = new TextPosition(textPosition3);
			textPosition2 = new TextPosition(textPosition4);
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.FillRule = FillRule.Nonzero;
			Line associatedLine = textPosition3.GetAssociatedLine();
			Line associatedLine2 = textPosition4.GetAssociatedLine();
			if (associatedLine != null && associatedLine == associatedLine2)
			{
				Rect boundingRect = new Rect(new Point(textPosition.Location.X, associatedLine.BoundingRect.Top), new Point(textPosition2.Location.X, associatedLine.BoundingRect.Bottom));
				pathGeometry.Figures.Add(TextSelection.CreateSelectionRectangleFromBoundingRect(boundingRect));
				return pathGeometry;
			}
			Line line = null;
			for (;;)
			{
				if (line != null)
				{
					pathGeometry.Figures.Add(TextSelection.CreateSelectionRectangleFromBoundingRect(line.BoundingRect));
				}
				if (!textPosition3.MoveToStartOfTheNextLine())
				{
					break;
				}
				line = textPosition3.GetAssociatedLine();
				if (!(line != associatedLine2))
				{
					goto Block_10;
				}
			}
			return new PathGeometry();
			Block_10:
			if (associatedLine != null)
			{
				Rect boundingRect = new Rect(new Point(textPosition.Location.X, associatedLine.BoundingRect.Top), new Point(associatedLine.BoundingRect.Right, associatedLine.BoundingRect.Bottom));
				pathGeometry.Figures.Add(TextSelection.CreateSelectionRectangleFromBoundingRect(boundingRect));
			}
			if (associatedLine2 != null)
			{
				Rect boundingRect = new Rect(new Point(associatedLine2.BoundingRect.Left, associatedLine2.BoundingRect.Top), new Point(textPosition2.Location.X, associatedLine2.BoundingRect.Bottom));
				pathGeometry.Figures.Add(TextSelection.CreateSelectionRectangleFromBoundingRect(boundingRect));
			}
			return pathGeometry;
		}

		internal bool ContainsPosition(TextPosition pos)
		{
			return !(this.StartPosition == null) && !(this.EndPosition == null) && this.StartPosition < pos && pos < this.EndPosition;
		}

		static PathFigure CreateSelectionRectangleFromBoundingRect(Rect boundingRect)
		{
			if (boundingRect.IsEmpty)
			{
				return new PathFigure();
			}
			PathFigure pathFigure = new PathFigure();
			pathFigure.IsClosed = true;
			pathFigure.StartPoint = new Point(boundingRect.Left, boundingRect.Top);
			PolyLineSegment polyLineSegment = new PolyLineSegment();
			polyLineSegment.Points.Add(new Point(boundingRect.Right, boundingRect.Top));
			polyLineSegment.Points.Add(new Point(boundingRect.Right, boundingRect.Bottom));
			polyLineSegment.Points.Add(new Point(boundingRect.Left, boundingRect.Bottom));
			pathFigure.Segments.Add(polyLineSegment);
			pathFigure.IsFilled = true;
			return pathFigure;
		}

		void OnSelectionChanging()
		{
			if (this.SelectionChanging != null)
			{
				this.SelectionChanging(this, EventArgs.Empty);
			}
		}

		void OnSelectionChanged()
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, EventArgs.Empty);
			}
		}

		void GetSelectedText(object param)
		{
			Action<string> callback = param as Action<string>;
			if (callback == null)
			{
				return;
			}
			string selectedText = this.GetSelectedText();
			if (Helper.Dispatcher != null)
			{
				Helper.Dispatcher.BeginInvoke(new Action(delegate()
				{
					callback(selectedText);
				}), new object[0]);
			}
			else
			{
				callback(selectedText);
			}
			this.isRunning = false;
		}

		readonly TextDocument document;

		bool isRunning;
	}
}
