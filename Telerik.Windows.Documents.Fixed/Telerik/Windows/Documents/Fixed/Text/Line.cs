using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class Line
	{
		public Line()
		{
			this.words = new List<Word>();
		}

		internal int WordsCount
		{
			get
			{
				return this.words.Count;
			}
		}

		internal int FirstIndex { get; set; }

		internal int LastIndex { get; set; }

		internal int Length { get; set; }

		internal bool IsEmpty
		{
			get
			{
				return this.WordsCount == 0;
			}
		}

		internal Word FirstWord
		{
			get
			{
				return this.words.FirstOrDefault<Word>();
			}
		}

		internal Word LastWord
		{
			get
			{
				return this.words.LastOrDefault<Word>();
			}
		}

		internal Rect BoundingRect { get; set; }

		public static int CompareWithCharacterIndex(Line line, int index)
		{
			if (line.FirstIndex <= index && index <= line.LastIndex)
			{
				return 0;
			}
			if (index > line.LastIndex)
			{
				return 1;
			}
			return -1;
		}

		void CalculateValue()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Word word in this.words)
			{
				stringBuilder.Append(word.ToString());
				if (word.EndsWithSpace)
				{
					stringBuilder.Append(Line.WordsSeparator);
				}
			}
			this.value = stringBuilder.ToString();
		}

		void CalculateBoundingRect()
		{
			double num2;
			double num = (num2 = double.MaxValue);
			double val;
			double num4;
			double num3 = (num4 = (val = double.MinValue));
			for (int i = 0; i < this.words.Count; i++)
			{
				Rect boundingRect = this.words[i].BoundingRect;
				num2 = System.Math.Min(boundingRect.Left, num2);
				num = System.Math.Min(boundingRect.Top, num);
				num4 = Math.Max(boundingRect.Right, num4);
				num3 = Math.Max(boundingRect.Bottom, num3);
				val = Math.Max(boundingRect.Height, val);
			}
			this.BoundingRect = new Rect(new Point(num2, num), new Point(num4, num3));
		}

		internal Word GetWordFromIndex(int index)
		{
			return CollectionsExtensions.FindElement<Word, int>(this.words, index, new CompareDelegate<Word, int>(Word.CompareWithCharacterIndex));
		}

		internal Word GetWordAt(int index)
		{
			return this.words[index];
		}

		internal void AddWord(Word word)
		{
			if (word.IsEmpty)
			{
				return;
			}
			this.words.Add(word);
		}

		internal void Finish()
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.Length = this.words.Sum((Word w) => w.Length);
			this.FirstIndex = this[0].FirstIndex;
			this.LastIndex = this[this.WordsCount - 1].LastIndex;
			this.CalculateBoundingRect();
			this.CalculateValue();
		}

		internal void FinishEmpty(Rect boundingRect)
		{
			if (!this.IsEmpty)
			{
				return;
			}
			this.Length = 0;
			this.FirstIndex = 0;
			this.LastIndex = 0;
			this.BoundingRect = boundingRect;
			this.value = "";
		}

		internal Word this[int index]
		{
			get
			{
				return this.words[index];
			}
		}

		internal int IndexOf(Word word)
		{
			return this.words.IndexOf(word);
		}

		internal IEnumerator<Word> GetEnumerator()
		{
			return this.words.GetEnumerator();
		}

		internal int GetIndexFromLocation(double location)
		{
			if (this.IsEmpty)
			{
				return 0;
			}
			Word word = CollectionsExtensions.FindElement<Word, double>(this.words, location, new CompareDelegate<Word, double>(Word.CompareWithWordBoundingRect));
			return word.GetIndexFromLocation(location);
		}

		public override string ToString()
		{
			return this.value;
		}

		public static bool operator ==(Line left, Line right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return object.ReferenceEquals(right, null);
			}
			return left.Equals(right);
		}

		public static bool operator !=(Line left, Line right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return !object.ReferenceEquals(right, null);
			}
			return !left.Equals(right);
		}

		public override bool Equals(object obj)
		{
			Line line = obj as Line;
			return !(line == null) && (this.FirstIndex.Equals(line.FirstIndex) && this.LastIndex.Equals(line.LastIndex)) && this.value.Equals(line.value);
		}

		bool WordsAreEqual(Line other)
		{
			for (int i = 0; i < this.words.Count; i++)
			{
				if (!this.words[i].Equals(other.words[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.FirstIndex.GetHashCode(), this.LastIndex.GetHashCode());
		}

		internal static readonly string WordsSeparator = " ";

		internal static readonly string LineSeparator = Environment.NewLine;

		internal static readonly double Delta = 1E-05;

		readonly List<Word> words;

		string value;
	}
}
