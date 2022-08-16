using System;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	class PostScriptString
	{
		public PostScriptString(string str)
		{
			this.value = str.ToCharArray();
		}

		public PostScriptString(int capacity)
		{
			this.value = new char[capacity];
		}

		public string Value
		{
			get
			{
				return this.value.ToString();
			}
		}

		public int Capacity
		{
			get
			{
				return this.value.Length;
			}
		}

		public char this[int index]
		{
			get
			{
				return this.value[index];
			}
			set
			{
				this.value[index] = value;
			}
		}

		public byte[] ToByteArray()
		{
			byte[] array = new byte[this.value.Length];
			for (int i = 0; i < this.value.Length; i++)
			{
				array[i] = (byte)this.value[i];
			}
			return array;
		}

		public override string ToString()
		{
			return new string(this.value);
		}

		readonly char[] value;
	}
}
