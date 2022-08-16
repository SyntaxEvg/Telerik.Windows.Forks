using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	public class TextPosition
	{
		public TextPosition(RadFixedDocument document)
			: this(document.TextDocument)
		{
		}

		public TextPosition(TextPosition position)
			: this(position.TextPage, position.Index)
		{
			this.lastTrackedLocation = position.lastTrackedLocation;
		}

		public TextPosition(RadFixedPage page)
			: this(TextPosition.GetTextPage(page))
		{
		}

		public TextPosition(RadFixedPage page, int index)
			: this(TextPosition.GetTextPage(page), index)
		{
		}

		internal TextPosition(TextPage page, int index)
		{
			Guard.ThrowExceptionIfNull<TextPage>(page, "page");
			if (index < 0 || page.LastIndex < index)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.TextPage = page;
			this.Index = index;
		}

		internal TextPosition(TextDocument document)
			: this(document.FirstPage)
		{
		}

		internal TextPosition(TextPage page)
			: this(page, 0)
		{
		}

		public event EventHandler PositionChanging;

		public event EventHandler PositionChanged;

		public int Index { get; set; }

		public RadFixedPage Page
		{
			get
			{
				return this.TextPage.Page;
			}
		}

		internal TextPage TextPage { get; set; }

		internal TextDocument Document
		{
			get
			{
				return this.TextPage.Parent;
			}
		}

		internal Point Location
		{
			get
			{
				return this.GetLocation();
			}
		}

		public static bool operator ==(TextPosition left, TextPosition right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return object.ReferenceEquals(right, null);
			}
			return left.Equals(right);
		}

		public static bool operator !=(TextPosition left, TextPosition right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return !object.ReferenceEquals(right, null);
			}
			return !left.Equals(right);
		}

		public static bool operator <(TextPosition left, TextPosition right)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(left, "left");
			Guard.ThrowExceptionIfNull<TextPosition>(right, "right");
			if (left.Document != right.Document)
			{
				throw new ArgumentException("The two positions must be from the same TextDocument.");
			}
			if (left.TextPage == right.TextPage)
			{
				return left.Index < right.Index;
			}
			return left.TextPage < right.TextPage;
		}

		public static bool operator >(TextPosition left, TextPosition right)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(left, "left");
			Guard.ThrowExceptionIfNull<TextPosition>(right, "right");
			if (left.Document != right.Document)
			{
				throw new ArgumentException("The two positions must be from the same TextDocument.");
			}
			if (left.TextPage == right.TextPage)
			{
				return left.Index > right.Index;
			}
			return left.TextPage > right.TextPage;
		}

		public static bool operator <=(TextPosition left, TextPosition right)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(left, "left");
			Guard.ThrowExceptionIfNull<TextPosition>(right, "right");
			if (left.Document != right.Document)
			{
				throw new ArgumentException("The two positions must be from the same TextDocument.");
			}
			return left < right || left == right;
		}

		public static bool operator >=(TextPosition left, TextPosition right)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(left, "left");
			Guard.ThrowExceptionIfNull<TextPosition>(right, "right");
			if (left.Document != right.Document)
			{
				throw new ArgumentException("The two positions must be from the same TextDocument.");
			}
			return left > right || left == right;
		}

		public override bool Equals(object obj)
		{
			TextPosition textPosition = obj as TextPosition;
			if (textPosition == null)
			{
				return false;
			}
			if (this.Document != textPosition.Document)
			{
				throw new ArgumentException("The two positions must be from the same TextDocument.");
			}
			return this.Index == textPosition.Index && this.TextPage == textPosition.TextPage;
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.Index.GetHashCode();
			return num * 23 + ((this.TextPage != null) ? this.TextPage.GetHashCode() : 0);
		}

		public bool MoveToPosition(TextPosition position)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(position, "position");
			if (position.Document != this.Document)
			{
				throw new ArgumentException("Position should belong to the same document");
			}
			return this.MoveToIndex(position.TextPage, position.Index);
		}

		public bool MoveToNextPosition()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				int num = this.Index + 1;
				TextPage page = this.TextPage;
				if (num > this.TextPage.LastIndex)
				{
					page = this.Document.GetNextPage(this.TextPage);
					num = 0;
				}
				return this.MoveToIndex(page, num);
			});
		}

		public bool MoveToPreviousPosition()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				int num = this.Index - 1;
				TextPage textPage = this.TextPage;
				if (num < 0)
				{
					textPage = this.Document.GetPrevPage(this.TextPage);
					if (textPage == null)
					{
						return false;
					}
					num = textPage.LastIndex;
				}
				return this.MoveToIndex(textPage, num);
			});
		}

		public bool MoveToNextWord()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Line lineFromIndex = this.TextPage.GetLineFromIndex(this.Index);
				if (lineFromIndex == null)
				{
					return false;
				}
				Word wordFromIndex = lineFromIndex.GetWordFromIndex(this.Index);
				if (wordFromIndex == null)
				{
					return false;
				}
				int num = lineFromIndex.IndexOf(wordFromIndex);
				num++;
				TextPage textPage = this.TextPage;
				Word word;
				if (num >= lineFromIndex.WordsCount)
				{
					Line line;
					this.GetNextLine(lineFromIndex, out line, out textPage);
					if (line == null)
					{
						return false;
					}
					word = line.FirstWord;
				}
				else
				{
					word = lineFromIndex[num];
				}
				return this.MoveToIndex(textPage, word.FirstIndex);
			});
		}

		public bool MoveToPreviousWord()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Line lineFromIndex = this.TextPage.GetLineFromIndex(this.Index);
				if (lineFromIndex == null)
				{
					return false;
				}
				Word wordFromIndex = lineFromIndex.GetWordFromIndex(this.Index);
				if (wordFromIndex == null)
				{
					return false;
				}
				if (wordFromIndex.FirstIndex < this.Index)
				{
					return this.MoveToIndex(wordFromIndex.FirstIndex);
				}
				int num = lineFromIndex.IndexOf(wordFromIndex);
				num--;
				TextPage textPage = this.TextPage;
				Word word;
				if (num < 0)
				{
					Line line;
					this.GetPrevLine(lineFromIndex, out line, out textPage);
					if (line == null)
					{
						return false;
					}
					word = line.LastWord;
				}
				else
				{
					word = lineFromIndex[num];
				}
				return this.MoveToIndex(textPage, word.FirstIndex);
			});
		}

		public bool MoveToCurrentWordStart()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Word wordFromIndex = this.TextPage.GetWordFromIndex(this.Index);
				return wordFromIndex != null && this.MoveToIndex(wordFromIndex.FirstIndex);
			});
		}

		public bool MoveToCurrentWordEnd()
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Word wordFromIndex = this.TextPage.GetWordFromIndex(this.Index);
				return wordFromIndex != null && wordFromIndex.LastIndex != this.Index && this.MoveToIndex(wordFromIndex.LastIndex);
			});
		}

		public bool MoveToLineStart()
		{
			Line associatedLine = this.GetAssociatedLine();
			return !(associatedLine == null) && this.MoveToIndex(associatedLine.FirstIndex);
		}

		public bool MoveToLineEnd()
		{
			Line associatedLine = this.GetAssociatedLine();
			return !(associatedLine == null) && this.MoveToIndex(associatedLine.LastIndex);
		}

		public bool MoveLineUp()
		{
			Line lineFromIndex = this.TextPage.GetLineFromIndex(this.Index);
			if (lineFromIndex == null)
			{
				return false;
			}
			Line line;
			TextPage textPage;
			this.GetPrevLine(lineFromIndex, out line, out textPage);
			if (textPage == null || line == null)
			{
				return false;
			}
			double location = this.lastTrackedLocation;
			this.MoveToLineAtLocation(textPage, line, location);
			this.lastTrackedLocation = location;
			return true;
		}

		public bool MoveLineDown()
		{
			Line lineFromIndex = this.TextPage.GetLineFromIndex(this.Index);
			if (lineFromIndex == null)
			{
				return false;
			}
			Line line;
			TextPage textPage;
			this.GetNextLine(lineFromIndex, out line, out textPage);
			if (textPage == null || line == null)
			{
				return false;
			}
			double location = this.lastTrackedLocation;
			this.MoveToLineAtLocation(textPage, line, location);
			this.lastTrackedLocation = location;
			return true;
		}

		public bool MoveToStartOfDocument()
		{
			return this.MoveToIndex(this.Document.FirstPage, 0);
		}

		public bool MoveToEndOfDocument()
		{
			TextPage lastPage = this.Document.LastPage;
			return this.MoveToIndex(lastPage, lastPage.LastIndex);
		}

		public override string ToString()
		{
			Word wordFromIndex = this.TextPage.GetWordFromIndex(this.Index);
			string text = wordFromIndex.ToString();
			return string.Format("{0}@{1}", text.Substring(0, this.Index - wordFromIndex.FirstIndex), text.Substring(this.Index - wordFromIndex.FirstIndex));
		}

		internal Line GetAssociatedLine()
		{
			return this.TextPage.GetLineFromIndex(this.Index);
		}

		internal Word GetAssociatedWord()
		{
			return this.TextPage.GetWordFromIndex(this.Index);
		}

		internal Character GetAssociatedCharacter()
		{
			return this.TextPage.GetCharacterFromIndex(this.Index);
		}

		internal bool MoveToStartOfTheNextLine()
		{
			TextPosition textPosition = new TextPosition(this);
			bool flag = textPosition.MoveToLineEnd();
			return (flag & textPosition.MoveToNextPosition()) && this.MoveToPosition(textPosition);
		}

		internal bool MoveToLocation(Point location)
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Line nearestLine = this.TextPage.GetNearestLine(location);
				if (nearestLine == null)
				{
					return false;
				}
				int indexFromLocation = nearestLine.GetIndexFromLocation(location.X);
				return this.MoveToIndex(indexFromLocation);
			});
		}

		internal bool MoveToLocation(TextPage page, Point location)
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				Line nearestLine = page.GetNearestLine(location);
				if (nearestLine == null)
				{
					return false;
				}
				int indexFromLocation = nearestLine.GetIndexFromLocation(location.X);
				return this.MoveToIndex(page, indexFromLocation);
			});
		}

		internal bool MoveToIndex(int index)
		{
			return this.MoveToIndex(this.TextPage, index);
		}

		internal bool MoveToIndex(TextPage page, int index)
		{
			return this.ExecuteInsideBeginEndUpdate(delegate
			{
				if (page == null || index < 0 || index > page.LastIndex)
				{
					return false;
				}
				this.TextPage = page;
				this.Index = index;
				return true;
			});
		}

		protected virtual void OnPositionChanging()
		{
			if (this.PositionChanging != null)
			{
				this.PositionChanging(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPositionChanged()
		{
			if (this.PositionChanged != null)
			{
				this.PositionChanged(this, EventArgs.Empty);
			}
		}

		static TextPage GetTextPage(RadFixedPage page)
		{
			RadFixedDocument radFixedDocument = page.Parent as RadFixedDocument;
			Guard.ThrowExceptionIfNull<RadFixedDocument>(radFixedDocument, "RadFixedPage must be added to a RadFixedDocument");
			return radFixedDocument.TextDocument.GetTextPage(page.PageNumber - 1);
		}

		Point GetLocation()
		{
			WordPosition wordPositionFromIndex = this.TextPage.GetWordPositionFromIndex(this.Index);
			if (wordPositionFromIndex == null)
			{
				return default(Point);
			}
			return wordPositionFromIndex.Location;
		}

		void MoveToLineAtLocation(TextPage page, Line line, double location)
		{
			Guard.ThrowExceptionIfNull<TextPage>(page, "page");
			Guard.ThrowExceptionIfNull<Line>(line, "line");
			int indexFromLocation = line.GetIndexFromLocation(location);
			this.MoveToIndex(page, indexFromLocation);
		}

		void GetNextLine(Line line, out Line nextLine, out TextPage page)
		{
			Guard.ThrowExceptionIfNull<Line>(line, "line");
			int num = this.TextPage.IndexOfLine(line);
			num++;
			if (num < this.TextPage.LinesCount)
			{
				page = this.TextPage;
				nextLine = page.GetLineAt(num);
				return;
			}
			page = this.Document.GetNextPage(this.TextPage);
			nextLine = ((page == null) ? null : page.Lines.FirstOrDefault<Line>());
		}

		void GetPrevLine(Line line, out Line nextLine, out TextPage page)
		{
			Guard.ThrowExceptionIfNull<Line>(line, "line");
			int num = this.TextPage.IndexOfLine(line);
			num--;
			if (num >= 0)
			{
				page = this.TextPage;
				nextLine = page.GetLineAt(num);
				return;
			}
			page = this.Document.GetPrevPage(this.TextPage);
			nextLine = ((page == null) ? null : page.Lines.Last<Line>());
		}

		bool ExecuteInsideBeginEndUpdate(Func<bool> method)
		{
			this.BeginUpdate();
			bool flag = method();
			this.EndUpdate(flag);
			return flag;
		}

		void TrackLocation()
		{
			Word wordFromIndex = this.TextPage.GetWordFromIndex(this.Index);
			if (wordFromIndex != null)
			{
				WordPosition wordPositionFromIndex = wordFromIndex.GetWordPositionFromIndex(this.Index);
				if (wordPositionFromIndex != null)
				{
					this.lastTrackedLocation = wordPositionFromIndex.Location.X;
				}
			}
		}

		void EndUpdate(bool shouldFirePositionChanged)
		{
			this.beginUpdateCount--;
			if (this.beginUpdateCount == 0)
			{
				this.TrackLocation();
				if (shouldFirePositionChanged)
				{
					this.OnPositionChanged();
				}
			}
		}

		void BeginUpdate()
		{
			if (this.beginUpdateCount == 0)
			{
				this.OnPositionChanging();
			}
			this.beginUpdateCount++;
		}

		int beginUpdateCount;

		double lastTrackedLocation;
	}
}
