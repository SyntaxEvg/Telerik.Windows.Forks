using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class HashArray : IEquatable<HashArray>
	{
		public HashArray(object[] items)
		{
			this.items = items;
		}

		public override bool Equals(object obj)
		{
			HashArray hashArray = obj as HashArray;
			return hashArray != null && this.Equals(hashArray);
		}

		public bool Equals(HashArray other)
		{
			if (other == this)
			{
				return true;
			}
			int num = this.items.Length;
			int num2 = other.items.Length;
			if (num != num2)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (!this.items[i].Equals(other.items[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 17;
			if (this.items != null)
			{
				foreach (object obj in this.items)
				{
					num = num * 16777619 + ((obj == null) ? 0 : obj.GetHashCode());
				}
				return num;
			}
			return 0;
		}

		const int Base = 16777619;

		const int InitialHashValue = 17;

		readonly object[] items;
	}
}
