using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class Word
	{
		public static int CompareWithCharacterIndex(Word word, int index)
		{
			if (word.FirstIndex <= index && index <= word.LastIndex)
			{
				return 0;
			}
			if (index > word.LastIndex)
			{
				return 1;
			}
			return -1;
		}

		public static int CompareWithWordPosition(WordPosition position, double location)
		{
			if (position.Location.X == location)
			{
				return 0;
			}
			if (location > position.Location.X)
			{
				return 1;
			}
			return -1;
		}

		public static int CompareWithWordBoundingRect(Word word, double location)
		{
			if (location < word.BoundingRect.Left)
			{
				return -1;
			}
			if (word.BoundingRect.Right - location < 1E-05 && word.positions.Last<WordPosition>().Location.X >= location)
			{
				return 0;
			}
			if (word.BoundingRect.Right < location)
			{
				return 1;
			}
			return 0;
		}

		public Word()
		{
			this.chars = new List<Character>();
			this.positions = new List<WordPosition>();
			this.children = new List<object>();
		}

		internal int FirstIndex { get; set; }

		internal int LastIndex { get; set; }

		internal bool EndsWithSpace { get; set; }

		internal int Length
		{
			get
			{
				return this.chars.Count;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				return this.chars.Count == 0 && this.positions.Count == 0;
			}
		}

		internal Rect BoundingRect { get; set; }

		internal Character LastCharacter
		{
			get
			{
				return this.chars.LastOrDefault<Character>();
			}
		}

		public int CharactersCount
		{
			get
			{
				return this.chars.Count;
			}
		}

		double GetDistanceToWordPositionAtIndex(int index, double location)
		{
			if (this.positions.Count <= index || index < 0)
			{
				return 1.0;
			}
			return Math.Abs(this.positions[index].Location.X - location);
		}

		Point GetStart(int index)
		{
			if (index >= this.children.Count || index < 0)
			{
				return default(Point);
			}
			Point location;
			if (this.children[index] is Character)
			{
				Character character = (Character)this.children[index];
				location = new Point(character.BoundingRect.X, character.BoundingRect.Y);
			}
			else
			{
				location = ((WordPosition)this.children[index]).Location;
			}
			return location;
		}

		Point GetEnd(int index)
		{
			if (index > this.children.Count || index < 0)
			{
				return default(Point);
			}
			Point location;
			if (this.children[index] is Character)
			{
				Character character = (Character)this.children[index];
				location = new Point(character.BoundingRect.Right, character.BoundingRect.Y);
			}
			else
			{
				location = ((WordPosition)this.children[index]).Location;
			}
			return location;
		}

		void CalculateLocationByIndex(int index)
		{
			if (index > this.children.Count || index < 0 || !(this.children[index] is WordPosition))
			{
				return;
			}
			Point point2;
			Point point;
			if (index == 0)
			{
				point = (point2 = this.GetStart(index + 1));
			}
			else if (index == this.children.Count - 1)
			{
				point = (point2 = this.GetEnd(index - 1));
			}
			else
			{
				point2 = this.GetEnd(index - 1);
				point = this.GetStart(index + 1);
			}
			((WordPosition)this.children[index]).Location = new Point(Math.Min(point2.X, point.X) + Math.Abs(point2.X - point.X) / 2.0, point.Y);
		}

		void CalculateBoundingRect()
		{
			double num2;
			double num = (num2 = double.MaxValue);
			double val;
			double num4;
			double num3 = (num4 = (val = double.MinValue));
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i] is WordPosition)
				{
					this.CalculateLocationByIndex(i);
				}
				else
				{
					Rect boundingRect = ((Character)this.children[i]).BoundingRect;
					num2 = System.Math.Min(boundingRect.Left, num2);
					num = System.Math.Min(boundingRect.Top, num);
					num4 = Math.Max(boundingRect.Right, num4);
					num3 = Math.Max(boundingRect.Bottom, num3);
					val = Math.Max(boundingRect.Height, val);
				}
			}
			this.BoundingRect = new Rect(new Point(num2, num), new Point(num4, num3));
		}

		internal void AddCharacter(Character ch)
		{
			this.chars.Add(ch);
			this.children.Add(ch);
		}

		internal void AddPosition(int index)
		{
			WordPosition item = new WordPosition(index);
			this.positions.Add(item);
			this.children.Add(item);
		}

		internal WordPosition GetWordPositionFromIndex(int index)
		{
			int num = index - this.FirstIndex;
			if (num < 0 || num >= this.positions.Count)
			{
				return null;
			}
			return this.positions[num];
		}

		public Character GetCharacterAt(int index)
		{
			return this.chars[index];
		}

		internal Character GetCharacterFromIndex(int index)
		{
			WordPosition wordPositionFromIndex = this.GetWordPositionFromIndex(index);
			if (wordPositionFromIndex == null)
			{
				return null;
			}
			int num = this.children.IndexOf(wordPositionFromIndex);
			if (num < 0)
			{
				return null;
			}
			int num2 = 1;
			if (num == this.children.Count - 1)
			{
				num2 = -1;
			}
			while (!(this.children[num] is Character))
			{
				num += num2;
			}
			return (Character)this.children[num];
		}

		internal void Finish()
		{
			this.Finish(true);
		}

		internal void Finish(bool endsWithSpace)
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.EndsWithSpace = endsWithSpace;
			this.FirstIndex = this.positions.First<WordPosition>().Index;
			this.LastIndex = this.positions.Last<WordPosition>().Index;
			this.CalculateBoundingRect();
		}

		internal int GetIndexFromLocation(double location)
		{
			global::Telerik.Windows.Documents.Fixed.Text.WordPosition item = global::Telerik.Windows.Documents.Utilities.CollectionsExtensions.FindElement<global::Telerik.Windows.Documents.Fixed.Text.WordPosition, double>(this.positions, location, new global::Telerik.Windows.Documents.Utilities.CompareDelegate<global::Telerik.Windows.Documents.Fixed.Text.WordPosition, double>(global::Telerik.Windows.Documents.Fixed.Text.Word.CompareWithWordPosition));
			int num = this.positions.IndexOf(item);
			double num2 = this.GetDistanceToWordPositionAtIndex(num, location);
			int index = num;
			double distanceToWordPositionAtIndex = this.GetDistanceToWordPositionAtIndex(num - 1, location);
			if (distanceToWordPositionAtIndex > 0.0 && distanceToWordPositionAtIndex < num2)
			{
				num2 = distanceToWordPositionAtIndex;
				index = num - 1;
			}
			distanceToWordPositionAtIndex = this.GetDistanceToWordPositionAtIndex(num + 1, location);
			if (distanceToWordPositionAtIndex > 0.0 && distanceToWordPositionAtIndex < num2)
			{
				index = num + 1;
			}
			return this.positions[index].Index;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Character character in this.chars)
			{
				if (character.Char == '\0')
				{
					stringBuilder.Append(' ');
				}
				else
				{
					stringBuilder.Append(character);
				}
			}
			return stringBuilder.ToString();
		}

		internal const double DELTA = 1E-05;

		readonly List<Character> chars;

		readonly List<WordPosition> positions;

		readonly List<object> children;
	}
}
