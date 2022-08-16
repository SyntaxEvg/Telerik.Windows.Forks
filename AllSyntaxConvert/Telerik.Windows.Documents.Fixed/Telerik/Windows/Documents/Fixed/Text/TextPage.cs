using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class TextPage
	{
		public TextPage(TextDocument parent, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<TextDocument>(parent, "parent");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			this.page = fixedPage;
			this.parent = parent;
			this.lines = new List<Line>();
		}

		public RadFixedPage Page
		{
			get
			{
				return this.page;
			}
		}

		public int PageNumber
		{
			get
			{
				return this.Page.PageNumber;
			}
		}

		public TextDocument Parent
		{
			get
			{
				return this.parent;
			}
		}

		public IEnumerable<Line> Lines
		{
			get
			{
				return this.lines;
			}
		}

		public int LinesCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		public int Length { get; set; }

		public int LastIndex { get; set; }

		public Line LastLine
		{
			get
			{
				return this.lines.LastOrDefault<Line>();
			}
		}

		public static bool operator ==(TextPage left, TextPage right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return object.ReferenceEquals(right, null);
			}
			return object.ReferenceEquals(left, right) || left.Equals(right);
		}

		public static bool operator !=(TextPage left, TextPage right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return !object.ReferenceEquals(right, null);
			}
			return !left.Equals(right);
		}

		public static bool operator <(TextPage left, TextPage right)
		{
			Guard.ThrowExceptionIfNull<TextPage>(left, "left");
			Guard.ThrowExceptionIfNull<TextPage>(right, "right");
			if (left.Parent != right.Parent)
			{
				throw new ArgumentException("The text pages should be from the same TextDocument.");
			}
			return left.PageNumber < right.PageNumber;
		}

		public static bool operator >(TextPage left, TextPage right)
		{
			Guard.ThrowExceptionIfNull<TextPage>(left, "left");
			Guard.ThrowExceptionIfNull<TextPage>(right, "right");
			if (left.Parent != right.Parent)
			{
				throw new ArgumentException("The text pages should be from the same TextDocument.");
			}
			return left.PageNumber > right.PageNumber;
		}

		public override bool Equals(object obj)
		{
			TextPage textPage = obj as TextPage;
			if (textPage == null)
			{
				return false;
			}
			if (this.Parent != textPage.Parent)
			{
				throw new ArgumentException("The text pages should be from the same TextDocument.");
			}
			return this.PageNumber == textPage.PageNumber;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.PageNumber.GetHashCode(), this.Page.GetHashCode());
		}

		public void Finish()
		{
			if (this.LinesCount > 0)
			{
				this.Length = this.Lines.Sum((Line l) => l.Length);
				this.LastIndex = this.lines.Last<Line>().LastIndex;
				return;
			}
			Line line = new Line();
			line.FinishEmpty(new Rect(0.0, 0.0, 0.0, 0.0));
			this.lines.Add(line);
		}

		public void AddLine(Line line)
		{
			Guard.ThrowExceptionIfNull<Line>(line, "line");
			if (!line.IsEmpty)
			{
				this.lines.Add(line);
			}
		}

		public int IndexOfLine(Line line)
		{
			Guard.ThrowExceptionIfNull<Line>(line, "line");
			return this.lines.IndexOf(line);
		}

		public Line GetLineAt(int index)
		{
			return this.lines[index];
		}

		public global::Telerik.Windows.Documents.Fixed.Text.Line GetLineFromIndex(int index)
		{
			return global::Telerik.Windows.Documents.Utilities.CollectionsExtensions.FindElement<global::Telerik.Windows.Documents.Fixed.Text.Line, int>(this.lines, index, new global::Telerik.Windows.Documents.Utilities.CompareDelegate<global::Telerik.Windows.Documents.Fixed.Text.Line, int>(global::Telerik.Windows.Documents.Fixed.Text.Line.CompareWithCharacterIndex));
		}

		public Word GetWordFromIndex(int index)
		{
			Line lineFromIndex = this.GetLineFromIndex(index);
			if (lineFromIndex == null)
			{
				return null;
			}
			return lineFromIndex.GetWordFromIndex(index);
		}

		public WordPosition GetWordPositionFromIndex(int index)
		{
			Word wordFromIndex = this.GetWordFromIndex(index);
			if (wordFromIndex == null)
			{
				return null;
			}
			return wordFromIndex.GetWordPositionFromIndex(index);
		}

		public Character GetCharacterFromIndex(int index)
		{
			Word wordFromIndex = this.GetWordFromIndex(index);
			if (wordFromIndex == null)
			{
				return null;
			}
			return wordFromIndex.GetCharacterFromIndex(index);
		}

		public Line GetNearestLine(Point location)
		{
			double num2;
			double num = (num2 = double.MaxValue);
			Line line = null;
			foreach (Line line2 in this.lines)
			{
				if (!line2.IsEmpty)
				{
					Rect rect = line2.BoundingRect;
					if (rect.Contains(location))
					{
						double num3 = TextPage.CalcXDistanceToRect(rect, location);
						double num4 = TextPage.CalcYDistanceToRect(rect, location);
						if (num4 < num)
						{
							num = num4;
							line = line2;
						}
						else if (Math.Abs(num - num4) < 1E-05 && num3 < num2)
						{
							num = num4;
							num2 = num3;
							line = line2;
						}
					}
				}
			}
			if (line != null)
			{
				return line;
			}
			foreach (Line line3 in this.lines)
			{
				if (!line3.IsEmpty)
				{
					Rect rect = new Rect(line3.BoundingRect.X - 15.0, line3.BoundingRect.Y - 15.0, line3.BoundingRect.Width + 30.0, line3.BoundingRect.Height + 30.0);
					if (rect.Contains(location))
					{
						double num3 = TextPage.CalcXDistanceToCenter(rect, location);
						double num4 = TextPage.CalcYDistanceToCenter(rect, location);
						if (num4 < num)
						{
							num = num4;
							line = line3;
						}
						else if (Math.Abs(num - num4) < 1E-05 && num3 < num2)
						{
							num = num4;
							num2 = num3;
							line = line3;
						}
					}
				}
			}
			return line;
		}

		public string ToString(string linesSeparator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = this.lines.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				stringBuilder.Append(this.lines[i].ToString());
				if (i != num)
				{
					stringBuilder.Append(linesSeparator);
				}
			}
			return stringBuilder.ToString();
		}

		static double CalcXDistanceToRect(Rect rect, Point p)
		{
			return Math.Min(Math.Abs(rect.Left - p.X), Math.Abs(rect.Right - p.X));
		}

		static double CalcYDistanceToRect(Rect rect, Point p)
		{
			return Math.Min(Math.Abs(rect.Top - p.Y), Math.Abs(rect.Bottom - p.Y));
		}

		static double CalcXDistanceToCenter(Rect rect, Point p)
		{
			return Math.Abs(rect.Center().X - p.X);
		}

		static double CalcYDistanceToCenter(Rect rect, Point p)
		{
			return Math.Abs(rect.Center().Y - p.Y);
		}

		const double Delta = 1E-05;

		const double BoundingRectOffset = 15.0;

		readonly TextDocument parent;

		readonly RadFixedPage page;

		readonly List<Line> lines;
	}
}
