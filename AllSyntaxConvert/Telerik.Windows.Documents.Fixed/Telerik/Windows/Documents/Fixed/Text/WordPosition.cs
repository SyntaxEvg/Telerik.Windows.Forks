using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class WordPosition
	{
		public WordPosition(int index)
		{
			this.Index = index;
		}

		public int Index { get; set; }

		public Point Location { get; internal set; }
	}
}
