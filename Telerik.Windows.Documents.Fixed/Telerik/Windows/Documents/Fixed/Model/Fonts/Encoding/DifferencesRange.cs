using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class DifferencesRange
	{
		public DifferencesRange(byte startCharCode)
		{
			this.startCharCode = startCharCode;
			this.differences = new List<string>();
		}

		public void AddNextDifference(string difference)
		{
			this.differences.Add(difference);
		}

		public byte StartCharCode
		{
			get
			{
				return this.startCharCode;
			}
		}

		public byte EndCharCode
		{
			get
			{
				int num = (int)this.startCharCode + this.differences.Count - 1;
				return (byte)num;
			}
		}

		public IEnumerable<string> Differences
		{
			get
			{
				foreach (string difference in this.differences)
				{
					yield return difference;
				}
				yield break;
			}
		}

		readonly byte startCharCode;

		readonly List<string> differences;
	}
}
